using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalacticMonopoly.Core.Enums
{
    public enum FieldType
    {
        Start,              // Pole startowe, po przejściu przez pole start gracz dosttaje kredyty
        Planet,             // Pole z planetą – gracz może ją zająć, rozbudowywać i czerpać z niej zyski
        Space,              // Puste pole w przestrzeni kosmicznej – brak efektu po wylądowaniu
        //Wormhole,           // Tunel czasoprzestrzenny – może teleportować gracza w inne miejsce na planszy
        //BlackHole,          // Czarna dziura – możliwa strata tury lub zasobów (można dodać efekt specjalny w przyszłości)
        SingularZone,       // Obszar osobliwości – gracz dobiera kartę szansy (losowe zdarzenia)
        PirateAttack,       // Atak piratów – strata 2 kolejek chyba że gracz ma kartę obrony lub zapłaci okup
        GalacticTrainStop   // Przystanek kolei galaktycznej – jeśli gracz ma bilet, może teleportować się w inne miejsce
    }
}
