using System;
using System.Collections.Generic;
using System.IO;
using NaiveBayes.Category;
using NaiveBayes.Variables;

namespace NaiveBayes.Data
{
    public class DataLoader : IDataLoader
    {
        public const string AttributesCommand = "#Attributes";
        public const string CategoryAttributesCommand = "#CategoryAttributes";
        public const string CategoryTypeCommand = "#CategoryType";
        public const string CategoryTypesCommand = "#CategoryTypes";
        public const string CountCommand = "#Count";

        public const string EndCommand = "#End";
        public const string EndObjectCommand = "#EndObject";
        public const string NewObjectCommand = "#NewObject";

        #region IDataLoader Members

        public void LoadCategory(string categoryName, string fileName)
        {
            if (String.IsNullOrEmpty(categoryName))
            {
                throw new ArgumentNullException("categoryName", "Category name cannot be null or empty.");
            }

            var fileInfo = new FileInfo(fileName);

            List<string> categoryTypes;
            List<string> categoryAttributes;

            StreamReader reader = fileInfo.OpenText();

            LoadCategoryTypesAndAttributes(out categoryTypes, out categoryAttributes, reader);

            if (categoryTypes.Count == 0 || categoryAttributes.Count == 0)
            {
                throw new ArgumentNullException("fileName",
                                                "File has incorrect format. You should use #CategoryTypeses <categoryTypesList> #End and #CategoryAttributes <categoryAttributesList> #End format of a file.");
            }

            CategoryFactory.AddCategoryType(categoryName, categoryTypes.ToArray(), categoryAttributes.ToArray());
        }

        public List<ITargetObject> LoadTeachingData(ICategory category, string fileName)
        {
            var targetObjects = new List<ITargetObject>();
            var fileInfo = new FileInfo(fileName);

            TextReader reader = fileInfo.OpenText();

            string text;

            do
            {
                text = reader.ReadLine();
                if (String.Equals(text, NewObjectCommand))
                {
                    AddNewObjectFromFile(category, targetObjects, reader);
                }
            } while (text != null);

            return targetObjects;
        }

        #endregion

        private static void AddNewObjectFromFile(ICategory category, ICollection<ITargetObject> targetObjects,
                                                 TextReader reader)
        {
            string text;
            var attributes = new List<string>();
            string categoryType = String.Empty;
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
                    case CountCommand:
                        count = LoadCount(reader);
                        break;
                }
            }

            if (count <= 0 || String.IsNullOrEmpty(categoryType) || attributes.Count == 0)
            {
                throw new InvalidDataException(
                    "Count of the new object must be higher than 0, categoryType cannot be null or empty and number of attributes must higher than 0.");
            }

            ITargetObject targetObject = new TargetObject(category, categoryType);

            foreach (string attribute in attributes)
            {
                targetObject.SetAttributeExist(attribute);
            }

            for (int i = 0; i < count; i++)
            {
                targetObjects.Add(targetObject);
            }
        }

        private static int LoadCount(TextReader reader)
        {
            int result = Int32.Parse(reader.ReadLine());

            if (String.Equals(reader.ReadLine(), EndCommand))
            {
                return result;
            }

            throw new InvalidDataException("After count number in the file you must type end command - #End.");
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

        private static void LoadCategoryTypesAndAttributes(out List<string> categoryTypes,
                                                           out List<string> categoryAttributes, TextReader reader)
        {
            categoryTypes = new List<string>();
            categoryAttributes = new List<string>();

            string text;

            do
            {
                text = reader.ReadLine();
                switch (text)
                {
                    case CategoryTypesCommand:
                        LoadFromFile(categoryTypes, reader);
                        break;
                    case CategoryAttributesCommand:
                        LoadFromFile(categoryAttributes, reader);
                        break;
                }
            } while (text != null);

            reader.Close();
        }

        private static void LoadFromFile(ICollection<string> listOfItems, TextReader streamReader)
        {
            string text;

            while (!String.Equals(text = streamReader.ReadLine(), EndCommand))
            {
                listOfItems.Add(text);
            }
        }
    }
}