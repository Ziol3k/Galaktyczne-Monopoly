using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using GalacticMonopoly.Core.Game;
using GalacticMonopoly.Core.Models;
using GalacticMonopoly.Core.Enums;
using System.Numerics;

namespace GalacticMonopoly.Core.Services
{
    public static class GameEventLogger
    {
        public static void Log(string message)
        {
            Console.WriteLine($"[LOG] {DateTime.Now}: {message}");
        }

        // Podstawowe logi
        public static void LogGameStart(int playerCount)
        {
            Log($"Game started with {playerCount} players.");
        }

        public static void LogPlayerTurnStart(Player player)
        {
            Log($"--- {player.Name}'s turn (Position: {player.Position}, Credits: {player.Credits}) ---");
        }

        public static void LogPlayerMove(Player player, int oldPosition, int newPosition)
        {
            Log($"{player.Name} moved from position {oldPosition} to {newPosition}.");
        }

        public static void LogDiceRoll(Player player, int roll)
        {
            Log($"{player.Name} rolled a {roll}.");
        }

        // Logi związane z planetami i strukturami
        public static void LogPlanetPurchase(Player player, Planet planet)
        {
            Log($"{player.Name} purchased planet {planet.Name} for {planet.Price} credits.");
        }

        public static void LogStructureBuilt(Player player, Planet planet, StructureType structureType)
        {
            Log($"{player.Name} built {structureType} on {planet.Name}.");
        }

        public static void LogStructurredBuilt(Player player, SystemPlanet system, StructureType structureType)
        {
            Log($"{player.Name} built {structureType} on {system.Name}.");
        }

        public static void LogStructureUpgrade(Player player, Structure structure)
        {
            Log($"{player.Name} upgraded {structure.Type} on {structure.Planet.Name} to level {structure.Level}.");
        }

        public static void LogStructureDowngrade(Player player, Structure structure)
        {
            Log($"{player.Name}'s {structure.Type} on {structure.Planet.Name} was downgraded to level {structure.Level}.");
        }

        public static void LogSystemOwnership(Player player, SystemPlanet system)
        {
            Log($"{player.Name} gained ownership of the {system.Name} system!");
        }

        // Logi finansowe
        public static void LogIncome(Player player, int amount)
        {
            Log($"{player.Name} received income of {amount} credits.");
        }

        public static void LogPayment(Player from, Player to, int amount)
        {
            Log($"{from.Name} paid {amount} credits to {to.Name}.");
        }

        public static void LogFailedPayment(Player player, int amount)
        {
            Log($"{player.Name} failed to pay {amount} credits (insufficient funds).");
        }

        public static void LogCreditChange(Player player, int amount, string reason)
        {
            string action = amount >= 0 ? "gained" : "lost";
            Log($"{player.Name} {action} {Math.Abs(amount)} credits ({reason}). New balance: {player.Credits}.");
        }

        // Logi kart
        public static void LogCardDraw(Player player, Card card)
        {
            Log($"{player.Name} drew a card: {card.Description}.");
        }

        public static void LogCardUsed(Player player, Card card)
        {
            Log($"{player.Name} used card: {card.Description}.");
        }

        // Logi specjalnych zdarzeń
        public static void LogPirateAttack(Player player)
        {
            Log($"{player.Name} encountered a pirate attack!");
        }

        public static void LogPirateDefense(Player player)
        {
            Log($"{player.Name} successfully defended against pirates using a defense card.");
        }

        public static void LogPirateRansom(Player player, int amount)
        {
            Log($"{player.Name} paid {amount} credits ransom to pirates.");
        }

        public static void LogPirateConsequence(Player player)
        {
            Log($"{player.Name} will lose 2 turns due to pirate attack.");
        }

        public static void LogTrainTeleport(Player player, int newPosition)
        {
            Log($"{player.Name} used Galactic Ticket to teleport to position {newPosition}.");
        }

        public static void LogTurnSkip(Player player, int consecutiveSkips)
        {
            Log($"{player.Name} chose to skip turn ({consecutiveSkips}/2 skips used).");
        }

        public static void LogPlayerBankruptcy(Player player)
        {
            Log($"{player.Name} has gone bankrupt and is out of the game!");
        }

        public static void LogGameEvent(string eventDescription)
        {
            Log($"EVENT: {eventDescription}");
        }


        public static void LogBoardState(GalaxyMap map)
        {
            var sb = new StringBuilder();
            sb.AppendLine("Current board state:");
            foreach (var field in map.Fields)
            {
                sb.Append($"Field {map.Fields.IndexOf(field)}: {field.Name} ({field.Type})");
                if (field.Planet != null)
                {
                    sb.Append($", Planet: {field.Planet.Name}");
                    if (field.Planet.IsOwned)
                        sb.Append($"(Owned by: {field.Planet.Owner.Name})");
                }
                sb.AppendLine();
            }
            Log(sb.ToString());
        }

        public static void LogPlayerAssets(Player player)
        {
            var sb = new StringBuilder();
            sb.AppendLine($"{player.Name}'s assets:");
            sb.AppendLine($"- Credits: {player.Credits}");
            sb.AppendLine($"- Planets: {player.OwnedPlanets.Count}");
            sb.AppendLine($"- Systems: {player.OwnedSystemPlanets.Count}");
            sb.AppendLine($"- Cards: {player.Cards.Count}");
            Log(sb.ToString());
        }


        public static void LogStatusChange(Player player, PlayerStatus oldStatus, PlayerStatus newStatus)
        {
            Log($"{player.Name} status changed from {oldStatus} to {newStatus}");
        }
    }
}
