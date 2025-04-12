using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalacticMonopoly.Core.Enums
{
    public enum PirateResponseType
    {
        PayRansom,          // Gracz płaci okup
        UseDefenseCard,     // Gracz używa karty obrony przed piratami jeśli taką posiada
        TakeLoss            // Gracz traci dwie tury
    }
}
