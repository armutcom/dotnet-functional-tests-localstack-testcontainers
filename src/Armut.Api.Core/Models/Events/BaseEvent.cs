using System;
using Armut.Api.Core.Contracts;

namespace Armut.Api.Core.Models.Events
{
    public abstract class BaseEvent<TModel> : IEvent where TModel : class, new()
    {
        public string Sender { get; set; }

        public TModel Payload { get; set; }

        public DateTime CreateDate { get; set; }
    }
}