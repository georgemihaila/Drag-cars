using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Drag_cars;

namespace Extensions
{
    public static class Main
    {
        public static int NumberOfCarsThatHaveFinishedTheRace(this GeneticCar[] cars)
        {
            int c = 0;
            for (int i = 0; i < cars.Length; i++)
                if (cars[i].ReachedDestination) c++;
            return c;
        }
    }
}
