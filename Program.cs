using System;

namespace TSP
{
    public static class Program
    {
        public static void Main(string[] args)
        {                                                
            //****************** GRAPH INTIALIZE *****************************
            Graph graph = new Graph();
            graph = GraphMethods.CreateFullGraph(10, 1, 1000, 0.001);            
            //******************** BRUTE FORCE *******************************
             Hamilton h = new Hamilton();
             h.graph = graph;                         
            
             Console.WriteLine("Brute Force");
             foreach(Vertex v in h.ShortestHamiltonCycle())
             {
                 Console.Write(v.index+" ");
             }
             Console.WriteLine();
             Console.WriteLine("Distance: "+h.minDistance);
             Console.WriteLine();
            //******************** ANT ***************************************
            AntOptimization ant = new AntOptimization();
            ant.graph = graph;        
            ant.alpha = 1;
            ant.beta = 5;
            ant.ro = 0.5;                                
            
            Console.WriteLine("Ant");
            foreach(Vertex v in ant.AntColonyOptimization(100,100))
            {
                Console.Write(v.index+" ");
            }
            Console.WriteLine();
            Console.WriteLine("Distance: "+ant.minDistance);
            Console.WriteLine();
            //**************** GENETIC ***************************************
            Genetic genetic = new Genetic();
            genetic.graph = graph;  
            
            genetic.GenerateChromosomes(100);
            
            genetic.mutationRate = (int)0.047*genetic.chromosomes.Count;
            genetic.crossingRate = genetic.chromosomes.Count;
            
            Console.WriteLine("Genetic");
            foreach(Vertex v in genetic.GeneticOptimization(100))
            {
                Console.Write(v.index+" ");
            }
            Console.Write(genetic.chromosomes[0].genes[0].index);
            Console.WriteLine();
            Console.WriteLine("Distance: "+genetic.chromosomes[0].rating); 
            Console.WriteLine();     
            //************ NEAREST NEIGHBOUR *********************************
            NearestNeighbour nn = new NearestNeighbour();
            nn.graph = graph;
                        
            Console.WriteLine("Nearest Neighbour");
            foreach(Vertex v in nn.NearestNeighbourOptimization())
            {
                Console.Write(v.index+" ");
            }  
            Console.WriteLine();                                       
            Console.WriteLine("Distance: "+nn.minDistance);
            Console.WriteLine();
        }
    }
}
