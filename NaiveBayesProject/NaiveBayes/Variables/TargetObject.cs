using NaiveBayes.Category;

namespace NaiveBayes.Variables
{
    public class TargetObject : ITargetObject
    {
        private readonly IAttributesCollection _variablesProbabilityMap;

        public TargetObject(ICategory category, string categoryType)
        {
            this.CategoryType = categoryType;
            this._variablesProbabilityMap = new AttributesCollection();

            foreach (string variableName in category.Attributes)
            {
                this._variablesProbabilityMap.Add(variableName);
            }
        }

        #region ITargetObject Members

        public string CategoryType { get; private set; }

        public IAttributesCollection AllAttributes
        {
            get { return this._variablesProbabilityMap; }
        }

        public IAttributesCollection ExistsAttributes
        {
            get
            {
                IAttributesCollection existAttributes = new AttributesCollection();

                foreach (string key in this._variablesProbabilityMap.Keys)
                {
                    if (this._variablesProbabilityMap[key])
                    {
                        existAttributes.Add(key, this._variablesProbabilityMap[key]);
                    }
                }

                return existAttributes;
            }
        }

        public void SetAttributeExist(string name)
        {
            this.ChangeExistingOfAttribute(name, true);
        }

        public void SetAttributeNotExist(string name)
        {
            this.ChangeExistingOfAttribute(name, false);
        }

        #endregion

        private void ChangeExistingOfAttribute(string name, bool result)
        {
            if (this._variablesProbabilityMap.ContainsKey(name))
            {
                this._variablesProbabilityMap[name] = result;
            }
        }
    }
}