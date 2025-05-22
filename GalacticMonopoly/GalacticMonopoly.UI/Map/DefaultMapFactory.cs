using System.Collections.Generic;
using GalacticMonopoly.Core.Enums;
using GalacticMonopoly.Core.Models;

namespace GalacticMonopoly.UI.Map
{
    public static class DefaultMapFactory
    {
        public static GalaxyMap Create()
        {
            var map = new GalaxyMap();

            // 1. Start
            map.AddField(new Field("Start", FieldType.Start));

            // 2. Układy planetarne
            var systems = new Dictionary<string, string[]>
            {
                ["Zyphor"] = new[] { "Varkos-9", "Eltar Prime" },
                ["Drakthar"] = new[] { "Xyros-7", "Morvex" },
                ["Soltharion"] = new[] { "Nythos", "Vezzar" },
                ["Tarvex"] = new[] { "Krelos", "Omegara" },
                ["Veldros"] = new[] { "Ymiris", "Zarkon" },
                ["Quorix"] = new[] { "Draxon", "Felmoria" },
                ["Aetheris"] = new[] { "Typhar", "Lunaris" },
                ["Sythar"] = new[] { "Exanor", "Velthos" },
            };

            foreach (var sys in systems)
            {
                foreach (var planetName in sys.Value)
                {
                    var planet = new Planet(planetName, 1000);
                    map.AddField(new Field(planetName, FieldType.Planet, planet));
                }
                // po każdym układzie wstawiamy jedno pole Space
                map.AddField(new Field("Space", FieldType.Space));
            }

            // 3. Inne pola losowo lub w sekwencji
            map.AddField(new Field("Wormhole", FieldType.Wormhole));
            map.AddField(new Field("Black Hole", FieldType.BlackHole));
            map.AddField(new Field("Singular Zone", FieldType.SingularZone));
            map.AddField(new Field("Pirate Attack", FieldType.PirateAttack));
            map.AddField(new Field("Train Stop", FieldType.GalacticTrainStop));

            return map;
        }
    }
}
