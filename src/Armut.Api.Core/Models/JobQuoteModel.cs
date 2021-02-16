using System;

namespace Armut.Api.Core.Models
{
    public class JobQuoteModel
    {
        public int Id { get; set; }

        public int JobId { get; set; }

        public int UserId { get; set; }

        public int ProviderId { get; set; }

        public double QuotePrice { get; set; }

        public DateTime CreateDate { get; set; }
    }
}