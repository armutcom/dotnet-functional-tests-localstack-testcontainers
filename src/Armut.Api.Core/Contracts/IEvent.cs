using System;

namespace Armut.Api.Core.Contracts
{
    public interface IEvent
    {
        string Sender { get; set; }

        DateTime CreateDate { get; set; }
    }
}