using System.Web;

namespace SCP.Web.Models
{
    public class OutputEncodingModel
    {
        public string Subject { get; set; } = "Hacking";

        public string Message { get; set; } =
            "<iframe src=\"http://www.w3schools.com/\" width=\"100%\" height=\"400px\"></iframe>";

        public string Path { get; set; } = HttpRuntime.AppDomainAppPath;

        public string CreditCardNumber { get; set; } = "5311492130660972";

    }
}