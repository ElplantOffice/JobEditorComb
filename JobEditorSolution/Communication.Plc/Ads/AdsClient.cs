
using System.Collections.Generic;
using System.IO;
using TwinCAT.Ads;

namespace Communication.Plc.Ads
{
  public class AdsClient
  {
    private List<int> callBackHandles;

    public TcAdsClient TcAdsClient { get; private set; }

    public AdsClientEvents Events { get; set; }

    public bool IsConnected { get; set; }

    public AdsClient()
    {
      this.TcAdsClient = new TcAdsClient();
      this.Events = new AdsClientEvents(this.TcAdsClient);
      this.callBackHandles = new List<int>();
    }

    public void Connect(AmsAddress amsAddress)
    {
      this.TcAdsClient.Connect(amsAddress.NetId, amsAddress.Port);
      this.IsConnected = this.TcAdsClient.IsConnected;
    }

    public void Write<TType>(long group, long offset, TType value)
    {
      try
      {
        this.TcAdsClient.WriteAny(group, offset, (object) value);
      }
      catch (AdsException ex)
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs(ex));
      }
      catch
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs());
      }
    }

    public void Write<TType>(TcAdsSymbolInfo symbol, TType value)
    {
      try
      {
        this.TcAdsClient.WriteAny(symbol.IndexGroup, symbol.IndexOffset, (object) value);
      }
      catch (AdsException ex)
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs(ex));
      }
      catch
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs());
      }
    }

    public TType Read<TType>(long group, long offset)
    {
      TType type = default (TType);
      try
      {
        type = (TType) this.TcAdsClient.ReadAny(group, offset, typeof (TType));
      }
      catch (AdsException ex)
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs(ex));
      }
      catch
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs());
      }
      return type;
    }

    public TType Read<TType>(TcAdsSymbolInfo symbol)
    {
      TType type = default (TType);
      try
      {
        type = (TType) this.TcAdsClient.ReadAny(symbol.IndexGroup, symbol.IndexOffset, typeof (TType));
      }
      catch (AdsException ex)
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs(ex));
      }
      catch
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs());
      }
      return type;
    }

    public void WriteArray<TType>(long group, long offset, TType[] array)
    {
      try
      {
        this.TcAdsClient.WriteAny(group, offset, (object) array);
      }
      catch (AdsException ex)
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs(ex));
      }
      catch
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs());
      }
    }

    public void WriteArray<TType>(TcAdsSymbolInfo symbol, TType[] array)
    {
      try
      {
        this.TcAdsClient.WriteAny(symbol.IndexGroup, symbol.IndexOffset, (object) array);
      }
      catch (AdsException ex)
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs(ex));
      }
      catch
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs());
      }
    }

    public TType[] ReadArray<TType>(TcAdsSymbolInfo symbol, int count)
    {
      TType[] typeArray = new TType[count];
      try
      {
        typeArray = (TType[]) this.TcAdsClient.ReadAny(symbol.IndexGroup, symbol.IndexOffset, typeof (TType[]), new int[1]
        {
          count
        });
      }
      catch (AdsException ex)
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs(ex));
      }
      catch
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs());
      }
      return typeArray;
    }

    public void WriteBlock(TcAdsSymbolInfo symbol, AdsStream stream)
    {
      try
      {
        this.TcAdsClient.Write(symbol.IndexGroup, symbol.IndexOffset, stream);
      }
      catch (AdsException ex)
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs(ex));
      }
      catch
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs());
      }
    }

    public AdsBinaryReader ReadBlock(int indexGroup, int indexOffset, uint size)
    {
      AdsBinaryReader adsBinaryReader = (AdsBinaryReader) null;
      try
      {
        adsBinaryReader = new AdsBinaryReader(new AdsStream((int) size));
        this.TcAdsClient.Read(indexGroup, indexOffset, (AdsStream) ((BinaryReader) adsBinaryReader).BaseStream, 0, (int) size);
      }
      catch (AdsException ex)
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs(ex));
      }
      catch
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs());
      }
      return adsBinaryReader;
    }

    public int HookCallBack<TType>(TcAdsSymbolInfo symbol)
    {
      int num = 0;
      try
      {
        num = this.TcAdsClient.AddDeviceNotificationEx(symbol.IndexGroup, symbol.IndexOffset, (AdsTransMode) 4, 10, 0, (object) symbol, typeof (TType));
        this.callBackHandles.Add(num);
      }
      catch (AdsException ex)
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs(ex));
      }
      catch
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs());
      }
      return num;
    }

    public TcAdsSymbolInfo QuerySymbolsFlat(AdsClient.Query query, TcAdsSymbolInfoCollection symbolCollection, string name = "", string type = "", long group = 0, long offset = 0)
    {
      if (symbolCollection == null)
        return (TcAdsSymbolInfo) null;
      for (int index = 0; index < symbolCollection.Count; ++index)
      {
        TcAdsSymbolInfo symbol = symbolCollection[index];
        if (query(symbol, name, type, group, offset))
          return symbol;
      }
      return (TcAdsSymbolInfo) null;
    }

    public TcAdsSymbolInfo QuerySymbols(AdsClient.Query query, TcAdsSymbolInfoCollection symbolCollection, string name = "", string type = "", long group = 0, long offset = 0)
    {
      if (symbolCollection == null)
        return (TcAdsSymbolInfo) null;
      for (int index = 0; index < symbolCollection.Count; ++index)
      {
        TcAdsSymbolInfo symbol = symbolCollection[index];
        if (this.QuerySymbols(query, ref symbol, name, type, group, offset))
          return symbol;
      }
      return (TcAdsSymbolInfo) null;
    }

    public bool QuerySymbols(AdsClient.Query query, ref TcAdsSymbolInfo symbol, string name = "", string type = "", long group = 0, long offset = 0)
    {
      if (symbol == null)
        return false;
      if (query(symbol, name, type, group, offset))
        return true;
      for (TcAdsSymbolInfo symbol1 = symbol.FirstSubSymbol; symbol1 != null; symbol1 = symbol1.NextSymbol)
      {
        if (this.QuerySymbols(query, ref symbol1, name, type, group, offset))
        {
          symbol = symbol1;
          return true;
        }
      }
      return false;
    }

    public bool Equals(TcAdsSymbolInfo symbol, string name = "", string type = "", long group = 0, long offset = 0)
    {
      if (string.IsNullOrEmpty(type) && string.IsNullOrEmpty(name))
        return (group != 0L || offset != 0L) && (symbol.IndexGroup == group && symbol.IndexOffset == offset);
      if (string.IsNullOrEmpty(name))
        return string.Compare(symbol.Type, type) == 0;
      bool flag = false;
      string shortName = symbol.ShortName;
      char[] chArray = new char[1]{ '.' };
      foreach (string strA in shortName.Split(chArray))
      {
        if (string.Compare(strA, name) == 0)
        {
          flag = true;
          break;
        }
      }
      if (string.IsNullOrEmpty(type))
        return flag;
      if (flag)
        return string.Compare(symbol.Type, type) == 0;
      return false;
    }

    public bool Contains(TcAdsSymbolInfo symbol, string name = "", string type = "", long group = 0, long offset = 0)
    {
      if (string.IsNullOrEmpty(type) && string.IsNullOrEmpty(name))
        return (group != 0L || offset != 0L) && (symbol.IndexGroup == group && symbol.IndexOffset == offset);
      if (string.IsNullOrEmpty(type))
        return symbol.Name.EndsWith(name);
      if (string.IsNullOrEmpty(name) || symbol.Name.EndsWith(name))
        return symbol.Type.EndsWith(type);
      return false;
    }

    public TcAdsSymbolInfoCollection GetAllSymbols()
    {
      TcAdsSymbolInfoCollection symbolInfoCollection = (TcAdsSymbolInfoCollection) null;
      try
      {
        return this.TcAdsClient.CreateSymbolInfoLoader().GetSymbols(true);
      }
      catch (AdsException ex)
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs(ex));
        return symbolInfoCollection;
      }
      catch
      {
        this.Events.RaiseOnFault((object) this, new AdsClientEvents.AdsClientFaultEventArgs());
        return symbolInfoCollection;
      }
    }

    public delegate bool Query(TcAdsSymbolInfo symbol, string name = "", string type = "", long group = 0, long offset = 0);
  }
}
