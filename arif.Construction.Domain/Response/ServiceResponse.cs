using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace arif.Construction.Domain.Response
{
    public class ServiceResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Payload { get; set; }

        public static ServiceResponse SuccessResponse()
        {
            return new ServiceResponse { Success = true };
        }
        public static ServiceResponse SuccesWithPayload(object payload)
        {
            return new ServiceResponse { Success = true, Payload = payload };
        }
        public static ServiceResponse ErrorResponse(string Message) { 
            return new ServiceResponse() { Message = Message, Success = false };
        }
    }
}
