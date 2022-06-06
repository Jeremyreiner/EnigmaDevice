using Enigma.Configurations;
using Enigma.Common.Models;
using EnigmaSimulator.Extensions;

namespace ActualEnigma.DeviceRotorPrograms
{
    public class Device
    {
        //AlphabetConfigs alphabet = new();

        //public DeviceModel deviceModel { get; set; }

        public List<Rotor> rotors { get; }


        /// <summary>
        /// Creates enigma using list of rotors
        /// </summary>
        public Device(List<Rotor> rotors)
        {
            //check footnotes at bottem
            if (rotors == null)
                throw new ArgumentNullException("Rotors Error 2: No Rotors Were Supplied");

            if (rotors.Any(r => r is null))
                throw new ArgumentNullException("Rotors Error 3: Rotors cannot have a Null Value");

            //this.rotors.AddRange(rotors);
            this.rotors = rotors;
        }


        /// <summary>
        /// Upon key press the machine has to do a couple of things
        /// The machine must remember where the rotors were located, and what letter was initially pressed
        /// if the rotor is at a stepping position and how the letter has been changed along the way
        /// </summary>
        /// <param name="c">The Letter Pressed</param>
        /// <param name="permutations">A list of the Letter changes upon key press, How the letter is encrypted</param>
        /// <returns></returns>
        public char KeyPress(char c, out List<int> permutations)
        {
            char lastLetter = c;
            int lastIndex = AlphabetConfigs.Instance[lastLetter];
            bool endOfCircuit = false;

            int rotorIndex = 0;

            int increment = 1;

            permutations = new List<int>();

            while (!endOfCircuit)
            {
                Rotor currentRotor = rotors[rotorIndex];

                // from lines 79-92 the machine will send a signal through the rotors to the reflector at the end
                // and then transverse directions back through to the nth indexed rotor at the begining
                int lastStep = currentRotor.GetStepFromKey(lastIndex, increment < 0);
                //Call an event to rotate wheels
               
                lastIndex += lastStep;

                if (lastIndex < 0)
                    lastIndex += 26;
                else
                {
                    lastIndex %= 26;
                }
                permutations.Add(lastStep);

                //When the Machine arrives to the reflector (The last rotor)
                // This is where the signal is reversed and transverses
                //back through the machines in reversed order

                if (currentRotor.IsReflect)
                    increment *= -1;
                rotorIndex += increment;

                if (rotorIndex < 0)
                    endOfCircuit = true;
            }
            return AlphabetConfigs.Instance[lastIndex];

        }

        /// <summary>
        /// Function to return the encrypted letter upon key press
        /// </summary>
        public char KeyPress(char c)
        {
            return KeyPress(c, out _); //_ = discard List<int>
        }

        /// <summary>
        /// To Set initial locations of the Rotor Positions
        /// </summary>
        /// <param name="key">A string of letters the length of the amount of rotors</param>
        /// <exception cref="ArgumentException"></exception>
        public void SetRotorEncryptKey(string key, Device device)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("The encryption key must not be null, empty or contain whitespace.");

            int k = 0;
            foreach (Rotor r in device.rotors)
            {
                if (r.IsReflect == false)
                    r.SetRotorKey(key[k++]);
            }

            int rotorCount = device.rotors.Count();
            if (key.Length != rotorCount -1)
                throw new ArgumentException($"Encryption ERROR 1: {key.Length} != {rotorCount}");
        }

        public string SubmitString(string input)
        {
            if (input == null)
                throw new ArgumentNullException("No Input Entered");

            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var result = input
                .RemoveSpaceAndToUpper()
                .Select(x => KeyPress(x))
                .ConvertToString();

            return result;
        }

    }
}
