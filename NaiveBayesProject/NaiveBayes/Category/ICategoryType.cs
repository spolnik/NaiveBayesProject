namespace NaiveBayes.Category
{
    public interface ICategoryType
    {
        void AddAttribute(string attribute);
        int this[string attributeName] { get; }

        void SetProbability(string attribute, double value);
        double GetProbability(string attribute);
    }
}