using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

using Enigma.Configurations;
using EnigmaSimulator;
using ActualEnigma.DeviceRotorPrograms;
using Enigma.Common.Models;

namespace EnigmaMachine.Tests
{
    public class RotorTests
    {
        [Fact]
        public void RotorModelConstructor()
        {
            Rotor rotor = InstanciateRotor();

            Assert.Equal(26, rotor.Length);
        }

        [Theory]
        [InlineData('F')]
        public void RotorSteppedAndSetRotorToAreEqual(char c)
        {
            Rotor rotor = InstanciateRotor();

            Connection[] setConnections = SetRotorConnections(rotor, c);

            Connection[] steppedConnections = SteppedRotorConnections(rotor, c);

            Assert.Equal(setConnections, steppedConnections);
        }

        /*---------------------End Of Testing Methods-------------------------*/

        public Rotor InstanciateRotor()
        {
            var rotor = new RotorModel
            {
                Id = Guid.NewGuid(),
                RotorId = 1,
                Name = "I",
                Type = "rotor",
                IsReflect = false,
                Wiring = AlphabetConfigs.AlphaString
            };
            return new Rotor(rotor);
        }
        static Connection[] SetRotorConnections(Rotor rotor, char c)
        {
            rotor.SetRotorKey(c);
            return rotor.Wiring;
        }
        static Connection[] SteppedRotorConnections(Rotor rotor, char c)
        {
            int step = FindStep(c);
            rotor.StepRotor(step);

            return rotor.Wiring;

        }
        static int FindStep(char c)
        {
            int keyToInt = -1;

            for (int i = 0; i < AlphabetConfigs.Alphabet.Length; i++)
            {
                if (AlphabetConfigs.Alphabet[i].Equals(c))
                {
                    keyToInt = i;
                    break;
                }
            }
            return keyToInt;
        }
    }

}
