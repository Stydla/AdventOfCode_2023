using AoCLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_03
{
  internal class Grid
  {

    public int RowCnt { get; }
    public int ColumnCnt { get; }
    public List<List<char>> Values { get; } = new List<List<char>>();

    public List<Part> Parts { get; } = new List<Part>();

    public Grid(string input)
    {
      using(StringReader sr = new StringReader(input))
      {
        int row = 0;
        string line = null;
        while((line = sr.ReadLine())!= null)
        {

          Values.Add(new List<char>());
          Values[row].Add('.');
          for (int i = 0; i < line.Length; i++)
          {
            char c = line[i];
            Values[row].Add(c);
          }
          Values[row].Add('.');
          row++;
        }
      }
      ColumnCnt = Values[0].Count;
      Values.Add(new List<char>(new string('.', ColumnCnt)));
      Values.Insert(0, new List<char>(new string('.', ColumnCnt)));
      RowCnt = Values.Count;

      LoadParts();
    }

    private void LoadParts()
    {
      int number = 0;
      char symbol = 'x';
      Point2D symbolPos = null;
      for (int i = 1; i < RowCnt - 1; i++)
      {
        if (symbol != 'x')
        {
          Part p = new Part(number, symbol, symbolPos);
          Parts.Add(p);
        }
        number = 0;
        symbol = 'x';
        for(int j = 1; j < ColumnCnt - 1; j++)
        {

          char c = Values[i][j];
          if(c >= '0'&& c <= '9')
          {
            number *= 10;
            number += (int)c - '0';
            Tuple<char, Point2D> tmp = FindSymbol(i, j);
            if(tmp != null)
            {
              symbol = tmp.Item1;
              symbolPos = tmp.Item2;
            }
          } else
          {
            if(symbol != 'x')
            {
              Part p = new Part(number, symbol, symbolPos);
              Parts.Add(p);
            }
            
            number = 0;
            symbol = 'x';
          }
        }
      }
      if (symbol != 'x')
      {
        Part p = new Part(number, symbol, symbolPos);
        Parts.Add(p);
      }

      number = 0;
      symbol = 'x';
    }

    private Tuple<char, Point2D> FindSymbol(int row, int column)
    {
      for(int i =  - 1; i <= 1; i++)
      {
        for(int j =  - 1; j <=  1; j++)
        {
          if (i == 0 && j == 0) continue;

          char val = Values[row + i][column + j];
          if((val < '0'|| val > '9') && val != '.')
          {
            return new Tuple<char, Point2D>(val, new Point2D(row+i, column +j));
          }
        }
      }
      return null;
    }

  }
}
