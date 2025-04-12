using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalacticMonopoly.Core.Enums;
using GalacticMonopoly.Core.Game;

namespace GalacticMonopoly.Core.Models
{
    public class Card
    {
        public string Description { get; set; }
        public CardType Type { get; set; }
        public int EffectValue { get; set; }

        public Card(string description, CardType type, int effectValue)
        {
            Description = description;
            Type = type;
            EffectValue = effectValue;
        }

    }
}

