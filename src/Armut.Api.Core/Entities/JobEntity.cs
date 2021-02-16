using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Armut.Api.Core.Entities
{
    public class JobEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ServiceId { get; set; }

        public string Description { get; set; }

        public DateTime JobStartDateTime { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
