using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_02
{
  internal class Game
  {

    public int GameId { get; set; }
    public List<Round> Rounds { get; set; } = new List<Round>();

    public Game(string line)
    {

      string[] items = line.Split(':');
      GameId = int.Parse(items[0].Substring(5));

      string[] rounds = items[1].Split(';');
      foreach(string round in rounds)
      {
        Round r = new Round(round);
        Rounds.Add(r);
      }

    }

    public int GetPower()
    {
      return Rounds.Max(x => x.R) * Rounds.Max(x => x.G) * Rounds.Max(x => x.B);
    }

  }
}
