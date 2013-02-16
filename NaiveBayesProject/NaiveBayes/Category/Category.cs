using System.Collections.Generic;

namespace NaiveBayes.Category
{
    public class Category : ICategory
    {
        internal Category(string name, IEnumerable<string> categoryTypes, IEnumerable<string> attributes)
        {
            this.Name = name;
            this.Engine = new CategoryEngine(new List<string>(categoryTypes), new List<string>(attributes));
        }

        #region ICategory Members

        public ICategoryEngine Engine { get; private set; }

        public List<string> CategoryTypes
        {
            get { return ((CategoryEngine) this.Engine).CategoryTypes; }
        }

        public List<string> Attributes
        {
            get { return ((CategoryEngine) this.Engine).Attributes; }
        }

        public string Name { get; private set; }

        #endregion
    }
}