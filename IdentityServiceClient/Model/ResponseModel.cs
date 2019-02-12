using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace IdentityServiceClient.Model
{
    public class ResponseModel<T>
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
        public T Payload { get; set; }
    }
}
