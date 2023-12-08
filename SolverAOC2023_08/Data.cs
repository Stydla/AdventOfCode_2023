using AoCLib;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SolverAOC2023_08
{
  internal class Data : DataBase
  {

    public List<Node> Nodes = new List<Node>();

    public List<char> Path = new List<char>();


    

    public Data(string input) : base(input)
    {

      Path.AddRange(Lines[0]);

      for(int i = 2; i < Lines.Count; i++)
      {
        Regex reg = new Regex("([A-Z\\d]{3}) = \\(([A-Z\\d]{3}), ([A-Z\\d]{3})\\)");
        Match m = reg.Match(Lines[i]);
        string nameCurrent = m.Groups[1].Value;
        string nameL = m.Groups[2].Value;
        string nameR = m.Groups[3].Value;
        Node nodeCurrent = GetOrCreate(nameCurrent);
        Node nodeL = GetOrCreate(nameL);
        Node nodeR = GetOrCreate(nameR);
        nodeCurrent.R = nodeR;
        nodeCurrent.L = nodeL;
      }

      // optionaly parse data
    }

    private Node GetOrCreate(string nodeName)
    {
      Node n = Nodes.FirstOrDefault(x=>x.Name == nodeName);
      if(n == null)
      {
        n = new Node(nodeName);
        Nodes.Add(n);
      }
      return n;
    }

    public object Solve1()
    {
      Node current = GetOrCreate("AAA");

      int step = 0;
      while(current.Name != "ZZZ")
      {
        char dir = Path[step % Path.Count];

        if(dir == 'R')
        {
          current = current.R;
        } else
        {
          current = current.L;
        }
        step++;
      }
      return step;
    }

    public object Solve2()
    {
      List<RepeatData> repeats = new List<RepeatData>();

      List <Node> nodes = Nodes.Where(x=>x.IsStartNode).ToList();

      
      foreach (Node node in nodes)
      {
        RepeatData data = new RepeatData();
        repeats.Add(data);
        data.StartNode = node;

        Node current = node;
        int step = 0;
        while(data.Length == -1)
        {
          if (current.IsEndNode)
          {
            if (data.Offset == -1)
            {
              data.Offset = step;
              data.EndNode = current;
            }
            else
            {
              data.Length = step - data.Offset;
              if (data.EndNode != current) throw new Exception("Not repeating!!!");
              break;
            }
          }

          char dir = Path[step % Path.Count];

          if (dir == 'R')
          {
            current = current.R;
          }
          else
          {
            current = current.L;
          }

        
          step++;
        }
      }

      ulong res = 1;
      foreach(RepeatData data in repeats)
      {
        ulong gcd = ModuloMath.GCD(res, (ulong)data.Length);
        res *= (ulong)data.Length;
        res /= gcd;
      }

      return res;
    }


  }
}
