using AoCLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_17
{
  internal class EntranceKey
  {


    public string Key { get; }
    public int Size { get; }
    
    public int DirectionCount { get; }

    private List<EDirection4> Directions { get; }

    public EntranceKey(List<EDirection4> directions)
    {
      Directions = directions;
      DirectionCount = directions.Count;
      List<char> dirs = new List<char>();
      int cnt = directions.Count;
      for(int i = 0; i < cnt; i++)
      {
        char c = '\0';
        switch(directions[cnt - i - 1])
        {
          case EDirection4.RIGHT: c = 'R'; break;
          case EDirection4.DOWN: c = 'D'; break;
          case EDirection4.LEFT: c = 'L'; break;
          case EDirection4.UP: c = 'U'; break;
        }

        if (i > 0)
        {
          if (dirs[0] != c) break;
        }
        dirs.Add(c);
      }

      Key = string.Join("", dirs);
    }

    public bool IsValid()
    {
      return Key.Length <= 3;
    }

    public bool IsValid2(bool checkLast)
    {
      EDirection4 lastDirection = Directions.Last();
      EDirection4 prevDirection = EDirection4.UP;
      bool prevDirFound = false;
      int i = Directions.Count - 2;
      int currentDir = 1;
      for (; i >= 0; i--)
      {
        if (lastDirection != Directions[i])
        {
          prevDirection = Directions[i];
          prevDirFound = true;
          break;
        }
        currentDir++;
      }
      if (!prevDirFound) return currentDir <= 10;

      int prevCnt = 1;
      for(int j = i - 1; j >= 0; j--)
      {
        if (Directions[j] != prevDirection)
        {
          break;
        }
        prevCnt++;
      }

      if (checkLast)
      {
        return prevCnt >= 4 && prevCnt <= 10 && currentDir >= 4 && currentDir <= 10;
      } else
      {
        return prevCnt >= 4 && prevCnt <= 10 && currentDir <= 10;
      }
      
    }

    public override int GetHashCode()
    {
      return Key.GetHashCode();
    }

    public override bool Equals(object obj)
    {
      if(obj is EntranceKey ek)
      {
        return ek.Key == Key;
      }
      return false;
    }


  }
}
