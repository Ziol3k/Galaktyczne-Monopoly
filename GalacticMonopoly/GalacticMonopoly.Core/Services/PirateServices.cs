using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalacticMonopoly.Core.Enums;
using GalacticMonopoly.Core.Game;
using GalacticMonopoly.Core.Models;

namespace GalacticMonopoly.Core.Services
{
    public static class PirateServices
    {
        public static void HandlePirateAttack(Player player, GameState state)
        {
            GameEventLogger.LogPirateAttack(player);
            var defenseCard = player.Cards.FirstOrDefault(c => c.Type == CardType.PirateDefense);
            if (defenseCard != null)
            {
                player.RemoveCard(defenseCard);
                GameEventLogger.LogPirateDefense(player);
                return;
            }

            int ransom = 200;

            if (player.Credits >= ransom)
            {
                player.Pay(ransom);
                GameEventLogger.LogPirateRansom(player, ransom);
            }
            else
            {
                player.Status = PlayerStatus.LostTurnDueToPirates;
                GameEventLogger.LogPirateConsequence(player);
            }
        }
    }
}

