using RetroAchievementsMod.Network;

namespace RetroAchievementsModTests.Network
{
    [TestClass]
    public sealed class RequestManagerTests
    {
        [TestMethod]
        public void BuildLoginRequestTest()
        {
            NetworkRequest.BuildLoginRequest("retroachievements.org", "TimmoneSimmons", "supersecretpass", out UriBuilder request);
            Assert.AreEqual("https://retroachievements.org/dorequest.php?r=login2&u=TimmoneSimmons&p=supersecretpass", request.Uri.ToString());
        }

        [TestMethod]
        public void BuildStartSessionRequestTest()
        {
            NetworkRequest.BuildStartSessionRequest("retroachievements.org", "TimmoneSimmons", "0123456789abcdef", 32123, out UriBuilder request);
            Assert.AreEqual("https://retroachievements.org/dorequest.php?r=startsession&u=TimmoneSimmons&t=0123456789abcdef&g=32123", request.Uri.ToString());
        }

        [TestMethod]
        public async Task BuildPingRequestTest()
        {
            NetworkRequest.BuildPingRequest("retroachievements.org", "TimmoneSimmons", "0123456789abcdef", 32123, "Digging a hellevator 👌", out UriBuilder request, out MultipartFormDataContent multipart);
            Assert.AreEqual("https://retroachievements.org/dorequest.php?r=ping&u=TimmoneSimmons&t=0123456789abcdef&g=32123", request.Uri.ToString());

            Assert.IsNotNull(multipart.First().Headers.ContentDisposition);
            Assert.AreEqual("m", multipart.First().Headers.ContentDisposition?.Name);
            Assert.AreEqual("Digging a hellevator 👌", await multipart.First().ReadAsStringAsync());
        }

        [TestMethod]
        public void BuildAwardAchievementTest()
        {
            NetworkRequest.BuildAwardAchievementRequest("retroachievements.org", "TimmoneSimmons", "0123456789abcdef", true, 32123, out UriBuilder request);
            Assert.AreEqual("https://retroachievements.org/dorequest.php?r=awardachievement&u=TimmoneSimmons&t=0123456789abcdef&h=1&a=32123&v=7a3f30386627952180d5afbae3beee6f", request.Uri.ToString());
        }

        [TestMethod]
        public async Task BuildAwardAchievementsRequestTest()
        {
            NetworkRequest.BuildAwardAchievementsRequest("retroachievements.org", "TimmoneSimmons", "0123456789abcdef", true, [483244, 483245, 483246], out UriBuilder request, out MultipartFormDataContent multipart);
            Assert.AreEqual("https://retroachievements.org/dorequest.php?r=awardachievements&u=TimmoneSimmons&t=0123456789abcdef", request.Uri.ToString());

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
