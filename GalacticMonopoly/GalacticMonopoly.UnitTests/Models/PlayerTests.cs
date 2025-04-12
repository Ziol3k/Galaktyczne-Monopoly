using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GalacticMonopoly.Core.Enums;
using GalacticMonopoly.Core.Models;
using GalacticMonopoly.Core.Services;
using GalacticMonopoly.Core.Game;
using GalacticMonopoly.Core.Rules;




namespace GalacticMonopoly.UnitTests.Models
{
    [TestClass]
    public class PlayerTests
    {


        [TestMethod]
        public void Pay_WithSufficientCredits_ShouldReduceBalance()
        {
            var player = new Player("Test") { Credits = 1000 };

            bool result = player.Pay(300);

            Assert.IsTrue(result);
            Assert.AreEqual(700, player.Credits);
        }

        [TestMethod]
        public void Pay_WithInsufficientCredits_ShouldNotChangeBalance()
        {
            var player = new Player("Test") { Credits = 100 };

            bool result = player.Pay(500);

            Assert.IsFalse(result);
            Assert.AreEqual(100, player.Credits);
        }

        [TestMethod]
        public void SkipTurn_WhenUnderLimit_ShouldChangeStatus()
        {
            var player = new Player("Test") { ConsecutiveSkips = 0 };

            player.SkipTurn();

            Assert.AreEqual(PlayerStatus.SkippedTurn, player.Status);
            Assert.AreEqual(1, player.ConsecutiveSkips);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void SkipTurn_WhenExceedsLimit_ShouldThrowException()
        {
            var player = new Player("Test") { ConsecutiveSkips = 2 };
            player.SkipTurn();
        }
    }
}