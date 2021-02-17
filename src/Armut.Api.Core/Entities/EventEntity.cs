using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Armut.Api.Core.Entities
{
    public class EventEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Sender { get; set; }

        public int EventRelationId { get; set; }

        public string EventType { get; set; }

        public string Message { get; set; }

        public DateTime CreateDate { get; set; }
    }
}