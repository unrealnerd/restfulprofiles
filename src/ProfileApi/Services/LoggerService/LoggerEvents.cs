namespace ProfileApi.Services.LoggerService
{
    public class LoggingEvents
    {
        public const int GetItem = 1000;        
        public const int InsertItem = 1001;
        public const int UpdateItem = 1002;
        public const int DeleteItem = 1003;
        public const int QueryItem = 1004;
        public const int InputError = 4001;
        public const int UnkownError = 5000;
    }
}