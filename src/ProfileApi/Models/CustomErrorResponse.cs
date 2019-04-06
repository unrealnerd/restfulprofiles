using Newtonsoft.Json;
 
namespace ProfileApi.Models
{
    public class CustomErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string StackTrace { get; set; } 
 
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}