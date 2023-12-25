using AoCLib.BFS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_25
{
  public class Component : IBFSNode<BFSContext>
  {

    public List<Wire> Wires { get; set; } = new List<Wire>();

    public List<Component> ConnectedComponents = new List<Component>();

    public string Name { get; set; }


    public Component(string name)
    {
      Name = name;
    }

    public void AssignComponents()
    {
      foreach(Wire wire in Wires) 
      {
        if(wire.Component1 == this)
        {
          ConnectedComponents.Add(wire.Component2);
        }else
        {
          ConnectedComponents.Add(wire.Component1);
        }
      }

    }

    public override string ToString() 
    {
      return $"{Name}: {string.Join(", ", Wires.Select(x=>this == x.Component1 ? x.Component2.Name : x.Component1.Name))}";
    }

    internal void GetGroup(HashSet<Component> group, HashSet<Wire> removedWires)
    {
      if (group.Contains(this)) return;

      group.Add(this);

      foreach(Wire wire in Wires)
      {
        if (removedWires.Contains(wire)) continue;
        if(wire.Component1 == this)
        {
          wire.Component2.GetGroup(group, removedWires);
        } 
        else
        {
          wire.Component1.GetGroup(group, removedWires);
        }
      }
      
    }

    public IEnumerable<IBFSNode<BFSContext>> GetNeighbours(BFSContext context)
    {
      foreach(Wire wire in Wires)
      {
        if(context.RemovedWires.Contains(wire)) continue;

        if(wire.Component1 == this)
        {
          yield return wire.Component2;
        } else
        {
          yield return wire.Component1;
        }
      }
    }

    public bool IsOpen(BFSContext context)
    {
      return true;
    }
  }

  public class BFSContext
  {
    public HashSet<Wire> RemovedWires = new HashSet<Wire>();
  }
}
