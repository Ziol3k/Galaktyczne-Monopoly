using System;
using GalacticMonopoly.Core.Enums;
using GalacticMonopoly.Core.Services;

namespace GalacticMonopoly.Core.Models
{
    public class Structure
    {
        public StructureType Type { get; set; }
        public int Level { get; set; }

        // Struktura może być przypisana do planety lub systemu
        public Planet Planet { get; set; }
        public SystemPlanet System { get; set; }

        // Konstruktor planetarny
        public Structure(StructureType type, Planet planet)
        {
            Type = type;
            Level = 1;
            Planet = planet;
            System = null;
            GameEventLogger.LogStructureBuilt(planet?.Owner, planet, type);
        }

        // Konstruktor systemowy
        public Structure(StructureType type, SystemPlanet system)
        {
            Type = type;
            Level = 1;
            Planet = null;
            System = system;
            GameEventLogger.LogSystemStructureBuilt(system?.Owner, system, type);
        }

        public void Upgrade()
        {
            Level++;
            var owner = Planet != null ? Planet.Owner : System?.Owner;
            if (owner != null)
                owner.Pay(GetUpgradeCost());
            GameEventLogger.LogStructureUpgrade(owner, this);
        }

        public void Downgrade()
        {
            if (Level > 1)
            {
                Level--;
                var owner = Planet != null ? Planet.Owner : System?.Owner;
                GameEventLogger.LogStructureDowngrade(owner, this);
            }
        }

        public int GetUpgradeCost()
        {
            return (int)Type * Level * 100;
        }
    }
}
