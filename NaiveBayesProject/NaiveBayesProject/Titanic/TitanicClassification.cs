using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NaiveBayes.Data;
using NaiveBayes.Category;
using NaiveBayes.Variables;
using NaiveBayes.Classification;

namespace NaiveBayesProject.Titanic
{
    class TitanicClassification
    {
        private IDataLoader _dataLoader;
        private ICategory _category;
        private string SampleCategoryName = "ududu";

        public const string AttributesCommand = "#Attributes";
        public const string NumberCommand = "#Number";
        public const string EndCommand = "#End";
        public const string EndObjectCommand = "#EndObject";
        public const string NewObjectCommand = "#NewObject";
        int number = -1;

        public void classify(string titanicTeachingDataFile, string titanicTestingDataFile,string titanicOutputDataFile)
        {
            this._dataLoader = new DataLoader();
            this._category = this.PrepareCategoryFromFile();

            Console.WriteLine("Loading data");
            List<ITargetObject> targetObjects = this._dataLoader.LoadTeachingData(_category, titanicTeachingDataFile);

            Console.WriteLine("Teaching category engine");
            _category.Engine.TeachCategory(targetObjects);

            _category.Engine.PrepareToClassification();

            var fileInfo = new FileInfo(titanicTestingDataFile);

            TextReader reader = fileInfo.OpenText();
            TextWriter writer = new StreamWriter(titanicOutputDataFile);

            string text;
            double survivedProbability, notSurvivedProbability;
            List<string> attributes;

            Console.WriteLine("Classifing");
            do
            {
                text = reader.ReadLine();
                if (String.Equals(text, NewObjectCommand))
                {
                    ITargetObject titanicPassenger = new TargetObject(_category, String.Empty);

                    attributes = getAttributes(_category, reader);

                    foreach (string attribute in attributes)
                    {
                        titanicPassenger.SetAttributeExist(attribute);
                    }

                    GetProbabilities(titanicPassenger, _category, out survivedProbability, out notSurvivedProbability);

                    writer.WriteLine(number + " " + survivedProbability + " " + notSurvivedProbability);
                    writer.Flush();
                }
            } while (text != null);
            writer.Close();
            reader.Close();
        }

        private List<string> getAttributes(ICategory category, TextReader reader)
        {
            string text;
            var attributes = new List<string>();
            string categoryType = String.Empty;


            while (!String.Equals(text = reader.ReadLine(), EndObjectCommand))
            {
                switch (text)
                {
                    case AttributesCommand:
                        attributes.AddRange(LoadAttributes(reader));
                        break;
                    case NumberCommand:
                        number = LoadNumber(reader);
                        break;
                }
            }
            if (number <= 0 || attributes.Count == 0)
            {
                throw new InvalidDataException(
                    "Count of the new object must be higher than 0, categoryType cannot be null or empty and number of attributes must higher than 0.");
            }

            return attributes;
        }

        private static List<string> LoadAttributes(TextReader reader)
        {
            var result = new List<string>();
            string text;

            while (!String.Equals(text = reader.ReadLine(), EndCommand))
            {
                result.Add(text);
            }

            return result;
        }

        private int LoadNumber(TextReader reader)
        {
            int result = Int32.Parse(reader.ReadLine());

            if (String.Equals(reader.ReadLine(), EndCommand))
            {
                return result;
            }

            throw new InvalidDataException("After count number in the file you must type end command - #End.");
        }


        private ICategory PrepareCategoryFromFile()
        {
            this._dataLoader.LoadCategory(SampleCategoryName, @"../../TestFiles/TitanicCategoryFile.txt");

            return CategoryFactory.GetCategory(SampleCategoryName);
        }

        private static void GetProbabilities(ITargetObject titanicPassenger, ICategory category, out double survivedProbability, out double notSurvivedProbability)
        {
            IClassifer classifer = new Classifer();
            classifer.Init(category);

            Dictionary<string, double> classification = classifer.GetClassification(titanicPassenger);

            survivedProbability = classification["Survived"];
            notSurvivedProbability = classification["NotSurvived"];
        }
    }
}
