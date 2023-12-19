using AoCLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_17
{
  internal class PathStep
  {

    public Field Field { get; }
    public EDirection4 Direction { get; }

    public PathStep(EDirection4 dir, Field field)
    {
      Direction = dir;
      Field = field;
    }

  }
}
