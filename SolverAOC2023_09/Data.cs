using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_09
{
  internal class Data : DataBase
  {

    public List<Sequence> Sequences = new List<Sequence>();

    public Data(string input) : base(input)
    {
      foreach(string line in Lines)
      {
        Sequence s = new Sequence(line);
        Sequences.Add(s);
      }
    }

    public object Solve1()
    {
      long sum = 0;
      foreach (Sequence s in Sequences)
      {
        s.GenerateNext();
        sum += s.Numbers.Last();
      }
      return sum;      
    }

    public object Solve2()
    {
      long sum = 0;
      foreach (Sequence s in Sequences)
      {
        s.GeneratePrev();
        sum += s.Numbers.First();
      }
      return sum;
    }


  }
}
