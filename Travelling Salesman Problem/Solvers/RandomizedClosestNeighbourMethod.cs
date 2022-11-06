using System.Collections.Generic;
using System.Linq;
using Common;
using EA.BaseProblem;

namespace Travelling_Salesman_Problem.Solvers;

public sealed class RandomizedClosestNeighbourMethod : IProblemSolver<List<int>, Tsp>
{
	public List<int> FindSolution(Tsp problem)
	{
		var distances = problem.Distances;
		var firstCity = Randomness.GetInt(distances.CityCount);

		var route = new List<int> { firstCity };

		var unvisitedCities = Enumerable.Range(0, distances.CityCount).ToList();
		unvisitedCities.Remove(firstCity);

		while (unvisitedCities.Count > 0)
		{
			var lastCity = route.Last();
			var nextCity = NextCity(distances, lastCity, unvisitedCities);

			route.Add(nextCity);

			unvisitedCities.Remove(nextCity);
		}

		return route;
	}

	private static int NextCity(DistanceMatrix distances, int currentCity, List<int> unvisitedCities)
	{
		var weights = new List<double>();
		foreach (var unvisitedCity in unvisitedCities)
		{
			weights.Add(1 / (double)distances[currentCity, unvisitedCity]);
		}

		return Roulette.Spin(unvisitedCities, weights);
	}
}
