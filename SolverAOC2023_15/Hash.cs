using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_15
{
  internal class Hash
  {

    public string Input { get; }
    public string Key { get; }
    public int Value { get; private set; }
    public int KeyValue { get; private set; }


    public char Operation { get; }
    public int OperationValue { get; }

    public Hash(string s)
    {
      Input = s;
      if(s.Contains('-'))
      {
        string[] items = s.Split('-');
        Key = items[0];
        Operation = '-';
      }
      else
      {
        string[] items = s.Split('=');
        Key = items[0];
        Operation = '=';
        OperationValue = int.Parse(items[1]);
      }
      SolveValue();
    }

    private void SolveValue()
    {
      int tmpVal = 0;
      foreach(char c in Input)
      {
        tmpVal = ((tmpVal + c) * 17) % 256;
      }
      Value = tmpVal;

      tmpVal = 0;
      foreach (char c in Key)
      {
        tmpVal = ((tmpVal + c) * 17) % 256;
      }
      KeyValue = tmpVal;
    }
  }
}
