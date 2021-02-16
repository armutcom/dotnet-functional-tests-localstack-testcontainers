namespace Armut.Api.Core.Models
{
    public class AddJobQuoteModel
    {
        public int UserId { get; set; }

        public int JobId { get; set; }

        public int ProviderId { get; set; }

        public double QuotePrice { get; set; }
    }
}