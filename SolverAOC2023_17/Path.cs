using AoCLib;
using AoCLib.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_17
{
  internal class Path
  {

    public List<EDirection4> Directions { get; } = new List<EDirection4>();
    public int Distance { get; private set; }

    public List<PathStep> PathSteps { get; } = new List<PathStep>();

    public Path(Path original)
    {
      Distance = original.Distance;
      Directions.AddRange(original.Directions);
    }

    public Path()
    {
      Distance = 0;
    }


    public void Add(PathStep pathItem)
    {
      PathSteps.Add(pathItem);
      Distance += pathItem.Field.Value;
      Directions.Add(pathItem.Direction);
    }

    internal List<Path> GetNextPaths(Dictionary<Point2D, Field> allFields, bool part2)
    {
      List<Path> nextPaths = new List<Path>();

      Field currentField = PathSteps.Last().Field;
      EDirection4 currentDirection = PathSteps.Last().Direction;

      Path tmpPath;
      //Left
      tmpPath = GetPath(currentDirection.Prev(), currentField, allFields, part2);
      if(tmpPath != null)
      {
        nextPaths.Add(tmpPath);
      }

      //Streight
      tmpPath = GetPath(currentDirection, currentField, allFields, part2);
      if (tmpPath != null)
      {
        nextPaths.Add(tmpPath);
      }

      //Right
      tmpPath = GetPath(currentDirection.Next(), currentField, allFields, part2);
      if (tmpPath != null)
      {
        nextPaths.Add(tmpPath);
      }

      return nextPaths;
    }

    private Path GetPath(EDirection4 direction, Field fromField, Dictionary<Point2D, Field> allFields, bool part2)
    {
      Point2D location = fromField.Location.Move(direction);

      if (allFields.ContainsKey(location))
      {
        Field targetField = allFields[location];
        PathStep newPathStep = new PathStep(direction, targetField);
        Path newPath = new Path(this);
        newPath.Add(newPathStep);

        EntranceKey ek = new EntranceKey(newPath.Directions);
        if (part2 ? ek.IsValid2(false) : ek.IsValid())
        {
          if (targetField.Paths.TryGetValue(ek, out Path existingPath))
          {
            if (existingPath.Distance > newPath.Distance)
            {
              targetField.Paths.Remove(ek);
              targetField.Paths.Add(ek, newPath);
              return newPath;
            }
          }
          else
          {
            targetField.Paths.Add(ek, newPath);
            return newPath;
          }
        }
      }
      return null;
    }
  }
}
