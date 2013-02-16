using System;
using System.Collections.Generic;
using NaiveBayes.Variables;

namespace NaiveBayes.Category
{
    public class CategoryEngine : ICategoryEngine
    {
        private readonly Dictionary<string, double> _apriories;
        private readonly List<string> _attributes;
        private readonly Dictionary<string, ICategoryType> _categoryTypeAttributes;
        private readonly Dictionary<string, int> _categoryTypeEvidence;

        private readonly List<string> _categoryTypes;

        public CategoryEngine(List<string> categoryTypes, List<string> attributes)
        {
            this._categoryTypes = categoryTypes;
            this._attributes = attributes;

            this.CanBeClassify = false;

            this._apriories = new Dictionary<string, double>(this._categoryTypes.Count);

            this._categoryTypeEvidence = new Dictionary<string, int>(categoryTypes.Count);
            this.PrepareCategoryTypeEvidence();

            this._categoryTypeAttributes = new Dictionary<string, ICategoryType>(this._categoryTypes.Count);
            this.PrepareCategoryTypeAttributes();
        }

        #region ICategoryEngine Members

        public void PrepareToClassification()
        {
            this.PrepareApriori();

            this.PrepareProbabilities();

            this.CanBeClassify = true;
        }

        public bool CanBeClassify { get; private set; }

        public void TeachCategory(ITargetObject targetObject)
        {
            if (String.IsNullOrEmpty(targetObject.CategoryType))
            {
                throw new CannotTeachByTargetObjectToClassifyException("Object to classify cannot be used by teaching method.");
            }
            
            ICategoryType categoryType = this._categoryTypeAttributes[targetObject.CategoryType];
            this._categoryTypeEvidence[targetObject.CategoryType] += 1;

            foreach (string attribute in targetObject.ExistsAttributes.Keys)
            {
                categoryType.AddAttribute(attribute);
            }

            this.SetCanBeClassifyToFalse();
        }

        public void TeachCategory(List<ITargetObject> targetObjects)
        {
            foreach (ITargetObject targetObject in targetObjects)
            {
                this.TeachCategory(targetObject);
            }
        }

        public void Reset()
        {
            foreach (string categoryType in this._categoryTypes)
            {
                foreach (string attribute in this._attributes)
                {
                    ((CategoryType) this._categoryTypeAttributes[categoryType]).ResetAttribute(attribute);
                }
            }

            this.SetCanBeClassifyToFalse();
        }

        public double GetApriori(string categoryTypeName)
        {
            return this._apriories[categoryTypeName];
        }

        public ICategoryType GetCategoryType(string key)
        {
            return this._categoryTypeAttributes[key];
        }

        #endregion

        internal List<string> CategoryTypes
        {
            get
            {
                return this._categoryTypes;
            }
        }

        internal List<string> Attributes
        {
            get
            {
                return this._attributes;
            }
        }

        private void SetCanBeClassifyToFalse()
        {
            if (this.CanBeClassify)
            {
                this.CanBeClassify = false;
            }
        }

        private int GetSummaryEvidence()
        {
            int summaryEvidence = 0;

            foreach (int value in this._categoryTypeEvidence.Values)
            {
                summaryEvidence += value;
            }

            return summaryEvidence;
        }

        private void PrepareCategoryTypeEvidence()
        {
            foreach (string categoryType in this._categoryTypes)
            {
                this._categoryTypeEvidence.Add(categoryType, 0);
            }
        }

        private void PrepareCategoryTypeAttributes()
        {
            foreach (string categoryType in this._categoryTypes)
            {
                this._categoryTypeAttributes.Add(categoryType, new CategoryType(this._attributes));
            }
        }

        private void PrepareProbabilities()
        {
            foreach (string attribute in this._attributes)
            {
                this.PrepareProbabilityForAttribute(attribute);
            }
        }

        private void PrepareProbabilityForAttribute(string attribute)
        {
            foreach (string categoryType in this._categoryTypes)
            {
                int categoryTypeAttributeCount = this._categoryTypeAttributes[categoryType][attribute];
                double categoryTypeEvidence = this._categoryTypeEvidence[categoryType];

                if (categoryTypeEvidence == 0)
                {
                    this._categoryTypeAttributes[categoryType].SetProbability(attribute, 0.0);
                }
                else
                {
                    this._categoryTypeAttributes[categoryType].SetProbability(attribute,
                                                                              categoryTypeAttributeCount/
                                                                              categoryTypeEvidence);  
                }
            }
        }

        private void PrepareApriori()
        {
            int summaryEvidence = this.GetSummaryEvidence();

            foreach (string categoryType in this._categoryTypes)
            {
                this._apriories[categoryType] = this._categoryTypeEvidence[categoryType]/(double) summaryEvidence;
            }
        }
    }
}