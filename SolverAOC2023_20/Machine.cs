using SolverAOC2023_20.Modules;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace SolverAOC2023_20
{
  internal class Machine
  {


    public Dictionary<string, Module> Modules { get; } = new Dictionary<string, Module>();

    public ButtonModule ButtonModule { get; }
    public BroadcastModule BroadcastModule { get; }

    public Machine(List<string> input)
    {

      foreach(string line in input)
      {
        string [] items = line.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries);
        List<string> names = items[1].Split(',').Select(x=>x.Trim()).ToList();

        if (items[0].StartsWith("&"))
        {
          string name = items[0].Substring(1);
          GetOrCreate(name, EModuleType.Conjunction);
        } else if (items[0].StartsWith("%"))
        {
          string name = items[0].Substring(1);
          GetOrCreate(name, EModuleType.FlipFlop);
        } else
        {
          GetOrCreate("broadcaster", EModuleType.Broadcast);
        }
      }
      GetOrCreate("aptly", EModuleType.Button);

      //Add links
      foreach (string line in input)
      {

        string[] items = line.Split(new string[] { " -> " }, StringSplitOptions.RemoveEmptyEntries);

        List<string> names = items[1].Split(',').Select(x => x.Trim()).ToList();

        if (items[0].StartsWith("&"))
        {
          string name = items[0].Substring(1);
          Module m = GetOrCreate(name, EModuleType.Conjunction);
          foreach(string targetName in names )
          {
            Module target = GetOrCreate(targetName, EModuleType.Conjunction);
            m.AddTarget(target);
          }

        }
        else if (items[0].StartsWith("%"))
        {
          string name = items[0].Substring(1);
          Module m = GetOrCreate(name, EModuleType.FlipFlop);
          foreach (string targetName in names)
          {
            Module target = GetOrCreate(targetName, EModuleType.Conjunction);
            m.AddTarget(target);
          }
        }
        else
        {
          Module m = GetOrCreate("broadcaster", EModuleType.Broadcast);
          foreach (string targetName in names)
          {
            Module target = GetOrCreate(targetName, EModuleType.Conjunction);
            m.AddTarget(target);
          }
        }
      }

      ButtonModule = (ButtonModule)GetOrCreate("aptly", EModuleType.Button);
      BroadcastModule = (BroadcastModule)GetOrCreate("broadcaster", EModuleType.Broadcast);
      ButtonModule.AddTarget(BroadcastModule);
      

    }

    private Module GetOrCreate(string moduleName, EModuleType type)
    {
      if(!Modules.ContainsKey(moduleName))
      {
        switch (type)
        {
          case EModuleType.Broadcast:
            Modules.Add(moduleName, new BroadcastModule(moduleName));
            break;
          case EModuleType.Button:
            Modules.Add(moduleName, new ButtonModule(moduleName));
            break;
          case EModuleType.Conjunction:
            Modules.Add(moduleName, new ConjunctionModule(moduleName));
            break;
          case EModuleType.FlipFlop:
            Modules.Add(moduleName, new FlipFlopModule(moduleName));
            break;
        }
      }
      return Modules[moduleName];
    }

    public long LowSignalCount = 0;
    public long HighSignalCount = 0;

    public void PushButton()
    {
      LinkedList<PulseMessage> currentMessages = new LinkedList<PulseMessage>();
      currentMessages.AddLast(new PulseMessage(EPulse.Low, ButtonModule, BroadcastModule));

      while (currentMessages.Count > 0)
      {
        PulseMessage currentMessage = currentMessages.First();
        if(currentMessage.Pulse == EPulse.Low)
        {
          LowSignalCount++;
        } else
        {
          HighSignalCount++;
        }
        currentMessages.RemoveFirst();
        List<PulseMessage> nextModules = currentMessage.ModuleTo.Execute(currentMessage);
        
        foreach (PulseMessage module in nextModules)
        {
          currentMessages.AddLast(module);
        }
      }
    }

    private static int index = 0;

    public bool PushButton2()
    {

      Module RXModule = Modules["rx"];
      LinkedList<PulseMessage> currentMessages = new LinkedList<PulseMessage>();
      currentMessages.AddLast(new PulseMessage(EPulse.Low, ButtonModule, BroadcastModule));

      Debug.WriteLine($"{GetCustomHash()} | {GetCustomHash().Count(x=>x== 'I')} {index++}");
      while (currentMessages.Count > 0)
      {
      
        PulseMessage currentMessage = currentMessages.First();
        if(currentMessage.ModuleTo == RXModule && currentMessage.Pulse == EPulse.Low)
        {
          return true;
        }
        currentMessages.RemoveFirst();
        List<PulseMessage> nextModules = currentMessage.ModuleTo.Execute(currentMessage);

        foreach (PulseMessage module in nextModules)
        {
          currentMessages.AddLast(module);
        }
      }
      return false;
    }


    public string GetCustomHash()
    {
      StringBuilder sb = new StringBuilder();
      foreach(Module m in Modules.Values)
      {
        sb.Append(m.GetCustomHash());
      }
      return sb.ToString();
    }

  }
}
