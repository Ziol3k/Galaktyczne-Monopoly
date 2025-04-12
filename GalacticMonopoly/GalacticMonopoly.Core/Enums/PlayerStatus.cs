using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalacticMonopoly.Core.Enums
{
    public enum PlayerStatus
    {
        Active,                 // Gracz aktywny – może wykonywać ruch i podejmować akcje w turze
        Waiting,                // Gracz czeka na swoją kolej (nie jest obecnie aktywny)
        SkippedTurn,            // Gracz zdecydował się pominąć turę – może to zrobić maksymalnie 2 razy z rzędu
        LostTurnDueToPirates,   // Gracz stracił turę w wyniku ataku piratów (brak karty obrony i brak zapłaty okupu)
        Lost                    // Gracz przegrrał z powodu braku pieniędzy
    }
}

