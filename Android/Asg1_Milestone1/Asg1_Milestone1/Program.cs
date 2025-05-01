using System;
using System.Collections.Generic;
using System.Linq;
using static System.Console;

namespace Asg1_Milestone1
{
    class Bike
    {
        private string _frame;
        private string _wheels;
        private string _seat;
        private int _speed;

        public int Speed
        {
            get { return _speed; }
        }

        public Bike()
        {
            _frame = "";
            _wheels = "";
            _seat = "";
            _speed = 0;
        }

        public void AddPartToBike(string part_, string name_)
        {
            switch (part_)
            {
                case "F":
                    _frame = name_;
                    break;
                case "W":
                    _wheels = name_;
                    break;
                case "S":
                    _seat = name_;
                    break;
            }
        }

        public void RemovePartFromBike(string part_)
        {
            switch (part_)
            {
                case "F":
                    _frame = "";
                    break;
                case "W":
                    _wheels = "";
                    break;
                case "S":
                    _seat = "";
                    break;
            }
        }

        public bool IsItComplete()
        {
            if (_frame.Length != 0 && _wheels.Length != 0 && _seat.Length != 0)
                return true;
            return false;
        }

        public string DisplayBike()
        {
            return string.Format("Bike Frame: {0}, Bike Seat: {1}, Bike Wheels: {2}", _frame.Length != 0 ? _frame : "NONE", _seat.Length != 0 ? _seat : "NONE", _wheels.Length != 0 ? _wheels : "NONE");
        }

        public void Pedal()
        {
            _speed += 2;
        }

        public void Brake()
        {
            if (_speed <= 2)
                _speed = 0;
            else
                _speed -= 2;
        }

        public string RingBell()
        {
            return "Ring! Ring!";
        }

        public override string ToString()
        {
            return string.Format("A bike with a {0} frame, a {1} seat and {2} wheels has been created", _frame, _seat, _wheels);
        }

    }

    class HelmetsToSell
    {
        public string Desc { get; set; }
        public double Price { get; set; }
        public string Colour { get; set; }
        public int NumberInStock { get; set; }
        public int NumberOnOrder { get; set; }
    }

    class BikesToSell
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string Colour { get; set; }
        public int NumberInStock { get; set; }
        public int NumberOnOrder { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Bike myBike = new Bike();
            string part;
            string response;


            ////////
            ////////
            //PART 3
            #region part3
            List<BikesToSell> listBikes = new List<BikesToSell>
            {
                new BikesToSell {Type="Mountain Bike", Name="Plasma Team Issue", Price=1300, Colour="red",NumberInStock=10,NumberOnOrder=0},
                new BikesToSell {Type="Mountain Bike", Name="Evolution 26", Price=1125, Colour="blue",NumberInStock=8,NumberOnOrder=0},
                new BikesToSell {Type="Mountain Bike", Name="OIZ M-Team", Price=806, Colour="black",NumberInStock=4,NumberOnOrder=3},
                new BikesToSell {Type="Road Bike", Name="Occam TR123", Price=2300, Colour="grey",NumberInStock=2,NumberOnOrder=2},
                new BikesToSell {Type="Road Bike", Name="Occam TR461", Price=1600, Colour="orange",NumberInStock=8,NumberOnOrder=0},
                new BikesToSell {Type="Road Bike", Name="Plasma Speedster", Price=2010, Colour="red",NumberInStock=8,NumberOnOrder=0},
                new BikesToSell {Type="Road Bike", Name="Look 765 Ultegra", Price=1930, Colour="black",NumberInStock=3,NumberOnOrder=5},
                new BikesToSell {Type="Road Bike", Name="Cirro Road Bike V5", Price=988, Colour="black",NumberInStock=3,NumberOnOrder=6},
                new BikesToSell {Type="Hybrid Bike", Name="Cross MX10", Price=1012, Colour="red",NumberInStock=8,NumberOnOrder=0},
                new BikesToSell {Type="Hybrid Bike", Name="Cross MX25", Price=950, Colour="blue",NumberInStock=7,NumberOnOrder=4},
                new BikesToSell {Type="Hybrid Bike", Name="OIZ Hybrid ", Price=840, Colour="black",NumberInStock=4,NumberOnOrder=10}
            };

            List<HelmetsToSell> listHelmets = new List<HelmetsToSell>
            {
                new HelmetsToSell {Desc="Lightning McQueen", Price=60, Colour="red",NumberInStock=20,NumberOnOrder=0},
                new HelmetsToSell {Desc="Royal", Price=40, Colour="blue",NumberInStock=13,NumberOnOrder=0},
                new HelmetsToSell {Desc="Neon 1", Price=30, Colour="yellow",NumberInStock=11,NumberOnOrder=0},
                new HelmetsToSell {Desc="Bright Series", Price=50, Colour="orange",NumberInStock=18,NumberOnOrder=0},
                new HelmetsToSell {Desc="Bright Series", Price=55, Colour="red",NumberInStock=6,NumberOnOrder=12},
                new HelmetsToSell {Desc="Standard", Price=70, Colour="red",NumberInStock=12,NumberOnOrder=0},
                new HelmetsToSell {Desc="Standard", Price=62, Colour="black",NumberInStock=11,NumberOnOrder=0},
                new HelmetsToSell {Desc="Standard", Price=38, Colour="grey",NumberInStock=1,NumberOnOrder=12},
            };

            var bikesForSale = from b in listBikes
                               orderby b.Name
                               select b;

            var cheapBikes = from b in listBikes
                             where b.Price < 1000
                             orderby b.Price
                             select b;

            var bikeTypes = from b in listBikes
                            group b by b.Type into bikeGroup
                            orderby bikeGroup.Key
                            select bikeGroup;

            var bikeAndHelmetCombos = from b in listBikes
                                      join h in listHelmets
                                      on b.Colour equals h.Colour
                                      select new { bike = b.Name, helmet = h.Desc };

            var avgBikeCost = (from b in listBikes
                               select b.Price).Average();

            WriteLine("\nBikes in alphabetical order:\n");
            foreach(BikesToSell bike in bikesForSale)
            {
                WriteLine($" - {bike.Name}");
            }

            WriteLine("\nBikes costing less than $1000\n");
            foreach (BikesToSell bike in cheapBikes)
            {
                WriteLine($" * {bike.Name} ({bike.Price:c2})");
            }

            WriteLine("\nBikes grouped by type:\n");
            foreach (var bikeGroup in bikeTypes)
            {
                WriteLine(bikeGroup.Key);
                foreach(BikesToSell bike in bikeGroup)
                {
                    WriteLine($"     {bike.Name}");
                }
            }

            WriteLine("\nBike and helmet combinations (for the same colour):\n");
            foreach (var combo in bikeAndHelmetCombos)
            {
                WriteLine($"{combo.bike} bike with {combo.helmet} helmet");
            }

            WriteLine($"\nAverage cost of the bikes for sale: {avgBikeCost:c2}");

            ReadKey();
            #endregion
        }
    }
}
