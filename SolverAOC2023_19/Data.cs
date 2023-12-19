using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_19
{
  internal class Data : DataBase
  {

    public Dictionary<string, Workflow> AllWorkflows { get; } = new Dictionary<string, Workflow>();

    public List<Rating> Ratings { get; } = new List<Rating>();

    public Data(string input) : base(input)
    {
      // optionaly parse data

      int inputType = 0;
      for(int i = 0; i < Lines.Count; i++) 
      { 
        string line = Lines[i];

        if(string.IsNullOrEmpty(line))
        {
          inputType++;
          continue;
        }

        if (inputType == 0)
        {
          Workflow wf = new Workflow(line);
          AllWorkflows.Add(wf.Name, wf);
        }

        if(inputType== 1)
        {
          Rating rating = new Rating(line);
          Ratings.Add(rating);
        }
      }

      AllWorkflows.Add("A", new Workflow("A{}"));
      AllWorkflows.Add("R", new Workflow("R{}"));

      foreach (Workflow wf in AllWorkflows.Values)
      {
        foreach(Rule rule in wf.Rules)
        {
          rule.AssignTargetRule(AllWorkflows);
        }
      }

    }

    public object Solve1()
    {

      long sum = 0;

      foreach(Rating rating in Ratings)
      {
        Workflow current = AllWorkflows["in"];

        while(!current.IsAccepted() && !current.IsRejected())
        {
          current = current.Solve(rating);
        }

        if (current.IsAccepted())
        {
          sum += rating.Value;
        }
      }

      return sum;
    }

    public object Solve2()
    {
      Workflow current = AllWorkflows["in"];

      long res = current.GetAllPossibilities(new List<Rule>(), new List<Rule>());
      return res;
    
    }


  }
}
