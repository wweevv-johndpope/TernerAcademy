using System;
using System.Runtime.Serialization;

namespace Application.Common.Exceptions
{
    [Serializable]
    public class ThetaTokenExplorerServerErrorException : Exception
    {
        public ThetaTokenExplorerServerErrorException(string message)
                : base(message)
        {
        }

        protected ThetaTokenExplorerServerErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
