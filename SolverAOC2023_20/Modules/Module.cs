using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_20.Modules
{
  internal abstract class Module
  {

    public string Name { get; }

    public List<Module> TargetModules { get; } = new List<Module>();

    public EModuleType ModuleType { get; protected set; }

    public Module(string name)
    {
      Name = name;
    }


    public override string ToString()
    {
      return $"{Name} -> {string.Join(",", TargetModules.Select(x=>x.Name))}";
    }
    
    internal abstract string GetCustomHash();

    public abstract List<PulseMessage> Execute(PulseMessage pm);

    public void AddTarget(Module module)
    {
      TargetModules.Add(module);
      if(module is ConjunctionModule cm)
      {
        cm.Inputs.Add(this, EPulse.Low);
      }
    }

  }

  public enum EModuleType
  {
    Broadcast,
    Button,
    Conjunction,
    FlipFlop
  }

}
