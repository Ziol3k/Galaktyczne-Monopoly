using System;

namespace GalacticMonopoly.Core.Utils
{
    public static class Dice
    {
        private static readonly Random _random = new Random();

        public static int Roll()
        {
            return _random.Next(1, 7); 
        }
    }
}
