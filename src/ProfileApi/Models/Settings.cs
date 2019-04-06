namespace ProfileApi.Models
{
    public class Settings
    {
        public string ConnectionString { get; set; }
        public string Database { get; set; }
        public string SecretToken { get; set; }
        public string TokenIssuer { get; set; }
    }
}