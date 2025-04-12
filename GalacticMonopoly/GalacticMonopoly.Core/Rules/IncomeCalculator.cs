using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalacticMonopoly.Core.Game;

namespace GalacticMonopoly.Core.Rules
{
    public class IncomeCalculator
    {
        public static int Calculate(Player player)
        {
            int income = 0;

            foreach (var planet in player.OwnedPlanets)
            {
                if (planet.structure == null) continue;
                switch (planet.structure.Type)
                {
                    case Enums.StructureType.Farm:
                        income += 50 * planet.structure.Level;
                        break;
                    case Enums.StructureType.Mine:
                        income += 100 * planet.structure.Level;
                        break;
                    case Enums.StructureType.Outpost:
                        income += 50;
                        break;
                    case Enums.StructureType.Habitat:
                        income += 100;
                        break;
                    case Enums.StructureType.Colony:
                        income += 200;
                        break;
                    case Enums.StructureType.GalacticHotel:
                        income += 300;
                        break;
                }

            foreach (var system in player.OwnedSystemPlanets)
                {
                    switch (system.structure.Type)
                    {
                        case Enums.StructureType.PlanetaryHotelNetwork:
                            income += 500;
                            break;
                        case Enums.StructureType.GalacticShipyard:
                            income += 1000;
                            break;
                        case Enums.StructureType.AsteroidBeltMine:
                            income += 200 * system.structure.Level;
                            break;
                    }
                }
            }

            return income;
        }
    }
}
