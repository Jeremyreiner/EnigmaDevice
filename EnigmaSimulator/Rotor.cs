
using Enigma.Common.Models;
using Enigma.Configurations;

namespace EnigmaSimulator
{
    /*
     * Construction of a Rotor class
     * Rotors consisted of 26 points for a current to cycle through
     * Rotors have Two sides, one for the wiring to enter and the other for the wiring to leave
     * This means a current will enter at one position and leave at another (Encryption)
     * Reflecter is a type of rotor that is static.
     * This recieves an electrical current only from one side. Returning a constant wiring pair
     */
    public class Rotor
    {
        public enum RotorType { Rotor, Reflector }
        private Connection[] initWiring;

        public int Length { get; set; }
        public Connection[] Wiring { get; set; }
        public RotorModel Rotor_Model { get; set; }
        public RotorType Type { get; set; }
        public bool IsReflect { get; set; }
        //step marks when to rotate Rotor
        public int Step { get; set; }

        /// <summary>
        /// To Set the Rotor at a specified position
        /// ie; A = 0 notches and Z = 25 notches
        /// </summary>
        /// <param name="c">A letter where we need to find the indexed position within the rotor</param>
        public void SetRotorKey(char c)
        {
            int keyToInt = -1;
            // AlphabetConfigs.Alphabet.Equals(c)

            for (int i = 0; i < AlphabetConfigs.Alphabet.Length; i++)
            {
                if (AlphabetConfigs.Alphabet[i].Equals(c))
                {
                    keyToInt = i;
                    break;
                }
            }
            if (keyToInt == -1)
            {
                throw new ArgumentOutOfRangeException(string.Format($"Encryption Error 2: The rotor key {c} is not a valid key"));
            }
            StepRotor(keyToInt);
        }
        /// <summary>
        /// Rotates the rotor x times clockwise. 
        /// Re-configuring the wiring for every rotation
        /// </summary>
        /// <param name="step">How many notches to step the rotor forward</param>
        public void StepRotor(int step)
        {

            //Initialize the wiring constraint and re-configure based on initial model
            Connection[] constructRotor = new Connection[Length];

            for (int i = 0; i < Length; i++)
            {
                constructRotor[i] = initWiring[(i + step) % Length];
            }
            Step = step;
            Wiring = constructRotor;
        }
        public Rotor()
        {
            Type = RotorType.Rotor;
            Wiring = GenerateAlphaRotor(26, false);
            CalcReverseStep(); ;
            Length = 26;
            initWiring = Wiring;
        }
        public Rotor(string wiring, string type)
        {
            Wiring = GenerateWireConnections(wiring);
            CalcReverseStep();
            initWiring = Wiring;
            Length = Wiring.Length;
            SetRotorType(type);

        }
        public Rotor(string wiring, RotorType type, bool ignoreInvalidWiring = false)
        {
            Wiring = GenerateWireConnections(wiring, ignoreInvalidWiring);
            CalcReverseStep();
            initWiring = Wiring;
            Length = Wiring.Length;
            Type = type;
        }
        public Rotor(RotorModel rotorModel)
        {
            Rotor_Model = rotorModel;
            Wiring = GenerateWireConnections(rotorModel.Wiring);
            CalcReverseStep();
            initWiring = Wiring;
            Length = Wiring.Length;
            SetRotorType(rotorModel.Type);
        }

        /// <summary>
        /// Where the rotor goes in reverse instead of forwards
        /// </summary>
        private void CalcReverseStep()
        {
            for (int i = 0; i < Wiring.Length; i++)
            {
                int step = Wiring[i].Step;
                Wiring[(i + step) % 26].ReverseStep = -step;
            }
        }

        private void SetRotorType(string type)
        {
            switch (type)
            {
                case "rotor":
                    Type = RotorType.Rotor;
                    break;
                case "reflector":
                    Type = RotorType.Reflector;
                    IsReflect = true;
                    break;
                default:
                    throw new Exception(string.Format($"Rotor Error 1: {type} is not a valid rotor type"));
            }
        }

