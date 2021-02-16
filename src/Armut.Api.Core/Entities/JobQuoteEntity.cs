using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Armut.Api.Core.Entities
{
    public class JobQuoteEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int JobId { get; set; }

        public int UserId { get; set; }

        public int ProviderId { get; set; }

        public double QuotePrice { get; set; }

        public DateTime CreateDate { get; set; }
    }
}