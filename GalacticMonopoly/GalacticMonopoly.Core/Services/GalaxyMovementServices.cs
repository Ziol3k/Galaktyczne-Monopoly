using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using GalacticMonopoly.Core.Game;
using GalacticMonopoly.Core.Models;
using GalacticMonopoly.Core.Services;

namespace GalacticMonopoly.Core.Services
{
    public static class GalaxyMovementServices
    {
        private static readonly Random random = new();

        public static void MovePlayer(Player player, GameState state)
        {
            int roll = random.Next(1, 7);
            player.Position = (player.Position + roll) % state.GalaxyMap.Fields.Count;
            GameEventLogger.LogDiceRoll(player, roll);
        }

        public static void OfferGalacticTicket(Player player, GameState state)
        {
            var ticketCard = player.Cards.FirstOrDefault(c => c.Type == Enums.CardType.GalacticTicket);
            if (ticketCard != null)
            {
                // Przykładowa implementacji - przesuń na losowy przystanek
                var stops = state.GalaxyMap.Fields
                    .Select((f, i) => new { Field = f, Index = i })
                    .Where(x => x.Field.Type == Enums.FieldType.GalacticTrainStop)
                    .ToList();

                if (stops.Any())
                {
                    var chosen = stops[random.Next(stops.Count)];
                    player.Position = chosen.Index;
                    player.RemoveCard(ticketCard);
                    GameEventLogger.LogTrainTeleport(player, chosen.Index);
                }
            }
        }
    }
}

