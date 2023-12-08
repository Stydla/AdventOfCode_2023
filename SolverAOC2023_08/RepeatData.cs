using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_08
{
  internal class RepeatData
  {

    public Node StartNode { get; set; }
    public Node EndNode { get; set; }
    public long Offset { get; set; } = -1;
    public long Length { get; set; } = -1;

  }
}
