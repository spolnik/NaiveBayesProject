using System;
using System.Collections.Generic;
using NaiveBayes.Category;
using NaiveBayes.Classification;
using NaiveBayes.Data;
using NaiveBayes.Variables;
using NUnit.Framework;

namespace TestNaiveBayes
{
    [TestFixture]
    public class TestDataLoader
    {
        private const string SampleCategoryName = "Snowboard";
        private IDataLoader _dataLoader;

        [SetUp]
        public void SetUp()
        {
            this._dataLoader = new DataLoader();
        }

        [TearDown]
        public void TearDown()
        {
            CategoryFactory.RemoveCategory(SampleCategoryName);
        }

        [Test]
        public void TestLoadCategory()
        {
            ICategory category = this.PrepareCategoryFromFile();

            foreach (string categoryType in TestCategoryFactory.CategoryTypes)
            {
                Assert.IsTrue(category.CategoryTypes.Contains(categoryType));
            }

            Assert.AreEqual(TestCategoryFactory.CategoryTypes.Length, category.CategoryTypes.Count);

            foreach (string attribute in TestCategoryFactory.CategoryAttributes)
            {
                Assert.IsTrue(category.Attributes.Contains(attribute));
            }

            Assert.AreEqual(TestCategoryFactory.CategoryAttributes.Length, category.Attributes.Count);
        }

        private ICategory PrepareCategoryFromFile()
        {
            this._dataLoader.LoadCategory(SampleCategoryName, @"../../TestFiles/SampleCategoryFile.txt");

            return CategoryFactory.GetCategory(SampleCategoryName);
        }

        [Test]
        public void LoadTeachingData()
        {
            ICategory category = this.PrepareCategoryFromFile();

            List<ITargetObject> targetObjects = this._dataLoader.LoadTeachingData(category,
                                                                                  @"../../TestFiles/SampleTeachingData.txt");

            category.Engine.TeachCategory(targetObjects);

            category.Engine.PrepareToClassification();

            TestClassifyAllMountainCheapAsBeginnerCategory(category);
        }

        private static void TestClassifyAllMountainCheapAsBeginnerCategory(ICategory category)
        {
            ITargetObject snowboardCheapAllMountainToClassify = new TargetObject(category, String.Empty);
            snowboardCheapAllMountainToClassify.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.AllMountain]);
            snowboardCheapAllMountainToClassify.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]);

            double beginnerProbability, mediumProbability, advancedProbability;

            GetProbabilities(snowboardCheapAllMountainToClassify, category, out beginnerProbability, out mediumProbability, out advancedProbability);

            Assert.IsTrue(beginnerProbability > mediumProbability);
            Assert.IsTrue(beginnerProbability > advancedProbability);
        }

        private static void GetProbabilities(ITargetObject snowboardExpensiveFreestyleToClassify, ICategory category, out double beginnerProbability, out double mediumProbability, out double advancedProbability)
        {
            IClassifer classifer = new Classifer();
            classifer.Init(category);

            Dictionary<string, double> classification = classifer.GetClassification(snowboardExpensiveFreestyleToClassify);

            beginnerProbability = classification[category.CategoryTypes[TestCategoryFactory.BeginnerCategory]];
            mediumProbability = classification[category.CategoryTypes[TestCategoryFactory.MediumCategory]];
            advancedProbability = classification[category.CategoryTypes[TestCategoryFactory.AdvancedCategory]];
        }
    }
}