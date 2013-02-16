namespace NaiveBayes.Variables
{
    public interface ITargetObject
    {
        IAttributesCollection AllAttributes { get; }
        IAttributesCollection ExistsAttributes { get; }

        string CategoryType { get; }

        void SetAttributeExist(string name);
        void SetAttributeNotExist(string name);
    }
}