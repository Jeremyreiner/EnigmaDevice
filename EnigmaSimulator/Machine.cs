using Enigma.Common.Models;
using Enigma.Configurations;
using EnigmaSimulator.Extensions;


namespace EnigmaSimulator
{
    public class Machine
    {
        AlphabetConfigs alphabet = new();

        public DeviceModel machineModel { get; set; }

        public List<Rotor> Rotors { get; }

        /// <summary>
        /// Creates enigma using list of rotors
        /// </summary>
        public Machine(List<Rotor> rotors)
        {
            //check footnotes at bottem
            if (rotors == null)
                throw new ArgumentNullException("Rotors Error 2: No Rotors Were Supplied");

            if (rotors.Any(r => r is null)) 
                throw new ArgumentNullException("Rotors Error 3: Rotors cannot have a Null Value");

            Rotors.AddRange(rotors);
        }

        public Machine(IEnumerable<Rotor> rotors, DeviceModel deviceModel)
        {
            //check footnotes at bottem
            if (rotors == null)
                throw new ArgumentNullException("Rotors Error 2: No Rotors Were Supplied");
            
            if (rotors.Any(r => r is null))
                throw new ArgumentNullException("Rotors Error 3: Rotors cannot have a Null Value");

            Rotors.AddRange(rotors);
            this.machineModel = deviceModel;
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

            Rotor currentRotor;
            int rotorIndex = 0;

            int increment = 1;

            permutations = new List<int>();

            while (!endOfCircuit)
            {
                currentRotor = Rotors[rotorIndex];

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
        public void SetRotorEncryptKey(string key)
        {
            if (string.IsNullOrWhiteSpace(key))
                throw new ArgumentException("The encryption key must not be null, empty or contain whitespace.");
            
            //check footnotes at bottem
            var rotorsNotReflect = (from r in Rotors
                                   where !r.IsReflect
                                   select r);
            int k = 0;
            foreach (Rotor r in rotorsNotReflect)
                r.SetRotorKey(key[k++]);

            int rotorCount = rotorsNotReflect.Count();
            if (key.Length != rotorCount)
                throw new ArgumentException($"Encryption ERROR 1: {key.Length} != {rotorCount}");
        }

        public string SubmitString(string input)
        {
            if (input == null)
                throw new ArgumentNullException("No Input Entered");
         
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            var result = input
                //Check Notes at bottem
                .PreProcess() 
                .Select(x => KeyPress(x)) 
                .ConvertToString(); 
            return result;
        }

    }
}

/*--------------------------------machine footnotes------------------------------------------*/
/*
 * 1: Version 2 Updated To: [line 24, line 35]
  if (rotors.Any(r => r is null)) 
        throw new ArgumentNullException("Rotors Error 3: Rotors cannot have a Null Value");

    Rotors.AddRange(rotors);

 * 1: Version 1 Updated From: [line 24, line 35]
  Inside of public machine(rotors)/ public machine(rotors, machineModel)
    foreach (var rotor in rotors)
    {
        if (rotor == null)
        {
            throw new ArgumentNullException("Rotors Error 3: Rotors cannot have a Null Value");
        }
    }
                 

 * 2: Version 2 Updated To: [Line 153]
      var result = input
        //extension function for string manipulation * function programming
        //using EnigmaExtensions.cs file

        .PreProcess() //Converts Lower To Uppercase, and removes spaces
        .Select(x => KeyPress(x)) // takes each char in the string and runs it through keypress method
        .ConvertToString(); // takes the char array and converts it back into a string
 
 * 2: Version 1 Updated From:  [Line 153]
  
    input = input.ToUpperInvariant();
    input = input.Replace(" ", "");

    StringBuilder sb = new();
    for (int i = 0; i < input.Length; i++)
    {
        sb.Append(KeyPress(input[i]));
    }
    string result = sb.ToString();



 * 3: Version 2 Updated To:[lines 124 - 135]
      var rotorNotReflect = (from r in Rotors
                            where !r.IsReflect
                            select r);
        int k = 0;
        foreach (Rotor r in rotorNotReflect)
            r.SetRotorKey(key[k++]);

 
 * 3: Version 1 Updated From: [lines 124 - 135]
        foreach (Rotor rotor in Rotors)
            {
                //rotor length will not include the Reflector as a rotor
                //if (rotor.Type != Rotor.RotorType.Reflector)
                if (!currentRotor.IsReflect)
                {
                    rotorCount++;
                    rotor.SetRotorKey(key[k]);
                    k++;
                }
            }
 */