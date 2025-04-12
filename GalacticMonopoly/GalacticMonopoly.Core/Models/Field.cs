using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalacticMonopoly.Core.Game;
using GalacticMonopoly.Core.Enums;

namespace GalacticMonopoly.Core.Models
{
    public class Field
    {
        public string Name { get; set; }
        public FieldType Type { get; set; }
        public Planet Planet { get; set; }
        public Field(string name, FieldType type, Planet? planet = null)
        {
            Name = name;
            Type = type;
            Planet = planet;
        }

        public void LandOn(Player player)
        {
            
        }
    }
}

