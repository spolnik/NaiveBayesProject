using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using NaiveBayes.Data;
using NaiveBayes.Category;
using NaiveBayes.Variables;
using NaiveBayes.Classification;

namespace NaiveBayesProject.Mushroom
{
    class MushroomClassification
    {
        private IDataLoader _dataLoader;
        private ICategory _category;
        private string SampleCategoryName = "ududu";

        public const string CategoryTypeCommand = "#CategoryType";
        public const string AttributesCommand = "#Attributes";
        public const string NumberCommand = "#Count";
        public const string EndCommand = "#End";
        public const string EndObjectCommand = "#EndObject";
        public const string NewObjectCommand = "#NewObject";

        public void classify(string mushroomTeachingDataFile, string mushroomTestingDataFile, string outputDataFile)
        {
            this._dataLoader = new DataLoader();
            this._category = this.PrepareCategoryFromFile();

            Console.WriteLine("Loading data");
            List<ITargetObject> targetObjects = this._dataLoader.LoadTeachingData(_category, mushroomTeachingDataFile);

            Console.WriteLine("Teaching category engine");
            _category.Engine.TeachCategory(targetObjects);

            _category.Engine.PrepareToClassification();

            var fileInfo = new FileInfo(mushroomTestingDataFile);
            TextReader reader = fileInfo.OpenText();
            TextWriter writer = new StreamWriter(outputDataFile);

            string text;
            double edible, poisonous;
            List<string> attributes;


            Console.WriteLine("Classifing");
            do
            {
                text = reader.ReadLine();
                if (String.Equals(text, NewObjectCommand))
                {
                    ITargetObject mushroom = new TargetObject(_category, String.Empty);
                    string categoryType = String.Empty;

                    attributes = getAttributes(_category, reader,out categoryType);

                    foreach (string attribute in attributes)
                    {
                        mushroom.SetAttributeExist(attribute);
                    }

                    GetProbabilities(mushroom, _category, out edible, out poisonous);

                    writer.WriteLine(edible + " " + poisonous + " " + categoryType);
                    writer.Flush();
                }
            } while (text != null);
            writer.Close();
            reader.Close();
        }

        private List<string> getAttributes(ICategory category, TextReader reader,out string categoryType)
        {
            string text;
            var attributes = new List<string>();
            categoryType = String.Empty;
            int count = -1;


            while (!String.Equals(text = reader.ReadLine(), EndObjectCommand))
            {
                switch (text)
                {
                    case CategoryTypeCommand:
                        LoadCategoryType(out categoryType, reader);
                        break;
                    case AttributesCommand:
                        attributes.AddRange(LoadAttributes(reader));
                        break;
                    case NumberCommand:
                        count = LoadCount(reader);
                        break;
                }
            }
            if (count <= 0 || String.IsNullOrEmpty(categoryType) || attributes.Count == 0)
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

        private static void LoadCategoryType(out string categoryType, TextReader reader)
        {
            categoryType = reader.ReadLine();

            if (String.Equals(reader.ReadLine(), EndCommand))
            {
                return;
            }

            throw new InvalidDataException("After category type name in the file you must type end command - #End.");
        }

        private int LoadCount(TextReader reader)
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
            this._dataLoader.LoadCategory(SampleCategoryName, @"../../TestFiles/MushroomCategoryFile.txt");

            return CategoryFactory.GetCategory(SampleCategoryName);
        }

        private static void GetProbabilities(ITargetObject titanicPassenger, ICategory category, out double survivedProbability, out double notSurvivedProbability)
        {
            IClassifer classifer = new Classifer();
            classifer.Init(category);

            Dictionary<string, double> classification = classifer.GetClassification(titanicPassenger);

            survivedProbability = classification["Edible"];
            notSurvivedProbability = classification["Poisonous"];
        }
    }
}
