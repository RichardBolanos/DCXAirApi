using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace DCXAirApi.Domain
{
    public class Graph
    {
        private Dictionary<string, List<Flight>> adjacencyList;

        public Graph(List<Flight> flights)
        {
            adjacencyList = new Dictionary<string, List<Flight>>();
            foreach (var flight in flights)
            {
                if (!adjacencyList.ContainsKey(flight.Origin))
                {
                    adjacencyList[flight.Origin] = new List<Flight>();
                }
                adjacencyList[flight.Origin].Add(flight);
            }
        }

        public List<Flight> Dijkstra(string start, string end)
        {
            var distances = new Dictionary<string, (double cost, Flight previous)>();
            var visited = new HashSet<string>();
            var queue = new PriorityQueue<(string, double)>((a, b) => a.Item2.CompareTo(b.Item2));
            var path = new Dictionary<string, Flight>();

            foreach (var vertex in adjacencyList.Keys)
            {
                distances[vertex] = (double.MaxValue, null);
            }

            distances[start] = (0, null);
            queue.Enqueue((start, 0));

            while (queue.Count > 0)
            {
                var (current, distance) = queue.Dequeue();

                if (visited.Contains(current))
                {
                    continue;
                }

                visited.Add(current);
                try
                {
                    foreach (var flight in adjacencyList[current])
                    {
                        var newDistance = distance + flight.Price;
                        if (newDistance < distances[flight.Destination].cost)
                        {
                            distances[flight.Destination] = (newDistance, flight);
                            queue.Enqueue((flight.Destination, newDistance));
                            path[flight.Destination] = flight;
                        }
                    }
                }
                catch
                {
                    return new List<Flight>();
                }
            }

            // Reconstruct the path
            var flightPath = new List<Flight>();

            try
            {
                var currentFlight = path[end];
                while (currentFlight != null)
                {
                    flightPath.Insert(0, currentFlight);
                    currentFlight = path.ContainsKey(currentFlight.Origin) ? path[currentFlight.Origin] : null;
                }
            }
            catch {
                return new List<Flight>();
            }
            return flightPath;
        }
    }

    public class PriorityQueue<T>
    {
        private List<T> data;
        private Func<T, T, int> comparer;

        public PriorityQueue(Func<T, T, int> comparer)
        {
            data = new List<T>();
            this.comparer = comparer;
        }

        public int Count => data.Count;

        public void Enqueue(T item)
        {
            data.Add(item);
            int ci = data.Count - 1; // child index; start at end
            while (ci > 0)
            {
                int pi = (ci - 1) / 2; // parent index
                if (comparer(data[ci], data[pi]) >= 0) break; // child item is larger than (or equal) parent so we're done
                T tmp = data[ci]; data[ci] = data[pi]; data[pi] = tmp;
                ci = pi;
            }
        }

        public T Dequeue()
        {
            // assumes pq is not empty; up to calling code
            int li = data.Count - 1; // last index (before removal)
            T frontItem = data[0];   // fetch the front
            data[0] = data[li];
            data.RemoveAt(li);

            --li; // last index (after removal)
            int pi = 0; // parent index. start at front of pq
            while (true)
            {
                int ci = pi * 2 + 1; // left child index of parent
                if (ci > li) break;  // no children so done
                int rc = ci + 1;     // right child
                if (rc <= li && comparer(data[rc], data[ci]) < 0) // if there is a rc (ci + 1), and it is smaller than left child, use the rc instead
                    ci = rc;
                if (comparer(data[pi], data[ci]) <= 0) break; // parent is smaller than (or equal to) smallest child so done
                T tmp = data[pi]; data[pi] = data[ci]; data[ci] = tmp; // swap parent and child
                pi = ci;
            }
            return frontItem;
        }
    }
}
