using System.Collections.Generic;
using NaiveBayes.Category;
using NUnit.Framework;

namespace TestNaiveBayes
{
    [TestFixture]
    public class TestCategoryFactory
    {
        static TestCategoryFactory()
        {
            Expensive = 0;
            Cheap = 1;
            AllMountain = 2;
            Freestyle = 3;
            Freeride = 4;

            BeginnerCategory = 0;
            MediumCategory = 1;
            AdvancedCategory = 2;
        }

        internal static readonly int Expensive;
        internal static readonly int Cheap;
        internal static readonly int AllMountain;
        internal static readonly int Freestyle;
        internal static readonly int Freeride;

        internal static readonly string[] CategoryAttributes =
            {
                "Expensive",
                "Cheap",
                "AllMountain",
                "Freestyle",
                "Freeride"
            };

        internal static readonly int BeginnerCategory;
        internal static readonly int MediumCategory;
        internal static readonly int AdvancedCategory;

        internal static readonly string[] CategoryTypes =
            {
                "Beginner",
                "Medium",
                "Advanced"
            };

        internal static readonly string CategoryName = "Snowboard";

        [SetUp]
        public void SetUp()
        {
            CategoryFactory.AddCategoryType(CategoryName, CategoryTypes, CategoryAttributes);
        }

        [TearDown]
        public void TearDown()
        {
            CategoryFactory.RemoveCategory(CategoryName);
        }

        [Test]
        public void TestAddingCategoryType()
        {
            ICategory testedCategory = CategoryFactory.GetCategory(CategoryName);
            Assert.AreEqual(testedCategory.Name, CategoryName);
            Assert.AreEqual(testedCategory.CategoryTypes, new List<string>(CategoryTypes));
            Assert.AreEqual(testedCategory.Attributes, new List<string>(CategoryAttributes));
        }
    }
}