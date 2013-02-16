using System.Collections.Generic;

namespace NaiveBayes.Variables
{
    public interface IAttributesCollection
    {
        bool this[string key] { get; set; }

        IEnumerable<string> Keys { get; }
        int Count { get; }

        void Add(string name);
        void Add(string name, bool exist);
        bool ContainsKey(string name);
    }
}