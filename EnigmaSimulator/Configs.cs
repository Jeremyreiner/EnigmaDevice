using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

using DeviceModels;
using EnigmaSimulator;
using EnigmaSimulator.Extensions;

namespace Enigma.Configurations
{
    public class Configs
    {
        Dictionary<string, Rotor> rotorModels = new Dictionary<string, Rotor>();
        Dictionary<string, Machine> machineModels = new Dictionary<string, Machine>();

        public Dictionary<string, Machine> MachinModels { get { return machineModels; } }
        public Dictionary<string, Rotor> RotorModels { get { return rotorModels; } }
        UI ui;

        public Configs(string path)
        {
            DeserializeDataFile(path);
        }
        /// <summary>
        /// From a specific url use the given information to create a new device/ machine
        /// </summary>
        /// <param name="path">url of file</param>
        private void DeserializeDataFile(string path)
        {

            try
            {
                XDocument doc = XDocument.Load(path);
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\nWelcome To The Enigma Menu. What Would You Like To Do?\n[1] Continue to Machine Selection\n[2] Create Machine\n");
                Console.ResetColor();
                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        break;
                    case "2":
                        SerializeNewMachine(doc, path);
                        break;
                }
                Deserialize(doc);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Send to the XML File a new machine object
        /// </summary>
        /// <param name="doc">Xml Document Information</param>
        /// <param name="path">url for saving the new object</param>
        public void SerializeNewMachine(XDocument doc, string path)
        {
            XElement parentElement = doc.Root.Element("presets");
            bool loop = true;
            var newDevice = FillNullDevice(parentElement);

            while (loop)
            {

                Console.WriteLine("Enter Machine Configureations\n[1] Machine Name\n[2] Machine Description\n[3] " +
                    "Machine Rotors [Rotors: The Final Indexed Item Of Rotors Should Always Be A Reflector]\n[4] Done");


                switch (Console.ReadLine().PrintTextGreen())
                {
                    case "1":
                        CheckDeviceNameIsComplete(newDevice);
                        break;
                    case "2":
                        CheckDeviceDescriptionIsComplete(newDevice);
                        break;
                    case "3":
                        CheckDeviceRotorsAreComplete(newDevice, doc);
                        break;
                    case "4":
                        loop = CheckDeviceIsComplete(newDevice);
                        break;
                }
                DisplayNewDevice(newDevice);
            }
            parentElement.Add(newDevice);
            doc.Save(path);
        }
        /*------------------Serialize Machine Object Methods------------------------*/

        private XElement FillNullDevice(XElement parentElement)
        {
            var newDevice = new XElement("device");

            var lastdevice = parentElement.Elements("device");
            string LastElementId = lastdevice.Last().Element("id").Value;

            int id = 0;
            foreach (char c in LastElementId)
            {
                id = (int)c + 11;
            }
            newDevice.SetElementValue("id", id);
            newDevice.SetAttributeValue("name", "null");
            newDevice.SetElementValue("description", "null");
            newDevice.SetElementValue("rotors", "null");

            return newDevice;
        }

        private bool CheckDeviceIsComplete(XElement newDevice)
        {
            bool loop = true;
            if (
                newDevice.Attribute("name").Value == "null" || 
                newDevice.Element("description").Value == "null" ||
                newDevice.Element("rotors").Value == "null"
                )
            {
                loop = true;
            }
            else
            {
                loop = false;
            }
            return loop;
        }
        //--------------------Begining of Device Rotors----------------
        private void CheckDeviceRotorsAreComplete(XElement newDevice, XDocument doc)
        {
            var rotorsMainElement = from el in newDevice.Elements()
                                 where el.Name == "rotors"
                                 select el;

            if (rotorsMainElement.First().Value == "null")
            {
                SetDeviceRotors(rotorsMainElement, newDevice, doc);
            }
            else
            {
                Console.WriteLine("Would You Like To Update The Devices Rotors?\n[1] Yes\n[2] No\n");
                if (Console.ReadLine().PrintTextRed() == "1")
                {
                    SetDeviceRotors(rotorsMainElement, newDevice, doc);
                }
            }
        }
        private void SetDeviceRotors(IEnumerable<XElement> rotorsMainElement, XElement newDevice, XDocument doc)
        {
            rotorsMainElement.First().Remove();
            Console.WriteLine("Set Machine Rotors Order [The Rotor \"Reflector\" Is Always The Final Rotor]\n");
            Console.ReadLine().PrintTextGreen();
            
            var rotors = new XElement("rotors");
            var rotorList = SetRotorPosition(doc);

            foreach (string num in rotorList)
            {
                if (num != null)
                {
                    //not checking if rotor entry is correct
                    XElement rotor = new("rotor", num);
                    rotors.Add(rotor);
                }
            }
            newDevice.Add(rotors);
        }
        /// <summary>
        /// For setting rotor order when creating a new machine
        /// </summary>
        /// <returns>rotor tags inside of a rotor list</returns>
        private List<string> SetRotorPosition(XDocument doc)
        {
            int a = 0;
            bool loop = true;
            List<string> rotorsList = new();
            Console.WriteLine("Rotor Options: ");
            ViewRotorTempChoices(doc);
            while (loop)
            {
                string rotor = SelectRotorId(a);
                rotorsList.Add(rotor);
                PrintSelectedRotorsAddToCount(rotorsList);
                loop = ContinueAddingRotors(doc, loop);
                a++;
            }
            return rotorsList;
        }
        private string SelectRotorId(int a)
        {
            Console.WriteLine($"Enter Rotor ID At Index Position: [{a}]\n");
            return Console.ReadLine().PrintTextGreen();
        }
        private void PrintSelectedRotorsAddToCount(List<string> rotorsList)
        {
            Console.WriteLine("Current Machine Rotors\n");
            rotorsList.ForEach(x => Console.Write($"\nRotor {rotorsList.IndexOf(x)}: [{x}]"));
        }
        private bool ContinueAddingRotors(XDocument doc, bool loop = true)
        {
            Console.WriteLine("\n[CLICK ENTER] To Add Another Rotor\n[1 + ENTER] Exit Rotor Positioning\n");

            switch (Console.ReadLine().PrintTextGreen())
            {
                case "1":
                    loop = false;
                    break;
                default:
                    Console.WriteLine("Rotor Options: ");
                    ViewRotorTempChoices(doc);
                    loop = true;
                    break;
            }
            return loop;
        }
        private void ViewRotorTempChoices(XDocument doc)
        {

            foreach (var template in GetRotorsTemplates(doc))
            {
                Console.WriteLine($"[{template.Rotor_Model.Id}] {template.Rotor_Model.Name} {template.Rotor_Model.Type}");
            }
        }
        //--------------------End Of Device Rotors----------------

        //--------------------Begining of Device Description----------------
        static void CheckDeviceDescriptionIsComplete(XElement newDevice)
        {
            var ElementDescription = (from el in newDevice.Elements()
                                      where el.Name == "description"
                                      select el);

            if (ElementDescription.First().Value == "null")
            {
                ElementDescription.First().Remove();
                Console.WriteLine("Machine Description\nIE: ROTOR ID\'S: [1, 2, 3, 4], REFLECTOR ID: [7]\n");
                newDevice.SetElementValue("description", Console.ReadLine());
            }
            else
            {
                ReSetDeviceDescription(newDevice, ElementDescription);
            }
        }
        static void ReSetDeviceDescription(XElement newDevice, IEnumerable<XElement> ElementDescription)
        {
            Console.Write(ElementDescription.First().Value.ToString() + "\n");
            Console.WriteLine("Would You Like To Update The Devices Description?\n[1] Yes\n[2] No\n");
            if (Console.ReadLine().PrintTextRed() == "1")
            {
                ElementDescription.First().Remove();
                Console.WriteLine("Enter Machine Description:\n");
                newDevice.SetElementValue("description", Console.ReadLine());
            }
        }
        //--------------------End Of Device Description----------------

        //--------------------Begining of Device Name----------------
        static void CheckDeviceNameIsComplete(XElement newDevice)
        {
            if (newDevice.Attribute("name").Value == "null")
            {
                newDevice.Attribute("name").Remove();
                Console.WriteLine("Enter Machine Name:\n");
                newDevice.SetAttributeValue("name", Console.ReadLine());
            }
            else
            {
                ReSetDeviceName(newDevice);
            }
        }
        static void ReSetDeviceName(XElement newDevice)
        {
            var DeviceName = (from el in newDevice.Attributes()
                                      where el.Name == "name"
                                      select el);

            Console.Write(DeviceName.First().Value.ToString() + "\n");
            Console.WriteLine("Would You Like To Update The Devices Name?\n[1] Yes\n[2] No\n");
            if (Console.ReadLine().PrintTextRed() == "1")
            {
                DeviceName.First().Remove();
                Console.WriteLine("Enter Machine Name:\n");
                newDevice.SetAttributeValue("name", Console.ReadLine());
            }
        }
        //--------------------End Of Device Name----------------
        static void DisplayNewDevice(XElement newDevice)
        {
            var deviceAttributes = from atttr in newDevice.Attributes()
                                 select atttr.Name;
            
            var deviceElements = from el in newDevice.Elements()
                                 where el.Value != "null"
                                 select el;

            if (deviceAttributes.First() != "null")
            {
                deviceAttributes.First().NamespaceName.PrintTextGreen();
            }
            foreach (XElement el in deviceElements)
            {
                el.Value.PrintTextGreen();
            }
        }
        /*^^^^^^^^^^^^^^^End Of Serializing Machine Object Methods ^^^^^^^^^^^^^^^^^^^^^^^*/
 
        /// <summary>
        /// adds rotor models and machine models for selection
        /// </summary>
        /// <param name="doc"></param>
        private void Deserialize(XDocument doc)
        {
            foreach (var template in GetRotorsTemplates(doc))
            {
                rotorModels.Add(template.Rotor_Model.Id, template);
            }
            foreach (var template in GetEnigmaPresets(doc))
            {
                machineModels.Add(template.machineModel.Id, template);
            }
        }
        /// <summary>
        /// first finding all devices and storing in temp var enigcollection
        /// than with every machine/ device model in the collection. attach 
        /// a name, id, desc, and a list of rotors
        /// rotors then are filtered even more to attach a rotor object
        /// </summary>
        /// <param name="doc">takes stored informational data objects</param>
        /// <returns></returns>
        private IEnumerable<Machine> GetEnigmaPresets(XDocument doc)
        {
            IEnumerable<XElement> enigmaCollection;

            try
            {
                enigmaCollection = doc.Root.Element("presets").Elements("device");
            }
            catch (Exception)
            {
                throw;
            }
            var enigmaModels = from machine in enigmaCollection
                               //using Linq quering
                               select new DeviceModel
                               {
                                   Id = (string)machine.Element("id"),
                                   Name = (string)machine.Attribute("name"),
                                   Description = (string)machine.Element("description"),
                                   Rotors = from rotor in machine.Element("rotors").Elements("rotor") select rotor.Value
                               };
            List<Machine> result = new List<Machine>(enigmaModels.Count());

            foreach (var model in enigmaModels)
            {
                var rotors = from rotor in model.Rotors select rotorModels[rotor];
                Machine device = new Machine(rotors, model);
                result.Add(device);
            }
            return result;
        }

        private IEnumerable<Rotor> GetRotorsTemplates(XDocument doc)
        {
            IEnumerable<XElement> rotorCollection;
            try
            {
                rotorCollection = doc.Root.Element("rotors").Elements("rotor");

            }
            catch (Exception)
            {
                throw;
            }
            var rotorModels = from rotor in rotorCollection
                                  //using Linq quering
                              select new RotorModel
                              {
                                  Name = (string)rotor.Attribute("name"),
                                  Type = (string)rotor.Attribute("type"),
                                  Id = (string)rotor.Element("id"),
                                  Wiring = (string)rotor.Element("wiring"),
                                  Model = (string)rotor.Element("model"),
                              };
            List<Rotor> result = new List<Rotor>(rotorModels.Count());

            foreach (var model in rotorModels)
            {
                Rotor rotor = new Rotor(model);
                result.Add(rotor);
            }
            return result;
        }
        public Machine CreateFromTemplate(string tempId)
        {
            if (machineModels.ContainsKey(tempId))
            {
                return machineModels[tempId];
            }

            throw new Exception($"No template was found for this template Id : {tempId}");
        }
    }
}