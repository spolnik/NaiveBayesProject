using System.Collections.Generic;

namespace NaiveBayes.Variables
{
    public class AttributesCollection : IAttributesCollection
    {
        private readonly Dictionary<string, bool> _attributes;

        public AttributesCollection()
        {
            this._attributes = new Dictionary<string, bool>();
        }

        #region IAttributesCollection Members

        public void Add(string name)
        {
            this.Add(name, false);
        }

        public void Add(string name, bool exist)
        {
            this._attributes.Add(name, exist);
        }

        public bool this[string key]
        {
            get { return this._attributes[key]; }
            set { this._attributes[key] = value; }
        }

        public IEnumerable<string> Keys
        {
            get { return this._attributes.Keys; }
        }

        public int Count
        {
            get { return this._attributes.Count; }
        }

        public bool ContainsKey(string name)
        {
            return this._attributes.ContainsKey(name);
        }

        #endregion
    }
}