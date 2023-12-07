using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_07
{
  internal class Data : DataBase
  {

    public List<Hand> Hands = new List<Hand>();

    public Data(string input) : base(input)
    {
      foreach(string line in Lines)
      {
        Hand h = new Hand(line);
        Hands.Add(h);
      }
      // optionaly parse data
    }

    public object Solve1()
    {

      List<Hand> hands = Hands.OrderBy(x => x.HandType).ThenBy(x=>x.CardsValue).ToList();

      long res = 0;
      for(int i = 0; i < hands.Count; i++)
      {
        int multiplier = hands.Count - i;
        res += multiplier * hands[i].Bid;
      }
      return res;

      
    }

    public object Solve2()
    {
      List<Hand> hands = Hands.OrderBy(x => x.BestHandType).ThenBy(x => x.CardsValueJoker).ToList();

      long res = 0;
      for (int i = 0; i < hands.Count; i++)
      {
        int multiplier = hands.Count - i;
        res += multiplier * hands[i].Bid;
      }
      return res;
    }


  }
}
