using NaiveBayes.Category;
using NaiveBayes.Variables;
using NUnit.Framework;

namespace TestNaiveBayes
{
    [TestFixture]
    public class TestTargetObject
    {
        private ITargetObject _targetObject;

        private ICategory _category;

        [SetUp]
        public void SetUp()
        {
            CategoryFactory.AddCategoryType(TestCategoryFactory.CategoryName, TestCategoryFactory.CategoryTypes,
                                            TestCategoryFactory.CategoryAttributes);

            this._category = CategoryFactory.GetCategory(TestCategoryFactory.CategoryName);

            this._targetObject = new TargetObject(this._category, this._category.CategoryTypes[TestCategoryFactory.BeginnerCategory]);
        }

        [TearDown]
        public void TearDown()
        {
            CategoryFactory.RemoveCategory(TestCategoryFactory.CategoryName);
        }

        [Test]
        public void TestAfterInit()
        {
            Assert.AreEqual(this._targetObject.CategoryType, TestCategoryFactory.CategoryTypes[TestCategoryFactory.BeginnerCategory]);

            foreach (string variable in this._targetObject.AllAttributes.Keys)
            {
                Assert.IsFalse(this._targetObject.AllAttributes[variable]);
            }

            Assert.AreEqual(this._targetObject.AllAttributes.Count, TestCategoryFactory.CategoryAttributes.Length);
            Assert.AreEqual(this._targetObject.ExistsAttributes.Count, 0);
        }

        [Test]
        public void TestSetUpSomeAttribute()
        {
            this._targetObject.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]);
            this._targetObject.SetAttributeExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Freestyle]);

            Assert.AreEqual(this._targetObject.ExistsAttributes.Count, 2);

            this._targetObject.SetAttributeNotExist(TestCategoryFactory.CategoryAttributes[TestCategoryFactory.Cheap]);

            Assert.AreEqual(this._targetObject.ExistsAttributes.Count, 1);
        }
    }
}