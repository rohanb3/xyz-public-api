using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace IdentityServiceClient.Models
{
    public class ResponseModel
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; }
    }

    public class ResponseModel<T> : ResponseModel
    {
        public T Payload { get; set; }
    }
}
