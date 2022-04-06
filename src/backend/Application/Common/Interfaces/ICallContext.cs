using Domain.Enums;
using System;
using System.Collections.Generic;

namespace Application.Common.Interfaces
{
    public interface ICallContext
    {
        Guid CorrelationId { get; set; }

        string FunctionName { get; set; }
        UserType AccesibleBy { get; set; }

        string AuthenticationType { get; set; }

        IDictionary<string, string> AdditionalProperties { get; }

        bool UserRequiresAuthorization { get; set; }
        string UserBearerAuthorizationToken { get; set; }

        int UserId { get; set; }
        string UserEmail { get; set; }
    }
}