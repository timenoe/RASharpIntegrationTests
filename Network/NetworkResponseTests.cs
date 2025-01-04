using RetroAchievementsMod.Network;
using RetroAchievementsMod.Utils;

namespace RetroAchievementsModTests.Network
{
    [TestClass]
    public class PingResponseTests
    {
        [TestMethod]
        public void SuccessTest()
        {
            string json = "{\"Success\":true}";
            LoginResponse response = new(JsonUtil.DeserializeJson(json));
            Assert.IsTrue(response.success);
            Assert.AreEqual(ResponseType.SUCCESS, response.type);
        }


        [TestMethod]
        public void EmptyTest()
        {
            // Empty string
            string json = "";
            PingResponse response = new(JsonUtil.DeserializeJson(json));
            Assert.IsFalse(response.success);
            Assert.AreEqual(ResponseType.EMPTY, response.type);

            // Empty Dictionary
            json = "{}";
            response = new(JsonUtil.DeserializeJson(json));
            Assert.IsFalse(response.success);
            Assert.AreEqual(ResponseType.EMPTY, response.type);
        }
    }

    [TestClass]
    public class LoginResponseTests
    {
        [TestMethod]
        public void SuccessTest()
        {
            string json = "{\"Success\":true,\"User\":\"TimmoneSimmons\",\"Token\":\"0123456789abcdef\",\"Score\":69420,\"SoftcoreScore\":1337,\"Messages\":99,\"Permissions\":3,\"AccountType\":\"Developer\"}";
            LoginResponse response = new(JsonUtil.DeserializeJson(json));
            Assert.IsTrue(response.success);
            Assert.AreEqual("TimmoneSimmons", response.user);
            Assert.AreEqual("0123456789abcdef", response.token);
            Assert.AreEqual(69420, response.score);
            Assert.AreEqual(1337, response.softScore);
            Assert.AreEqual(99, response.messages);
            Assert.AreEqual(3, response.perms);
            Assert.AreEqual("Developer", response.accountType);
            Assert.AreEqual(ResponseType.SUCCESS, response.type);
        }

        [TestMethod]
        public void ErrorTest()
        {
            string json = "{\"Success\":false,\"Error\":\"Invalid user\\/token combination.\",\"Code\":\"invalid_credentials\",\"Status\":401}";
            PingResponse response = new(JsonUtil.DeserializeJson(json));
            Assert.IsFalse(response.success);
            Assert.AreEqual("Invalid user/token combination.", response.error);
            Assert.AreEqual("invalid_credentials", response.code);
            Assert.AreEqual(401, response.status);
            Assert.AreEqual(ResponseType.ERROR, response.type);
        }
    }

    [TestClass]
    public class StartSessionResponseTests
    {
        [TestMethod]
        public void SuccessTest()
        {
            string json = "{\"Success\":true,\"HardcoreUnlocks\":[{\"ID\":483244,\"When\":1735947878},{\"ID\":483245,\"When\":1735947888}],\"ServerNow\":1735947898}";
            StartSessionResponse response = new(JsonUtil.DeserializeJson(json));
            Assert.IsTrue(response.success);
            Assert.AreEqual(2, response.unlocks.Count);
            Assert.AreEqual(483244, response.unlocks[0]);
            Assert.AreEqual(483245, response.unlocks[1]);
            Assert.AreEqual(1735947898, response.serverTime);
            Assert.AreEqual(ResponseType.SUCCESS, response.type);
        }

        [TestMethod]
        public void ErrorTest()
        {
            string json = "{\"Success\":false,\"Error\":\"Unknown game\"}";
            StartSessionResponse response = new(JsonUtil.DeserializeJson(json));
            Assert.IsFalse(response.success);
            Assert.AreEqual("Unknown game", response.error);
            Assert.AreEqual(ResponseType.ERROR, response.type);
        }
    }

