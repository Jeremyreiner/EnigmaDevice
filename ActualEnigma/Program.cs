using Enigma.Common.Models;
using ActualEnigma.DeviceRotorPrograms;

using Newtonsoft.Json;
using System.Text;
using System;

HttpClient client = new();
List<RotorModel> _Rotors = new();
List<DeviceModel> _Devices = new();

await MainMethod();

async Task MainMethod()
{
    var deviceModel = await SelectDeviceMenu();
    var rotorsList = await ConvertModelToRotor(deviceModel);
    rotorsList.OrderByDescending(x => x.Type).ToList();
    Device device = new(rotorsList);

    Console.WriteLine($"Your Encryption Key Should Be of {deviceModel.RotorsIds.Count() - 1} Letters Long\nSET ROTOR POSITIONS\n");
    var key = Console.ReadLine().ToUpper();
    device.SetRotorEncryptKey(key, device);

    Console.WriteLine($"Enter Message To Encryption\n");
    var message = Console.ReadLine();   
    var encryption = device.SubmitString(message);

    Console.WriteLine("[ENTER] For Encryption\n");
    Console.ReadLine();
    Console.WriteLine($"\nEncrypted Message: {encryption}\n");

    Console.WriteLine("[ENTER] Continue To Decryption\n");
    Console.ReadLine();

    var decryption = device.SubmitString(encryption);
    Console.WriteLine($"{decryption}\n");

    await MainMethod();
}


async Task<DeviceModel> SelectDeviceMenu()
{
    Console.WriteLine("Welcome to The Console App.");


    DeviceModel deviceModel = null;
    while (deviceModel == null)
    {
        deviceModel = await DeviceOptions();
        if (deviceModel == null)
        {
            Console.WriteLine("[1] Exit Application\n[2] Delete Device\n[3] Return To Device Menu");
            var cont = Console.ReadLine();
            switch (cont)
            {
                case "1":
                    func();
                    break;
                case "2":
                    var device = await SelectDevice();
                    await DeleteRotorFromDb(device.Name);
                    break;
                case "3":
                    break;
            }
        }
    }
    return deviceModel;
}


async Task<List<Rotor>> ConvertModelToRotor(DeviceModel deviceModel)
{
    var rotors = await PrintDeviceRotorList(deviceModel);
    List<Rotor> rotorsList = new();

    foreach (var rotor in rotors)
    {
        Rotor newRotor = new Rotor(rotor);
        rotorsList.Add(newRotor);
    }
    return rotorsList;
}


async Task<DeviceModel> DeviceOptions()
{
    Console.WriteLine("Would you like to\n[1] Create New Device\n[2] Select a Pre-Made Device\n[3] Update a Pre-existing Device\n[ENTER] More Options");
    switch (Console.ReadLine())
    {

        case "1":
            var device = await AddingNewEnigmaDeviceToDb();
            device = await DevicePostUpdate(device);
            return device;

        case "2":
            device = await SelectDevice();
            await ViewSelectedDevice(device);
            return device;
        case "3":
            device = await SelectDevice();
            await UpdateSelectedDevice(device);
            device = await DevicePostUpdate(device);
            return device;
        default:
            //hhello
            return null;
    }
}


async Task ViewEnigmaDevices()
{
    var stream = await client.GetAsync("https://localhost:7260/api/device");
    if (stream.IsSuccessStatusCode)
    {
        var d = await stream.Content.ReadAsStringAsync();
        _Devices = JsonConvert.DeserializeObject<List<DeviceModel>>(d);
    }

    Console.WriteLine("Pre Designed Enigma Machines Listed Below\n");
    foreach (var device in _Devices)
    {
        Console.WriteLine($"\n{device.Name} | {device.Description}");
    }
}


async Task ViewAllRotors()
{
    var stream = await client.GetAsync("https://localhost:7260/api/rotor");
    if (stream.IsSuccessStatusCode)
    {
        var d = await stream.Content.ReadAsStringAsync();
        _Rotors = JsonConvert.DeserializeObject<List<RotorModel>>(d);
    }

    Console.WriteLine("\nPre Designed Enigma Rotors as Listed Below\n");
    foreach (var rotor in _Rotors)
    {
        Console.WriteLine($"\n[{rotor.Name}] {rotor.Type}");
    }
}


async Task ViewSelectedDevice(DeviceModel device)
{
    Console.WriteLine($"\n[{device.Name}] {device.Description}\nWith Rotors\n");
    await PrintDeviceRotorList(device);
    Console.ReadLine();
};


async Task<DeviceModel> SelectDevice()
{
    await ViewEnigmaDevices();

    Console.WriteLine("Enter A device name: ");
    var selection = Console.ReadLine();
    var selectedDevice = _Devices.FirstOrDefault(x => x.Name == selection);

    Console.WriteLine($"Selected Device: {selectedDevice.Name}");
    return selectedDevice;
}


async Task<DeviceModel> DevicePostUpdate(DeviceModel device)
{
    var stream = await client.GetAsync("https://localhost:7260/api/device");
    if (stream.IsSuccessStatusCode)
    {
        var d = await stream.Content.ReadAsStringAsync();
        _Devices = JsonConvert.DeserializeObject<List<DeviceModel>>(d);
    }

    var selectedDevice = _Devices.FirstOrDefault(x => x.Name == device.Name);
    return selectedDevice;
}


