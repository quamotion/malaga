using Moq;
using Moq.Protected;
using OpenQA.Selenium;
using OpenQA.Selenium.Remote;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Quamotion.Malaga.Tests
{
    /// <summary>
    /// Tests the <see cref="MjpegClient"/> class.
    /// </summary>
    public class MjpegClientTests
    {
        private const string TestUrl = "http://vanity-url-mock:1234/mjpeg";

        /// <summary>
        /// Tests the <see cref="MjpegClient.ReadImage"/> method using a recorded HTTP trace.
        /// </summary>
        [Fact]
        public void ReadImageTest()
        {
            using (Stream stream = File.OpenRead("mjpegstream.bin"))
            {
                var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
                var httpClient = new HttpClient(messageHandlerMock.Object);

                var response = new HttpResponseMessage()
                {
                    Content = new StreamContent(stream)
                };
                response.Content.Headers.ContentType = new MediaTypeHeaderValue("multipart/mixed");
                response.Content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary", "--BoundaryString"));

                messageHandlerMock
                    .Protected()
                    .Setup<Task<HttpResponseMessage>>(
                        "SendAsync",
                        ItExpr.Is<HttpRequestMessage>(m => m.RequestUri.ToString() == TestUrl),
                        ItExpr.IsAny<CancellationToken>()
                    )
                    .ReturnsAsync(response)
                    .Verifiable();


                var wdaDriverMock = new Mock<WdaDriver>(MockBehavior.Strict);
                wdaDriverMock.Protected().Setup("StartClient");
                wdaDriverMock.Protected().Setup<Response>("Execute", "newSession", ItExpr.IsAny<Dictionary<string, object>>()).Returns(new Response());
                wdaDriverMock.Setup(d => d.Rotation).Returns(ScreenOrientation.Portrait);

                SHA1 hasher = SHA1.Create();

                var feed = new MjpegClient(TestUrl, wdaDriverMock.Object, httpClient);
                feed.Connect();

                // This feed contains 4 (identical) images.
                for (int i = 0; i < 4; i++)
                {
                    Assert.True(feed.ReadImage());
                    using (MemoryStream bufferStream = new MemoryStream(feed.Buffer, 0, feed.Length))
                    {
                        Assert.Equal("ykShU5VIB4GtzA5x9TBSElmX57Q=", Convert.ToBase64String(hasher.ComputeHash(bufferStream)));
                        Assert.Equal(44243, feed.Length);
                    }
                }

                Assert.False(feed.ReadImage());
            }
        }

        /// <summary>
        /// Tests reading of images when the device rotates. Makes sure the WDA is pinged, so that it
        /// refreshes the 'rectangle' of the device screen.
        /// </summary>
        [Fact]
        public void ReadRotateImageTest()
        {
            var messageHandlerMock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            var httpClient = new HttpClient(messageHandlerMock.Object);

            int getCount = 0;

            messageHandlerMock
                .Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(m => m.RequestUri.ToString() == TestUrl),
                    ItExpr.IsAny<CancellationToken>()
                )
                .ReturnsAsync(this.GetHttpResponse)
                .Callback(() => getCount++)
                .Verifiable();

            var rotation = ScreenOrientation.Portrait;
            var wdaDriverMock = new Mock<WdaDriver>(MockBehavior.Strict);
            wdaDriverMock.Protected().Setup("StartClient");
            wdaDriverMock.Protected().Setup<Response>("Execute", "newSession", ItExpr.IsAny<Dictionary<string, object>>()).Returns(new Response());
            wdaDriverMock.Setup(d => d.Rotation).Returns(() => rotation);

            var feed = new MjpegClient(TestUrl, wdaDriverMock.Object, httpClient);
            feed.Connect();

            SHA1 hasher = SHA1.Create();
            Assert.True(feed.ReadImage());
            Assert.Equal(1, getCount);

            Assert.True(feed.ReadImage());
            Assert.Equal(1, getCount);

            // Let the rotation change.
            rotation = ScreenOrientation.Landscape;
            Assert.True(feed.ReadImage());
            Assert.Equal(2, getCount);
        }

        private HttpResponseMessage GetHttpResponse()
        {
            Stream stream = File.OpenRead("mjpegstream.bin");
            var response = new HttpResponseMessage()
            {
                Content = new StreamContent(stream)
            };
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("multipart/mixed");
            response.Content.Headers.ContentType.Parameters.Add(new NameValueHeaderValue("boundary", "--BoundaryString"));

            return response;
        }
    }
}
