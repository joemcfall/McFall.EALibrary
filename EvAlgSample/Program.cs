using McFall.EvAlg;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EvAlgSample
{
    class Program
    {
        static void Main(string[] args)
        {
            //This will attempt to match text input
            Console.WriteLine("Welcome to the genetic text matcher!");
            Console.WriteLine("Please enter some text to match");
            var textToMatch = Console.ReadLine();

            //Create gene definition objects
            //We'll cheat just a little bit by making
            //a chromosome of the same length as the text we're trying to match
            List<GeneDefBase> geneDefs = new List<GeneDefBase>();
            foreach (var i in Enumerable.Range(0, textToMatch.Length))
            {
                //My gene definition can be any type I want
                //There are some build in functions for handling ints, doubles, bitvectors, etc.
                //but I'm just going to use chars here and tell it how
                //to evolve
                var gd = GeneDef<char>.Create();

                //i can describe each gene so I know how to interpret the output
                gd.Description = $"Character at index {i} = {textToMatch[i]}";

                //I need to tell it how to initialize the values
                //I'm going to generate random ASCII chars from bytes
                gd.ValueInitializer = () =>
                {
                    byte[] buffer = new byte[1];
                    Numerical.RNG.NextBytes(buffer);
                    return Convert.ToChar(buffer[0]);
                };

                //There are some built in mutators for 
                //numerical types, but I'll supply my own to mutate characters
                
                gd.Mutator = c =>
                {                    
                    var myByte = Convert.ToByte(c);

                    myByte = (byte)MutatorMethods.IncrementMutate(myByte);

                    return Convert.ToChar(myByte);
                };

                //I would like to constrain the values
                //to be non-control characters
                gd.ValueConstrainer = c =>
                {
                    var myChar = c;
                    while (char.IsControl(myChar))
                    {
                        //pick a random character
                        byte[] buffer = new byte[1];
                        Numerical.RNG.NextBytes(buffer);
                        myChar = Convert.ToChar(buffer[0]);
                    }
                    return myChar;
                };

                gd.CrossoverMethod = (c1, c2) =>
                {
                    //randomly choose
                    if (Numerical.RNG.NextDouble() > 0.5)
                        return c1;
                    else
                        return c2;
                };


                geneDefs.Add(gd);
            }

            

            //I need to set my fitness function
            //It's a static function on Individual
            Individual.CalculateFitness = ind =>
            {
                var myGeneDefs = ind.GetGeneDefs();

                double distance = 0;
                foreach(var gd in myGeneDefs)
                {
                    //what's the matching character?
                    //LUCKY ME I put it in the genedef description as the last character
                    var matchy = gd.Description.Last();
                    var iMatch = Convert.ToByte(matchy);

                    //what's the value on this gene?
                    Gene<char> val = ind.GetGene(gd.Identifier) as Gene<char>;
                    //convert it to an int
                    var iVal = Convert.ToByte(val.Value);
                    distance += Math.Abs(iVal - iMatch);
                }

                //avg distance
                //distance /= myGeneDefs.Count();

                //I'll make fitness negative, since we're going to max the fitness (not min)
                ind.Fitness = 0.0 - distance;

                //I can put a note on this individual
                //so i can display some info about it later
                //I'll put the text it resolves to here so we can see it evolve
                var text = new string((from gd in ind.GetGeneDefs()
                                       let gv = ind.GetGene(gd.Identifier) as Gene<char>
                                       select gv.Value).ToArray());
                ind.Note = $"{text} : FITNESS={distance}";
            };

            //let's create a list of 500 individuals
            var population = (from i in Enumerable.Range(1, 100)
                              select IndividualFactory.CreateIndividual(geneDefs)).ToList();


            //initialize all
            Parallel.ForEach(population, ind =>
            {
                ind.InitializeValues();
                Individual.CalculateFitness(ind);
            });

            

            //Ok, time to evolve! Let's run this for 10000 generations
            foreach(var gen in Enumerable.Range(2, 20000))
            {
                //stop if best fitness is 0
                //create 20 children per generation, using weighted choice selection
                population = population.Evolve(2, SelectionMethods.WeightedChoice).ToList();

                var best = population.OrderByDescending(ind => ind.Fitness).First();
                //Console.Clear();
                Console.WriteLine($"Generation :{gen}: Avg Fitness: {0 - population.AverageFitness()} : Best: {best.Note}");

                if (best.Fitness == 0.0)
                    break;
            }

            Console.WriteLine("Press enter to exit");
            Console.ReadLine();

        }
    }
}
