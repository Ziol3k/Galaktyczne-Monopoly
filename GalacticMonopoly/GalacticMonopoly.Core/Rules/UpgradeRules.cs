using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalacticMonopoly.Core.Models;

namespace GalacticMonopoly.Core.Rules
{
    public class UpgradeRules
    {
        public static bool CanUpgrade(Structure structure)
        {
            switch (structure.Type)
            {
                case Enums.StructureType.Farm:
                    return structure.Level < 5;
                case Enums.StructureType.Mine:
                    return structure.Level < 3;
                case Enums.StructureType.AsteroidBeltMine:
                    return structure.Level < 5;
                default:
                    return false;
            }
        }

        public static Structure Upgrade(Structure structure)
        {
            if (CanUpgrade(structure))
            {
                Structure newStructure;
                switch (structure.Type)
                {
                    case Enums.StructureType.Farm:
                    case Enums.StructureType.Mine:
                    case Enums.StructureType.AsteroidBeltMine:
                        structure.Upgrade();
                        return structure;
                    case Enums.StructureType.Outpost:
                        newStructure = new Structure(Enums.StructureType.Habitat, structure.Planet);
                        return newStructure;
                    case Enums.StructureType.Habitat:
                        newStructure = new Structure(Enums.StructureType.Colony, structure.Planet);
                        return newStructure;
                    case Enums.StructureType.Colony:
                        newStructure = new Structure(Enums.StructureType.GalacticHotel, structure.Planet);
                        return newStructure;
                    default:
                        throw new InvalidOperationException("Cannot upgrade this structure.");
                }
            }
            else
            {
                throw new InvalidOperationException("Cannot upgrade this structure.");
            }
        }
    }
}
