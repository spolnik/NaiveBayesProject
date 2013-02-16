using System.Collections.Generic;

namespace NaiveBayes.Category
{
    public static class CategoryFactory
    {
        private static readonly Dictionary<string, ICategory> Categories;

        static CategoryFactory()
        {
            Categories = new Dictionary<string, ICategory>();
        }

        public static void AddCategoryType(string name, string[] categoryTypes, params string[] parameters)
        {
            Categories.Add(name, new Category(name, categoryTypes, parameters));
        }

        public static ICategory GetCategory(string categoryName)
        {
            return Categories.ContainsKey(categoryName)
                       ? Categories[categoryName]
                       : null;
        }

        public static void RemoveCategory(string categoryName)
        {
            Categories.Remove(categoryName);
        }
    }
}