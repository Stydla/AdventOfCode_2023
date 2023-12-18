using AoCLib;
using AoCLib.Enums;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_18
{
  internal class Data : DataBase
  {

    public List<DirectionItem> Directions{ get; set; } = new List<DirectionItem>();

    public Data(string input) : base(input)
    {
      // optionaly parse data
    }

    private void LoadDirections2()
    {
      EDirection4 currentDir = EDirection4.UP;

      for (int i = 0; i < Lines.Count; i++)
      {
        string line = Lines[i];
        string[] parts = line.Split(' ');

        string hex = parts[2];
        string valStr = hex.Substring(2, 5);

        int val = Convert.ToInt32(valStr, 16);
        char dirC;
        switch(hex[7])
        {
          case '0':
            dirC = 'R';
            break;
          case '1':
            dirC = 'D';
            break;
          case '2':
            dirC = 'L';
            break;
          case '3':
            dirC = 'U';
            break;
          default:
            throw new Exception("Invalid dir");
        }

        EDirection4 turning = GetTurning(currentDir, dirC);

        Directions.Add(new DirectionItem(turning, val));
        currentDir = GetDir(dirC);

      }
    }

    private void LoadDirections1()
    {

      EDirection4 currentDir = EDirection4.UP;

      for(int i = 0; i < Lines.Count; i++)
      {
        string line = Lines[i];
        string [] parts = line.Split(' ');

        char dirC = parts[0][0];
        long val = long.Parse(parts[1]);

        EDirection4 turning = GetTurning(currentDir, dirC);

        Directions.Add(new DirectionItem(turning, val));
        currentDir = GetDir(dirC);
        
      }
    }

    private EDirection4 GetDir(char c)
    {
      switch (c)
      {
        case 'U': return EDirection4.UP;
        case 'R': return EDirection4.RIGHT;
        case 'D': return EDirection4.DOWN;
        case 'L': return EDirection4.LEFT;
      }
      throw new Exception($"Invalid direction {c}");
    }

    private EDirection4 GetTurning(EDirection4 current, char c)
    {
      switch (c)
      {
        case 'U':
          {
            if (current == EDirection4.RIGHT) return EDirection4.LEFT;
            if (current == EDirection4.LEFT) return EDirection4.RIGHT;
            throw new Exception($"Invalid direction {c}");
          }
        case 'R':
          {
            if (current == EDirection4.UP) return EDirection4.RIGHT;
            if (current == EDirection4.DOWN) return EDirection4.LEFT;
            throw new Exception($"Invalid direction {c}");
          }
        case 'D':
          {
            if (current == EDirection4.RIGHT) return EDirection4.RIGHT;
            if (current == EDirection4.LEFT) return EDirection4.LEFT;
            throw new Exception($"Invalid direction {c}");
          }
        case 'L':
          {
            if (current == EDirection4.UP) return EDirection4.LEFT;
            if (current == EDirection4.DOWN) return EDirection4.RIGHT;
            throw new Exception($"Invalid direction {c}");
          }
      }
      throw new Exception($"Invalid direction {c}");
    }

    public long Solve()
    {
      int r = Directions.Count(x => x.Direction == EDirection4.RIGHT);
      int l = Directions.Count(x => x.Direction == EDirection4.LEFT);

      EDirection4 allowedDir = r>l ? EDirection4.RIGHT : EDirection4.LEFT;
      //System.Diagnostics.Debug.WriteLine(Print());
      long total = 0;
      while (Directions.Count > 4)
      {
        
        long tmp = Symplify(allowedDir);
        //System.Diagnostics.Debug.WriteLine(Print());
        total += tmp;
      }

      long tmp2 = (Directions[0].Value + 1) * (Directions[1].Value + 1);
      total += tmp2;

      return total;
    }

    private long Symplify(EDirection4 allowedDir)
    {

      int bestIndex = -1;
      long smallestVal = long.MaxValue;

      for(int i = 0; i < Directions.Count; i++)
      {
        if (Directions[i].Direction == allowedDir &&  Directions[(i+1) % Directions.Count].Direction == allowedDir)
        {
          DirectionItem item1tmp = Directions[(i - 1 + Directions.Count) % Directions.Count];
          DirectionItem item2tmp = Directions[i];
          DirectionItem item3tmp = Directions[(i + 1) % Directions.Count];

          DirectionItem itemtmp = Directions[i];

          long minTmp = Math.Min(item1tmp.Value, item3tmp.Value) * (item2tmp.Value + 1);

          if (minTmp < smallestVal)
          {
            smallestVal = minTmp;
            bestIndex = i;
          }

        }
      }

      long res = 0;

      DirectionItem item1 = Directions[(bestIndex - 1 + Directions.Count) % Directions.Count];
      DirectionItem item2 = Directions[bestIndex];
      DirectionItem item3 = Directions[(bestIndex + 1) % Directions.Count];

      long val1 = item1.Value;
      long val2 = item2.Value;
      long val3 = item3.Value;

      long min = Math.Min(val1, val3);
      item1.Value -= min;
      item3.Value -= min;
      res = (item2.Value + 1) * min;

      long tmp;
      while((tmp = RemoveZeroDir()) != 0)
      {
        if(tmp != -1)
        {
          res += tmp;
        }
      }

      return res;
    }

    private string Print()
    {
      List<List<char>> list = new List<List<char>>();
      int size = 550;
      for(int i = 0; i < size; i++)
      {
        list.Add(new List<char>());
        for(int j = 0; j < size; j++)
        {
          list[i].Add('.');
        }
      }

      EDirection4 currentDir = EDirection4.UP;
      Point2D pCurrent = new Point2D(0, 0);
      for (int i = 0;  i < Directions.Count; i++)
      {
        DirectionItem di = Directions[i];
        if(di.Direction == EDirection4.RIGHT)
        {
          currentDir = currentDir.Next();
        } else
        {
          currentDir = currentDir.Prev();
        }
        for(int j = 0; j < di.Value; j++)
        {
          list[pCurrent.Y+400][pCurrent.X+100] = '#';
          pCurrent = pCurrent.Move(currentDir);
        }
      }

      StringBuilder sb = new StringBuilder();
      for (int i = 0; i < size; i++)
      {
        for (int j = 0; j < size; j++)
        {
          sb.Append(list[i][j]);
        }
        sb.AppendLine();
      }
      return sb.ToString();

    }

    private long RemoveZeroDir()
    {
      int cnt = Directions.Count;
      for(int i = 0; i < cnt; i++)
      {

        DirectionItem di0 = Directions[((i - 1) + cnt) % cnt];
        DirectionItem di1 = Directions[i];
        DirectionItem di2 = Directions[(i + 1) % cnt];


        if (di1.Value == 0)
        {
          if(di1.Direction != di2.Direction)
          {
            di0.Value += di2.Value;
            Directions.Remove(di1);
            Directions.Remove(di2);
            return -1;
          } else
          {
            DirectionItem next = Directions[(i + 2) % cnt];
            
            di0.Value -= di2.Value;
            Directions.Remove(di1);
            Directions.Remove(di2);
            //next.Direction = next.Direction == EDirection4.RIGHT ? EDirection4.LEFT : EDirection4.RIGHT;
            return di2.Value - di0.Value;
          }
        }
      }
      return 0;
    }

    public object Solve1()
    {
      LoadDirections1();
      return  Solve();
    }

    public object Solve2()
    {
      LoadDirections2();
      return Solve();
    }


  }
}
