using GalacticMonopoly.Core.Game;
using GalacticMonopoly.Core.Models;
using GalacticMonopoly.Core.Rules;
using GalacticMonopoly.Core.Enums;
using System.Collections.Generic;
using System.Linq;

namespace GalacticMonopoly.UI.Views
{
    public class PlayerAssetsViewModel
    {
        public Player Player { get; }
        public List<PlanetSystemView> Systems { get; }

        public PlayerAssetsViewModel(Player player, GameState state, bool isCurrentPlayerTurn, bool hasUpgradedThisTurn)
        {
            Player = player;
            Systems = new List<PlanetSystemView>();

            foreach (var sys in state.GalaxyMap.Systems)
            {
                var planetsInSystem = sys.Planets.Where(p => p.Owner == player).ToList();
                if (planetsInSystem.Count == 0) continue;

                var systemView = new PlanetSystemView
                {
                    Name = sys.Name,
                    Planets = new List<PlanetView>(),
                    SystemStructures = new List<string>(),
                    SystemRef = sys
                };

                foreach (var planet in planetsInSystem)
                {
                    var structure = planet.structure;
                    // Rozwidlenie portu kosmicznego
                    bool isSpacePort = structure != null && structure.Type == StructureType.SpacePort;

                    systemView.Planets.Add(new PlanetView
                    {
                        Name = planet.Name,
                        ImagePath = $"/Images/{planet.Name}.png",
                        Structures = structure != null
                            ? new List<string> { $"{structure.Type} (poziom {structure.Level})" }
                            : new List<string> { "Brak budowli" },
                        // Dla portu kosmicznego – specjalne przyciski
                        CanUpgradeToFarm = isCurrentPlayerTurn && !hasUpgradedThisTurn && isSpacePort,
                        CanUpgradeToMine = isCurrentPlayerTurn && !hasUpgradedThisTurn && isSpacePort,
                        CanUpgradeToOutpost = isCurrentPlayerTurn && !hasUpgradedThisTurn && isSpacePort,
                        // Standardowy przycisk Ulepsz
                        CanUpgrade = isCurrentPlayerTurn && !hasUpgradedThisTurn && structure != null
                                     && !isSpacePort // port ma tylko 3 rozgałęzienia, nie standardowy upgrade
                                     && UpgradeRules.CanUpgrade(structure, player, sys),
                        PlanetRef = planet
                    });
                }

                // Pokazuj istniejące struktury systemowe (np. stocznia)
                if (sys.structure != null)
                {
                    systemView.SystemStructures.Add($"{sys.structure.Type}");
                }

                // Przyciski do budowy systemowych (stocznia, kopalnia asteroid)
                systemView.CanBuildShipyard = isCurrentPlayerTurn && !hasUpgradedThisTurn &&
                    UpgradeRules.CanBuildSystemStructure(sys, StructureType.GalacticShipyard, player);
                systemView.CanBuildAsteroidMine = isCurrentPlayerTurn && !hasUpgradedThisTurn &&
                    UpgradeRules.CanBuildSystemStructure(sys, StructureType.AsteroidBeltMine, player);

                Systems.Add(systemView);
            }
        }
    }

    public class PlanetSystemView
    {
        public string Name { get; set; }
        public List<PlanetView> Planets { get; set; }
        public List<string> SystemStructures { get; set; }
        public bool CanBuildShipyard { get; set; }
        public bool CanBuildAsteroidMine { get; set; }
        public SystemPlanet SystemRef { get; set; }
    }

    public class PlanetView
    {
        public string Name { get; set; }
        public string ImagePath { get; set; }
        public List<string> Structures { get; set; }
        public bool CanUpgrade { get; set; }
        public bool CanUpgradeToFarm { get; set; }
        public bool CanUpgradeToMine { get; set; }
        public bool CanUpgradeToOutpost { get; set; }
        public Planet PlanetRef { get; set; }
    }
}
