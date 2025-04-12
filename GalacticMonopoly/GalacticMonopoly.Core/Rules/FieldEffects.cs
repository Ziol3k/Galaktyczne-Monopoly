using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalacticMonopoly.Core.Enums;
using GalacticMonopoly.Core.Game;
using GalacticMonopoly.Core.Models;
using GalacticMonopoly.Core.Services;

namespace GalacticMonopoly.Core.Rules
{
    public class FieldEffects
    {
        public static void ApplyEffects(Player player, Field field, GameState state)
        {
            switch (field.Type)
            {
                case FieldType.SingularZone:
                    var card = CardServices.DrawCard(state);
                    CardServices.ResolveCard(player, card, state);
                    break;

                case FieldType.PirateAttack:
                    PirateServices.HandlePirateAttack(player, state);
                    break;

                case FieldType.GalacticTrainStop:
                    GalaxyMovementServices.OfferGalacticTicket(player, state);
                    break;

                case FieldType.Planet:
                    var planet = field.Planet;
                    if (planet == null) return;

                    if (!planet.IsOwned)
                    {
                        if (player.Credits >= planet.Price)
                        {
                            planet.Buy(player);
                        }
                    }
                    else if (planet.Owner != player)
                    {
                        // Obsługa opłat dla właściciela planety
                        int rent = CalculateRent(planet.structure);
                        if (player.Pay(rent))
                        {
                            planet.Owner.AddCredits(rent);
                            GameEventLogger.LogPayment(player, planet.Owner, rent);
                        }
                        else
                        {
                            GameEventLogger.LogFailedPayment(player, rent);
                            // Można dodać dodatkowe konsekwencje braku opłaty
                        }
                    }
                    break;
            }
        }

        private static int CalculateRent(Structure structure)
        {
            if (structure == null) return 0;

            switch (structure.Type)
            {
                case StructureType.SpacePort: return 100;
                case StructureType.Outpost: return 150;
                case StructureType.Habitat: return 200;
                case StructureType.Colony: return 300;
                case StructureType.GalacticHotel: return 500;
                case StructureType.Farm: return 50 * structure.Level;
                case StructureType.Mine: return 100 * structure.Level;
                default: return 0;
            }
        }
    }
}
