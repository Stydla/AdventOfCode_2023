using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_20.Modules
{
  internal class ButtonModule : Module
  {

    

    public ButtonModule(string name) : base(name)
    {
      ModuleType = EModuleType.Button;
    }

    public override List<PulseMessage> Execute(PulseMessage pm)
    {
      List<PulseMessage> ret = new List<PulseMessage>();
      ret.Add(new PulseMessage(EPulse.Low, this, TargetModules[0]));
      
      return ret;
    }


    internal override string GetCustomHash()
    {
      return "";
    }
  }
}
