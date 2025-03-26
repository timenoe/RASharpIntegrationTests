using System.Text.Json;
using RASharpIntegration.Data;

namespace RASharpIntegrationTests.Data
{
    [TestClass]
    public sealed class AchievementDataTests
    {
        [TestMethod]
        public void RaGameTest()
        {
            string json = "{\"Id\":32123,\"Name\":\"Terraria\"}";
            RaGame game = JsonSerializer.Deserialize<RaGame>(json);

            Assert.IsNotNull(game);
            Assert.AreEqual(32123, game.Id);
            Assert.AreEqual("Terraria", game.Name);
        }

        [TestMethod]
        public void RaAchievementTest()
        {
            string json = "{\"Title\":\"Timber!!\",\"Description\":\"Chop down your first tree.\",\"Points\":1,\"Type\":\"\",\"Category\":5,\"Id\":483247,\"Badge\":\"12345\",\"Set\":123}";
            RaAchievement achievement = JsonSerializer.Deserialize<RaAchievement>(json);

            Assert.IsNotNull(achievement);
            Assert.AreEqual("Timber!!", achievement.Title);
            Assert.AreEqual("Chop down your first tree.", achievement.Description);
            Assert.AreEqual(1, achievement.Points);
            Assert.AreEqual("", achievement.Type);
            Assert.AreEqual(5, achievement.Category);
            Assert.AreEqual(483247, achievement.Id);
            Assert.AreEqual("12345", achievement.Badge);
            Assert.AreEqual(123, achievement.Set);
        }
    }
}
