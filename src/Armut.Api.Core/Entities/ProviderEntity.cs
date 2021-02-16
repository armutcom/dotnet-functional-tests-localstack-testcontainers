using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Armut.Api.Core.Entities
{
    public class ProviderEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public int ServiceId { get; set; }

        public string ProfilePicture { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
