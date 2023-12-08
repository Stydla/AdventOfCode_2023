using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_08
{
  internal class Node
  {

    public Node R { get; set; }
    public Node L { get; set; }


    public string Name { get; }

    public bool IsStartNode { get; }
    public bool IsEndNode { get; }

    public Node(string name) 
    {
      Name = name;
      IsStartNode = name.EndsWith("A");
      IsEndNode = name.EndsWith("Z");
    }


  }
}
