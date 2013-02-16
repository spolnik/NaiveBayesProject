using System.Collections.Generic;
using NaiveBayes.Category;
using NaiveBayes.Variables;

namespace NaiveBayes.Data
{
    public interface IDataLoader
    {
        void LoadCategory(string categoryName, string fileName);
        List<ITargetObject> LoadTeachingData(ICategory category, string fileName);
    }
}