using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SolverAOC2023_04
{
  internal class Card
  {

    public int CardNumber { get; set; }
    public List<int> Winning { get; } = new List<int>(); 
    public List<int> MyNumbers { get; } = new List<int>();

    public Card(string input) 
    {

      Regex r = new Regex("Card[ ]* (\\d+):(.*)[|](.*)");
      Match m = r.Match(input);
      if (!m.Success) throw new Exception($"Not mathces! {input}");

      CardNumber = int.Parse(m.Groups[1].Value);

      string[] vals = m.Groups[2].Value.Split(new string[] {" " } , StringSplitOptions.RemoveEmptyEntries);
      Winning.AddRange(GetNumbers(vals));

      vals = m.Groups[3].Value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
      MyNumbers.AddRange(GetNumbers(vals));

    }

    private List<int> GetNumbers(string[] vals)
    {
      List<int> numbers = new List<int>();  
      foreach (string val in vals)
      {
        string trimmed = val.Trim();
        numbers.Add(int.Parse(trimmed));
      }
      return numbers;
    }

    public int GetScore()
    {
      int cnt = Winning.Join(MyNumbers, x => x, y => y, (a, b) => a).Count();
      if(cnt >0)
      {
        return (int)Math.Pow(2, cnt -1);
      }
      return 0;      
    }

    public List<int> GetScratchcards()
    {
      List<int> cards = new List<int>();
      int cnt = Winning.Join(MyNumbers, x => x, y => y, (a, b) => a).Count();
      for (int i = 0; i < cnt; i++)
      {
        cards.Add(CardNumber + i +1);
      }
      return cards;
    }


  }
}
