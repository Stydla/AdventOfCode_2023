using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SolverAOC2023_01
{
    internal class Data : DataBase
    {
        public Data(string input) : base(input)
        {
            // optionaly parse data
        }

        public object Solve1()
        {
            int sum = 0;
            foreach(string line in Lines)
            {
                int first = FirstDigit(line)*10;
                int last = LastDigit(line);
                sum += first + last;
            }

            return sum;
        }

        private List<string> Numbers = new List<string>()
        {
            "???!!!","one", "two", "three", "four", "five", "six", "seven", "eight", "nine",
            "???!!!","1", "2", "3", "4", "5", "6", "7", "8", "9"
        };

        public object Solve2()
        {
            int sum = 0;

            foreach(string line in Lines)
            {
                Dictionary<int, int> vals = new Dictionary<int, int>();
                
                var indexesFirst = Numbers.Select(x => line.IndexOf(x)).ToList();
                var indexesLast = Numbers.Select(x => line.LastIndexOf(x)).ToList();

                int min = indexesFirst.Where(x => x >= 0).Min();
                int max = indexesLast.Where(x => x >= 0).Max();

                int first = indexesFirst.IndexOf(min) % 10;
                int last = indexesLast.IndexOf(max) % 10;
                int numTmp = first * 10 + last;

                sum += numTmp;

            }
            return sum;
        }

        private int FirstDigit(string input)
        {
            return (int)input.First(x => x >= '0' && x <= '9') - '0';
        }

        private int LastDigit(string input)
        {
            return (int)input.Last(x => x >= '0' && x <= '9') - '0';
        }


    }
}
