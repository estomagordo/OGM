using NUnit.Framework;
using Moq;
using OffseasonGM.Assets.Managers;
using OffseasonGM.Models;

namespace Tests
{
    [TestFixture]
    public class BusinessTests
    {
        [Test]
        public void AllTeamsPlayCorrectAmountOfHomeAndAwayGames()
        {
            var mockLocator = new Mock<IRepositoryLocator>();            
            var manager = new GameManager(mockLocator.Object, "");

            manager.CreateLeague(League.LeagueConfiguration.Teams30Divisions4, 2016);
            manager.SetupLeague();
            manager.PlaySeason();

            Assert.That(manager.Teams.TrueForAll(team => team.AwayGames.Count == 41 && team.HomeGames.Count == 41));
        } 
    }
}
