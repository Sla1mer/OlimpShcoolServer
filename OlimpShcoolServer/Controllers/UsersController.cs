using FoodServer.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace FoodServer.Controllers
{
    public class UsersController : Controller
    {
        ApplicationContext dbContext = new ApplicationContext();


        [HttpGet]
        [Route("users")]
        public async Task<IEnumerable<Users>> get()
        {
            List<Users> data = await dbContext.Users.ToListAsync();
            return data;

            // Пример запроса /users
        }


        [HttpGet]
        [Route("users/{id}")]
        public async Task<Users> GetById(int id)
        {
            try
            {
                Users data = await dbContext.Users.Where(u => u.id == id).FirstAsync();
                System.Diagnostics.Debug.WriteLine(data);
                return data;
            }
            catch (InvalidOperationException e)
            {

                return null;
            }

            // Пример запроса /users/1
        }

        [HttpPost]
        [Route("users/createUser")]
        public async Task<string> createUser([FromBody] Users user)
        {
            try
            {
                await dbContext.Users.AddAsync(user);
                await dbContext.SaveChangesAsync();
                return "User created";
            }
            catch (DbUpdateException e)
            {
                return e.Message.ToString();
            }

            // Пример запроса
            /*
             * /users/createUser
                {
                    "login": "test5",
                    "password": "test5",
                    "last_name": "test5",
                    "name": "test5",
                    "middle_name": "test5",
                    "is_admin": false,
                    "class_group": "11-ГО",
                    "sport": "Горный спорт"
                }
             */
        }
    }
}