async Task<RotorModel> SelectRotor()
{
    await ViewAllRotors();
    
    Console.WriteLine("Select Rotor: ");
    var selection = Console.ReadLine().ToUpper();

    var selectedRotor = _Rotors.FirstOrDefault(x => x.Name.ToString() == selection);

    if (selectedRotor != null)
    {
        Console.WriteLine($"Rotor Selected: [{selectedRotor.Name}] {selectedRotor.Type}");
    }
    else
    {
        Console.WriteLine($"Error: No rotor with name {selection} exists");
    }
    return selectedRotor;
}


async Task<RotorModel> SelectRotorFromDevice(DeviceModel device)
{  
    await PrintDeviceRotorList(device);
    
    Console.WriteLine("Select A Rotor To Remove\n");
    var select = Console.ReadLine().ToUpper();
    var selectedRotor = _Rotors.FirstOrDefault(x => x.Name.ToString() == select);

    if (selectedRotor != null)
    {
        Console.WriteLine($"Rotor Selected: [{selectedRotor.Name}] {selectedRotor.Type}");
    }
    else
    {
        Console.WriteLine($"Error: No rotor with name {select} exists");
    }

    return selectedRotor;
}


async Task<DeviceModel> FillInDeviceInfo()
{
    DeviceModel newDevice = new();
    newDevice.Id = Guid.NewGuid();

    Console.WriteLine("Enter Device Name: ");
    newDevice.Name = Console.ReadLine();

    Console.WriteLine("Enter Device Description: ");
    newDevice.Description = Console.ReadLine();
    
    await ViewAllRotors();
    newDevice.RotorsIds = await UpdateDeviceRotors(newDevice);
    
    return newDevice;
}


async Task UpdateSelectedDevice(DeviceModel device)
{
    var updating = true;
    while (updating)
    {
        Console.WriteLine("\n[1] Update Name\n[2] Update Description\n[3] Update Rotors\n[4] View Device\n[Enter] Save Updated Device");
        
        var update  = Console.ReadLine();
        switch (update)
        {
            case "1":
                Console.WriteLine($"{device.Name}");
                device.Name = await UptdateDeviceName(device);
                break;
            case "2":
                Console.WriteLine($"{device.Description}");
                device.Description = await UptdateDeviceDescription(device);
                break;
            case "3":
                device.RotorsIds = await UpdateDeviceRotors(device);
                break;
            case "4":
                await ViewSelectedDevice(device);
                break;
            default:
                await SendUpdatedDeviceToDb(device);

                updating = false;
                break;
        }
    }
}


async Task<string> UptdateDeviceName(DeviceModel device)
{
    Console.WriteLine("\nEnter Device Name:\n");
    return device.Name = Console.ReadLine();
}


async Task<string> UptdateDeviceDescription(DeviceModel device)
{
    Console.WriteLine("\nEnter Device Description:\n");
    return device.Description = Console.ReadLine();
}


async Task<List<Guid>> UpdateRotorGuidListLoop(DeviceModel device)
{
    bool continueAddingRotors = true;
    while (continueAddingRotors)
    {
        await PrintDeviceRotorList(device);
        Console.WriteLine("\nWould you like to add new rotors or update previous Rotors?\n[1] Add New Rotor\n[2] Remove Rotor\n[Enter] Save Rotor Configurations\n");
        switch (Console.ReadLine())
        {
            case "1":
                var selectedRotor = await SelectRotor();
                device.RotorsIds.Add(selectedRotor.Id);
                break;
            case "2":
                selectedRotor = await SelectRotorFromDevice(device);
                device.RotorsIds.Remove(selectedRotor.Id);
                break;
            default:
                continueAddingRotors = false;
                break;
        }
    }
    return device.RotorsIds;
}


async Task<List<Guid>> UpdateDeviceRotors(DeviceModel device)
{
    device.RotorsIds = await UpdateRotorGuidListLoop(device);

    await PrintDeviceRotorList(device);

    return device.RotorsIds;
}


async Task<List<RotorModel>> PrintDeviceRotorList(DeviceModel device)
{
    if(_Rotors.Count == 0)
    {
        var stream = await client.GetAsync("https://localhost:7260/api/rotor");
        if (stream.IsSuccessStatusCode)
        {
            var d = await stream.Content.ReadAsStringAsync();
            _Rotors = JsonConvert.DeserializeObject<List<RotorModel>>(d);
        }
    }

    List<RotorModel> rotorsList = new();
    if (device.RotorsIds != null)
    {
        foreach (var gui in device.RotorsIds)
        {
            var rotor = _Rotors.FirstOrDefault(x => x.Id == gui);
            rotorsList.Add(rotor);
        }
        foreach (var rotor in rotorsList)
        {
            Console.WriteLine($"[{rotor.Name}] {rotor.Type}");
        }
    }
    else
    {
        device.RotorsIds = new List<Guid>();
        Console.WriteLine($"{device.Name} currently has [0] rotors.");
    }
    return rotorsList;
}


async Task<DeviceModel> AddingNewEnigmaDeviceToDb()
{
    var newDevice = await FillInDeviceInfo();

    HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(newDevice), Encoding.UTF8, "application/json");
     await client.PostAsync("https://localhost:7260/api/device", httpContent);

    return newDevice;
}


async Task SendUpdatedDeviceToDb(DeviceModel device)
{
    HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(device), Encoding.UTF8, "application/json");
    await client.PutAsync("https://localhost:7260/api/device", httpContent);
}


async Task DeleteRotorFromDb(string name)
{
    await client.DeleteAsync($"https://localhost:7260/api/device/{name}");
}

static void func()
{
    Environment.Exit(0);
}