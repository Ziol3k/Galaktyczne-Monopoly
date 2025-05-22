using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalacticMonopoly.Core.Enums;
using GalacticMonopoly.Core.Models;
using GalacticMonopoly.Core.Services;

namespace GalacticMonopoly.Core.Game
{
    public class Player
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; }
        public string AvatarPath { get; set; }
        public int Credits { get; set; }
        public List<Card> Cards { get; set; } = new List<Card>();
        public List<Planet> OwnedPlanets { get; set; } = new List<Planet>();
        public List<SystemPlanet> OwnedSystemPlanets { get; set; } = new List<SystemPlanet>();
        public PlayerStatus Status { get; set; } = PlayerStatus.Active;
        public int Position { get; set; } = 0;
        public int SkipedTurns { get; set; } = 0;
        public int ConsecutiveSkips { get; set; } = 0;
        public bool CanSkipTurn => ConsecutiveSkips < 2; // Gracz może pominąć turę maksymalnie 2 razy z rzędu

        public Player(string name)
        {
            Name = name;
            Credits = 1000; // Początkowe kredyty
        }

        public void AddCredits(int amount)
        {
            Credits += amount;
            GameEventLogger.LogCreditChange(this, amount, "income");
            AvatarPath = "Images/avatars/default.png";
        }

        public void AddPlanet(Planet planet)
        {
            OwnedPlanets.Add(planet);
        }

        public void AddCard(Card card)
        {
            Cards.Add(card);
        }

        public void RemoveCard(Card card)
        {
            Cards.Remove(card);
        }

        public void AddSystemPlanet(SystemPlanet systemPlanet)
        {
            OwnedSystemPlanets.Add(systemPlanet);
        }

        public bool Pay(int amount)
        {
            if (Credits >= amount)
            {
                Credits -= amount;
                GameEventLogger.LogCreditChange(this, -amount, "payment");
                return true;
            }
            GameEventLogger.LogFailedPayment(this, amount);
            return false;
        }


        public void SkipTurn()
        {
            if (CanSkipTurn)
            {
                Status = PlayerStatus.SkippedTurn;
                ConsecutiveSkips++;
                SkipedTurns++;
                GameEventLogger.LogTurnSkip(this, ConsecutiveSkips);
            }
        }

        public void ChangeStatus(PlayerStatus newStatus)
        {
            var oldStatus = Status;
            Status = newStatus;
            GameEventLogger.LogStatusChange(this, oldStatus, newStatus);
        }
    }
}
