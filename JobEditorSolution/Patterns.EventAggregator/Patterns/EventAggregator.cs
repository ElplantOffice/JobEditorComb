
using Messages;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Patterns.EventAggregator
{
  public class EventAggregator : IEventAggregator
  {
    private readonly IDictionary<Type, IDictionary<string, IList>> subscriptions = (IDictionary<Type, IDictionary<string, IList>>) new Dictionary<Type, IDictionary<string, IList>>();
    private readonly IDictionary<string, Task> queuedTaskDictionary = (IDictionary<string, Task>) new Dictionary<string, Task>();

    private void Publish<TMessage>(TMessage message, ISubscription<TMessage> subscription, string queueId) where TMessage : IEventMessage
    {
      if (string.IsNullOrWhiteSpace(queueId))
      {
        Task.Factory.StartNew((Action) (() => subscription.Action(message)));
      }
      else
      {
        lock (this.queuedTaskDictionary)
        {
          Task task = (Task) null;
          if (this.queuedTaskDictionary.TryGetValue(queueId, out task))
            this.queuedTaskDictionary[queueId] = task.ContinueWith((Action<Task>) (ant => subscription.Action(message)), TaskContinuationOptions.None);
          else
            this.queuedTaskDictionary.Add(queueId, Task.Factory.StartNew((Action) (() => subscription.Action(message))));
        }
      }
    }

    public void Publish<TMessage>(TMessage message, bool includeParentTargets = true) where TMessage : IEventMessage
    {
      this.Publish<TMessage>(message, (string) null, includeParentTargets);
    }

    public void Publish<TMessage>(TMessage message, string queueId, bool includeParentTargets = true) where TMessage : IEventMessage
    {
      if ((object) message == null)
        throw new ArgumentNullException(nameof (message));
      Type key1 = typeof (TMessage);
      if (!this.subscriptions.ContainsKey(key1))
        return;
      IDictionary<string, IList> dictionary = (IDictionary<string, IList>) new Dictionary<string, IList>(this.subscriptions[key1]);
      foreach (string key2 in new ParentStringIterator(message.Address.Target, ".", false, true, false))
      {
        if (dictionary.ContainsKey(key2))
        {
          foreach (ISubscription<TMessage> subscription in new List<ISubscription<TMessage>>(dictionary[key2].Cast<ISubscription<TMessage>>()))
            this.Publish<TMessage>(message, subscription, queueId);
        }
        if (!includeParentTargets)
          break;
      }
    }

    public ISubscription<TMessage> Subscribe<TMessage>(Action<TMessage> action, string target) where TMessage : IEventMessage
    {
      Type key = typeof (TMessage);
      Subscription<TMessage> subscription = new Subscription<TMessage>((IEventAggregator) this, action, target);
      if (this.subscriptions.ContainsKey(key))
      {
        IDictionary<string, IList> dictionary = (IDictionary<string, IList>) new Dictionary<string, IList>(this.subscriptions[key]);
        if (dictionary.ContainsKey(target))
          dictionary[target].Add((object) subscription);
        else
          dictionary.Add(target, (IList) new List<ISubscription<TMessage>>()
          {
            (ISubscription<TMessage>) subscription
          });
        this.subscriptions[key] = dictionary;
      }
      else
      {
        IDictionary<string, IList> dictionary = (IDictionary<string, IList>) new Dictionary<string, IList>();
        dictionary.Add(target, (IList) new List<ISubscription<TMessage>>()
        {
          (ISubscription<TMessage>) subscription
        });
        this.subscriptions.Add(key, dictionary);
      }
      return (ISubscription<TMessage>) subscription;
    }

    public void UnSubscribe<TMessage>(ISubscription<TMessage> subscription) where TMessage : IEventMessage
    {
      Type key = typeof (TMessage);
      string target = subscription.Target;
      if (!this.subscriptions.ContainsKey(key))
        return;
      IDictionary<string, IList> dictionary = (IDictionary<string, IList>) new Dictionary<string, IList>(this.subscriptions[key]);
      if (!dictionary.ContainsKey(target))
        return;
      dictionary[target].Remove((object) subscription);
      this.subscriptions[key] = dictionary;
    }

    public void UnSubscribe<TMessage>(string Target) where TMessage : IEventMessage
    {
      Type key = typeof (TMessage);
      if (!this.subscriptions.ContainsKey(key))
        return;
      IDictionary<string, IList> dictionary = (IDictionary<string, IList>) new Dictionary<string, IList>(this.subscriptions[key]);
      if (!dictionary.ContainsKey(Target))
        return;
      dictionary[Target].Clear();
      this.subscriptions[key] = dictionary;
    }

    public void ClearAllSubscriptions()
    {
      this.ClearAllSubscriptions((Type[]) null);
    }

    public void ClearAllSubscriptions(Type[] exceptMessages)
    {
      if (exceptMessages == null)
      {
        this.subscriptions.Clear();
      }
      else
      {
        for (int index = this.subscriptions.Count - 1; index >= 0; --index)
        {
          KeyValuePair<Type, IDictionary<string, IList>> keyValuePair = this.subscriptions.ElementAt<KeyValuePair<Type, IDictionary<string, IList>>>(index);
          if (!((IEnumerable<Type>) exceptMessages).Contains<Type>(keyValuePair.Key))
            ((ICollection<KeyValuePair<Type, IDictionary<string, IList>>>) this.subscriptions).Remove(keyValuePair);
        }
      }
    }
  }
}
