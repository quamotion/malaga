// <copyright file="MjpegClient.cs" company="Quamotion">
// Copyright (c) Quamotion. All rights reserved.
// </copyright>

using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using System;
using System.Buffers;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Quamotion.Malaga
{
    /// <summary>
    /// A very basic MJPEG client.
    /// </summary>
    public class MjpegClient : IDisposable
    {
        private readonly ILogger logger;
        private readonly string mjpegUrl;
        private readonly WdaDriver driver;
        private readonly byte[] expectedHeaderStart = Encoding.ASCII.GetBytes("--BoundaryString\r\nContent-type: image/jpg\r\nContent-Length: ");
        private readonly byte[] newLine = new byte[] { 0x0D, 0x0A, 0x0D, 0x0A }; // crlfcrlf
        private Stream stream;

        private readonly ManualResetEventSlim firstScreenshotReceived = new ManualResetEventSlim();
        private readonly HttpClient httpClient;
        private bool shouldRun;
        private Thread runLoop;
        private ScreenOrientation currentRotation;

        /// <summary>
        /// Initializes a new instance of the <see cref="MjpegClient"/> class.
        /// </summary>
        /// <param name="mjpegUrl">
        /// The URL of the MJPEG server.
        /// </param>
        /// <param name="httpClient">
        /// A <see cref="HttpClient"/> which can be used to communicate with the MJPEG server.
        /// </param>
        /// <param name="logger">
        /// A <see cref="ILogger"/> to use when logging.
        /// </param>
        public MjpegClient(string mjpegUrl, WdaDriver driver, HttpClient httpClient, ILogger logger = null)
        {
            this.logger = logger;
            this.mjpegUrl = mjpegUrl ?? throw new ArgumentNullException(nameof(mjpegUrl));
            this.driver = driver ?? throw new ArgumentNullException(nameof(driver));
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MjpegClient"/> class. Used for mocking only.
        /// </summary>
        protected MjpegClient()
        {
        }

        /// <summary>
        /// The event which is raised when a new screenshot is received.
        /// </summary>
        public event EventHandler ScreenshotReceived;

        /// <summary>
        /// Gets the buffer into which each individual image is read. This image is stored in its compressed form.
        /// </summary>
        public byte[] Buffer { get; private set; }

        /// <summary>
        /// Gets the amount of data in the <see cref="Buffer"/>.
        /// </summary>
        public int Length { get; private set; }

        /// <summary>
        /// Gets the lock to acquire when reading or writing to <see cref="Buffer"/>.
        /// </summary>
        public ReaderWriterLockSlim BufferLock { get; } = new ReaderWriterLockSlim();

        /// <inheritdoc/>
        public void Dispose()
        {
            this.Stop();

            if (this.Buffer != null)
            {
                ArrayPool<byte>.Shared.Return(this.Buffer);
            }

            this.stream?.Dispose();
        }

        public void Connect()
        {

            this.stream = this.GetStream(mjpegUrl, CancellationToken.None).GetAwaiter().GetResult();
            this.currentRotation = this.driver.Rotation;
        }

        /// <summary>
        /// Starts the MJPEG client, and optionally waits for the first picture to arrive.
        /// </summary>
        /// <param name="waitForFirstScreenshot">
        /// <see langword="true"/> to wait for the first picture to arrive; otherwise, <see langword="false"/>.
        /// </param>
        public void Start(bool waitForFirstScreenshot = true)
        {
            if (this.stream == null)
            {
                throw new InvalidOperationException("You should call Connect() first");
            }

            if (this.shouldRun)
            {
                throw new InvalidOperationException("Already running");
            }

            this.shouldRun = true;
            this.runLoop = new Thread(this.ReadImages);
            this.runLoop.Start();

            // Block for 2 seconds, waiting for the first screenshot to arrive.
            this.firstScreenshotReceived.Wait(TimeSpan.FromSeconds(2));
        }

        /// <summary>
        /// Stops the MJPEG client.
        /// </summary>
        public void Stop()
        {
            if (this.runLoop == null)
            {
                return;
            }

            this.shouldRun = false;

            // Block for 2 seconds, waiting for the run loop to stop.
            this.runLoop.Join(TimeSpan.FromSeconds(2));
        }

        // The buffer into which to read the header for each packet.
        private readonly byte[] headerBuffer = new byte[0x50];

        /// <summary>
        /// Reads an individual image.
        /// </summary>
        /// <returns>
        /// <see langword="true"/> when the image could be read successfully; otherwise, <see langword="false"/>.
        /// </returns>
        public bool ReadImage()
        {
            if (this.stream == null)
            {
                throw new InvalidOperationException("Call Connect() first");
            }

            // Get the current rotation, and determine whether the rotation has changed.
            var rotation = this.driver.Rotation;

            if (this.currentRotation != rotation)
            {
                // If so, create a new connection to the WDA. This will force the WDA to refresh
                // the orientation of the device.
                try
                {
                    this.GetStream(this.mjpegUrl, CancellationToken.None).GetAwaiter().GetResult().Dispose();
                }
                catch (HttpRequestException)
                {
                }

                this.currentRotation = rotation;
            }

            // We done manual parsing of HTTP messages here. There are two reasons:
            // 1. .NET (Core) doesn't include a way class for working with multi-message responses
            // 2. The data is generated by the WebDriverAgent, which doesn't use a full HTTP server either,
            //    but just writes raw bytes to the client, making it a bit more safe to do manual parsing
            //    here.

            // The message format is roughly:
            // > --BoundaryString
            // > Content-type: image/jpg
            // > Content-Length: 1609505
            // >
            // > [1609505 bytes of image data]

            // So we'll:
            // 1. Read the header, and read it up to the 'Content-Length: ' message.
            // 2. Find the next CRLF (new line) character, and use that position to get the length of the message
            // 3. Read all remaining bytes
            // So let's do this.

            // Read the header
            int totalRead = 0;
            int read = 0;
            while (totalRead < this.headerBuffer.Length)
            {
                read = this.stream.Read(this.headerBuffer, totalRead, headerBuffer.Length - totalRead);
                totalRead += read;

                if (read == 0)
                {
                    this.logger?.LogWarning("Unable to read the message header, because the server has disconnected");
                    return false;
                }
            }

            if (!this.headerBuffer.Take(this.expectedHeaderStart.Length).SequenceEqual(this.expectedHeaderStart))
            {
                this.logger?.LogWarning($"An invalid message header was sent by the server. Got '{Encoding.ASCII.GetString(this.headerBuffer)}', expected '{Encoding.ASCII.GetString(this.expectedHeaderStart)}'.");
                return false;
            }

            // Determine the exact length
            var lengthBuffer = new Span<byte>(headerBuffer, expectedHeaderStart.Length, headerBuffer.Length - expectedHeaderStart.Length);
            var newLineIndex = lengthBuffer.IndexOf(this.newLine);
            var lengthString = Encoding.ASCII.GetString(headerBuffer, expectedHeaderStart.Length, newLineIndex);
            if (!int.TryParse(lengthString, out int length))
            {
                this.logger?.LogWarning($"Unable to parse the length of the screenshot. Got '{lengthString}'.");
                return false;
            }

            this.Length = length;

            this.BufferLock.EnterWriteLock();

            try
            {
                if (this.Buffer == null || this.Buffer.Length < this.Length)
                {
                    if (this.Buffer != null)
                    {
                        ArrayPool<byte>.Shared.Return(this.Buffer);
                    }

                    this.Buffer = ArrayPool<byte>.Shared.Rent(this.Length);
                }

                // Read the data: starts with the remaining data in the header buffer, continue with
                var remainingData = lengthBuffer.Slice(newLineIndex + this.newLine.Length);
                remainingData.CopyTo(this.Buffer);

                totalRead = remainingData.Length;

                while (totalRead < this.Length)
                {
                    read = this.stream.Read(this.Buffer, totalRead, this.Length - totalRead);
                    totalRead += read;

                    if (read == 0)
                    {
                        this.logger?.LogWarning("Unable to read the image, because the server has disconnected");
                        this.Length = 0;
                        return false;
                    }
                }
            }
            finally
            {
                this.BufferLock.ExitWriteLock();
            }

            this.firstScreenshotReceived.Set();
            this.ScreenshotReceived?.Invoke(this, EventArgs.Empty);

            // Finally, the server will send 2 newline sequences to signal the message is complete.
            // Make sure we process these, too. If it fails, we still got a screenshot we send to the
            // client.
            totalRead = 0;
            while (totalRead < this.newLine.Length)
            {
                read = this.stream.Read(headerBuffer, totalRead, this.newLine.Length - totalRead);
                totalRead += read;

                if (read == 0)
                {
                    this.logger?.LogWarning("Unable to read the header header, because the server has disconnected");

                    // Break instead of return, we stil have a valid screenshot
                    break;
                }
            }

            if (!headerBuffer.Take(this.newLine.Length).SequenceEqual(this.newLine))
            {
                this.logger?.LogWarning($"Got an invalid trailer. Got '{Encoding.ASCII.GetString(this.headerBuffer, 0, this.newLine.Length)}'.");
                return false;
            }

            return true;
        }

        /// <summary>
        /// Reads all images.
        /// </summary>
        protected void ReadImages()
        {
            bool connected = true;

            while (this.shouldRun && connected)
            {
                connected = this.ReadImage();
            }
        }

        protected async Task<Stream> GetStream(string url, CancellationToken cancellationToken)
        {
            var response = await this.httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead, cancellationToken).ConfigureAwait(false);
            var stream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false);
            var boundaryString = response.Content.Headers.ContentType.Parameters.Single(p => p.Name == "boundary").Value;

            if (boundaryString != "--BoundaryString")
            {
                throw new ArgumentOutOfRangeException("Only the '--BoundaryString' boundary string is supported.");
            }

            return stream;
        }
    }
}
