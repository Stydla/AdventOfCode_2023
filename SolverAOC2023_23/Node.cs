using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_23
{
  internal class Node
  {

    public List<Edge> Edges { get; } = new List<Edge>();

    public int ID { get; }

    public Node(int nodeId)
    {
      ID = nodeId;
    }

    public void Solve(int dist, ref int maxDist, Node target, HashSet<Node> visited)
    {
      if(this == target)
      {
        if(maxDist < dist) maxDist = dist;
        return;
      }

      visited.Add(this);
      foreach(Edge e in Edges)
      {
        Node other = e.Node1 == this ? e.Node2 : e.Node1;
        if (visited.Contains(other)) continue;
        other.Solve(dist + e.Distance, ref maxDist, target, visited);
      }
      visited.Remove(this);
    }

  }
}
