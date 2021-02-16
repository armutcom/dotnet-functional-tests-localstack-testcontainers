namespace Armut.Api.Core.Models
{
    public class AddJobQuoteViewModel
    {
        public int UserId { get; set; }

        public int ProviderId { get; set; }

        public double QuotePrice { get; set; }
    }
}