using System.Collections.Generic;

namespace TSP
{
    public class Vertex
    {
        public Vertex()
        {
            this.neighbors = new List<Edge>();
        }
        public int index { get; set; }
        public double data { get; set; }
        public List<Edge> neighbors { get; set; }        
    }

    public class Edge
    {
        public Edge()
        {
            this.vertex1 = new Vertex();
            this.vertex2 = new Vertex();
        }
        public int distance { get; set; }
        public double pheromoneValue { get; set; }
        public Vertex vertex1 { get; set; }
        public Vertex vertex2 { get; set; }
    }

    public class Graph
    {
        public Graph()
        {
            this.vertices = new List<Vertex>();
            this.edges = new List<Edge>();
        }
        public List<Vertex> vertices { get; set; }
        public List<Edge> edges {get; set;}
    }
}