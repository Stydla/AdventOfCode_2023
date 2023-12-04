using Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_03
{
  public class Program : BaseAdventSolver, IAdventSolver
  {

    public override string SolverName => "Day 3: Gear Ratios"/*TODO: Task Name*/;

    public override string InputsFolderName => "SolverAOC2023_03";

    public override string SolveTask1(string InputData)
    {
      Grid g = new Grid(InputData);
      int sum = g.Parts.Sum(x => x.Number);
      return sum.ToString();
    }

    public override string SolveTask2(string InputData)
    {
      int sum = 0;
      Grid g = new Grid(InputData);
      List<Part> parts = g.Parts.Where(x => x.Symbol == '*').ToList();
      bool change = true;
      while(change)
      {
        change = false;
        foreach(Part part in parts)
        {
          List<Part> connected = parts.Where(x => x.SymbolPosition.Equals(part.SymbolPosition)).ToList();
          if(connected.Count == 2)
          {
            sum += (connected[0].Number * connected[1].Number);
            parts.Remove(connected[0]);
            parts.Remove(connected[1]);
            change = true;
            break;
          }
        }
      }
      return sum.ToString();
    }
  }
}
