using System;
using System.Collections.Generic;
using NaiveBayes.Category;
using NaiveBayes.Classification;
using NaiveBayes.Variables;
using NUnit.Framework;

namespace TestNaiveBayes
{
    [TestFixture]
    public class TestClassifer
    {
        private ICategory _category;
        private IClassifer _classifer;

        [SetUp]
        public void SetUp()
        {
            this.InitAndTeachCategory();

            this._classifer = new Classifer();
            this._classifer.Init(this._category);
        }

        [TearDown]
        public void TearDown()
        {
            CategoryFactory.RemoveCategory(TestCategoryFactory.CategoryName);
        }

        private void InitAndTeachCategory()
        {
            CategoryFactory.AddCategoryType(TestCategoryFactory.CategoryName, TestCategoryFactory.CategoryTypes, TestCategoryFactory.CategoryAttributes);

            this._category = CategoryFactory.GetCategory(TestCategoryFactory.CategoryName);

            ITargetObject snowboardExpensiveAllMountain = new TargetObject(this._category, this._category.CategoryTypes[TestCategoryFactory.MediumCategory]);
            snowboardExpensiveAllMountain.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Expensive]);
            snowboardExpensiveAllMountain.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.AllMountain]);

            ITargetObject snowboardCheapAllMountain = new TargetObject(this._category, this._category.CategoryTypes[TestCategoryFactory.BeginnerCategory]);
            snowboardCheapAllMountain.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]);
            snowboardCheapAllMountain.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.AllMountain]);

            ITargetObject snowboardCheapFreestyle = new TargetObject(this._category, this._category.CategoryTypes[TestCategoryFactory.BeginnerCategory]);
            snowboardCheapFreestyle.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]);
            snowboardCheapFreestyle.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Freestyle]);

            ITargetObject snowboardExpensiveFreestyle = new TargetObject(this._category, this._category.CategoryTypes[TestCategoryFactory.AdvancedCategory]);
            snowboardExpensiveFreestyle.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Expensive]);
            snowboardExpensiveFreestyle.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Freestyle]);

            ITargetObject snowboardCheapFreeride = new TargetObject(this._category, this._category.CategoryTypes[TestCategoryFactory.MediumCategory]);
            snowboardCheapFreeride.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]);
            snowboardCheapFreeride.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Freeride]);

            ITargetObject snowboardExpensiveFreeride = new TargetObject(this._category, this._category.CategoryTypes[TestCategoryFactory.AdvancedCategory]);
            snowboardExpensiveFreeride.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Expensive]);
            snowboardExpensiveFreeride.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Freeride]);

            this._category.Engine.TeachCategory(new List<ITargetObject>
                                                    {
                                                        snowboardCheapAllMountain,
                                                        snowboardCheapAllMountain,
                                                        snowboardCheapAllMountain,
                                                        snowboardCheapAllMountain,
                                                        snowboardCheapAllMountain,
                                                        snowboardCheapAllMountain,
                                                        snowboardCheapAllMountain,
                                                        snowboardExpensiveAllMountain,
                                                        snowboardExpensiveAllMountain,
                                                        snowboardExpensiveAllMountain,
                                                        snowboardCheapFreeride,
                                                        snowboardCheapFreeride,
                                                        snowboardCheapFreeride,
                                                        snowboardCheapFreeride,
                                                        snowboardExpensiveFreeride,
                                                        snowboardCheapFreestyle,
                                                        snowboardCheapFreestyle,
                                                        snowboardCheapFreestyle,
                                                        snowboardCheapFreestyle,
                                                        snowboardExpensiveFreestyle
                                                    });

            this._category.Engine.PrepareToClassification();
        }

        [Test]
        [ExpectedException("NaiveBayes.Classification.CannotBeClassifyException")]
        public void TestCanNotBeClassify()
        {
            CategoryFactory.AddCategoryType("dowolna", new[] {"ok", "no"}, new[] {"pierwszy", "drugi"});
            this._classifer.Init(CategoryFactory.GetCategory("dowolna"));
        }

        [Test]
        [ExpectedException("NaiveBayes.Classification.TargetObjectIsClassifiedBeforeException")]
        public void TestObjectIsClassified()
        {
            ITargetObject snowboardExpensiveFreestyle = new TargetObject(this._category, this._category.CategoryTypes[TestCategoryFactory.AdvancedCategory]);
            this._classifer.GetClassification(snowboardExpensiveFreestyle);
        }

        [Test]
        public void TestClassifyAllMountainCheapAsBeginnerCategory()
        {
            ITargetObject snowboardCheapAllMountainToClassify = new TargetObject(this._category, String.Empty);
            snowboardCheapAllMountainToClassify.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.AllMountain]);
            snowboardCheapAllMountainToClassify.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]);

            double beginnerProbability, mediumProbability, advancedProbability;

            this.GetProbabilities(snowboardCheapAllMountainToClassify, out beginnerProbability, out mediumProbability, out advancedProbability);

            Assert.IsTrue(beginnerProbability > mediumProbability);
            Assert.IsTrue(beginnerProbability > advancedProbability);
        }

        [Test]
        public void TestClassifyFreestyleExpensiveAsAdvancedCategory()
        {
            ITargetObject snowboardExpensiveFreestyleToClassify = new TargetObject(this._category, String.Empty);
            snowboardExpensiveFreestyleToClassify.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Freestyle]);
            snowboardExpensiveFreestyleToClassify.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Expensive]);

            double beginnerProbability, mediumProbability, advancedProbability;

            this.GetProbabilities(snowboardExpensiveFreestyleToClassify, out beginnerProbability, out mediumProbability, out advancedProbability);

            Assert.IsTrue(advancedProbability > mediumProbability);
            Assert.IsTrue(advancedProbability > beginnerProbability);
        }

        [Test]
        public void TestClassifyFreerideCheapAsMediumCategory()
        {
            ITargetObject snowboardFreerideCheapToClassify = new TargetObject(this._category, String.Empty);
            snowboardFreerideCheapToClassify.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Freeride]);
            snowboardFreerideCheapToClassify.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]);

            double beginnerProbability, mediumProbability, advancedProbability;

            this.GetProbabilities(snowboardFreerideCheapToClassify, out beginnerProbability, out mediumProbability, out advancedProbability);

            Assert.IsTrue(mediumProbability > advancedProbability);
            Assert.IsTrue(mediumProbability > beginnerProbability);
        }

        private void GetProbabilities(ITargetObject snowboardExpensiveFreestyleToClassify, out double beginnerProbability, out double mediumProbability, out double advancedProbability)
        {
            Dictionary<string, double> classification = this._classifer.GetClassification(snowboardExpensiveFreestyleToClassify);

            beginnerProbability = classification[this._category.CategoryTypes[TestCategoryFactory.BeginnerCategory]];
            mediumProbability = classification[this._category.CategoryTypes[TestCategoryFactory.MediumCategory]];
            advancedProbability = classification[this._category.CategoryTypes[TestCategoryFactory.AdvancedCategory]];
        }
    }
}