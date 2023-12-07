using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_07
{
  internal class Hand
  {

    public List<char> Cards { get; } = new List<char>();
    public List<char> SortedCards { get; } = new List<char>();
    public long Bid { get; }
    public HandType HandType { get; private set; }
    public HandType BestHandType { get; private set; } = HandType.HighCard;
    public int CardsValue
    {
      get
      {
        int res = 0;
        for(int i = 0; i < Cards.Count; i++)
        {
          res = res * 20;
          int val = SortMap[Cards[i]];
          res += val;
        }
        return res;
      }
    }
    public int CardsValueJoker
    {
      get
      {
        int res = 0;
        for (int i = 0; i < Cards.Count; i++)
        {
          res = res * 20;
          int val = SortMapJoker[Cards[i]];
          res += val;
        }
        return res;
      }
    }

    public string CardsString
    {
      get
      {
        return string.Join("", Cards);
      }
    }

    Dictionary<char, int> SortMap = new Dictionary<char, int>()
    {
      { 'A', 1 },
      { 'K', 2 },
      { 'Q', 3 },
      { 'J', 4 },
      { 'T', 5 },
      { '9', 6 },
      { '8', 7 },
      { '7', 8 },
      { '6', 9 },
      { '5', 10 },
      { '4', 11 },
      { '3', 12 },
      { '2', 13 }
    };

    Dictionary<char, int> SortMapJoker = new Dictionary<char, int>()
    {
      { 'A', 1 },
      { 'K', 2 },
      { 'Q', 3 },
      { 'J', 20 },
      { 'T', 5 },
      { '9', 6 },
      { '8', 7 },
      { '7', 8 },
      { '6', 9 },
      { '5', 10 },
      { '4', 11 },
      { '3', 12 },
      { '2', 13 }
    };


    public Hand(string input)
    {
      string[] splitted = input.Split(' ');
      foreach(char c in splitted[0])
      {
        Cards.Add(c);
      }
      Bid = long.Parse(splitted[1]);

      SortedCards = Cards.OrderBy((char x) =>
      {
        return SortMap[x];
      }).ToList();

      HandType = GetHandType(Cards);
      BestHandType = HandType;


      foreach(char c in SortMap.Keys)
      {
        List<char> cardsTmp = Cards.ToList();
        for (int i = 0; i < 5; i++)
        {
          if (cardsTmp[i] == 'J')
          {
            cardsTmp[i] = c;
            HandType ht = GetHandType(cardsTmp);
            if (ht < BestHandType)
            {
              BestHandType = ht;
            }
          }
        }
      }


    }

    private HandType GetHandType(List<char> cards)
    {
      List<IGrouping<char, char>> groups = cards.GroupBy(x => x).ToList();
      if(groups.Count == 1)
      {
        return HandType.FiveOfAKind;
      }
      else if (groups.Any(x=>x.Count() == 4))
      {
        return HandType.FourOfAKind;
      }
      else if(groups.Count == 2)
      {
        return HandType.FullHouse;
      }
      else if(groups.Count == 3 && groups.Any(x => x.Count() == 3))
      {
        return HandType.ThreeoOfAKind;
      }
      else if(groups.Count == 3)
      {
        return HandType.TwoPair;
      }
      else if(groups.Count == 4)
      {
        return HandType.OnePair;
      }
      else if(groups.Count == 5)
      {
        return HandType.HighCard;
      }

      throw new Exception($"Unknown HandType: {CardsString}");

    }

    public override string ToString()
    {
      return $"{CardsString}, {HandType}, {Bid}";
    }
  }

  public enum HandType
  {
    FiveOfAKind,
    FourOfAKind,
    FullHouse,
    ThreeoOfAKind,
    TwoPair,
    OnePair,
    HighCard
  }
}
