namespace NetBank.Core.Application.Dtos.Email
{
    public class EmailRequest
    {
        public string Subject { get; set; }

        public string Body { get; set; }

        public string To { get; set; }
    }
}
