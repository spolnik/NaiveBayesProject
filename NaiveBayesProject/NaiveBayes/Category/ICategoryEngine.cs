using System.Collections.Generic;
using NaiveBayes.Variables;

namespace NaiveBayes.Category
{
    public interface ICategoryEngine
    {
        ICategoryType GetCategoryType(string key);
        bool CanBeClassify { get; }
        void TeachCategory(ITargetObject targetObject);
        void TeachCategory(List<ITargetObject> targetObjects);

        void PrepareToClassification();

        double GetApriori(string categoryTypeName);
        void Reset();
    }
}