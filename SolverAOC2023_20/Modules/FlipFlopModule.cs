using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_20.Modules
{
  internal class FlipFlopModule : Module
  {

    
    public EFlipFlopValue Value { get; private set; } = EFlipFlopValue.OFF;

    public FlipFlopModule(string name) : base(name)
    {
      ModuleType = EModuleType.FlipFlop;
    }

    public override List<PulseMessage> Execute(PulseMessage pm)
    {
      if(pm.Pulse == EPulse.High) return new List<PulseMessage>();

      Value = Value == EFlipFlopValue.OFF ? EFlipFlopValue.ON : EFlipFlopValue.OFF;

      List<PulseMessage> ret = new List<PulseMessage>();
      foreach(Module m in TargetModules)
      {
        EPulse p = Value == EFlipFlopValue.ON ? EPulse.High : EPulse.Low;
        ret.Add(new PulseMessage(p, this, m));
      }

      return ret;
    }

    internal override string GetCustomHash()
    {
      return Value == EFlipFlopValue.ON ? "I" : "O";
    }

  }

  public enum EFlipFlopValue
  {
    ON,
    OFF
  }
}
