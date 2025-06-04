using GalacticMonopoly.Core.Enums;
using GalacticMonopoly.Core.Models;
using System.Collections.Generic;

namespace GalacticMonopoly.Core.Factories
{
    public static class DefaultMapFactory
    {
        public static GalaxyMap Create()
        {
            var map = new GalaxyMap();

            var systemsData = new List<(string name, string[] planets)>
            {
                ("Zyphor",      new[] { "Varkos-9", "Eltar" }),
                ("Drakthar",    new[] { "Xyros-7", "Morvex" }),
                ("Soltharion",  new[] { "Nythos", "Vezzar" }),
                ("Tarvex",      new[] { "Krelos", "Omegara" }),
                ("Veldros",     new[] { "Ymiris", "Zarkon" }),
                ("Quorix",      new[] { "Draxon", "Felmoria" }),
                ("Aetheris",    new[] { "Typhar", "Lunaris" }),
                ("Sythar",      new[] { "Exanor", "Velthos" }),
            };

            FieldType[] doubleFields = { FieldType.SingularZone, FieldType.PirateAttack };
            FieldType[] singleFields = { FieldType.GalacticTrainStop };

            int planetPrice = 1000;
            int planetPriceStep = 100;
            var systemList = new List<SystemPlanet>();

            // START
            map.AddField(new Field("Start", FieldType.Start));

            for (int i = 0; i < systemsData.Count; i++)
            {
                // SYSTEM + PLANETY
                var (systemName, planetNames) = systemsData[i];
                var sys = new SystemPlanet(systemName);
                foreach (var planetName in planetNames)
                {
                    var planet = new Planet(planetName, planetPrice);
                    planetPrice += planetPriceStep;
                    sys.AddPlanet(planet); // automatycznie ustawia planet.System
                    map.AddField(new Field(planet.Name, FieldType.Planet, planet));
                }
                systemList.Add(sys);

                // Pola dodatkowe po układzie
                if (i == 0 || i == 2 || i == 4 || i == 6)
                {
                    // 2 pola (szansa, piraci, itp.)
                    map.AddField(new Field(doubleFields[(i / 2) % doubleFields.Length].ToString(), doubleFields[(i / 2) % doubleFields.Length]));
                    map.AddField(new Field(doubleFields[((i / 2) + 1) % doubleFields.Length].ToString(), doubleFields[((i / 2) + 1) % doubleFields.Length]));
                }
                else if (i == 1 || i == 3 || i == 5)
                {
                    // 1 pole (czarna dziura, pociąg, tunel)
                    map.AddField(new Field(singleFields[((i - 1) / 2) % singleFields.Length].ToString(), singleFields[((i - 1) / 2) % singleFields.Length]));
                }
                // Po systemie 7 (i==6) -> 2 pola, po 8 (i==7) -> nic, bo zaraz start
            }

            map.Systems = systemList;

            return map;
        }
    }
}
