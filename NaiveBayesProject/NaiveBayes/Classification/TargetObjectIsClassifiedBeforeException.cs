using System;
using System.Runtime.Serialization;

namespace NaiveBayes.Classification
{
    public class TargetObjectIsClassifiedBeforeException : Exception
    {
        public TargetObjectIsClassifiedBeforeException(string message) : base(message)
        {
        }

        public TargetObjectIsClassifiedBeforeException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected TargetObjectIsClassifiedBeforeException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}