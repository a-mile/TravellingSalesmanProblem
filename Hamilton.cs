using System.Collections.Generic;

namespace TSP
{
    public class Hamilton
    {
        public Hamilton()
        {
            verticesStack = new Stack<Vertex>();
            usedVertices = new Stack<Vertex>();
            shortestPath = new List<Vertex>();
            minDistance = 0;
        }

        public Graph graph { get; set; }        
        public double minDistance { get; private set; }

        Vertex startVertex;
        List<Vertex> shortestPath;        
        Stack<Vertex> verticesStack;
        Stack<Vertex> usedVertices;

        public List<Vertex> ShortestHamiltonCycle()
        {
            startVertex = graph.vertices[0];
            HamiltonCycleRecurring(startVertex);
            return shortestPath;
        }

        int counter = 0;
        double distance = 0;

        void HamiltonCycleRecurring(Vertex vertex)
        {
            counter++;         
            usedVertices.Push(vertex);            
            verticesStack.Push(vertex);

            foreach (Edge edge in vertex.neighbors)
            {
                if (edge.vertex2 == startVertex)
                {
                    if (counter == graph.vertices.Count)
                    {
                        verticesStack.Push(edge.vertex2);
                        distance += edge.distance;

                        if (minDistance == 0 || distance < minDistance)
                        {
                            shortestPath.Clear();
                            shortestPath.AddRange(verticesStack);
                            minDistance = distance;
                        }

                        distance -= edge.distance;
                        verticesStack.Pop();
                    }
                }

                if (!usedVertices.Contains(edge.vertex2))
                {
                    distance += edge.distance;
                    HamiltonCycleRecurring(edge.vertex2);
                    distance -= edge.distance;
                }
            }

            verticesStack.Pop();
            usedVertices.Pop();
            counter--;
        }
    }
}
