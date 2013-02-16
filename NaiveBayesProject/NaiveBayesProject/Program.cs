using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NaiveBayes.Data;
using NaiveBayes.Category;
using NaiveBayes.Variables;
using NaiveBayes.Classification;
using NaiveBayesProject.Mushroom;
using NaiveBayesProject.Titanic;

namespace NaiveBayesProject
{
    class Program
    {
        static Program()
        {
        }


        static void Main(string[] args)
        {
            if ((args.Length == 3 || args.Length == 4) && String.Equals(args[0], "-mushroom")  ||
                args.Length == 5 && String.Equals(args[0], "-mushroom") && String.Equals(args[3], "-out") ||
                args.Length == 6 && String.Equals(args[0], "-mushroom") && String.Equals(args[4], "-out") ||
                args.Length == 5 && String.Equals(args[0], "-titanic") && String.Equals(args[1], "-1") ||
                args.Length == 7 && String.Equals(args[0], "-titanic") && String.Equals(args[1], "-1") ||
                args.Length == 5 && String.Equals(args[0], "-titanic") && String.Equals(args[1], "-2") ||
                args.Length == 7 && String.Equals(args[0], "-titanic") && String.Equals(args[1], "-2")
                )
            {
                if (String.Equals(args[0], "-mushroom"))
                {
                    string mushroomCategoryFile = args[1];                       // @"../../TestFiles/MushroomCategoryFile.txt";
                    string mushroomDataFile = @"../../TestFiles/agaricus-lepiota.data";
                    string mushroomTeachingDataFile = @"../../TestFiles/MushroomTeachingData.txt";
                    string mushroomTestingDataFile = @"../../TestFiles/MushroomTestingData.txt";
                    string mushroomOutputDataFile = @"out.txt";
                    
                    if (args.Length == 3 || args.Length == 5)
                    {
                        mushroomDataFile = args[2];
                        Mushroom.DataPreparation dt = new Mushroom.DataPreparation();
                        dt.loadData(mushroomCategoryFile, mushroomDataFile, mushroomTeachingDataFile, mushroomTestingDataFile);
                    }
                    if((args.Length == 4 || args.Length == 6))
                    {
                        mushroomTeachingDataFile = args[2];
                        mushroomTestingDataFile = args[3];
                    }
                    if (args.Length == 5)
                    {
                        mushroomOutputDataFile = args[4];
                    }
                    if (args.Length == 6)
                    {
                        mushroomOutputDataFile = args[5];
                    }

                    MushroomClassification mc = new MushroomClassification();
                    mc.classify(mushroomTeachingDataFile, mushroomTestingDataFile, mushroomOutputDataFile);

                    Verification v = new Verification();
                    v.verify(mushroomOutputDataFile);

                }
                else if (String.Equals(args[0], "-titanic"))
                {
                    string titanicCategoryFile = @"../../TestFiles/TitanicCategoryFile.txt";
                    string titanicDataFile = @"../../TestFiles/titanic.dat";
                    string titanicTeachingDataFile = @"../../TestFiles/TitanicTeachingData.txt";
                    string titanicTestingDataFile = @"../../TestFiles/TitanicTestingData.txt";
                    string titanicOutputDataFile = @"out.txt";

                    titanicCategoryFile = args[2];
                    titanicTestingDataFile = args[4];
                    if( String.Equals(args[1], "-1"))
                    {
                        titanicDataFile = args[3];
                        Titanic.DataPreparation dt = new Titanic.DataPreparation();
                        dt.loadData(titanicCategoryFile, titanicDataFile, titanicTeachingDataFile);
                    }else if (String.Equals(args[1], "-2"))
                    {
                        titanicTeachingDataFile = args[3];
                    }
                    if(args.Length == 7  && String.Equals(args[4], "-out")){
                        titanicOutputDataFile = args[5];
                    }
                    

                    TitanicClassification tc = new TitanicClassification();
                    tc.classify(titanicTeachingDataFile, titanicTestingDataFile, titanicOutputDataFile);
                }

            }
            else
            {
                usage();
            }
        }

        public static void usage()
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Usage:");
            Console.WriteLine();
            Console.WriteLine("      NaiveBayesProject.exe -mushroom CategoryFilePath DataFilePath [-out OutputFilePath]");
            Console.WriteLine();
            Console.WriteLine("      or");
            Console.WriteLine();
            Console.WriteLine("      NaiveBayesProject.exe -mushroom CategoryFilePath TeachingDataFilePath TestingDataFilePath [-out OutputFilePath]");
            Console.WriteLine();
            Console.WriteLine("      or");
            Console.WriteLine();
            Console.WriteLine("      NaiveBayesProject.exe -titanic -1 CategoryFilePath DataFilePath TestingDataFilePath [-out OutputFilePath]");
            Console.WriteLine();
            Console.WriteLine("      or");
            Console.WriteLine();
            Console.WriteLine("      NaiveBayesProject.exe -titanic -2 CategoryFilePath TeachingDataFilePath TestingDataFilePath [-out OutputFilePath]");
            Console.WriteLine();
        }
    }
}
