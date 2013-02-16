using System;
using System.Runtime.Serialization;

namespace NaiveBayes.Classification
{
    public class CannotBeClassifyException : Exception
    {
        public CannotBeClassifyException(string message) : base(message)
        {
        }

        public CannotBeClassifyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CannotBeClassifyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}