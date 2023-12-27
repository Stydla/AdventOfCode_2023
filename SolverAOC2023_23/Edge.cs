using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_23
{
  internal class Edge
  {
    public Node Node1 { get; }
    public Node Node2 { get; }  
    public int Distance { get; }

    public Edge(Node node1, Node node2, int distance)
    {
      Node1 = node1;
      Node2 = node2;
      Distance = distance;
      node1.Edges.Add(this);
      node2.Edges.Add(this);
    }

    public Node GetLinkedNode(Node node)
    {
      return node == Node1 ? Node2 : Node1;
    }
  }
}
