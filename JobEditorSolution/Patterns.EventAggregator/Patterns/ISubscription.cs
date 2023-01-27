
using Messages;
using System;

namespace Patterns.EventAggregator
{
  public interface ISubscription<TEventMessage> : IDisposable where TEventMessage : IEventMessage
  {
    System.Action<TEventMessage> Action { get; }

    IEventAggregator EventAggregator { get; }

    string Target { get; }
  }
}
