using AoCLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_18
{
  internal class DirectionItem
  {

    private long _Value;
    public long Value
    {
      get
      {
        return _Value;
      }
      set
      {
        if (value < 0)
        {
          _Value = -value;
          Direction = Direction == EDirection4.RIGHT ? EDirection4.LEFT : EDirection4.RIGHT;
        } else
        {
          _Value = value;
        }
      }
      
    }

    public EDirection4 Direction { get; set; }


    public DirectionItem(EDirection4 dir, long value) 
    {
      Direction = dir;
      Value = value;
    }

    public override string ToString()
    {
      return $"{Direction} {Value}";
    }


  }
}
