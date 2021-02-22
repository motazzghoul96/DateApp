namespace API.Errors
{
    public class ApiExeptions
    {
        public ApiExeptions(int satusCode, string message=null, string details=null)
        {
            SatusCode = satusCode;
            Message = message;
            Details = details;
        }

        public int SatusCode { get; set; }
        public string Message { get; set; }
        public string Details { get; set; }
    }
}