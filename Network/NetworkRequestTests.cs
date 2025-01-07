using System;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using RASharpIntegration.Network;

namespace RAStandaloneIntegrationTests.Network
{
    /// <summary>
    /// Used to test the NetworkRequest class
    /// </summary>
    [TestClass]
    public sealed class NetworkRequestTests
    {
        /// <summary>
        /// Request header shared between different NetworkRequest tests
        /// </summary>
        private readonly RequestHeader _header = new("ra.org", 32123, true, "TimmoneSimmons", "0123456789abcdef");

        /// <summary>
        /// Tests if a login2 request can be built correctly
        /// </summary>
        [TestMethod]
        public void BuildLoginRequestTest()
        {
            Uri request = NetworkRequest.BuildLoginRequest(_header, "supersecretpass");
            Assert.AreEqual("https://ra.org/dorequest.php?u=TimmoneSimmons&r=login2&p=supersecretpass", request.ToString());
        }

        /// <summary>
        /// Tests if a startsession request can be built correctly
        /// </summary>
        [TestMethod]
        public void BuildStartSessionRequestTest()
        {
            Uri request = NetworkRequest.BuildStartSessionRequest(_header);
            Assert.AreEqual("https://ra.org/dorequest.php?u=TimmoneSimmons&t=0123456789abcdef&r=startsession&g=32123", request.ToString());
        }

        /// <summary>
        /// Tests if a ping request can be built correctly
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task BuildPingRequestTest()
        {
            Uri request = NetworkRequest.BuildPingRequest(_header, "Digging a hellevator 👌", out MultipartFormDataContent multipart);
            Assert.AreEqual("https://ra.org/dorequest.php?u=TimmoneSimmons&t=0123456789abcdef&r=ping&g=32123", request.ToString());

            Assert.IsNotNull(multipart.First().Headers.ContentDisposition);
            Assert.AreEqual("m", multipart.First().Headers.ContentDisposition?.Name);
            Assert.AreEqual("Digging a hellevator 👌", await multipart.First().ReadAsStringAsync());
        }

        /// <summary>
        /// Tests if an awardachievement request can be built correctly
        /// </summary>
        [TestMethod]
        public void BuildAwardAchievementTest()
        {
            Uri request = NetworkRequest.BuildAwardAchievementRequest(_header, 32123);
            Assert.AreEqual("https://ra.org/dorequest.php?u=TimmoneSimmons&t=0123456789abcdef&r=awardachievement&h=1&a=32123&v=7a3f30386627952180d5afbae3beee6f", request.ToString());
        }

        /// <summary>
        /// Tests if an awardachievements request can be built correctly
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task BuildAwardAchievementsRequestTest()
        {
            Uri request = NetworkRequest.BuildAwardAchievementsRequest(_header, [483244, 483245, 483246], out MultipartFormDataContent multipart);
            Assert.AreEqual("https://ra.org/dorequest.php?u=TimmoneSimmons&t=0123456789abcdef&r=awardachievements", request.ToString());

            Assert.IsNotNull(multipart.First().Headers.ContentDisposition);
            Assert.AreEqual("h", multipart.First().Headers.ContentDisposition?.Name);
            Assert.AreEqual("1", await multipart.First().ReadAsStringAsync());

            Assert.IsNotNull(multipart.ElementAt(1).Headers.ContentDisposition);
            Assert.AreEqual("a", multipart.ElementAt(1).Headers.ContentDisposition?.Name);
            Assert.AreEqual("483244,483245,483246", await multipart.ElementAt(1).ReadAsStringAsync());

            Assert.IsNotNull(multipart.ElementAt(2).Headers.ContentDisposition);
            Assert.AreEqual("v", multipart.ElementAt(2).Headers.ContentDisposition?.Name);
            Assert.AreEqual("9e9cf35e8d0c9fc0cc26061b556ce226", await multipart.ElementAt(2).ReadAsStringAsync());
        }
    }
}
