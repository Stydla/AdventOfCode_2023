using AoCLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_13
{
  internal class Map
  {

    public List<List<Field>> Fields { get; set; } = new List<List<Field>>();
    public List<List<Field>> FieldsInverted { get; set; } = new List<List<Field>>();
    public List<Field> AllFields { get; set; } = new List<Field>();
    public int MaxX { get; set; }
    public int MaxY { get; set; }

    public Map(List<string> input)
    {

      for (int i = 0; i < input.Count; i++)
      {
        List<Field> fieldTmp = new List<Field>();
        Fields.Add(fieldTmp);
        string line = input[i];
        for (int j = 0; j < line.Length; j++)
        {
          char c = line[j];
          Point2D location = new Point2D(j, i);
          Field f = new Field(c, location);
          fieldTmp.Add(f);
          AllFields.Add(f);
        }
      }
      MaxX = AllFields.Max(x=>x.Location.X);
      MaxY = AllFields.Max(x => x.Location.Y);

      for(int i = 0; i <= MaxX; i++)
      {
        FieldsInverted.Add(AllFields.Where(x => x.Location.X == i).ToList());
      }
    }

    internal long Solve()
    {
      long vertical = FindVerticalMirrorIndex(-1);
      long horizontal = FindHorizontalMirrorIndex(-1);

      long res = vertical + 100 * horizontal;
      return res;
    }

    private Dictionary<int, Tuple<List<Field>, List<Field>>> Cache = new Dictionary<int, Tuple<List<Field>, List<Field>>>();

    private Tuple<List<Field>, List<Field>> GetFieldsFromCache(int key)
    {
      if (Cache.ContainsKey(key))
      {
        return Cache[key];
      }
      return null;
    }

    private void AddToCache(int key, List<Field> left, List<Field> right)
    {
      Cache.Add(key, Tuple.Create(left, right));
    }


    private int FindVerticalMirrorIndex(long ignoreIndex)
    {
      for (int j = 1; j <= MaxX; j++)
      {
        if (ignoreIndex != -1 && j == ignoreIndex) continue;
        bool mirror = true;
        for (int i = 0; i <= MaxY; i++)
        {
          int key = j * 100 + i + 10000;
          var tmp = GetFieldsFromCache(key);
          List<Field> left, right;
          if (tmp == null)
          {
            List<Field> sourceFields = Fields[i];
            left = sourceFields.Take(j).Reverse().ToList();
            right = sourceFields.Skip(j).ToList();
            AddToCache(key, left, right);
          } else
          {
            left = tmp.Item1;
            right = tmp.Item2;
          }
          
          if (!IsMirror(left, right))
          {
            mirror = false;
            break;
          }
          
        }
        if (mirror)
        {
          return j;
        }
      }
      return 0;
    }
    private int FindHorizontalMirrorIndex(long ignoreIndex)
    {
      for (int j = 1; j <= MaxY ; j++)
      {
        if (ignoreIndex != -1 && j == ignoreIndex) continue;
        bool mirror = true;
        for (int i = 0; i <= MaxX; i++)
        {
          int key = j * 100 + i + 20000;
          var tmp = GetFieldsFromCache(key);
          List<Field> left, right;
          if (tmp == null)
          {
            List<Field> sourceFields = FieldsInverted[i];
            left = sourceFields.Take(j).Reverse().ToList();
            right = sourceFields.Skip(j).ToList();
            AddToCache(key, left, right);
          }
          else
          {
            left = tmp.Item1;
            right = tmp.Item2;
          }

          if (!IsMirror(left, right))
          {
            mirror = false;
            break;
          }
        }
        if (mirror)
        {
          return j;
        }
      }
      return 0;
    }

    private bool IsMirror(List<Field> f1, List<Field> f2)
    {
      int targetIndex = Math.Min(f1.Count, f2.Count);
      for (int i = 0; i < targetIndex; i++)
      {
        if (f1[i].Value != f2[i].Value)
        {
          return false;
        }
      }
      return true;
    }

    

    internal long Solve2()
    {
      long verticalOriginal = FindVerticalMirrorIndex(-1);
      long horizontalOriginal = FindHorizontalMirrorIndex(-1);

      long previousRes = verticalOriginal + 100 * horizontalOriginal;
      foreach (Field f in AllFields)
      {
        f.SwitchValue();
        long vertical = FindVerticalMirrorIndex(verticalOriginal);
        long horizontal = FindHorizontalMirrorIndex(horizontalOriginal);

        long res = vertical + 100 * horizontal;
        f.SwitchValue();
        if(res != 0)
        {
          return res;
        }
      }

      return -1;
    }
  }
}
