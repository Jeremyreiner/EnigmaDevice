using System.Xml.Linq;

using Enigma.Configurations;
using EnigmaSimulator.Extensions;

namespace EnigmaSimulator
{
    class Program
    {
        static void Main(string[] args)
        {
        
            Configs config = new(data_file);

            UI ui = new(config);
            bool ai = false;
            bool exit = false;
            XDocument doc = XDocument.Load(data_file);

            while (!exit)
            {
                Dictionary<bool, Machine> machineSelect = ui.RequestEnigmaTempChoice(config, ai);
                bool choice = machineSelect.First().Key;

                Machine machine = machineSelect.First().Value;
                switch (choice)
                {
                    case false:
                        ContinueEncrypting(machine, ui);

                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Would You Like To\n[1] Continue Encrypting?\n[2] Exit\n");
                        Console.ResetColor();

                        string response = Console.ReadLine();
                        if (response == "2")
                        {
                            exit = true;
                        }
                        break;
                    case true:
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("Would You Like To\n[1] Continue Encrypting?\n[2] Exit\n");
                        Console.ResetColor();

                        response = Console.ReadLine();
                        if (response == "2")
                        {
                            exit = true;
                        }
                        break;
                }
            }
        }
        static void ContinueEncrypting(Machine machine, UI ui)
        {
            ui.SetMachine(machine);
            try
            {
                string rotorKey = ui.RequestEncryptionKey();
                string encryption = ui.RequestInputString();
                string toDec = "[PRESS ENTER] FOR DECRYPTION\n";

                machine.SetRotorEncryptKey(rotorKey);
                ui.RequestDecryption(encryption);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
    }
}