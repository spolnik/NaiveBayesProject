using System.Collections.Generic;

namespace NaiveBayes.Category
{
    public interface ICategory
    {
        string Name { get; }

        ICategoryEngine Engine { get; }

        List<string> CategoryTypes { get; }
        List<string> Attributes { get; }
    }
}