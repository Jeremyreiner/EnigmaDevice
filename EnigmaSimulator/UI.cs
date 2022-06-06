using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Enigma.Configurations;
using DeviceModels;
using EnigmaSimulator.Extensions;

namespace EnigmaSimulator
{
    public class UI
    {
        Configs config;
        Machine machine;
        Rotor rotor;

        public UI(Configs config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("Configuration Error 1: Configurations cannot be empty");
            }
            this.config = config;
        }
        public void SetMachine(Machine machine)
        {
            this.machine = machine;
        }
        /// <summary>
        /// This function allows the user to select a machine model saved from an imported file
        /// </summary>
        /// <param name="config">Rotor Data/ Machine Data Stored in non local file</param>
        /// <returns>List of machines/ devices</returns>
        public Dictionary<bool, Machine> RequestEnigmaTempChoice(Configs config, bool ai = false)
        {
            Dictionary<bool, Machine> defaultdict = new();

            string select = "SELECT ENIGMA MACHINE MODEL\n";
            select.PrintTextYellow();

            foreach (var device in config.MachinModels)
            {
                string id = device.Value.machineModel.Id;//---------------------------------------------------------------
                if (id == "1")
                {
                    string defaultMachine = $"[{id}] \"DEFAULT MACHINE\":\n[{device.Value.machineModel.Name} " +
                        $"{device.Value.machineModel.Description}]\n";
                    defaultMachine.PrintTextYellow();
                }
                else
                {
                    string selections = $"[{id}] {device.Value.machineModel.Name}: {device.Value.machineModel.Description}\n";
                    selections.PrintTextYellow();

                }
            }
            var choice = Console.ReadLine();
            if (choice == "1")
            {
                ai = true;
                string defaultm = $"\nDefault Machine: {choice}\n";
                defaultm.PrintTextYellow();

                var defaultM = config.MachinModels[choice];

                return RunDefaultMachine(defaultM, defaultdict, ai);
            }
            if (choice != null)
            {
                string selected = $"\nMACHINE: {choice}\n";
                selected.PrintTextYellow();

                defaultdict.Add(ai, config.MachinModels[$"{choice}"]);
                return defaultdict;
            }
            return null;
        }
        /// <summary>
        /// This asks the user to set the initial rotor positions
        /// </summary>
        /// <returns>Sets Rotor Positions</returns>
        public string RequestEncryptionKey()
        {
           string encryptKey = "Input Encryption Key:\nIE: A Letter Point For Each Rotor In Enigma\n" +
                "[If the Machine has 3 Rotors (Not Including a Reflector) Then a Valid Encryption key is \"ASF\"]";
            encryptKey.PrintTextYellow();

            string key = Console.ReadLine().ToUpper();

            machine.SetRotorEncryptKey(key);
            string stringkey = $"Encryption Key Has Been Set To: {key}\n";
            stringkey.PrintTextYellow();

            return key;
        }

        public string RequestInputString()
        {
            string input = "Input Text To Encrypt:\n";
            input.PrintTextYellow();

            var encryption = machine.SubmitString(Console.ReadLine());

            string inputEnc = $"The Encrypted Text is: {encryption}\n";
            inputEnc.PrintTextYellow(); 

            return encryption;
        }
        public string RequestDecryption(string encr)
        {
            string decrypt = machine.SubmitString(encr);
            string toDec = $"The Decrypted Text is: {decrypt}\n";
            decrypt.PrintTextYellow();
            return decrypt;
        }

        private Dictionary<bool, Machine> RunDefaultMachine(Machine defaultM, Dictionary<bool, Machine> defaultdict, bool ai)
        {
            defaultM.SetRotorEncryptKey("ASDF");
            string mssg = "Hello World";
            string encryt = defaultM.SubmitString(mssg);
            string decr = defaultM.SubmitString(encryt);
            defaultdict.Add(ai, defaultM);
            string defaultMessage = $"Message Sent: {mssg}\nMessage Encrypted: {encryt}\nMessage Decrypted: {decr}";
            defaultMessage.PrintTextYellow();

            return defaultdict;
        }
    }
}