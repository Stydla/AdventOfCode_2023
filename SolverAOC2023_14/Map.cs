using AoCLib;
using AoCLib.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_14
{
  internal class Map
  {

    public List<List<char>> Values { get; } = new List<List<char>>();

    public int MaxX, MaxY;

    public Map(List<string> lines)
    {
      for(int i = 0; i < lines.Count; i++) 
      { 
        string line = lines[i];
        List<char> chars = new List<char>();
        Values.Add(chars);
        for(int j = 0;  j < line.Length; j++)
        {
          char c = line[j];
          chars.Add(c);
        }
      }
      MaxX = Values[0].Count - 1;
      MaxY = Values.Count - 1;
    }

    public void Move(EDirection4 dir)
    {
      switch (dir)
      {
        case EDirection4.UP:
          for(int i = 1; i <= MaxY; i++)
          {
            for(int j = 0; j <= MaxX; j++)
            {
              char c = Values[i][j];
              if(c == 'O')
              {
                for (int k = i - 1; k >= 0; k--)
                {
                  if (Values[k][j] != '.') break;
                  Values[k][j] = 'O';
                  Values[k + 1][j] = '.';
                }
              }
            }
          }
          break;
        case EDirection4.RIGHT:
          for (int j = MaxX - 1; j >= 0; j--)
          {
            for (int i = 0; i <= MaxY; i++)
            {
              char c = Values[i][j];
              if (c == 'O')
              {
                for (int k = j + 1; k <= MaxX; k++)
                {
                  if (Values[i][k] != '.') break;
                  Values[i][k] = 'O';
                  Values[i][k - 1] = '.';
                }
              }
            }
          }
          break;
        case EDirection4.DOWN:
          for (int i = MaxY - 1; i >= 0; i--)
          {
            for (int j = 0; j <= MaxX; j++)
            {
              char c = Values[i][j];
              if (c == 'O')
              {
                for (int k = i + 1; k <= MaxY; k++)
                {
                  if (Values[k][j] != '.') break;
                  Values[k][j] = 'O';
                  Values[k - 1][j] = '.';
                }
              }
            }
          }
          break;
        case EDirection4.LEFT:
          for (int j = 1; j <= MaxX; j++)
          {
            for (int i = 0; i <= MaxY; i++)
            {
              char c = Values[i][j];
              if (c == 'O')
              {
                for (int k = j - 1; k >= 0; k--)
                {
                  if (Values[i][k] != '.') break;
                  Values[i][k] = 'O';
                  Values[i][k + 1] = '.';
                }
              }
            }
          }
          break;
      }
    }

   


    internal object GetValue()
    {
      long res = 0;
      for (int i = 0; i <= MaxY; i++)
      {
        for (int j = 0; j <= MaxY; j++)
        {
          if (Values[i][j] == 'O')
          {
            res += (MaxY + 1 - i);
          }
        }
      }

      return res;
    }

    public string Print()
    {
      StringBuilder sb = new StringBuilder();
      for(int i = 0; i <= MaxY; i++) 
      { 
        for(int j = 0; j <= MaxX; j++)
        {
          sb.Append(Values[i][j]);
        }
        sb.AppendLine();
      }
      return sb.ToString();
    }

    internal string GetHash()
    {
      return Print();
    }
  }
}
