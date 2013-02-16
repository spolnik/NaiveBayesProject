using System.Collections.Generic;
using NaiveBayes.Category;
using NaiveBayes.Variables;

namespace NaiveBayes.Classification
{
    public interface IClassifer
    {
        void Init(ICategory category);
        Dictionary<string, double> GetClassification(ITargetObject targetObject);
    }
}