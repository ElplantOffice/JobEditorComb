
using Messages;
using System;

namespace Patterns.EventAggregator
{
  public class Subscription<TMessage> : ISubscription<TMessage>, IDisposable where TMessage : IEventMessage
  {
    public System.Action<TMessage> Action { get; private set; }

    public IEventAggregator EventAggregator { get; private set; }

    public string Target { get; private set; }

    public Subscription(IEventAggregator eventAggregator, System.Action<TMessage> action, string target)
    {
      if (eventAggregator == null)
        throw new ArgumentNullException(nameof (eventAggregator));
      if (action == null)
        throw new ArgumentNullException(nameof (action));
      if (target == null)
        throw new ArgumentNullException(nameof (target));
      if (string.IsNullOrWhiteSpace(target))
        throw new ArgumentException(nameof (target));
      this.EventAggregator = eventAggregator;
      this.Action = action;
      this.Target = target;
    }

    public void Dispose()
    {
      this.Dispose(true);
      GC.SuppressFinalize((object) this);
    }

    protected virtual void Dispose(bool disposing)
    {
      if (!disposing)
        return;
      this.EventAggregator.UnSubscribe<TMessage>((ISubscription<TMessage>) this);
    }
  }
}
