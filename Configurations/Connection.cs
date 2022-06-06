using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Configurations
{
    public class Connection
    {
        /// <summary>
        /// This function marks the wiring configurations on any given rotor
        /// The connections are the point where the encryption letter enters into a given point on a rotor
        /// and leaves from another exit point. Ie Start and end.
        /// Wiring also gives the configuration of where the steping point (rotation point) is located for any given rotor
        /// </summary>
        private readonly int start;
        private readonly int end;
        private readonly int step;

        public int Start { get { return start; } }
        public int End { get { return end; } }
        public int Step { get { return step; } }
        public int ReverseStep { get; set; }
        /// <summary>
        /// Mapping of char array, from 0 -25
        /// </summary>
        /// <param name="start">indicating the begining of the rotor</param>
        /// <param name="end">indicating last index of rotor</param>
        public Connection(char start, char end)
        {
            //var in place of ...
            var alpha = new AlphabetConfigs();

            this.start = alpha[start];
            this.end = alpha[end];
            step = end - start;
        }
        /// <summary>
        /// To overide the string method to pri
        /// https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/how-to-override-the-tostring-method
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return string.Format("{0} / {1}", step, ReverseStep);
        }

    }
}