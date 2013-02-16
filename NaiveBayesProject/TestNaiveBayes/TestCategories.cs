using System;
using System.Collections.Generic;
using NUnit.Framework;
using NaiveBayes.Category;
using NaiveBayes.Variables;

namespace TestNaiveBayes
{
    [TestFixture]
    public class TestCategories
    {
        [SetUp]
        public void SetUp()
        {
            CategoryFactory.AddCategoryType(TestCategoryFactory.CategoryName, TestCategoryFactory.CategoryTypes,
                                            TestCategoryFactory.CategoryAttributes);

            _category = CategoryFactory.GetCategory(TestCategoryFactory.CategoryName);
            _targetObject = new TargetObject(_category, _category.CategoryTypes[TestCategoryFactory.BeginnerCategory]);
        }

        [TearDown]
        public void TearDown()
        {
            CategoryFactory.RemoveCategory(TestCategoryFactory.CategoryName);
        }

        private ICategory _category;
        private ITargetObject _targetObject;

        [Test]
        public void TestAfterInit()
        {
            Assert.AreEqual(_category.Name, TestCategoryFactory.CategoryName);
            Assert.AreEqual(_category.CategoryTypes.Count, TestCategoryFactory.CategoryTypes.Length);
            Assert.AreEqual(_category.Attributes.Count, TestCategoryFactory.CategoryAttributes.Length);
        }

        [Test]
        public void TestApriori()
        {
            _targetObject.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]);
            _targetObject.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.AllMountain]);

            _category.Engine.TeachCategory(_targetObject);

            ITargetObject targetObjectOther = new TargetObject(_category,
                                                               _category.CategoryTypes[
                                                                   TestCategoryFactory.MediumCategory]);

            _category.Engine.TeachCategory(new List<ITargetObject> {targetObjectOther, _targetObject, _targetObject});

            _category.Engine.PrepareToClassification();

            Assert.AreEqual(
                _category.Engine.GetApriori(TestCategoryFactory.CategoryTypes[TestCategoryFactory.BeginnerCategory]),
                3.0/4.0);
            Assert.AreEqual(
                _category.Engine.GetApriori(TestCategoryFactory.CategoryTypes[TestCategoryFactory.MediumCategory]),
                1.0/4.0);
            Assert.AreEqual(
                _category.Engine.GetApriori(TestCategoryFactory.CategoryTypes[TestCategoryFactory.AdvancedCategory]),
                0.0/4.0);
        }

        [Test]
        public void TestCountOfVariablesAfterInit()
        {
            foreach (var attribute in TestCategoryFactory.CategoryAttributes)
            {
                foreach (var categoryType in _category.CategoryTypes)
                {
                    Assert.AreEqual(_category.Engine.GetCategoryType(categoryType)[attribute], 1);
                }
            }
        }

        [Test]
        public void TestProbability()
        {
            _targetObject.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]);
            _targetObject.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.AllMountain]);

            _category.Engine.TeachCategory(_targetObject);

            ITargetObject targetObjectOther = new TargetObject(_category,
                                                               _category.CategoryTypes[
                                                                   TestCategoryFactory.BeginnerCategory]);
            targetObjectOther.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]);
            targetObjectOther.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Freestyle]);

            _category.Engine.TeachCategory(new List<ITargetObject> {targetObjectOther, _targetObject});

            _category.Engine.PrepareToClassification();

            const double numberOfTargetObject = 3.0;
            Assert.AreEqual(
                _category.Engine.GetCategoryType(_category.CategoryTypes[TestCategoryFactory.BeginnerCategory])
                         .GetProbability(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]),
                4.0/numberOfTargetObject);
            Assert.AreEqual(
                _category.Engine.GetCategoryType(_category.CategoryTypes[TestCategoryFactory.BeginnerCategory])
                         .GetProbability(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.AllMountain]),
                3.0/numberOfTargetObject);
            Assert.AreEqual(
                _category.Engine.GetCategoryType(_category.CategoryTypes[TestCategoryFactory.BeginnerCategory])
                         .GetProbability(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Freestyle]),
                2.0/numberOfTargetObject);

            Assert.AreEqual(
                _category.Engine.GetCategoryType(_category.CategoryTypes[TestCategoryFactory.MediumCategory])
                         .GetProbability(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]),
                0.0/numberOfTargetObject);
        }

        [Test]
        public void TestReset()
        {
            _targetObject.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]);
            _targetObject.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.AllMountain]);

            _category.Engine.TeachCategory(_targetObject);

            Assert.AreEqual(
                _category.Engine.GetCategoryType(_targetObject.CategoryType)[
                    TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]], 2);
            Assert.AreEqual(
                _category.Engine.GetCategoryType(_targetObject.CategoryType)[
                    TestCategoryFactory.CategoryAttributes[TestCategoryFactory.AllMountain]], 2);

            _category.Engine.Reset();

            foreach (var attribute in TestCategoryFactory.CategoryAttributes)
            {
                foreach (var categoryType in _category.CategoryTypes)
                {
                    Assert.AreEqual(_category.Engine.GetCategoryType(categoryType)[attribute], 1);
                }
            }
        }

        [Test]
        public void TestTeachCategoryByTargetObject()
        {
            _targetObject.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]);
            _targetObject.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.AllMountain]);

            _category.Engine.TeachCategory(_targetObject);

            Assert.AreEqual(
                _category.Engine.GetCategoryType(_targetObject.CategoryType)[
                    TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]], 2);
            Assert.AreEqual(
                _category.Engine.GetCategoryType(_targetObject.CategoryType)[
                    TestCategoryFactory.CategoryAttributes[TestCategoryFactory.AllMountain]], 2);

            ITargetObject targetObjectOther = new TargetObject(_category, _category.CategoryTypes[0]);
            targetObjectOther.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]);
            targetObjectOther.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Freestyle]);
            targetObjectOther.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Freeride]);

            _category.Engine.TeachCategory(new List<ITargetObject> {targetObjectOther, _targetObject});

            Assert.AreEqual(
                _category.Engine.GetCategoryType(_targetObject.CategoryType)[
                    TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Freestyle]], 2);
            Assert.AreEqual(
                _category.Engine.GetCategoryType(_targetObject.CategoryType)[
                    TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Freeride]], 2);
            Assert.AreEqual(
                _category.Engine.GetCategoryType(_targetObject.CategoryType)[
                    TestCategoryFactory.CategoryAttributes[TestCategoryFactory.AllMountain]], 3);
            Assert.AreEqual(
                _category.Engine.GetCategoryType(_targetObject.CategoryType)[
                    TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]], 4);
            Assert.AreEqual(
                _category.Engine.GetCategoryType(_targetObject.CategoryType)[
                    TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Expensive]], 1);
        }

        [Test]
        [ExpectedException("NaiveBayes.Variables.CannotTeachByTargetObjectToClassifyException")]
        public void TestTeachingByObjectToClassify()
        {
            ITargetObject targetObjectOther = new TargetObject(_category, String.Empty);
            _category.Engine.TeachCategory(targetObjectOther);
        }
    }
}