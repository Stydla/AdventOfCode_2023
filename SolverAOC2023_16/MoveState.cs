using AoCLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_16
{
  internal class MoveState
  {

    public Field Field { get; set; }
    public EDirection4 Direction { get; set; }

    public MoveState(Field field, EDirection4 direction)
    {
      Field = field;
      Direction = direction;
    }

    internal IEnumerable<MoveState> Move()
    {
      return Field.Move(Direction);
    }
  }
}
