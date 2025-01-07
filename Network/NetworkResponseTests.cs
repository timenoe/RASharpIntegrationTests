using System.Text.Json;
using RASharpIntegration.Network;

namespace RAStandaloneIntegrationTests.Network
{
    /// <summary>
    /// Used to test the BaseResponse class
    /// </summary>
    [TestClass]
    public class BaseResponseTests
    {
        /// <summary>
        /// Tests if a successful base response can be deserialized
        /// </summary>
        [TestMethod]
        public void SuccessTest()
        {
            string json = "{\"Success\":true}";
            BaseResponse response = JsonSerializer.Deserialize<BaseResponse>(json);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);
        }

        /// <summary>
        /// Tests if a failed base response can be deserialized
        /// </summary>
        [TestMethod]
        public void ErrorTest()
        {
            string json = "{\"Success\":false,\"Error\":\"There is an error here.\"}";
            BaseResponse response = JsonSerializer.Deserialize<BaseResponse>(json);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("There is an error here.", response.Error);
        }
    }

    /// <summary>
    /// Used to test the LoginResponse class
    /// </summary>
    [TestClass]
    public class LoginResponseTests
    {
        /// <summary>
        /// Tests if a successful login2 response can be deserialized
        /// </summary>
        [TestMethod]
        public void SuccessTest()
        {
            string json = "{\"Success\":true,\"User\":\"TimmoneSimmons\",\"Token\":\"0123456789abcdef\",\"Score\":69420,\"SoftcoreScore\":1337,\"Messages\":99,\"Permissions\":3,\"AccountType\":\"Developer\"}";
            LoginResponse response = JsonSerializer.Deserialize<LoginResponse>(json);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);
            Assert.AreEqual("TimmoneSimmons", response.User);
            Assert.AreEqual("0123456789abcdef", response.Token);
            Assert.AreEqual(69420, response.Score);
            Assert.AreEqual(1337, response.SoftScore);
            Assert.AreEqual(99, response.Messages);
            Assert.AreEqual(3, response.Perms);
            Assert.AreEqual("Developer", response.AccountType);
        }

        /// <summary>
        /// Tests if a failed login2 response can be deserialized
        /// </summary>
        [TestMethod]
        public void ErrorTest()
        {
            string json = "{\"Success\":false,\"Error\":\"Invalid user\\/token combination.\",\"Code\":\"invalid_credentials\",\"Status\":401}";
            LoginResponse response = JsonSerializer.Deserialize<LoginResponse>(json);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Invalid user/token combination.", response.Error);
        }
    }

    /// <summary>
    /// Used to test the StartSessionResponse class
    /// </summary>
    [TestClass]
    public class StartSessionResponseTests
    {
        /// <summary>
        /// Tests if a successful startsession response can be deserialized
        /// </summary>
        [TestMethod]
        public void SuccessTest()
        {
            string json = "{\"Success\":true,\"HardcoreUnlocks\":[{\"ID\":483244,\"When\":1735947878},{\"ID\":483245,\"When\":1735947888}],\"ServerNow\":1735947898}";
            StartSessionResponse response = JsonSerializer.Deserialize<StartSessionResponse>(json);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);
            Assert.AreEqual(2, response.GetUnlockedAchIds().Count);
            Assert.AreEqual(483244, response.GetUnlockedAchIds()[0]);
            Assert.AreEqual(483245, response.GetUnlockedAchIds()[1]);
            Assert.AreEqual(1735947898, response.ServerTime);
        }

        /// <summary>
        /// Tests if a failed startsession response can be deserialized
        /// </summary>
        [TestMethod]
        public void ErrorTest()
        {
            string json = "{\"Success\":false,\"Error\":\"Unknown game\"}";
            StartSessionResponse response = JsonSerializer.Deserialize<StartSessionResponse>(json);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Unknown game", response.Error);
        }
    }

    /// <summary>
    /// Used to test the AwardAchievementResponse class
    /// </summary>
    [TestClass]
    public class AwardAchievementResponseTests
    {
        /// <summary>
        /// Tests if a successful awardachievement response can be deserialized
        /// </summary>
        [TestMethod]
        public void SuccessTest()
        {
            string json = "{\"Success\":true,\"AchievementsRemaining\":2,\"Score\":69420,\"SoftcoreScore\":1337,\"AchievementID\":483244}";
            AwardAchievementResponse response = JsonSerializer.Deserialize<AwardAchievementResponse>(json);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);
            Assert.AreEqual(2, response.AchsRemaining);
            Assert.AreEqual(69420, response.Score);
            Assert.AreEqual(1337, response.SoftScore);
            Assert.AreEqual(483244, response.AchId);
        }

        /// <summary>
        /// Tests if a failed awardachievement response can be deserialized<br/>
        /// Specifically tests the failure when the achievement is already unlocked
        /// </summary>
        [TestMethod]
        public void ErrorTestAlreadyUnlocked()
        {
            string json = "{\"Success\":false,\"AchievementsRemaining\":0,\"Error\":\"User already has this achievement unlocked.\",\"Score\":69420,\"SoftcoreScore\":1337,\"AchievementID\":213632}";
            AwardAchievementResponse response = JsonSerializer.Deserialize<AwardAchievementResponse>(json);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("User already has this achievement unlocked.", response.Error);
            Assert.AreEqual(69420, response.Score);
            Assert.AreEqual(1337, response.SoftScore);
            Assert.AreEqual(213632, response.AchId);
        }

        /// <summary>
        /// Tests if a failed awardachievement response can be deserialized<br/>
        /// Specifically tests the failure when the achievement is invalid
        /// </summary>
        [TestMethod]
        public void ErrorTestInvalid()
        {
            string json = "{\"Success\":false,\"Error\":\"Data not found for achievement 0\",\"Score\":69420,\"SoftcoreScore\":1337,\"AchievementID\":0}";
            AwardAchievementResponse response = JsonSerializer.Deserialize<AwardAchievementResponse>(json);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Data not found for achievement 0", response.Error);
            Assert.AreEqual(69420, response.Score);
            Assert.AreEqual(1337, response.SoftScore);
            Assert.AreEqual(0, response.AchId);
        }

        /// <summary>
        /// Tests if a failed awardachievement response can be deserialized<br/>
        /// Specifically tests the failure when the achievement is in Unoffical
        /// </summary>
        [TestMethod]
        public void ErrorTestUnofficial()
        {
            string json = "{\"Success\":false,\"Error\":\"Unofficial achievements cannot be unlocked\",\"Score\":69420,\"SoftcoreScore\":1337,\"AchievementID\":483244}";
            AwardAchievementResponse response = JsonSerializer.Deserialize<AwardAchievementResponse>(json);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("Unofficial achievements cannot be unlocked", response.Error);
            Assert.AreEqual(69420, response.Score);
            Assert.AreEqual(1337, response.SoftScore);
            Assert.AreEqual(483244, response.AchId);
        }
    }

    /// <summary>
    /// Used to test the AwardAchievementsResponse class
    /// </summary>
    [TestClass]
    public class AwardAchievementsResponseTests
    {
        /// <summary>
        /// Tests if a successful awardachievements response can be deserialized
        /// </summary>
        [TestMethod]
        public void SuccessTest()
        {
            string json = "{\"Success\":true,\"Score\":69420,\"SoftcoreScore\":1337,\"ExistingIDs\":[483244],\"SuccessfulIDs\":[483245,483246]}";
            AwardAchievementsResponse response = JsonSerializer.Deserialize<AwardAchievementsResponse>(json);

            Assert.IsNotNull(response);
            Assert.IsTrue(response.Success);
            Assert.AreEqual(69420, response.Score);
            Assert.AreEqual(1337, response.SoftScore);

            Assert.IsNotNull(response.ExistingIds);
            Assert.AreEqual(1, response.ExistingIds.Count);
            Assert.AreEqual(483244, response.ExistingIds[0]);

            Assert.IsNotNull(response.SuccessfulIds);
            Assert.AreEqual(2, response.SuccessfulIds.Count);
            Assert.AreEqual(483245, response.SuccessfulIds[0]);
            Assert.AreEqual(483246, response.SuccessfulIds[1]);
        }

        /// <summary>
        /// Tests if a failed awardachievements response can be deserialized<br/>
        /// Specifically tests the failure when a delegated user is not specified
        /// </summary>
        [TestMethod]
        public void ErrorTestNoDelegation()
        {
            string json = "{\"Success\":false,\"Error\":\"You must specify a target user.\",\"Status\":400}";
            AwardAchievementsResponse response = JsonSerializer.Deserialize<AwardAchievementsResponse>(json);

            Assert.IsNotNull(response);
            Assert.IsFalse(response.Success);
            Assert.AreEqual("You must specify a target user.", response.Error);
        }
    }
}
