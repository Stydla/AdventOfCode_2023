using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_19
{
  internal class RatingRange
  {

    public Rating RatingMin { get; }
    public Rating RatingMax { get; }

    public RatingRange()
    {
      RatingMin = new Rating(1, 1, 1, 1);
      RatingMax = new Rating(4000, 4000, 4000, 4000);
    }


    public long GetValue()
    {
      if (RatingMax.X < RatingMin.X ||
        RatingMax.M < RatingMin.M ||
        RatingMax.A < RatingMin.A ||
        RatingMax.S < RatingMin.S
        ) return 0;
      return (RatingMax.X - RatingMin.X + 1) *
        (RatingMax.M - RatingMin.M + 1) *
        (RatingMax.A - RatingMin.A + 1) *
        (RatingMax.S - RatingMin.S + 1);
    }

    public override string ToString()
    {
      return $"{RatingMin}   |   {RatingMax}";
    }

    internal void ApplyRule(Rule rule)
    {
      
      if(rule.Operator == '<')
      {
        switch(rule.VariableName)
        {
          case 'x': RatingMax.X = Math.Min(RatingMax.X, rule.Value - 1); break;
          case 'm': RatingMax.M = Math.Min(RatingMax.M, rule.Value - 1); break;
          case 'a': RatingMax.A = Math.Min(RatingMax.A, rule.Value - 1); break;
          case 's': RatingMax.S = Math.Min(RatingMax.S, rule.Value - 1); break;
            throw new Exception("Invalid rule");
        }
      } else
      {
        switch (rule.VariableName)
        {
          case 'x': RatingMin.X = Math.Max(RatingMin.X, rule.Value + 1); break;
          case 'm': RatingMin.M = Math.Max(RatingMin.M, rule.Value + 1); break;
          case 'a': RatingMin.A = Math.Max(RatingMin.A, rule.Value + 1); break;
          case 's': RatingMin.S = Math.Max(RatingMin.S, rule.Value + 1); break;
            throw new Exception("Invalid rule");
        }
      }

    }

    internal void ApplyRuleFailed(Rule rule)
    {
      if (rule.Operator == '<')
      {
        switch (rule.VariableName)
        {
          case 'x': RatingMin.X = Math.Max(RatingMin.X, rule.Value); break;
          case 'm': RatingMin.M = Math.Max(RatingMin.M, rule.Value); break;
          case 'a': RatingMin.A = Math.Max(RatingMin.A, rule.Value); break;
          case 's': RatingMin.S = Math.Max(RatingMin.S, rule.Value); break;
            throw new Exception("Invalid rule");
        }
      }
      else
      {
        switch (rule.VariableName)
        {
          case 'x': RatingMax.X = Math.Min(RatingMax.X, rule.Value); break;
          case 'm': RatingMax.M = Math.Min(RatingMax.M, rule.Value); break;
          case 'a': RatingMax.A = Math.Min(RatingMax.A, rule.Value); break;
          case 's': RatingMax.S = Math.Min(RatingMax.S, rule.Value); break;
            throw new Exception("Invalid rule");
        }
      }
    }
  }
}

