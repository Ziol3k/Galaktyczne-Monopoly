using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalacticMonopoly.Core.Enums;
using GalacticMonopoly.Core.Services;

namespace GalacticMonopoly.Core.Models
{
    public class Structure
    {
        public StructureType Type { get; set; }
        public int Level { get; set; }
        public Planet Planet { get; set; }

        public Structure(StructureType type, Planet planet)
        {
            Type = type;
            Level = 1;
            Planet = planet;
            GameEventLogger.LogStructureBuilt(planet.Owner, planet, type);
        }

        public void Upgrade()
        {
            Level++;
            Planet.Owner.Pay(GetUpgradeCost());
            GameEventLogger.LogStructureUpgrade(Planet.Owner, this);
        }

        public void Downgrade()
        {
            if (Level > 1)
            {
                Level--;
                GameEventLogger.LogStructureDowngrade(Planet.Owner, this);
            }
        }

        public int GetUpgradeCost()
        {
            return (int)Type * Level * 100;
        }
    }
}

