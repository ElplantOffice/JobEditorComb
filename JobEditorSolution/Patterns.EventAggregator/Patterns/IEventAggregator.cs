
using Messages;
using System;

namespace Patterns.EventAggregator
{
  public interface IEventAggregator
  {
    void Publish<TMessage>(TMessage message, bool includeParentTargets = true) where TMessage : IEventMessage;

    void Publish<TMessage>(TMessage message, string queueId, bool includeParentTargets = true) where TMessage : IEventMessage;

    ISubscription<TMessage> Subscribe<TMessage>(Action<TMessage> action, string target) where TMessage : IEventMessage;

    void UnSubscribe<TMessage>(ISubscription<TMessage> subscription) where TMessage : IEventMessage;

    void UnSubscribe<TMessage>(string Target) where TMessage : IEventMessage;

    void ClearAllSubscriptions();

    void ClearAllSubscriptions(Type[] exceptMessages);
  }
}
