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
    public static class CardServices
    {
        private static readonly Random random = new();

        public static Card DrawCard(GameState state)
        {
            var cards = new List<Card>
            {
                new Card("Atak piratów!", CardType.PirateAttack, 0),
                new Card("Obrona przed piratami", CardType.PirateDefense, 0),
                new Card("Bilet galaktyczny", CardType.GalacticTicket, 0),
                new Card("Podatek od nieruchomości", CardType.PropertyTax, 10),
                new Card("Wygrałeś w loterii", CardType.LotteryWin, 500),
                new Card("Awaria silnika", CardType.EngineFailure, 100),
                new Card("Awaria stoczni", CardType.ShipyardMalfunction, 200)
            };
            

            return cards[random.Next(cards.Count)];
        }

        public static void ResolveCard(Player player, Card card, GameState state)
        {
            switch (card.Type)
            {
                case CardType.PirateAttack:
                    PirateServices.HandlePirateAttack(player, state);
                    break;
                case CardType.PirateDefense:
                    player.AddCard(card);
                    break;
                case CardType.GalacticTicket:
                    player.AddCard(card);
                    break;
                case CardType.PropertyTax:
                    int tax = (int)(player.OwnedPlanets.Sum(p => p.Price) * (card.EffectValue / 100.0));
                    player.Pay(tax);
                    break;
                case CardType.LotteryWin:
                    player.AddCredits(card.EffectValue);
                    break;
                case CardType.EngineFailure:
                    player.Status = PlayerStatus.SkippedTurn;
                    player.Pay(card.EffectValue);
                    break;
                case CardType.ShipyardMalfunction:
                    if (player.OwnedSystemPlanets.Any(s => s.structure?.Type == StructureType.GalacticShipyard))
                    {
                        if (player.Pay(card.EffectValue))
                        {
                            // zapłacono za naprawę
                        }
                        else
                        {
                            // utrata stoczni
                            var shipyard = player.OwnedSystemPlanets
                                .First(s => s.structure?.Type == StructureType.GalacticShipyard);
                            shipyard.RemoveStructure();
                        }
                    }
                    break;
            }

        }
    }
}
