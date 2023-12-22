using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_20.Modules
{
  internal class ConjunctionModule : Module
  {

    public Dictionary<Module, EPulse> Inputs { get; } = new Dictionary<Module, EPulse>();

    public ConjunctionModule(string name) : base(name)
    {
      ModuleType = EModuleType.Conjunction;
    }

    private EPulse PreviousPulse = EPulse.Low;

    public override List<PulseMessage> Execute(PulseMessage pm)
    {
      Inputs[pm.ModuleFrom] = pm.Pulse;
      EPulse currentPulse = Inputs.Values.All(x => x == EPulse.High) ? EPulse.Low : EPulse.High;


      List<PulseMessage> ret = new List<PulseMessage>();
      foreach (Module m in TargetModules)
      {
        ret.Add(new PulseMessage(currentPulse, this, m));
      }
      return ret;
    }

    internal override string GetCustomHash()
    {
      return "";
    }

  }
}
