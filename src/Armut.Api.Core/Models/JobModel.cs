using System;

namespace Armut.Api.Core.Models
{
    public class JobModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public int ServiceId { get; set; }

        public string Description { get; set; }

        public DateTime JobStartDateTime { get; set; }
    }
}