        /// <summary>
        /// Generating the Correct Wire patterns for a given rotor
        /// </summary>
        /// <param name="wiring">Rotor wiring configurations</param>
        /// <param name="invalidWiring"></param>
        /// <returns>List of connection points for given Rotor</returns>
        /// <exception cref="InvalidMappingException"></exception>
        public Connection[] GenerateWireConnections(string wiring, bool isValidWiring = false)
        {
            bool valid = AlphabetConfigs.IsValidWiring(wiring);

            if (valid || isValidWiring)
            {
                Connection[] result = new Connection[wiring.Length];

                for (int i = 0; i < wiring.Length; i++)
                {
                    result[i] = new Connection(AlphabetConfigs.Alphabet[i], wiring[i]); //starting at the indexed letter value in alphabet, and ending at the 
                }
                return result;
            }
            else
            {
                throw new Exception(string.Format($"Wire Error 1: {wiring} is not a valid option! Wire Length {wiring.Length}"));
            }
        }

        /// <summary>
        /// At Each connecting point a letter is transmitted
        /// </summary>
        /// <param name="num">number index of the letter</param>
        /// <returns>returning an array of connection points on the rotor</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public Connection[] GenerateAlphaRotor(int num, bool reverse = false)
        {
            if (num < 1 || num > AlphabetConfigs.Alphabet.Length)
            {
                throw new ArgumentOutOfRangeException($"Wiring Error 2: {num} is out of range of ASCII letters");
            }
            Connection[] result = new Connection[num];
            for (int i = 0; i < num; i++)
            {
                if (reverse)
                {
                    //0 =26, 1=0, 2=1 the number reverses direction
                    result[i] = new Connection(AlphabetConfigs.Alphabet[i], AlphabetConfigs.Alphabet[num - 1 - i]);
                }
                else
                {
                    //Each letter will return back to itself
                    result[i] = new Connection(AlphabetConfigs.Alphabet[i], AlphabetConfigs.Alphabet[i]);
                }
            }
            return result;
        }

        /// <summary>
        /// When a key is pressed and a value is passed throught the rotors
        /// this function will get the wiring value of that key.
        /// </summary>
        /// <param name="key">Entry point at given rotor</param>
        /// <param name="reverse">If the rotor is returning a value or passing one</param>
        /// <returns>Exiting point value of rotor</returns>
        public int GetKeyValue(int key, bool reverse)
        {
            key = (key + Step) % Length;
            int result = -1;

            foreach (Connection connection in Wiring)
            {
                if (reverse)
                {
                    if (connection.End.Equals(key))
                    {
                        result = key - connection.Step;
                        break;
                    }
                }
                else
                {
                    if (connection.Start.Equals(key))
                    {
                        result = key + connection.Step;
                        break;

                    }
                }
            }
            if (result >= 0)
            {
                return result % Length;
            }
            else
            {
                throw new ArgumentException(string.Format($"Rotor Key Error 1: {key} cannot be found in the rotor's connections."));
            }
        }
        /// <summary>
        /// Function that dictates when the rotor will rotate forward/ backwords
        /// </summary>
        /// <param name="key">value passed through rotor</param>
        /// <param name="reverse">if the wheel is passing forwards or backwords</param>
        /// <returns>The position of the next rotor step</returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public int GetStepFromKey(int key, bool reverse)
        {
            if (key > 25 || key < 0)
            {
                throw new ArgumentOutOfRangeException(string.Format($"Rotor Error 2: {key} is out of range. It must be between 0 and 25."));
            }
            if (reverse)
            {
                int value = Wiring[key].ReverseStep;
                value %= 26;
                return value;
            }
            else
            {
                return Wiring[key].Step % 26;
            }
        }
    }
}