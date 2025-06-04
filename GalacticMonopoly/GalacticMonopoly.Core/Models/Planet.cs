using GalacticMonopoly.Core.Enums;
using GalacticMonopoly.Core.Game;
using GalacticMonopoly.Core.Services;

namespace GalacticMonopoly.Core.Models
{
    public class Planet
    {
        public string Name { get; set; }
        public int Price { get; set; }
        public bool IsOwned { get; set; }
        public Player Owner { get; set; }
        public Structure structure { get; set; }
        public SystemPlanet System { get; set; }

        public Planet(string name, int price)
        {
            Name = name;
            Price = price;
            IsOwned = false;
        }

        public bool Buy(Player player)
        {
            if (!IsOwned && player.Pay(Price))
            {
                IsOwned = true;
                Owner = player;
                player.AddPlanet(this);
                GameEventLogger.LogPlanetPurchase(player, this);
                SetStructure(new Structure(StructureType.SpacePort, this));
                return true;
            }
            return false;
        }

        public void SetStructure(Structure structure)
        {
            this.structure = structure;
        }
    }
}
