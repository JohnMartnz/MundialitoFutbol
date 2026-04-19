using System;
using System.Collections.Generic;
using System.Text;

namespace Mundialito.Domain.Entities
{
    public class IdempotencyRequest
    {
        public Guid Id { get; private set; }
        public string RequestBody { get; private set; } = string.Empty;
        public string ResponseBody { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }

        private IdempotencyRequest() { }

        public IdempotencyRequest(Guid id, string requestBody, string responseBody)
        {
            Id = id;
            RequestBody = requestBody;
            ResponseBody = responseBody;
            CreatedAt = DateTime.UtcNow;
        }
    }
}
