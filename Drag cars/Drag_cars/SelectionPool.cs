using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Drag_cars
{
    class SelectionPool
    {
        public SelectionPool(GeneticCar[] cars, int randomSeed)
        {
            this.cars = cars;
            this.randomSeed = randomSeed;
            random = new Random(randomSeed); 
        }

        GeneticCar[] cars;
        readonly int randomSeed;
        const int carWidth = 40;
        const int carHeight = 20;
        Vector2 startingPosition = new Vector2(0, 155 + carHeight);
        Random random;

        public GeneticCar[] Select()
        {
            GeneticCar[] selected = new GeneticCar[cars.Length];
            for (int i = 0; i < cars.Length; i++)
            {
                int index = i;
                for (int j = i + 1; j < cars.Length; j++)
                {
                    if (cars[index].TotalTime > cars[j].TotalTime)
                    {
                        index = j;
                    }
                }
                selected[i] = cars[index];
                selected[i].ReachedDestination = false;
                selected[i].Position = startingPosition;
                selected[i].TotalTime = double.PositiveInfinity;
                selected[i].Speed = 0;
            }
            for (int i = 0; i < selected.Length; i++)
            {
                if (i >= 4)
                {
                    selected[i] = new GeneticCar(random.Next(), startingPosition);
                    int indexOfSelectedCar = random.Next(4);
                    bool change = (random.Next(2) == 1) ? true : false;
                    if (change) selected[i].MaxAcceleration = (float)((random.NextDouble() * 80 + 50) / 100) * selected[indexOfSelectedCar].MaxAcceleration;
                    change = (random.Next(2) == 1) ? true : false;
                    if (change) selected[i].AirDrag = (float)((random.NextDouble() * 80 + 50) / 100) * selected[indexOfSelectedCar].AirDrag;
                    change = (random.Next(2) == 1) ? true : false;
                    if (change) selected[i].Power = (float)((random.NextDouble() * 80 + 50) / 100) * selected[indexOfSelectedCar].Power;
                    change = (random.Next(2) == 1) ? true : false;
                    if (change) selected[i].Weight = (float)((random.NextDouble() * 80 + 50) / 100) * selected[indexOfSelectedCar].Weight;
                }/*
                else
                {
                    bool change = (random.Next(5) == 1) ? true : false;
                    if (change) selected[i].MaxAcceleration = (float)((random.NextDouble() * 80 + 50) / 100) * selected[i].Acceleration;
                    change = (random.Next(5) == 1) ? true : false;
                    if (change) selected[i].AirDrag = (float)((random.NextDouble() * 80 + 50) / 100) * selected[i].AirDrag;
                    change = (random.Next(5) == 1) ? true : false;
                    if (change) selected[i].Power = (float)((random.NextDouble() * 80 + 50) / 100) * selected[i].Power;
                    change = (random.Next(5) == 1) ? true : false;
                    if (change) selected[i].Weight = (float)((random.NextDouble() * 80 + 50) / 100) * selected[i].Weight;
                }*/
            }
            int c = 0;
            for (int i = 0; i < selected.Length; i++)
            {
                selected[i].Position.Y = (i != 0) ? selected[i - 1].Position.Y + 35 : startingPosition.Y;
                if (++c == 3)
                {
                    c = 0;
                    selected[i].Position.Y += 10;
                }
            }
            return selected;
        }
        
    }
}
