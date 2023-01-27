
using Communication.Plc;
using Communication.Plc.Shared;
using System;
using System.Collections.Generic;

namespace Models
{
  public class CommandHandle : IPlcMappable
  {
    private CommandHandle entryHandle;

    public CommandHandle()
    {
      this.Alias = -1;
      this.Command = -1;
      this.Handle = 0UL;
    }

    public CommandHandle Reset()
    {
      return this.entryHandle;
    }

    public static CommandHandle Init(object rawType)
    {
            CommandHandle commandHandle = new CommandHandle();
            if (rawType is CommandHandle)
            {
                commandHandle.entryHandle = rawType as CommandHandle;
                commandHandle = commandHandle.Get(rawType);
                return commandHandle;
            }
            if (!(rawType is PlcCommandHandleRaw))
            {
                return null;
            }
            commandHandle.Map(rawType);
            CommandHandle commandHandle1 = commandHandle;
            commandHandle1.entryHandle = commandHandle1;
            return commandHandle;
        }

    public CommandHandle Get(object rawType)
    {
      if (rawType is CommandHandle)
      {
        CommandHandle commandHandle = rawType as CommandHandle;
        commandHandle.entryHandle = this.entryHandle;
        return commandHandle;
      }
      if (!(rawType is PlcCommandHandleRaw))
        return (CommandHandle) null;
      CommandHandle commandHandle1 = new CommandHandle();
      commandHandle1.entryHandle = this.entryHandle;
      commandHandle1.Map(rawType);
      return commandHandle1;
    }

    public bool Map(object rawType)
    {
      if (!(rawType is PlcCommandHandleRaw))
        return false;
      PlcCommandHandleRaw commandHandleRaw = (PlcCommandHandleRaw) rawType;
      this.Alias = Convert.ToInt32((short) commandHandleRaw.Alias);
      this.Type = Convert.ToInt32((short) commandHandleRaw.Type);
      this.Command = Convert.ToInt32((short) commandHandleRaw.Command);
      this.Handle = (ulong) commandHandleRaw.Handle;
      this.DataPntrHandle = (ulong) commandHandleRaw.DataPntrHandle;
      this.NextPntr = (PlcBaseRefPntrRaw) commandHandleRaw.NextPntr;
      this.Target = (string) commandHandleRaw.Target;
      return true;
    }

    public string Map(List<byte> dataBlock)
    {
      return (string) null;
    }

    public int Alias { get; private set; }

    public int Command { get; private set; }

    public ulong Handle { get; private set; }

    public int Type { get; private set; }

    public string Target { get; private set; }

    public ulong DataPntrHandle { get; private set; }

    public PlcBaseRefPntrRaw NextPntr { get; private set; }
  }
}
