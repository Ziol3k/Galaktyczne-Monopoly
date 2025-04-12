using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalacticMonopoly.Core.Enums;
using GalacticMonopoly.Core.Models;
using GalacticMonopoly.Core.Services;
using GalacticMonopoly.Core.Game;

namespace GalacticMonopoly.UnitTests.Services
{
    [TestClass]
    public class CardServiceTests
    {
        [TestMethod]
        public void DrawCard_ShouldReturnValidCard()
        {
            var gameState = new GameState { TurnCounter = 1 };

            var card = CardService.DrawCard(gameState);

            Assert.IsNotNull(card);
            Assert.IsTrue(Enum.IsDefined(typeof(CardType), card.Type));
        }

        [TestMethod]
        public void ResolveCard_PirateAttack_ShouldTriggerPirateLogic()
        {
            var player = new Player("Test");
            var card = new Card("Pirate Attack", CardType.PirateAttack, 0, 1);
            var gameState = new GameState();

            CardService.ResolveCard(player, card, gameState);

            Assert.IsTrue(player.Status == PlayerStatus.LostTurnDueToPirates ||
                         player.Credits == 800); // Okup 200, jeśli zapłacił
        }
    }
}