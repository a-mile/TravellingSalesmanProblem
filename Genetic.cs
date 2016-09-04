using System;
using System.Collections.Generic;
using System.Linq;

namespace TSP
{
    public class Chromosome
    {
        public Chromosome()
        {
            genes = new List<Vertex>();
        }
        public List<Vertex> genes { get; set; }
        public int rating { get; set; }
    }

    public class Genetic
    {
        public Genetic()
        {
            chromosomes = new List<Chromosome>();
            random = new Random();
        }
        
        Random random;
        public Graph graph { get; set; }
        public List<Chromosome> chromosomes { get; set; }

        public void GenerateChromosomes(int amont)
        {
            for (int i = 0; i < amont; i++)
            {
                Chromosome chromosome = new Chromosome();
                chromosome.genes.AddRange(graph.vertices);

                for (int j = 0; j < chromosome.genes.Count; j++)
                {
                    Mutation(chromosome);
                }

                chromosomes.Add(chromosome);
            }
        }
        
        public void Mutation(Chromosome chromosome)
        {
            int a = random.Next(0, chromosome.genes.Count - 1);
            int b = random.Next(0, chromosome.genes.Count - 1);
            if (a == b)
            {
                b++;
            }

            Vertex tempVer = chromosome.genes[a];
            chromosome.genes[a] = chromosome.genes[b];
            chromosome.genes[b] = tempVer;

            chromosome.rating = Rating(chromosome);
        }

        public void MutationOperations(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                Mutation(chromosomes[random.Next(0, chromosomes.Count - 1)]);
            }
        }

        public int Rating(Chromosome chromosome)
        {
            int rating = 0;

            for (int i = 0; i < chromosome.genes.Count - 1; i++)
            {
                foreach (Edge e in chromosome.genes[i].neighbors)
                {
                    if (e.vertex2 == chromosome.genes[i + 1])
                    {
                        rating += e.distance;
                    }
                }
            }

            foreach (Edge e in chromosome.genes[chromosome.genes.Count - 1].neighbors)
            {
                if (e.vertex2 == chromosome.genes[0])
                {
                    rating += e.distance;
                }
            }

            return rating;
        }

        public int crossingRate {get;set;}
        public int mutationRate {get;set;}
        
        public List<Vertex> GeneticOptimization(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                CrossingOperations(crossingRate);
                MutationOperations(mutationRate);
                chromosomes = chromosomes.OrderBy(x => x.rating).ToList();

                bool same = true;
                foreach (Chromosome c in chromosomes)
                {
                    if (c.rating != chromosomes[0].rating)
                    {
                        same = false;
                        break;
                    }
                }

                if (same)
                {
                    Reintialize();
                    chromosomes = chromosomes.OrderBy(x => x.rating).ToList();
                }
               
                Kill();                
            }
            
            return chromosomes[0].genes;
        }

        public void Reintialize()
        {
            int i = 0;
            foreach (Chromosome c in chromosomes)
            {
                if (i > (chromosomes.Count * 0.1))
                {
                    Mutation(c);
                }
                i++;
            }
        }

        public void Kill()
        {
            chromosomes.RemoveRange((int)Math.Round((double)chromosomes.Count / 2, 0, MidpointRounding.AwayFromZero), chromosomes.Count / 2);
        }

        public void CrossingOperations(int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                int a = random.Next(0, chromosomes.Count - 1);
                int b = random.Next(0, chromosomes.Count - 1);

                if (a == b)
                {
                    b++;
                }
                Crossing(chromosomes[a], chromosomes[b]);
            }
        }

        public void Crossing(Chromosome parent1, Chromosome parent2)
        {
            Chromosome[] child = new Chromosome[2];
            child[0] = new Chromosome();
            child[1] = new Chromosome();
            child[0].genes.Add(parent1.genes[0]);
            child[1].genes.Add(parent2.genes[0]);

            for (int j = 0; j < 2; j++)
            {
                for (int i = 0; i < graph.vertices.Count - 1; i++)
                {
                    int rating = 0;

                    int i1 = parent1.genes.FindIndex(x => x.index == child[j].genes[i].index) + 1;
                    if (i1 == graph.vertices.Count)
                    {
                        i1 = parent1.genes[0].index;
                    }
                    Vertex v1 = parent1.genes[i1];

                    int i2 = parent2.genes.FindIndex(x => x.index == child[j].genes[i].index) + 1;
                    if (i2 == graph.vertices.Count)
                    {
                        i2 = parent2.genes[0].index;
                    }
                    Vertex v2 = parent2.genes[i2];

                    Vertex nextVertex = null;

                    foreach (Edge e in child[j].genes[i].neighbors)
                    {
                        if (e.vertex2 == v1 || e.vertex2 == v2)
                        {
                            if ((rating == 0 || e.distance < rating) && !child[j].genes.Contains(e.vertex2))
                            {
                                rating = e.distance;
                                nextVertex = e.vertex2;
                            }                           
                        }
                    }

                    if (nextVertex == null)
                    {
                        foreach (Vertex v in graph.vertices)
                        {
                            if (!child[j].genes.Contains(v))
                            {
                                nextVertex = v;
                                break;
                            }
                        }
                    }
                    child[j].genes.Add(nextVertex);
                }
            }

            child[0].rating = Rating(child[0]);
            child[1].rating = Rating(child[1]);
            chromosomes.Add(child[0]);
            chromosomes.Add(child[1]);
        }
    }
}