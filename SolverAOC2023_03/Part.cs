using AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_03
{
  internal class Part
  {
    public int Number { get; set; }
    public char Symbol { get; set; }

    public Point2D SymbolPosition { get; }
    public Part(int number, char symbol, Point2D symbolPosition)
    {
      Number = number;
      Symbol = symbol;
      SymbolPosition = symbolPosition;
    }

    public override string ToString()
    {
      return $"{Symbol} {Number}";
    }
  }
}
