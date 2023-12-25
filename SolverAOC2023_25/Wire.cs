using AoCLib.BFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_25
{
  public class Wire
  {

    public Component Component1 { get; set; }
    public Component Component2 { get; set; } 

    public Wire(Component component1, Component component2)
    {
      Component1 = component1;
      Component2 = component2;
    }

  }
}
