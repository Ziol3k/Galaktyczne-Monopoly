using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalacticMonopoly.Core.Enums
{
    public enum CardType
    {
        PirateAttack,       // atak piratów - strata dwwwóch kolejek - możliwość zapłaty okupu lub karta obrony
        PirateDefense,      // obrona przed piratami
        GalacticTicket,     // bilet galaktycvzny - można się przenieść na wybrrane pole  przystanku koleii
        PropertyTax,        // podatek od nieruchommości - pobranie pewneggo procentu od wartoci nieruchomości
        LotteryWin,         // wygrana w loterii
        EngineFailure,      // awaria silnika - strata kolejki + opłata za cholowanie
        ShipyardMalfunction // awaria stoczni - konieczność zaapłaty za usunięcie awarii lub utrata stoczni
    }
}
