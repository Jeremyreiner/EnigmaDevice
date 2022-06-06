using Enigma.Data;

<<<<<<< HEAD
DeviceDALManager device = new();

await device.GetAllDevices();

Console.ReadLine();
=======
UserDALManager manager = new();
await manager.GetAllUsers();

await manager.AddUser();

Console.ReadKey();
>>>>>>> 87bef7d21623532defbb28b666aebbf36fec8111
