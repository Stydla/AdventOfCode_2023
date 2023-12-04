using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_04
{
  internal class Data : DataBase
  {


    public List<Card> Cards = new List<Card>();


    public Data(string input) : base(input)
    {
      // optionaly parse data

      foreach(string line in Lines)
      {
        Card c = new Card(line);
        Cards.Add(c);
      }
    }

    public object Solve1()
    {
      return Cards.Sum(x => x.GetScore());
     
    }

    public object Solve2()
    {
      Dictionary<int, int> cnt = new Dictionary<int, int>();
      foreach(Card c in  Cards)
      {
        cnt.Add(c.CardNumber, 1);
      }
      foreach(Card c in Cards)
      {
        int times = cnt[c.CardNumber];
        foreach(int num in c.GetScratchcards())
        {
          cnt[num] += times;
        }
      }

      return cnt.Sum(x => x.Value);
    }


  }
}
