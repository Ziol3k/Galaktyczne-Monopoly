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
using GalacticMonopoly.Core;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GalacticMonopoly.UnitTests.GameLogic
{
    [TestClass]
    public class FieldEffectsTests
    {
        [TestMethod]
        public void ApplyEffects_OnSingularZone_ShouldDrawCard()
        {
            var player = new Player("Test");
            var field = new Field("Singular Zone", FieldType.SingularZone);
            var gameState = new GameState();

            FieldEffects.ApplyEffects(player, field, gameState);

            Assert.IsTrue(gameState.CardHistory.Count > 0);
        }

        [TestMethod]
        public void ApplyEffects_OnPlanet_ShouldAllowPurchase()
        {
            var player = new Player("Test") { Credits = 1000 };
            var planet = new Planet("Earth", 500);
            var field = new Field("Earth", FieldType.Planet, planet);
            var gameState = new GameState();

            FieldEffects.ApplyEffects(player, field, gameState);

            Assert.IsTrue(planet.IsOwned);
        }
    }
}