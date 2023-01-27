
using Messages;
using Patterns.EventAggregator;
using System;

namespace Communication.Plc
{
  internal class PlcAggregator
  {
    private IEventAggregator aggregator;

    public bool IsListening { get; private set; }

    public PlcAggregator()
    {
      this.IsListening = false;
      this.aggregator = (IEventAggregator) SingletonProvider<EventAggregator>.Instance;
    }

    public void Publish(Telegram message, bool IncludeParents = false)
    {
      this.aggregator.Publish<Telegram>(message, IncludeParents);
    }

    public void Subscribe(Action<Telegram> action, string target)
    {
      this.aggregator.Subscribe<Telegram>(action, target);
      this.IsListening = true;
    }
  }
}
