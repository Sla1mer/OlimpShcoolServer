using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodServer.Models
{
    public class Users
    {
        public int id { get; set; }
        public string login { get; set; }
        public string password { get; set; }
        public string last_name { get; set; }
        public string name { get; set; }
        public string middle_name { get; set; }
        public bool is_admin { get; set; }
        public string class_group { get; set; }
        public string sport { get; set; }
    }
}
