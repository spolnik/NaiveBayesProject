using System;
using System.Collections.Generic;
using System.Linq;
using NaiveBayes.Category;
using NaiveBayes.Variables;

namespace NaiveBayes.Classification
{
    public class Classifer : IClassifer
    {
        private ICategory _category;

        #region IClassifer Members

        public void Init(ICategory category)
        {
            if (!category.Engine.CanBeClassify)
            {
                throw new CannotBeClassifyException("Class must be prepared to classification firstly.");
            }

            this._category = category;
        }

        public Dictionary<string, double> GetClassification(ITargetObject targetObject)
        {
            if (!String.IsNullOrEmpty(targetObject.CategoryType))
            {
                throw new TargetObjectIsClassifiedBeforeException(
                    "Object not need classification because it was classified before.");
            }


            return this.EvalClassifitaction(targetObject);
        }

        #endregion

        private Dictionary<string, double> EvalClassifitaction(ITargetObject targetObject)
        {
            var classifitaction = new Dictionary<string, double>(this._category.CategoryTypes.Count);

            foreach (string categoryType in this._category.CategoryTypes)
            {
                double probability = this.EvalProbabilityForCategoryType(categoryType, targetObject.ExistsAttributes);
                probability *= this._category.Engine.GetApriori(categoryType);
                classifitaction.Add(categoryType, probability);
            }

            return this.GetSortedDictionaryByValue(classifitaction);
        }

        private Dictionary<string, double> GetSortedDictionaryByValue(Dictionary<string, double> classifitaction)
        {
            var result = new Dictionary<string, double>(this._category.CategoryTypes.Count);

            foreach (var keyValuePair in classifitaction.OrderByDescending(key => key.Value))
            {
                result.Add(keyValuePair.Key, keyValuePair.Value);
            }

            return result;
        }

        private double EvalProbabilityForCategoryType(string categoryType, IAttributesCollection attributes)
        {
            double result = 1.0;

            foreach (string attribute in attributes.Keys)
            {
                double probability = this._category.Engine.GetCategoryType(categoryType).GetProbability(attribute);
                if (probability == 0.0)
                {
                    result *= 0.00001;
                }
                else
                {
                    result *= probability;    
                }
            }

            return result;
        }
    }
}