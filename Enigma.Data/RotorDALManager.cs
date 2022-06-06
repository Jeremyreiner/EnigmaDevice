using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Enigma.Data
{
    public class RotorDALManager
    {
        public async Task GetAllRotors()
        {
            var client = new HttpClient();

            var result = client.GetStreamAsync("https://localhost:7260/api/rotor");

            Console.Write(result);

        }
    }
}
