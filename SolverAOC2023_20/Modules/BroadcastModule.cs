using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_20.Modules
{
  internal class BroadcastModule : Module
  {


    public EPulse Input { get; private set; }

    public BroadcastModule(string name) : base(name)
    {
      ModuleType = EModuleType.Broadcast;
    }

    public override List<PulseMessage> Execute(PulseMessage pm)
    {
      List<PulseMessage> ret = new List<PulseMessage>();

      foreach (Module m in TargetModules)
      {
        ret.Add(new PulseMessage(pm.Pulse, this, m));
      }
      return ret;
    }

    internal override string GetCustomHash()
    {
      return "";
    }

  }
}