    [TestClass]
    public class AwardAchievementResponseTests
    {
        [TestMethod]
        public void SuccessTest()
        {
            string json = "{\"Success\":true,\"AchievementsRemaining\":2,\"Score\":69420,\"SoftcoreScore\":1337,\"AchievementID\":483244}";
            AwardAchievementResponse response = new(JsonUtil.DeserializeJson(json));
            Assert.IsTrue(response.success);
            Assert.AreEqual(2, response.achsRemaining);
            Assert.AreEqual(69420, response.score);
            Assert.AreEqual(1337, response.softScore);
            Assert.AreEqual(483244, response.achId);
            Assert.AreEqual(ResponseType.SUCCESS, response.type);
        }

        [TestMethod]
        public void ErrorTestAlreadyUnlocked()
        {
            string json = "{\"Success\":false,\"AchievementsRemaining\":0,\"Error\":\"User already has this achievement unlocked.\",\"Score\":69420,\"SoftcoreScore\":1337,\"AchievementID\":213632}";
            AwardAchievementResponse response = new(JsonUtil.DeserializeJson(json));
            Assert.IsFalse(response.success);
            Assert.AreEqual("User already has this achievement unlocked.", response.error);
            Assert.AreEqual(69420, response.score);
            Assert.AreEqual(1337, response.softScore);
            Assert.AreEqual(213632, response.achId);
            Assert.AreEqual(ResponseType.ERROR, response.type);
        }

        [TestMethod]
        public void ErrorTestInvalid()
        {
            string json = "{\"Success\":false,\"Error\":\"Data not found for achievement 0\",\"Score\":69420,\"SoftcoreScore\":1337,\"AchievementID\":0}";
            AwardAchievementResponse response = new(JsonUtil.DeserializeJson(json));
            Assert.IsFalse(response.success);
            Assert.AreEqual("Data not found for achievement 0", response.error);
            Assert.AreEqual(69420, response.score);
            Assert.AreEqual(1337, response.softScore);
            Assert.AreEqual(0, response.achId);
            Assert.AreEqual(ResponseType.ERROR, response.type);
        }

        [TestMethod]
        public void ErrorTestUnofficial()
        {
            string json = "{\"Success\":false,\"Error\":\"Unofficial achievements cannot be unlocked\",\"Score\":69420,\"SoftcoreScore\":1337,\"AchievementID\":483244}";
            AwardAchievementResponse response = new(JsonUtil.DeserializeJson(json));
            Assert.IsFalse(response.success);
            Assert.AreEqual("Unofficial achievements cannot be unlocked", response.error);
            Assert.AreEqual(69420, response.score);
            Assert.AreEqual(1337, response.softScore);
            Assert.AreEqual(483244, response.achId);
            Assert.AreEqual(ResponseType.ERROR, response.type);
        }
    }

    [TestClass]
    public class AwardAchievementsResponseTests
    {
        [TestMethod]
        public void SuccessTest()
        {
            string json = "{\"Success\":true,\"Score\":69420,\"SoftcoreScore\":1337,\"ExistingIDs\":[483244],\"SuccessfulIDs\":[483245,483246]}";
            AwardAchievementsResponse response = new(JsonUtil.DeserializeJson(json));
            Assert.IsTrue(response.success);
            Assert.AreEqual(69420, response.score);
            Assert.AreEqual(1337, response.softScore);
            Assert.AreEqual(1, response.existingIds.Count);
            Assert.AreEqual(483244, response.existingIds[0]);
            Assert.AreEqual(2, response.successfulIds.Count);
            Assert.AreEqual(483245, response.successfulIds[0]);
            Assert.AreEqual(483246, response.successfulIds[1]);
            Assert.AreEqual(ResponseType.SUCCESS, response.type);
        }

        [TestMethod]
        public void ErrorTestNoDelegation()
        {
            string json = "{\"Success\":false,\"Error\":\"You must specify a target user.\",\"Status\":400}";
            AwardAchievementsResponse response = new(JsonUtil.DeserializeJson(json));
            Assert.IsFalse(response.success);
            Assert.AreEqual("You must specify a target user.", response.error);
            Assert.AreEqual(400, response.status);
        }
    }
}
