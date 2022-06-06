using Xunit;



using EnigmaSimulator;
using ActualEnigma.DeviceRotorPrograms;
using Enigma.Common.Models;

namespace EnigmaMachine.Tests
{
    public class DeviceTests
    {
        [Theory]
        [InlineData("Hello World")]
        public void EncryptionStatus(string mssg)
        {
            var rotors = RotorsList();
            Device device = new(rotors);
            device.SetRotorEncryptKey("JF", device);

            string enc = device.SubmitString(mssg);

            Assert.NotEqual(mssg, enc);
        }
        [Theory]
        [InlineData("HELLOWORLD")]
        public void DecryptionStatus(string mssg)
        {
            var rotors = RotorsList();
            Device device = new(rotors);
            device.SetRotorEncryptKey("JF", device);

            string enc = device.SubmitString(mssg);
            string dec = device.SubmitString(enc);
            Assert.Equal(mssg, dec);

        }

        /*---------Everything Below are Testing Methods--------------*/
        static List<Rotor> RotorsList()
        {
            List<RotorModel> rotorModels = new();
            List<Rotor> rotors = new();

            string[] NameModelVars = { "I", "II", "A" };
            string[] TypeList = { "rotor", "reflector" };
            string[] WiringList =
            {
                "DMTWSILRUYQNKFEJCAZBPGXOHV",
                "HQZGPJTMOBLNCIFDYAWVEUSRKX",
                "IMETCGFRAYSQBZXWLHKDVUPOJN"
            };

            //hard push original rotors to database
            for (int i = 0; i <= NameModelVars.Length - 1; i++)
            {
                string RotorType = "rotor";
                bool IsReflect = false;
                if (NameModelVars[i] == "A")
                {
                    RotorType = TypeList[1];
                    IsReflect = true;
                }
                var rotorModel = new RotorModel
                {
                    Id = Guid.NewGuid(),
                    RotorId = i,
                    Name = NameModelVars[i],
                    Type = RotorType,
                    IsReflect = IsReflect,
                    Wiring = WiringList[i]
                };
                rotorModels.Add(rotorModel);
            }


            foreach (var rotor in rotorModels)
            {
                var r = new Rotor(rotor);
                rotors.Add(r);
            }

            return rotors;
        }
    }
}
