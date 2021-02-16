using System;

namespace Armut.Api.Core.Models
{
    public class AddJobModel
    {
        public int UserId { get; set; }

        public int ServiceId { get; set; }

        public string Description { get; set; }

        public DateTime JobStartDateTime { get; set; }
    }
}