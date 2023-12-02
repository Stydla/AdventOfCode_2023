using Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_02
{
  public class Program : BaseAdventSolver, IAdventSolver
  {

    public override string SolverName => "SolverAOC2023_02"/*TODO: Task Name*/;

    public override string InputsFolderName => "SolverAOC2023_02";

    public override string SolveTask1(string InputData)
    {
      List<Game> validGames = new List<Game>();

      using(StringReader sr = new StringReader(InputData))
      {
        string line;
        while((line = sr.ReadLine()) != null)
        {
          Game g = new Game(line);
          if (IsGameValid(g, 12, 13, 14))
          {
            validGames.Add(g);
          }
        }
      }

      int res = validGames.Sum(x => x.GameId);
      return res.ToString();
    }

    private bool IsGameValid(Game game, int r, int g, int b)
    {
      return !game.Rounds.Any(x => x.R > r || x.G > g || x.B > b);
    }

    public override string SolveTask2(string InputData)
    {
      List<Game> games = new List<Game>();

      using (StringReader sr = new StringReader(InputData))
      {
        string line;
        while ((line = sr.ReadLine()) != null)
        {
          Game g = new Game(line);
          games.Add(g);
        }
      }

      int res = games.Sum(x => x.GetPower());

      return res.ToString();
    }
  }
}
