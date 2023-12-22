using SolverAOC2023_20.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_20
{
  internal class PulseMessage
  {

    public EPulse Pulse { get; }


    public Module ModuleFrom { get; }
    public Module ModuleTo { get; }

    public PulseMessage(EPulse pulse, Module moduleFrom, Module moduleTo)
    {
      Pulse = pulse;
      ModuleFrom = moduleFrom;  
      ModuleTo = moduleTo;
    }

    public override string ToString()
    {
      return $"{ModuleFrom?.Name} -- {Pulse} --> {ModuleTo?.Name}";
    }

  }
}
