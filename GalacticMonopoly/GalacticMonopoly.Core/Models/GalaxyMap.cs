using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GalacticMonopoly.Core.Models
{
    public class GalaxyMap
    {
        public List<Field> Fields { get; set; } = new List<Field>();
        public List<SystemPlanet> Systems { get; set; } = new List<SystemPlanet>();

        public void AddField(Field field)
        {
            Fields.Add(field);
        }

        public void RemoveField(Field field)
        {
            Fields.Remove(field);
        }
    }
}

