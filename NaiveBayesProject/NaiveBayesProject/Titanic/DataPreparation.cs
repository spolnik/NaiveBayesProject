using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.IO;

namespace NaiveBayesProject.Titanic
{
    class DataPreparation
    {

        public bool loadData(string titanicCategoryFile, string titanicDataFile, string titanicTeachingDataFile)
        {
            if (!File.Exists(titanicCategoryFile))
            {
                return false;
            }
            if (!File.Exists(titanicTeachingDataFile))
            {
                prepareTitanicTeachingDataFile(titanicDataFile, titanicTeachingDataFile);
            }
            return true;
        }

        public void prepareTitanicTeachingDataFile(string inputFileName,string titanicTeachingDataFile)
        {
            Console.WriteLine("Preparing TeachingDataFile");
            var fileInfo = new FileInfo(inputFileName);

            TextReader reader = fileInfo.OpenText();
            TextWriter writer = new StreamWriter(titanicTeachingDataFile);
            string text;


            while ((text = reader.ReadLine()) != null)
            {
                AddNewObjectToTitanicTeachingDataFile(writer, text);
            }
            writer.Close();
            reader.Close();
        }

        private void AddNewObjectToTitanicTeachingDataFile(TextWriter writer,string text)
        {
            writer.WriteLine("#NewObject");
            writer.WriteLine("#CategoryType");
            string[] tab = Regex.Split(text,@"\s+");
            if(String.Equals(tab[3],"1"))
            {
                writer.WriteLine("Survived");
            }
            else
            {
                writer.WriteLine("NotSurvived");
            }
            writer.WriteLine("#End");
            writer.WriteLine("#Attributes");
            if (String.Equals(tab[2], "1"))
            {
                writer.WriteLine("Male");
            }
            else
            {
                writer.WriteLine("Female");
            }
            if (String.Equals(tab[1], "1"))
            {
                writer.WriteLine("Adult");
            }
            else
            {
                writer.WriteLine("Child");
            }
            if (String.Equals(tab[0], "0"))
            {
                writer.WriteLine("Crew");
            }
            else if (String.Equals(tab[0], "1"))
            {
                writer.WriteLine("FirstClass");
            }
            else if (String.Equals(tab[0], "2"))
            {
                writer.WriteLine("SecondClass");
            }
            else if (String.Equals(tab[0], "3"))
            {
                writer.WriteLine("ThirdClass");
            }
            writer.WriteLine("#End");
            writer.WriteLine("#Count");
            writer.WriteLine("1");
            writer.WriteLine("#End");
            writer.WriteLine("#EndObject");
            writer.WriteLine();
        }
    }
}
