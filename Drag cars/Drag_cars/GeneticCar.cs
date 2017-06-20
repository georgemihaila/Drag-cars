using System;
using Microsoft.Xna.Framework;

namespace Drag_cars
{
    public class GeneticCar
    {
        public GeneticCar(int randomSeed, Vector2 startingPosition)
        {
            random = new Random(randomSeed);
            Weight = random.Next(minWeight, maxWeight);
            Power = random.Next(minPower, maxPower);
            AirDrag = (float)random.Next(minAirDrag, maxAirDrag);
            MaxAcceleration = (float)random.Next(minAcceleration, maxAcceleration);
            Speed = 0;
            Acceleration = 0;
            Position = startingPosition;
        }

        public GeneticCar(float Weight, float Power, float AirDrag, Vector2 startingPosition)
        {
            this.Weight = Weight;
            this.Power = Power;
            this.AirDrag = AirDrag;
            Speed = 0;
            Acceleration = 0;
            Position = startingPosition;
        }
        private const int minWeight = 500;
        private const int maxWeight = 15000;
        private const int minPower = 100;
        private const int maxPower = 2500;
        private const int minAirDrag = 0;
        private const int maxAirDrag = 10;
        private const int minAcceleration = 1;
        private const int maxAcceleration = 5;
        private Random random;
        public bool ReachedDestination = false;
        public double TotalTime = double.PositiveInfinity;
        private float weight;
        public float Weight
        {
            get
            {
                return weight;
            }
            set
            {
                if (value >= minWeight && value <= maxWeight) weight = value;
            }
        }
        private float power;
        public float Power
        {
            get
            {
                return power;
            }
            set
            {
                if (value >= minPower && value <= maxPower) power = value;
            }
        }
        private float airDrag;
        public float AirDrag
        {
            get
            {
                return airDrag;
            }
            set
            {
                if (value >= minAirDrag && value <= maxAirDrag) airDrag = value;
            }
        }
        private float maxAcc = 0;
        public float MaxAcceleration
        {
            get
            {
                return maxAcc;
            }
            set
            {
                if (value >= minAcceleration && value <= maxAcceleration) maxAcc = value;
            }
        }
        public float Speed;
        private float acc = 0;
        private const float GravityConstant = 10f;
        private const float Friction = 0.4f;
        public float Acceleration
        {
            get
            {
                float x = (Power * 736 * 2 / Speed - AirDrag * 1 * Speed * Speed - Friction * GravityConstant * Weight) / (Weight * GravityConstant);
                return (x > MaxAcceleration) ? (MaxAcceleration) : x; 
            }
            set
            {
                acc = value;
            }
        }
        public Vector2 Position;

        public override string ToString()
        {
            return string.Format("Weight: {0} kg\nPower: {1} HP\nAir drag coefficient: {2}\nSpeed: {3} m/s ({4} km/h)\nAcceleration: {5} m/s^2", Weight, Power, AirDrag, Speed, Speed * 3.6, Acceleration);
        }
    }
}
