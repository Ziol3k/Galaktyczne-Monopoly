using System;
using System.Linq;
using GalacticMonopoly.Core.Enums;
using GalacticMonopoly.Core.Game;
using GalacticMonopoly.Core.Models;

namespace GalacticMonopoly.Core.Rules
{
    public static class UpgradeRules
    {
        public static bool CanUpgrade(Structure structure, Player owner, SystemPlanet owningSystem = null)
        {
            if (structure == null) return false;

            switch (structure.Type)
            {
                case StructureType.Farm:
                    return structure.Level < 5;
                case StructureType.Mine:
                    return structure.Level < 3;
                case StructureType.Outpost:
                case StructureType.Habitat:
                case StructureType.Colony:
                    return true;
                case StructureType.GalacticHotel:
                    // Można ulepszyć do sieci hoteli tylko jeśli gracz ma cały system
                    return owningSystem != null
                        && owningSystem.Owner == owner
                        && owningSystem.Planets.All(p => p.Owner == owner)
                        && owningSystem.structure == null;
                case StructureType.AsteroidBeltMine:
                    return structure.Level < 5 && owningSystem != null && owningSystem.Owner == owner;
                default:
                    return false;
            }
        }

        // --- Specjalny przypadek – SpacePort → wybór budowli ---
        public static Structure UpgradeSpacePortTo(Structure structure, StructureType targetType, Player owner)
        {
            if (structure == null) throw new ArgumentNullException(nameof(structure));
            if (structure.Type != StructureType.SpacePort)
                throw new InvalidOperationException("Można rozbudować tylko port kosmiczny.");

            if (targetType != StructureType.Outpost &&
                targetType != StructureType.Mine &&
                targetType != StructureType.Farm)
                throw new InvalidOperationException("Z portu kosmicznego można rozbudować tylko do: Posterunku, Kopalni lub Farmy.");

            int cost = 500;
            if (!owner.Pay(cost))
                throw new InvalidOperationException("Brak wystarczającej liczby kredytów.");

            // Przypisujemy strukturę do tej samej planety
            var newStructure = new Structure(targetType, structure.Planet);
            structure.Planet.SetStructure(newStructure);
            return newStructure;
        }

        public static Structure Upgrade(Structure structure, Player owner, SystemPlanet owningSystem = null)
        {
            if (structure == null) throw new ArgumentNullException(nameof(structure));
            if (!CanUpgrade(structure, owner, owningSystem))
                throw new InvalidOperationException("Nie można ulepszyć tej budowli.");

            switch (structure.Type)
            {
                case StructureType.Farm:
                case StructureType.Mine:
                case StructureType.AsteroidBeltMine:
                    structure.Upgrade();
                    return structure;
                case StructureType.Outpost:
                    {
                        int cost = 600;
                        if (!owner.Pay(cost))
                            throw new InvalidOperationException("Brak kredytów na rozbudowę!");
                        var newStructure = new Structure(StructureType.Habitat, structure.Planet);
                        structure.Planet.SetStructure(newStructure);
                        return newStructure;
                    }
                case StructureType.Habitat:
                    {
                        int cost = 800;
                        if (!owner.Pay(cost))
                            throw new InvalidOperationException("Brak kredytów na rozbudowę!");
                        var newStructure = new Structure(StructureType.Colony, structure.Planet);
                        structure.Planet.SetStructure(newStructure);
                        return newStructure;
                    }
                case StructureType.Colony:
                    {
                        int cost = 1200;
                        if (!owner.Pay(cost))
                            throw new InvalidOperationException("Brak kredytów na rozbudowę!");
                        var newStructure = new Structure(StructureType.GalacticHotel, structure.Planet);
                        structure.Planet.SetStructure(newStructure);
                        return newStructure;
                    }
                case StructureType.GalacticHotel:
                    {
                        if (owningSystem != null && owningSystem.Owner == owner)
                        {
                            int cost = 1600;
                            if (!owner.Pay(cost))
                                throw new InvalidOperationException("Brak kredytów na rozbudowę!");
                            var newStructure = new Structure(StructureType.PlanetaryHotelNetwork, structure.Planet);
                            structure.Planet.SetStructure(newStructure);
                            return newStructure;
                        }
                        else
                            throw new InvalidOperationException("Aby zbudować sieć hoteli, musisz mieć cały system!");
                    }
                default:
                    throw new InvalidOperationException("Nie można ulepszyć tej budowli.");
            }
        }

        // ==== SYSTEM PLANET STRUCTURES ====

        public static bool CanBuildSystemStructure(SystemPlanet sys, StructureType type, Player player)
        {
            if (sys == null || player == null) return false;
            bool ownsAllPlanets = sys.Planets.All(p => p.Owner == player);
            if (!ownsAllPlanets) return false;
            if (sys.structure != null) return false;

            if (type == StructureType.GalacticShipyard)
                return true;
            if (type == StructureType.AsteroidBeltMine)
                return true;

            return false;
        }

        public static void BuildSystemStructure(SystemPlanet sys, StructureType type, Player player)
        {
            if (!CanBuildSystemStructure(sys, type, player))
                throw new InvalidOperationException("Nie spełniasz warunków budowy tej struktury!");

            int cost = (type == StructureType.GalacticShipyard) ? 2000 : 1500;
            if (!player.Pay(cost))
                throw new InvalidOperationException("Brak wystarczających kredytów!");

            // Dodajemy strukturę do systemu
            sys.SetStructure(new Structure(type, sys));
        }
    }
}
