using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SolverAOC2023_19
{
  internal class Rule
  {

    public string RawInput { get; }


    public char VariableName { get; }
    public char Operator { get; }
    public long Value { get; }
    public string TargetWorkflowString { get; }

    public Workflow TargetWorkflow { get; private set; }

    public Rule(string input)
    {
      RawInput = input;

      Regex r = new Regex("([xmas])([<>])(\\d*):([a-zA-Z]*)|([a-zA-Z]*)");
      Match m = r.Match(input);

      if(m.Groups[5].Success)
      {
        TargetWorkflowString = m.Groups[5].Value;
      } else
      {
        VariableName = m.Groups[1].Value[0];
        Operator = m.Groups[2].Value[0];
        Value = long.Parse(m.Groups[3].Value);
        TargetWorkflowString = m.Groups[4].Value;
      }
    }

    public override string ToString()
    {
      return $"{RawInput} | {VariableName} {Operator} {Value} : {TargetWorkflowString}";
    }

    public void AssignTargetRule(Dictionary<string, Workflow> allWorkflows)
    {
      
      TargetWorkflow = allWorkflows[TargetWorkflowString];
    }



    public bool IsFinal()
    {
      return VariableName == '\0';
    }

    internal bool Solve(Rating rating)
    {
      if(IsFinal())
      {
        return true;
      }

      switch(VariableName)
      {
        case 'x': return Evaluate(rating.X, Value, Operator);
        case 'm': return Evaluate(rating.M, Value, Operator);
        case 'a': return Evaluate(rating.A, Value, Operator);
        case 's': return Evaluate(rating.S, Value, Operator);
      }
      throw new Exception("Not Solvable");
    }

    private bool Evaluate(long val1, long val2, char op)
    {
      switch(op)
      {
        case '<':
          {
            return val1 < val2;
          }
        case '>':
          {
            return val1 > val2;
          }
      }
      throw new Exception($"Invalid operation");
    }
  }
}
