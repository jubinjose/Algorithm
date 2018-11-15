using Jubin.ShortestPath;
using Xunit;

namespace Algorithm.Test
{
    public class UnitTest
    {
        [Fact]
        public void find_route_with_least_hops()
        {
            var flights = Flight.LoadFlights();
            var result = Flight.FindRouteWithLeastHops(flights, "ATL", "SAN");
            Assert.Equal("ATL", result[0].FromAirportCode);
            Assert.Equal("COS", result[1].FromAirportCode);
            Assert.Equal("OAK", result[2].FromAirportCode);
            Assert.Equal("SAN", result[2].ToAirportCode);

        }
    }
}
