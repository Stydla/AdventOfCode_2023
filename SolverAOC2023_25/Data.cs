using AoCLib.BFS;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_25
{
  internal class Data : DataBase
  {

    public Dictionary<string, Component> AllComponents { get; set; } = new Dictionary<string, Component>();

    public List<Wire> Wires { get; set; } = new List<Wire>();


    public Data(string input) : base(input)
    {
      // optionaly parse data

      foreach(string line in Lines)
      {
        string name1 = line.Substring(0, 3);
        string[] items = line.Substring(5).Split(' ').Select(x=>x.Trim()).ToArray();

        Component cSource = GetOrCreate(name1);

        foreach(string item in items)
        {
          Component cTarget = GetOrCreate(item);
          Wire w = new Wire(cSource, cTarget);
          Wires.Add(w);
          cSource.Wires.Add(w);
          cTarget.Wires.Add(w);
        }
      }
    }

    public Component GetOrCreate(string componentName)
    {
      if(!AllComponents.ContainsKey(componentName))
      {
        Component c = new Component(componentName);
        AllComponents.Add(componentName, c);
      }
      return AllComponents[componentName];
    }

    public object Solve1()
    {

      HashSet<Wire> removedWires = new HashSet<Wire>();

      for(int i = 0; i < Wires.Count; i++)
      {
        Wire wire = Wires[i];
        Component componentFrom = wire.Component1;
        Component componentTo = wire.Component2;


        BFSContext context = new BFSContext();
        context.RemovedWires.Add(wire);

        for(int j = 0; j < 3; j++)
        {
          BFS<BFSContext> bfs = new BFS<BFSContext>(componentFrom, context);
          if(bfs.IsReachable(componentTo))
          {
            List<IBFSNode<BFSContext>> nodes = bfs.GetPath(componentTo).ToList();
            List<Wire> wires = GetWires(nodes);
            foreach (Wire w in wires)
            {
              context.RemovedWires.Add(w);
            }
          } else
          {
            removedWires.Add(wire);
            break;
          }
         
        }
      }

      List<Component> componentList = AllComponents.Values.ToList();

      HashSet<Component> notVisited = new HashSet<Component>(componentList);
      List<HashSet<Component>> groups = new List<HashSet<Component>>();

      while (notVisited.Count > 0)
      {
        Component component = notVisited.First();
        HashSet<Component> group = new HashSet<Component>();
        component.GetGroup(group, removedWires);
        groups.Add(group);
        foreach (Component visited in group)
        {
          notVisited.Remove(visited);
        }
      }

      if (groups.Count == 2)
      {
        return groups[0].Count * groups[1].Count;
      }


      return 0;
    }

    private List<Wire> GetWires(List<IBFSNode<BFSContext>> nodes)
    {
      if(nodes.Count == 2)
      {
        return new List<Wire>();
      }

      List<Wire> ret = new List<Wire>();
      for(int i = 0; i < nodes.Count - 1; i++)
      {
        Component c1 = (Component)nodes[i];
        Component c2 = (Component)nodes[i + 1];
        
        ret.Add(c1.Wires.First(x=>(x.Component1 == c1 && x.Component2 == c2) || (x.Component2 == c1 && x.Component1 == c2)));

      }
      return ret;
    }

    public object Solve2()
    {
      return 50;
    }


  }
}
