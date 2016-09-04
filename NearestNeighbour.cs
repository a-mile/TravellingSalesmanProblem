using System.Collections.Generic;
using System.Linq;

namespace TSP
{
    public class NearestNeighbour
    {
        public NearestNeighbour()
        {
            usedVertices = new Stack<Vertex>();
            verticesStack = new Stack<Vertex>();
            shortestPath = new List<Vertex>();
            minDistance = 0;
        }

        public Graph graph { get; set; }
        public int minDistance { get; private set; }

        List<Vertex> shortestPath;
        Vertex startVertex;
        Stack<Vertex> usedVertices;
        Stack<Vertex> verticesStack;

        public List<Vertex> NearestNeighbourOptimization()
        {          
            foreach (Vertex v in graph.vertices)
            {
                startVertex = v;
                NearestNeighbourRecurring(startVertex);
                if (minDistance == 0 || distance < minDistance)
                {
                    minDistance = distance;
                    shortestPath.Clear();
                    shortestPath.AddRange(verticesStack.ToList());
                }
            }
          
            return shortestPath;
        }
        
        int counter = 0;
        int distance = 0;

        private bool NearestNeighbourRecurring(Vertex vertex)
        {
            counter++;
            usedVertices.Push(vertex);           
            verticesStack.Push(vertex);

            Edge nextEdge = null;

            foreach (Edge e in vertex.neighbors)
            {
                if (e.vertex2 == startVertex)
                {
                    if (counter == graph.vertices.Count)
                    {
                        verticesStack.Push(e.vertex2);
                        distance += e.distance;
                        return true;
                    }
                }

                if (!usedVertices.Contains(e.vertex2))
                {
                    if (nextEdge == null || nextEdge.distance > e.distance)
                    {
                        nextEdge = e;
                    }
                }
            }

            if (nextEdge != null)
            {
                if (NearestNeighbourRecurring(nextEdge.vertex2))
                {
                    distance += nextEdge.distance;
                    return true;
                }
            }

            usedVertices.Pop();
            counter--;
            return false;
        }
    }
}