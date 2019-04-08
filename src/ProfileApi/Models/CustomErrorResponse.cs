using System;
using Newtonsoft.Json;

namespace ProfileApi.Models
{
    public class CustomErrorResponse
    {        
        public int ErrorCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }        
    }
}