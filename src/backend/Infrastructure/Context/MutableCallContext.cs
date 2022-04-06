using Application.Common.Interfaces;
using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Infrastructure.Context
{
    [ExcludeFromCodeCoverage]
    public class MutableCallContext : ICallContext
    {
        public Guid CorrelationId { get; set; }
        public string AuthenticationType { get; set; }
        public string FunctionName { get; set; }
        public IDictionary<string, string> AdditionalProperties { get; } = new Dictionary<string, string>();
        public UserType AccesibleBy { get; set; }

        public bool UserRequiresAuthorization { get; set; }
        public string UserBearerAuthorizationToken { get; set; }

        public int UserId { get; set; }
        public string UserEmail { get; set; }
    }
}