
using System;
using TwinCAT.Ads;

namespace Communication.Plc.Ads
{
    public class AdsClientEvents
    {
        private TcAdsClient tcAdsClient;

        public AdsClientEvents(TcAdsClient tcAdsClient)
        {
            this.tcAdsClient = tcAdsClient;

            tcAdsClient.AdsNotification += new AdsNotificationEventHandler((s, e) => { RaiseOnReceive(s, e); });
            tcAdsClient.AdsNotificationEx += new AdsNotificationExEventHandler((s, e) => { RaiseOnReceive(s, e); });
            tcAdsClient.AdsNotificationError += new AdsNotificationErrorEventHandler((s, e) => { RaiseOnFault(s, e); });
        }

        public event AdsClientEvents.OnStatusUpdateHandler OnStatusUpdate;


        public virtual void RaiseOnStatusUpdate(object sender, AdsClientEvents.AdsClientStateEventArgs e)
        {
            // ISSUE: reference to a compiler-generated field
            if (this.OnStatusUpdate == null)
                return;
            // ISSUE: reference to a compiler-generated field
            this.OnStatusUpdate(sender, e);
        }

        public event AdsClientEvents.OnReceiveHandler OnReceive;

        public virtual void RaiseOnReceive(object sender, AdsClientEvents.AdsClientReceiveEventArgs e)
        {
            // ISSUE: reference to a compiler-generated field
            if (this.OnReceive == null)
                return;
            // ISSUE: reference to a compiler-generated field
            this.OnReceive(sender, e);
        }

        public virtual void RaiseOnReceive(object sender, AdsNotificationEventArgs e)
        {
            // ISSUE: reference to a compiler-generated field
            if (this.OnReceive == null)
                return;
            AdsClientEvents.AdsClientReceiveEventArgs e1 = new AdsClientEvents.AdsClientReceiveEventArgs(e);
            // ISSUE: reference to a compiler-generated field
            this.OnReceive(sender, e1);
        }

        public virtual void RaiseOnReceive(object sender, AdsNotificationExEventArgs e)
        {
            // ISSUE: reference to a compiler-generated field
            if (this.OnReceive == null)
                return;
            AdsClientEvents.AdsClientReceiveEventArgs e1 = new AdsClientEvents.AdsClientReceiveEventArgs(e);
            // ISSUE: reference to a compiler-generated field
            this.OnReceive(sender, e1);
        }

        public event AdsClientEvents.OnFaultHandler OnFault;

        public virtual void RaiseOnFault(object sender, AdsClientEvents.AdsClientFaultEventArgs e)
        {
            // ISSUE: reference to a compiler-generated field
            if (this.OnFault == null)
                return;
            // ISSUE: reference to a compiler-generated field
            this.OnFault(sender, e);
        }

        public virtual void RaiseOnFault(object sender, AdsNotificationErrorEventArgs e)
        {
            // ISSUE: reference to a compiler-generated field
            if (this.OnFault == null)
                return;
            AdsClientEvents.AdsClientFaultEventArgs clientFaultEventArgs = new AdsClientEvents.AdsClientFaultEventArgs(e);
            this.RaiseOnFault(sender, e);
        }

        public class AdsClientStateEventArgs : EventArgs
        {
        }

        public delegate void OnStatusUpdateHandler(object sender, AdsClientEvents.AdsClientStateEventArgs e);

        public class AdsClientReceiveEventArgs : EventArgs
        {
            public TcAdsSymbolInfo Symbol { get; private set; }

            public AdsNotificationEventArgs Notification { get; private set; }

            public AdsNotificationExEventArgs NotificationEx { get; private set; }

            public AdsClientReceiveEventArgs()
            {
            }

            public AdsClientReceiveEventArgs(AdsNotificationEventArgs e)
            {
                this.Notification = e;
                this.Symbol = e.UserData as TcAdsSymbolInfo;
            }

            public AdsClientReceiveEventArgs(AdsNotificationExEventArgs e)
            {
                this.NotificationEx = e;
                this.Symbol = e.UserData as TcAdsSymbolInfo;
            }
        }

        public delegate void OnReceiveHandler(object sender, AdsClientEvents.AdsClientReceiveEventArgs e);

        public class AdsClientFaultEventArgs : EventArgs
        {
            private AdsException Exception;
            private AdsNotificationErrorEventArgs NotificationError;

            public AdsClientFaultEventArgs()
            {
            }

            public AdsClientFaultEventArgs(AdsException e)
            {
                this.Exception = e;
            }

            public AdsClientFaultEventArgs(AdsNotificationErrorEventArgs e)
            {
                this.NotificationError = e;
            }
        }

        public delegate void OnFaultHandler(object sender, AdsClientEvents.AdsClientFaultEventArgs e);

        public class AdsClientSymbolNotFoundException : Exception
        {
            public string SymbolName { get; private set; }

            public string SymbolTypeName { get; private set; }

            public AdsClientSymbolNotFoundException(string symbolName, string symbolTypeName)
            {
                this.SymbolName = symbolName;
                this.SymbolTypeName = symbolTypeName;
            }
        }
    }
}
