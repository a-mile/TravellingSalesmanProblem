using System;

namespace TSP
{
    public static class GraphMethods
    {
        public static Graph CreateFullGraph(int size, int minDisctance, int maxDistance, double pheromoneValue)
        {
            Graph graph = new Graph();
            Random rand = new Random();

            for (int i = 0; i < size; i++)
            {
                Vertex vertex = new Vertex();
                vertex.index = i;                
                graph.vertices.Add(vertex);
            }
            
            for(int i=0;i<size;i++)
            {
                for(int j=i+1;j<size;j++)
                {
                    Edge edge = new Edge();
                    edge.distance = rand.Next(minDisctance, maxDistance);                    
                    edge.pheromoneValue = pheromoneValue;
                    edge.vertex1 = graph.vertices[i];
                    edge.vertex2 = graph.vertices[j];
                    
                    graph.vertices[i].neighbors.Add(edge);
                    
                    Edge edge2 = new Edge();
                    edge2.distance = edge.distance;
                    edge2.pheromoneValue = pheromoneValue;
                    edge2.vertex1 = graph.vertices[j];
                    edge2.vertex2 = graph.vertices[i];
                    
                    graph.vertices[j].neighbors.Add(edge2);
                    
                    graph.edges.Add(edge);
                    graph.edges.Add(edge2);
                }
            }

            return graph;
        }
    }
}