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

namespace GalacticMonopoly.UnitTests.Services
{
    [TestClass]
    public class PirateServiceTests
    {
        [TestMethod]
        public void HandlePirateAttack_PlayerHasDefenseCard_RemovesCardAndLogsDefense()
        {
            // Arrange
            var player = new Player
            {
                Cards = new List<Card> { new Card { Type = CardType.PirateDefense } },
                Credits = 500
            };
            var gameState = new GameState();

            // Act
            PirateServices.HandlePirateAttack(player, gameState);

            // Assert
            Assert.AreEqual(0, player.Cards.Count, "Defense card should be removed.");
            // Additional assertions for logging can be added if GameEventLogger is mocked.
        }

        [TestMethod]
        public void HandlePirateAttack_PlayerHasEnoughCredits_PaysRansom()
        {
            // Arrange
            var player = new Player
            {
                Cards = new List<Card>(),
                Credits = 500
            };
            var gameState = new GameState();
            int expectedCredits = 300; // 500 - 200 ransom

            // Act
            PirateServices.HandlePirateAttack(player, gameState);

            // Assert
            Assert.AreEqual(expectedCredits, player.Credits, "Player should pay the ransom.");
            // Additional assertions for logging can be added if GameEventLogger is mocked.
        }

        [TestMethod]
        public void HandlePirateAttack_PlayerCannotPayRansom_LosesTurn()
        {
            // Arrange
            var player = new Player
            {
                Cards = new List<Card>(),
                Credits = 100
            };
            var gameState = new GameState();

            // Act
            PirateServices.HandlePirateAttack(player, gameState);

            // Assert
            Assert.AreEqual(PlayerStatus.LostTurnDueToPirates, player.Status, "Player should lose turn due to pirates.");
            // Additional assertions for logging can be added if GameEventLogger is mocked.
        }
    }
}
