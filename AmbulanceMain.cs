using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ambulance
{
    class AmbulanceMain
    {
        static void Main(string[] args)
        {
            City city = new City(10, 2, 2);
            city.AddRoad(0, 1, 3);
            city.AddRoad(0, 4, 3);
            city.AddRoad(1, 4, 3);
            city.AddRoad(1, 5, 3);
            city.AddRoad(4, 5, 3);
            city.AddRoad(5, 2, 3);
            city.AddRoad(5, 6, 3);
            city.AddRoad(9, 4, 3);
            city.AddRoad(8, 5, 3);
            city.AddRoad(4, 8, 3);
            city.AddRoad(7, 3, 3);
            city.AddRoad(3, 8, 3);
            city.AddRoad(9, 8, 3);
            city.AddHospital(6);
            city.AddHospital(0);
            city.AddAmbulance(1);
            city.AddAmbulance(9);
            city.DivideByAmbulance();
            city.DivideByHospitals();
            List<int> ar1 = city.SearchNearestAmbulance(3);
            List<int> hr1 = city.SearchNearestHospital(3);
            foreach (int item in ar1)
                Console.WriteLine(item);
            foreach (int item in hr1)
                Console.WriteLine(item);
            Console.ReadLine();

        }
    }
}
