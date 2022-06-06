using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Configurations
{
    public class AlphabetConfigs
    {
        /// <summary>
        /// Needed for configuring and wiring wire layouts for given rotor
        /// Allows rotor to identify where a indexed letter is at any given instance
        /// </summary>
        private static AlphabetConfigs instance;
        public static AlphabetConfigs Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new AlphabetConfigs();
                }
                return instance;
            }

        }
        //This referring to the instance of this connection
        public char this[int index]
        {
            get { return Alphabet[index]; } //returning the letter at the indexed position
        }
        public int this[char c]
        {
            get { return GetLetterIndex(c); }

        }

        public static readonly char[] Alphabet = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };

        public const string AlphaString = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
        public const string ReverseAlphaString = "ZYXWVUTSRQPONMLKJIHGFEDCBA";

        /// <summary>
        /// Simply returning the number value of the letter
        /// </summary>
        /// <param name="c">A ASCII letter</param>
        /// <returns>Number in the range of 0- 25</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        private int GetLetterIndex(char c)
        {
            for (int i = 0; i < Alphabet.Length; i++)
            {
                if (c.Equals(Alphabet[i])) return i;
            }
            throw new ArgumentOutOfRangeException(string.Format("The letter {0} is not a valid letter"));
        }
        /// <summary>
        /// MappingTrue: If len of char array == 25 && letters DO NOT repeat
        /// </summary>
        /// <param name="Wiring">A string of letters</param>
        /// <returns>Boolean response</returns>
        public static bool IsValidWiring(string Wiring)
        {
            if (string.IsNullOrWhiteSpace(Wiring)) return false;
            if (Wiring.Length != 26) return false;

            return SumTotalOfWiring(Wiring, Alphabet);
        }
        private static bool SumTotalOfWiring(string letters, char[] alphabet)
        {
            //Actual total sum of chars in alphabet
            int actSum = 0;
            for (int i = 0; i < letters.Length; i++)
            {
                //The sum of the configured wiring for any given rotor
                actSum += letters[i];
            }

            //Correct sum of ASCII Letters
            int expSum = 0;
            for (int i = 0; i < Alphabet.Length; i++)
            {
                //adding up the sum of the inexed letters in the fixed alphabet
                expSum += Alphabet[i];
            }
            return expSum == actSum; ;
        }

    }
}