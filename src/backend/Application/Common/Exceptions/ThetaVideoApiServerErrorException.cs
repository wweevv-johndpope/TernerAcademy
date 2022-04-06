using System;
using System.Runtime.Serialization;

namespace Application.Common.Exceptions
{
    [Serializable]
    public class ThetaVideoApiServerErrorException : Exception
    {
        public ThetaVideoApiServerErrorException(string message)
                : base(message)
        {
        }

        protected ThetaVideoApiServerErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
