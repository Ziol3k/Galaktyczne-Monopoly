using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalacticMonopoly.Core.Enums;
using GalacticMonopoly.Core.Models;
using GalacticMonopoly.Core.Services;
using GalacticMonopoly.Core.Game;
using GalacticMonopoly.Core.Rules;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalacticMonopoly.UnitTests.Models
{
    [TestClass]
    public class PlanetTests
    {
        [TestMethod]
        public void Buy_WhenUnowned_ShouldTransferOwnership()
        {
            var planet = new Planet("Mars", 500);
            var player = new Player("Test") { Credits = 1000 };

            planet.Buy(player);

            Assert.IsTrue(planet.IsOwned);
            Assert.AreEqual(player, planet.Owner);
            Assert.AreEqual(500, player.Credits);
        }

        [TestMethod]
        public void Buy_WhenAlreadyOwned_ShouldNotChangeOwner()
        {
            var planet = new Planet("Mars", 500);
            var player1 = new Player("Owner") { Credits = 1000 };
            var player2 = new Player("Intruder") { Credits = 1000 };

            planet.Buy(player1);
            planet.Buy(player2); // Próba przejęcia

            Assert.AreEqual(player1, planet.Owner);
            Assert.AreEqual(1000, player2.Credits); // Nie pobrano kredytów
        }
    }
}