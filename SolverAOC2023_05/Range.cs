using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_05
{
  internal class Range
  {
    public long Destination { get; set; }
    public long Source {  get; set; }
    public long Length { get; set; }

    public Range(long destination, long source, long length)
    {
      Destination = destination;
      Source = source;
      Length = length;
    }

    public override string ToString()
    {
      return $"{Destination} {Source} {Length}";
    }

    internal bool Contains(long source)
    {
      return (Source <= source) && ((Source + Length) > source);
    }

    internal long GetTarget(long source)
    {
      return source - Source + Destination;
    }
  }
}
