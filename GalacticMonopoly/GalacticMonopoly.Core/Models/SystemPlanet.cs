using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalacticMonopoly.Core.Game;
using GalacticMonopoly.Core.Services;

namespace GalacticMonopoly.Core.Models
{
    public class SystemPlanet
    {
        public string Name { get; set; }
        public List<Planet> Planets { get; set; } = new List<Planet>();
        public Player? Owner { get; set; }
        public Structure structure { get; set; }

        public SystemPlanet(string name)
        {
            Name = name;
            Owner = null;
        }

        public void AddPlanet(Planet planet)
        {
            if (planet != null && !Planets.Contains(planet))
            {
                Planets.Add(planet);
                planet.System = this; // USTAWIAMY DWUSTRONNĄ RELACJĘ!
            }
        }

        public void RemovePlanet(Planet planet)
        {
            if (planet != null && Planets.Contains(planet))
            {
                Planets.Remove(planet);
            }
        }

        public bool SetOwner(Player player)
        {
            if (player != null)
            {
                for (int i = 0; i < Planets.Count; i++)
                {
                    if (!(Planets[i].Owner == player))
                    {
                        return false;
                    }
                }
                Owner = player;
                Owner.AddSystemPlanet(this);
                GameEventLogger.LogSystemOwnership(player, this);
            }
            return true;
        }

        public void SetStructure(Structure structure)
        {
            this.structure = structure;
        }

        public void RemoveOwner()
        {
            Owner = null;
        }

        public void RemoveStructure()
        {
            structure = null;
        }
    }
}
