using System;
using System.Runtime.Serialization;

namespace NaiveBayes.Variables
{
    public class CannotTeachByTargetObjectToClassifyException : Exception
    {
        public CannotTeachByTargetObjectToClassifyException(string message) : base(message)
        {
        }

        public CannotTeachByTargetObjectToClassifyException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CannotTeachByTargetObjectToClassifyException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}