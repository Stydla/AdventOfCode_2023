using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_12
{
  internal class Data : DataBase
  {

    public List<Item> Items { get; set; } = new List<Item>();

    public Data(string input) : base(input)
    {
      // optionaly parse data
    }

    public object Solve1()
    {
      foreach (string line in Lines)
      {
        string[] items = line.Split(' ');

        List<int> lengths = items[1].Split(',').Select(x => int.Parse(x)).ToList();
        Item item= new Item(items[0], '!', '!', lengths);
        Items.Add(item);
      }

      long total = 0;
      foreach(Item i in Items)
      {
        i.Solve(new HashSet<Item>());
        long tmp = i.Count;
        total += tmp;
      }
      return total;

    }

    public object Solve2()
    {
      foreach (string line in Lines)
      {
        string[] items = line.Split(' ');
        string spring = items[0];
        string groups = items[1];
        for(int i = 0; i < 4; i++)
        {
          spring = spring + "?" + items[0];
          groups = groups + "," + items[1];
        }

        List<int> lengths = groups.Split(',').Select(x => int.Parse(x)).ToList();
        Item item = new Item(spring, '!', '!', lengths);
        Items.Add(item);
      }

      long total = 0;
      foreach (Item i in Items)
      {
        i.Solve(new HashSet<Item>());
        long tmp = i.Count;
        Debug.WriteLine(tmp);
        total += tmp;
      }
      return total;

    }


  }
}
