using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SolverAOC2023_12
{
  internal class Item
  {


    public string InputString { get; set; }
    public long Count { get; set; } = -1;
    public char PreviousChar { get; set; }
    public char NextChar { get; set; }

    public List<int> GroupLengths { get; set; } = new List<int>();

    private int HashCache;


    public Item(string inputString, char previousChar, char nextChar, List<int> groupLengths)
    {
      InputString = inputString;
      PreviousChar = previousChar;
      NextChar = nextChar;
      GroupLengths = groupLengths;

      HashCache = inputString.GetHashCode();
      HashCache *= 13 + previousChar.GetHashCode();
      HashCache *= 13 + nextChar.GetHashCode();
      for (int i = 0; i < GroupLengths.Count; i++)
      {
        HashCache *= 13 + GroupLengths[i];
      }
    }

    public override string ToString()
    {
      return $"{PreviousChar} {InputString} {NextChar}  |  {Count}  |  {string.Join(",", GroupLengths)}";
    }

    public override int GetHashCode()
    {
      return HashCache; 
    }
    public override bool Equals(object obj)
    {
      if(obj is Item other)
      {
        return
          InputString == other.InputString &&
          PreviousChar == other.PreviousChar &&
          NextChar == other.NextChar &&
          GroupLengths.SequenceEqual(other.GroupLengths);
      }
      return false;
    }


    public void Solve(HashSet<Item> items)
    {
      if (Count != -1) return;

      if (GroupLengths.Count == 0) throw new Exception("Invalid Group lengths: count = 0");
      if (string.IsNullOrEmpty(InputString))
      {
        Count = 0;
        return;
      }

      List<int> indexes = FindFirstGroupIndexes();

      if (GroupLengths.Count == 1)
      {
        int cnt = 0;
        foreach(int index in indexes)
        {
          string prefix = InputString.Substring(0, index);
          string postfix = InputString.Substring(index + GroupLengths[0]);
          if(!(prefix.Contains('#') || postfix.Contains('#')))
          {
            cnt++;
          }
        }
        Count = cnt;
        return;
      }

      long total = 0;
      int groupLength = GroupLengths[0];
      List<int> nextGroupsLeft = GroupLengths.Take(1).ToList();
      List<int> nextGroupsRight = GroupLengths.Skip(1).ToList();

      for (int i = 0; i < indexes.Count; i++)
      {
        int index = indexes[i];
        
        if (InputString.Substring(0, index).Contains('#')) continue;
        
        string str1 = InputString.Substring(index, groupLength);
        char previousChar1 = GetPreviousChar(index);
        char nextChar1 = GetNextChar(index + groupLength - 1);

        if (index + groupLength + 1 >= InputString.Length) continue;
        string str2 = InputString.Substring(index + groupLength + 1);
        char previousChar2 = GetPreviousChar(index + groupLength + 1);
        char nextChar2 = GetNextChar(InputString.Length -  1);

        Item tmpItem1 = new Item(str1, previousChar1, nextChar1, nextGroupsLeft);
        Item tmpItem2 = new Item(str2, previousChar2, nextChar2, nextGroupsRight);

        Item actualItem1 = GetItem(items, tmpItem1);
        Item actualItem2 = GetItem(items, tmpItem2);

        actualItem1.Solve(items);
        actualItem2.Solve(items);

        long cnt1 = actualItem1.Count;
        long cnt2 = actualItem2.Count;

        total += (cnt1 * cnt2);
      }

      Count = total;
    }

    private Item GetItem(HashSet<Item> items, Item item)
    {
      Item actualItem;
      if(items.TryGetValue(item, out actualItem))
      {
        return actualItem;
      }
      items.Add(item);
      return item;
    }

    private char GetPreviousChar(int index)
    {
      if(index == 0)
      {
        return '!';
      }
      return InputString[index - 1];
    }

    private char GetNextChar(int index)
    {
      if (index == InputString.Length - 1)
      {
        return '!';
      }
      return InputString[index+1];
    }

    private List<int> FindFirstGroupIndexes()
    {
      List<int> indexes = new List<int>();
      if (!InputString.Contains("#") && !InputString.Contains("?")) return indexes;

      int groupSize = GroupLengths[0];

      for (int i = 0; i < InputString.Length - groupSize+1; i++)
      {
        bool ok = true;
        for(int j = 0; j < groupSize; j++)
        {
          if (InputString[i+j] == '.')
          {
            ok = false;
            break;
          }
        }
        if(ok)
        {
          int index = i;
          if (index == 0)
          {
            if (PreviousChar == '#')
            {
              continue;
            }
          }
          else
          {
            if (InputString[index - 1] == '#')
            {
              continue;
            }
          }

          if (index + groupSize == InputString.Length)
          {
            if (NextChar == '#')
            {
              continue;
            }
          }
          else
          {
            if (InputString[index + groupSize] == '#')
            {
              continue;
            }
          }
          indexes.Add(index);
        }


      }
      return indexes;

    }

  }
}
