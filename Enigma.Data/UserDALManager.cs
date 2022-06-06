using Newtonsoft.Json;
using System.Text;

namespace Enigma.Data
{
    public class UserDALManager
    {
        public async Task GetAllUsers()
        {
            var client = new HttpClient();

            var result = await client.GetStringAsync("http://localhost:5126/api/User");
     
            Console.WriteLine(result);
        }

        public async Task AddUser()
        {
            var client = new HttpClient();

            var user = new UserEntity()
            {
                Id = Guid.NewGuid(),
                FirstName = "Chicken",
                LastName = "Nugget",
                Email = "WOW@WOW.com"
            };

            HttpContent content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            var add = await client.PostAsync("http://localhost:5126/api/User", content);
        }
    }
}
