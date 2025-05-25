using System;
using GalacticMonopoly.Core.Models;
using GalacticMonopoly.Core.Rules;
using GalacticMonopoly.Core.Services;
using GalacticMonopoly.Core.Utils;

namespace GalacticMonopoly.Core.Game
{
    public class Game
    {
        public GameState State { get; set; } = new GameState();

        public void InitializeGame(int numberOfPlayers, GalaxyMap map)
        {
            State.SetGalaxyMap(map);
            for (int i = 0; i < numberOfPlayers; i++)
            {
                var player = new Player($"Player {i + 1}");
                State.AddPlayer(player);
                GameEventLogger.LogGameStart(numberOfPlayers);
            }
        }

        // Wykonuje tylko ruch gracza i zwraca wynik rzutu
        public int PerformMovement()
        {
            var player = State.CurrentPlayer;
            GameEventLogger.LogPlayerTurnStart(player);

            if (player.Status == Enums.PlayerStatus.SkippedTurn)
            {
                player.Status = Enums.PlayerStatus.Active;
                return -1; // Skipped turn, no movement
            }

            int roll = Dice.Roll();
            GameEventLogger.LogDiceRoll(player, roll);

            int oldPosition = player.Position;
            player.Position = (player.Position + roll) % State.GalaxyMap.Fields.Count;

            GameEventLogger.LogPlayerMove(player, oldPosition, player.Position);

            return roll;
        }

        // Wykonuje efekty pola (bez zmiany tury)
        public void ApplyFieldEffects()
        {
            var player = State.CurrentPlayer;
            var field = State.GalaxyMap.Fields[player.Position];
            FieldEffects.ApplyEffects(player, field, State);
        }

        // Kończy turę gracza
        public void EndTurn()
        {
            State.NextTurn();
        }
    }
}
