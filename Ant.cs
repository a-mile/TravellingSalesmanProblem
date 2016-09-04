using System;
using System.Collections.Generic;

namespace TSP
{
    public class Ant
    {
        public Ant()
        {
            visitedVertices = new Stack<Vertex>();
            visitedEdges = new List<Edge>();
        }

        public Vertex startVertex { get; set; }
        public Stack<Vertex> visitedVertices { get; set; }
        public List<Edge> visitedEdges { get; set; }
        public int travelledDistance = 0;
    }

    public class AntOptimization
    {
        public AntOptimization()
        {
            ants = new List<Ant>();
            shortestPath = new List<Vertex>();
            random = new Random();
            minDistance = 0;
        }

        Random random;
        public Graph graph { get; set; }
        public List<Ant> ants { get; set; }
        List<Vertex> shortestPath;
        public int minDistance { get; private set; }

        private void AntGenerator(int amount)
        {
            for (int i = 0; i < amount; i++)
            {
                ants.Add(
                    new Ant()
                    {
                        startVertex = graph.vertices[random.Next(0, graph.vertices.Count - 1)]
                    }
                );
            }
        }

        public List<Vertex> AntColonyOptimization(int iterations, int antAmount)
        {
            AntGenerator(antAmount);
            
            for (int i = 0; i < iterations; i++)
            {
                AntReset();

                foreach (Ant a in ants)
                {
                    AntPath(a);
                    Pheromone(a);

                    if (minDistance == 0 || a.travelledDistance < minDistance)
                    {
                        minDistance = a.travelledDistance;
                        shortestPath.Clear();
                        shortestPath.AddRange(a.visitedVertices);
                    }
                }
            }
            return shortestPath;
        }

        private void AntReset()
        {
            foreach (Ant a in ants)
            {
                a.travelledDistance = 0;
                a.visitedEdges.Clear();
                a.visitedVertices.Clear();
            }
        }

        int counter;

        private void AntPath(Ant ant)
        {
            counter = 0;
            AntPathRecurring(ant, ant.startVertex);
        }

        private void AntPathRecurring(Ant ant, Vertex vertex)
        {
            counter++;
            ant.visitedVertices.Push(vertex);           
            Edge nextEdge = null;

            if (counter == graph.vertices.Count)
            {
                foreach (Edge e in vertex.neighbors)
                {
                    if (e.vertex2 == ant.startVertex)
                    {
                        ant.travelledDistance += e.distance;
                        AntPathRecurring(ant, e.vertex2);
                    }
                }
            }
            else
            {
                nextEdge = NextEdge(ant);
                if (nextEdge != null)
                {
                    ant.visitedEdges.Add(nextEdge);
                    ant.travelledDistance += nextEdge.distance;
                    AntPathRecurring(ant, nextEdge.vertex2);
                }
            }
        }

        private Edge NextEdge(Ant ant)
        {
            double c = 0;
            double r = random.NextDouble();
            
            Vertex vertex = ant.visitedVertices.Peek();

            foreach (Edge e in vertex.neighbors)
            {
                if (!ant.visitedVertices.Contains(e.vertex2))
                {
                    c += Probability(ant, e);
                    if (c >= r)
                    {
                        return e;
                    }
                }
            }
            return null;
        }

        public double ro { get; set; }

        private void Pheromone(Ant ant)
        {
            foreach (Vertex v in graph.vertices)
            {
                foreach (Edge e in v.neighbors)
                {
                    if (ant.visitedEdges.Contains(e))
                    {
                        e.pheromoneValue = (1 - ro) * e.pheromoneValue + (1 / (double)ant.travelledDistance);
                    }
                    else
                    {
                        e.pheromoneValue = (1 - ro) * e.pheromoneValue;
                    }
                }
            }
        }

        public int alpha { get; set; }
        public int beta { get; set; }

        private double Probability(Ant ant, Edge edge)
        {
            double nominator = 0;
            double denominator = 0;
            Vertex vertex = ant.visitedVertices.Peek();

            nominator = Math.Pow(edge.pheromoneValue, alpha) * Math.Pow(1 / (double)edge.distance, beta);

            foreach (Edge e in vertex.neighbors)
            {
                if (!ant.visitedVertices.Contains(e.vertex2))
                {
                    denominator += Math.Pow(e.pheromoneValue, alpha) * Math.Pow(1 / (double)e.distance, beta);
                }
            }
            return nominator / denominator;
        }
    }
}