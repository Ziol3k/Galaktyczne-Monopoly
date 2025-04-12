using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using GalacticMonopoly.Core.Models;
using GalacticMonopoly.Core.Rules;
using GalacticMonopoly.Core.Services;

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

        public void PerformTurn()
        {
            var player = State.CurrentPlayer;
            GameEventLogger.LogPlayerTurnStart(player);

            if (player.Status == Enums.PlayerStatus.SkippedTurn)
            {
                player.Status = Enums.PlayerStatus.Active;
                State.NextTurn();
                return;
            }

            int oldPosition = player.Position;
            GalaxyMovementServices.MovePlayer(player, State);
            GameEventLogger.LogPlayerMove(player, oldPosition, player.Position);

            var field = State.GalaxyMap.Fields[player.Position];
            FieldEffects.ApplyEffects(player, field, State);



        }
    }
}
