using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_09
{
  internal class Sequence
  {
    public List<int> Numbers { get; } = new List<int>();
    public Sequence Parent { get; } = null;
    public Sequence SubSequence { get; } = null;

    public Sequence(string input)
    {
      string[] numsStr = input.Split(' ');
      Numbers.AddRange(numsStr.Select(x=> int.Parse(x)));

      if(!IsAllZero())
      {
        SubSequence = new Sequence(this);
      }
    }

    public Sequence(Sequence parent)
    {
      Parent = parent;

      for(int i = 0; i < parent.Numbers.Count - 1; i++) 
      { 
        int diff = parent.Numbers[i+1] - parent.Numbers[i];
        Numbers.Add(diff);
      }

      if(!IsAllZero())
      {
        SubSequence = new Sequence(this);
      }
    }

    public bool IsAllZero()
    {
      return Numbers.All(x=>x.Equals(0));
    }

    internal void GenerateNext()
    {
      if(IsAllZero())
      {
        Numbers.Add(0);
      } else
      {
        SubSequence.GenerateNext();

        int newNumber = LastNumber + SubSequence.LastNumber;
        Numbers.Add(newNumber);
      }

      
    }

    internal void GeneratePrev()
    {
      if (IsAllZero())
      {
        Numbers.Insert(0, 0);
      }
      else
      {
        SubSequence.GeneratePrev();

        int newNumber = FirstNumber - SubSequence.FirstNumber;
        Numbers.Insert(0, newNumber);
      }
    }

    public int FirstNumber
    {
      get
      {
        return Numbers[0];
      }
    }

    public int LastNumber
    {
      get
      {
        return Numbers[Numbers.Count - 1];
      }
    }
  }
}
