using System;
using System.Runtime.Serialization;

namespace SimpleSonic
{

    [Serializable()]
    public class SonicException : Exception, ISerializable
    {

        public SonicException() : base()
        { }

        public SonicException(string message) : base(message)
        { }

        public SonicException(string message, Exception inner) : base(message, inner)
        { }

        public SonicException(SerializationInfo info, StreamingContext context) :
                 base(info, context)
        { }

    }
}
