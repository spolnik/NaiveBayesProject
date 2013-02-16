using System.Collections.Generic;

namespace NaiveBayes.Category
{
    public class CategoryType : ICategoryType
    {
        private readonly List<string> _attributes;
        private readonly Dictionary<string, int> _attributesMap;
        private readonly Dictionary<string, double> _probabilityMap;

        public CategoryType(List<string> attributes)
        {
            this._attributes = attributes;

            this._attributesMap = new Dictionary<string, int>(this._attributes.Count);
            this.PrepareAttributes();

            this._probabilityMap = new Dictionary<string, double>(this._attributes.Count);
            this.PrepareProbabilities();
        }

        #region ICategoryType Members

        public void AddAttribute(string attribute)
        {
            this._attributesMap[attribute] += 1;
        }

        public int this[string attributeName]
        {
            get { return this._attributesMap[attributeName]; }
        }

        public void SetProbability(string attribute, double value)
        {
            this._probabilityMap[attribute] = value;
        }

        public double GetProbability(string attribute)
        {
            return this._probabilityMap[attribute];
        }

        #endregion

        private void PrepareProbabilities()
        {
            foreach (string attribute in this._attributes)
            {
                this._probabilityMap.Add(attribute, 0.0);
            }
        }

        private void PrepareAttributes()
        {
            foreach (string variable in this._attributes)
            {
                this._attributesMap.Add(variable, 1);
            }
        }

        internal void ResetAttribute(string attribute)
        {
            this._attributesMap[attribute] = 1;
        }
    }
}