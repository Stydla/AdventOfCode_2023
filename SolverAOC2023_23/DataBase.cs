using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_23
{
    internal class DataBase
    {
        public string InputDataRaw { get; }
        public List<string> Lines { get; } = new List<string>();

        public DataBase(string inputData)
        {
            this.InputDataRaw = inputData;
            using (StringReader sr = new StringReader(inputData))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Lines.Add(line);
                }
            }
        }
    }
}
