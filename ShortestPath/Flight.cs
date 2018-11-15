using System.Collections.Generic;
using System.Linq;

namespace Jubin.ShortestPath
{
    public class Flight
    {
        public string FlightNumber { get; set; }
        public string FromAirportCode { get; set; }
        public string ToAirportCode { get; set; }
        public float Distance { get; set; }
        public double FlightDuration { get; set; }



        public static List<Flight> LoadFlights()
        {
            return new List<Flight>()
            {
                new Flight() { FlightNumber= "001", FromAirportCode= "ATL",  ToAirportCode= "ORD", Distance= 1000, FlightDuration= 2.5 },

                new Flight() { FlightNumber= "002", FromAirportCode= "OAK",  ToAirportCode= "SAN", Distance= 1100, FlightDuration= 2.6 },

                new Flight() { FlightNumber= "003", FromAirportCode= "OAK",  ToAirportCode= "ATL", Distance= 1100, FlightDuration= 2.6 },

                new Flight() { FlightNumber= "004", FromAirportCode= "ATL",  ToAirportCode= "MDW", Distance= 1200, FlightDuration= 2.7 },

                new Flight() { FlightNumber= "005", FromAirportCode= "MDW",  ToAirportCode= "ORD", Distance= 1300, FlightDuration= 2.8 },

                new Flight() { FlightNumber= "006", FromAirportCode= "BOI",  ToAirportCode= "MDW", Distance= 1400, FlightDuration= 2.9 },

                new Flight() { FlightNumber= "007", FromAirportCode= "ORD",  ToAirportCode= "ATL", Distance= 1500, FlightDuration= 3.0 },

                new Flight() { FlightNumber= "008", FromAirportCode= "ATL",  ToAirportCode= "COS", Distance= 1600, FlightDuration= 3.1 },

                new Flight() { FlightNumber= "009", FromAirportCode= "COS",  ToAirportCode= "OAK", Distance= 1700, FlightDuration= 3.2 },

                new Flight() { FlightNumber= "010", FromAirportCode= "COS",  ToAirportCode= "BOI", Distance= 1800, FlightDuration= 3.3 },

            };
        }

        public static List<Flight> FindRouteWithLeastHops(List<Flight> flights, string from, string to)
        {
            var noResults = new List<Flight>();

            const int MAXINT = int.MaxValue;

            var allSourceAirports = flights.Select(f => f.FromAirportCode);
            var allDestAirports = flights.Select(f => f.ToAirportCode);

            if (from == to || !allSourceAirports.Contains(from) || !allDestAirports.Contains(to))
                return noResults;

            List<string> allAirports = allSourceAirports.Union(allDestAirports).Distinct().ToList();

            Dictionary<string, int> Hops = new Dictionary<string, int>();
            allAirports.ForEach(a => Hops.Add(a, MAXINT));
            Hops[from] = 0;

            List<string> airportsToProcess = new List<string>();
            airportsToProcess.AddRange(allAirports);

            Dictionary<string, string> legToAirport = new Dictionary<string, string>();

            bool pathFound = false;

            while (true)
            {
                var currentAirport = airportsToProcess.OrderBy(a => Hops[a]).First();
                if (currentAirport == to)
                {
                    pathFound = true;
                    break;
                }

                airportsToProcess.Remove(currentAirport);

                foreach (var directAirport in flights.Where(f => f.FromAirportCode == currentAirport).Select(f => f.ToAirportCode))
                {
                    var hopsThruThisRoute = Hops[currentAirport] + 1;
                    if (hopsThruThisRoute < Hops[directAirport])
                    {
                        Hops[directAirport] = hopsThruThisRoute;
                        legToAirport[directAirport] = currentAirport;
                    }

                }

                if (!airportsToProcess.Any()) break;
            }

            if (!pathFound) return noResults;

            Stack<Flight> connectingAirportsList = new Stack<Flight>();

            var temp = to;
            while (legToAirport.ContainsKey(temp) && legToAirport[temp] != null)
            {
                connectingAirportsList.Push(flights.Where(f => f.FromAirportCode == legToAirport[temp] && f.ToAirportCode == temp).First());
                temp = legToAirport[temp];
            }

            return connectingAirportsList.ToList();
        }

        
        public override string ToString()
        {
            return string.Format("{0} -> {1} to {2}", FlightNumber, FromAirportCode, ToAirportCode);
        }
    }
}



