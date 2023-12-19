using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SolverAOC2023_19
{
  internal class Workflow
  {
    public string RawInput { get; }
    public string Name { get; }

    public List<Rule> Rules { get; } = new List<Rule>();

    public Workflow(string input)
    {
      RawInput = input;

      Regex r = new Regex("(^.*){(.*)}$");
      Match m = r.Match(RawInput);
      Name = m.Groups[1].Value;

      string rulesRaw = m.Groups[2].Value;
      string[] rules = rulesRaw.Split(',');

      foreach(string ruleInput in rules) 
      {
        if (string.IsNullOrEmpty(ruleInput)) continue;

        Rule rule = new Rule(ruleInput);
        Rules.Add(rule);
      }
    }

    public bool IsAccepted()
    {
      return Name == "A";
    }

    public bool IsRejected()
    {
      return Name == "R";
    }

    public Workflow Solve(Rating rating)
    {
      foreach(Rule rule in Rules)
      {
        if(rule.Solve(rating))
        {
          return rule.TargetWorkflow;
        }
      }
      throw new Exception($"Invalid rating/workflow");
    }

    public override string ToString()
    {
      return $"{Name} | {string.Join(",", Rules.Select(x => x))}";
    }

    internal long GetAllPossibilities(List<Rule> ruleListPassed, List<Rule> ruleListFailed)
    {
      if(IsAccepted())
      {
        RatingRange rr = new RatingRange();
        foreach(Rule rule in ruleListPassed) 
        {
          rr.ApplyRule(rule);
        }
        foreach (Rule rule in ruleListFailed)
        {
          rr.ApplyRuleFailed(rule);
        }
        long val = rr.GetValue();
        return val;
      }

      List<Rule> forRemove = new List<Rule>();
      long tmp = 0;
      foreach(Rule r in Rules)
      {
        Workflow nextWf = r.TargetWorkflow;

        ruleListPassed.Add(r);
        tmp += nextWf.GetAllPossibilities(ruleListPassed, ruleListFailed);
        ruleListFailed.Add(r);
        forRemove.Add(r);
        ruleListPassed.RemoveAt(ruleListPassed.Count - 1);
      }
      foreach(Rule r in forRemove)
      {
        ruleListFailed.Remove(r);
      }
      return tmp;
    }
  }
}
