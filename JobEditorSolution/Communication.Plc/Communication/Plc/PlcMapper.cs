
using Communication.Plc.Shared;
using Messages;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace Communication.Plc
{
  public static class PlcMapper
  {
    public static object Map(byte[] datablock, string type)
    {
      return PlcTypeResolver.Map(datablock, type);
    }

    public static object Map<T>(byte[] datablock, string type) where T : IMappable
    {
      GCHandle gcHandle = GCHandle.Alloc((object) datablock, GCHandleType.Pinned);
      T structure = (T) Marshal.PtrToStructure(gcHandle.AddrOfPinnedObject(), typeof (T));
      gcHandle.Free();
      return (object) structure;
    }

    public static TelegramCommand MapCommand(PlcCommAddressRaw plcCommAddressRaw)
    {
      switch (plcCommAddressRaw.Command)
      {
        case 1:
          return TelegramCommand.Create;
        case 2:
          return TelegramCommand.Created;
        case 3:
          return TelegramCommand.Dispose;
        case 4:
          return TelegramCommand.Created;
        case 201:
          return TelegramCommand.Show;
        case 202:
          return TelegramCommand.Visible;
        case 203:
          return TelegramCommand.Hide;
        case 204:
          return TelegramCommand.Hidden;
        default:
          return TelegramCommand.None;
      }
    }

    public static PlcAddress Map(PlcCommAddressRaw addressRaw, string channel)
    {
      return new PlcAddress(addressRaw, channel);
    }

    public static PlcCommAddressRaw Map(Telegram telegram, string channelName)
    {
      string[] strArray1 = channelName.Split('.');
      string[] strArray2 = telegram.Address.Target.Split('.');
      StringBuilder stringBuilder1 = new StringBuilder();
      for (int index = 0; index < ((IEnumerable<string>) strArray2).Count<string>(); ++index)
      {
        if (index != 1 || strArray1[index] != strArray2[index])
          stringBuilder1.Append(strArray2[index]).Append(".");
      }
      StringBuilder stringBuilder2 = stringBuilder1;
      stringBuilder2.Remove(stringBuilder2.Length - 1, 1);
      return new PlcCommAddressRaw()
      {
        Target = {
          Name = stringBuilder1.ToString()
        },
        Command = (ushort) telegram.Command
      };
    }

    public static bool Map(object value, ref PlcBaseRefPntrRaw dataPntr, List<byte> dataBlock)
    {
      int num = 0;
      if (dataBlock != null)
        num = dataBlock.Count<byte>();
      if (value is IPlcMappable)
      {
        IPlcMappable plcMappable = value as IPlcMappable;
        dataPntr.Type = plcMappable.Map(dataBlock);
        dataPntr.Size = (uint) (dataBlock.Count<byte>() - num);
        dataPntr.Offset += (ulong) (uint) num;
        return true;
      }
      dataPntr.Type = "VOID";
      dataPntr.Size = 0U;
      dataPntr.Offset = (ulong) (uint) dataBlock.Count<byte>();
      return true;
    }

    public static byte[] GetBytes(object obj)
    {
      int length = Marshal.SizeOf(obj);
      byte[] destination = new byte[length];
      IntPtr num = Marshal.AllocHGlobal(length);
      Marshal.StructureToPtr(obj, num, true);
      Marshal.Copy(num, destination, 0, length);
      Marshal.FreeHGlobal(num);
      return destination;
    }

    public static Telegram ConvertToTelegram(PlcTelegramRaw rawMessage, BinaryReader dataBlock, string channelName)
    {
      PlcAddress plcAddress = PlcMapper.Map(rawMessage.CommAddress, channelName);
      int command = (int) PlcMapper.MapCommand(rawMessage.CommAddress);
      if (rawMessage.CommDataPntr.IsArray)
      {
        List<object> objectList = new List<object>();
        for (int index = 0; index < (int) rawMessage.CommDataPntr.Count; ++index)
        {
          byte[] datablock = dataBlock.ReadBytes((int) rawMessage.CommDataPntr.Size);
          objectList.Add(PlcMapper.Map(datablock, rawMessage.CommDataPntr.Type));
        }
        return new Telegram((IAddress) plcAddress, command, (object) objectList, (string) null);
      }
      object obj = PlcMapper.Map(dataBlock.ReadBytes((int) rawMessage.CommDataPntr.Size), rawMessage.CommDataPntr.Type);
      return new Telegram((IAddress) plcAddress, command, obj, (string) null);
    }
  }
}
