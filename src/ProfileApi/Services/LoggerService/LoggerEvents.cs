namespace ProfileApi.Services.LoggerService
{
    public class LoggingEvents
    {
        public const int ReadItem = 1000;
        public const int ReadItemFailed = 1100;
        public const int CreateItem = 1001;
        public const int CreateItemFailed = 1101;
        public const int UpdateItem = 1002;
        public const int UpdateItemFailed = 1102;
        public const int DeleteItem = 1003;
        public const int DeleteItemFailed = 1103;
        public const int SearchItem = 1004;
        public const int SearchItemFailed = 1104;
        public const int InputError = 4001;
        public const int UnkownError = 5000;
    }
}