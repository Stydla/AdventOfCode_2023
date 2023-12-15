using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_15
{
  internal class Boxes
  {

    public Dictionary<int, List<Hash>> Values { get; } = new Dictionary<int, List<Hash>>();

    public Boxes() 
    { 
      for(int i = 0; i < 256; i++)
      {
        Values.Add(i, new List<Hash>());
      }
    }  


    public void SolveHash(Hash hash)
    {
      List<Hash> list = Values[hash.KeyValue];

      switch(hash.Operation)
      {
        case '-':
          {
            Hash item = list.FirstOrDefault(x=>x.Key == hash.Key);
            if (item != null)
            {
              int index = list.IndexOf(item);
              list.RemoveAt(index);
            }
            break;
          }
        case '=':
          {
            Hash item = list.FirstOrDefault(x => x.Key == hash.Key);
            if (item != null)
            {
              int index = list.IndexOf(item);
              list[index] = hash;
            } 
            else
            {
              list.Add(hash);
            }
            break;
          }
        default: throw new Exception($"Invalid operation {hash.Operation}");
      }
    }

    public int GetFocusingPower()
    {
      int total = 0;
      for(int i = 0; i < 256; i++)
      {
        List<Hash> box = Values[i];
        for(int j = 0; j < box.Count; j++)
        {
          Hash h = box[j];

          int valTmp = (i + 1) * (j + 1) * h.OperationValue;
          total += valTmp;
        }
      }
      return total;
    }

  }
}
