using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalacticMonopoly.Core.Models;
using GalacticMonopoly.Core.Services;

namespace GalacticMonopoly.Core.Game
{
    public class GameState
    {
        public List<Player> Players { get; set; } = new List<Player>();
        public int CurrentPlayerIndex { get; set; } = 0;
        public Player CurrentPlayer => Players[CurrentPlayerIndex];
        public GalaxyMap GalaxyMap { get; set; } = new GalaxyMap();
        public int TurnCounter { get; set; } = 0;


        public void NextTurn()
        {
            TurnCounter++;
            GameEventLogger.LogGameEvent($"Turn {TurnCounter} started");
            CurrentPlayerIndex = (CurrentPlayerIndex + 1) % Players.Count;
            GameEventLogger.LogBoardState(GalaxyMap);
            GameEventLogger.LogPlayerAssets(CurrentPlayer);
        }

        public void AddPlayer(Player player)
        {
            if (player != null && !Players.Contains(player))
            {
                Players.Add(player);
            }
        }

        public void RemovePlayer(Player player)
        {
            if (player != null && Players.Contains(player))
            {
                Players.Remove(player);
            }
        }
        
        public void SetGalaxyMap(GalaxyMap galaxyMap)
        {
            GalaxyMap = galaxyMap;
        }


    }
}
