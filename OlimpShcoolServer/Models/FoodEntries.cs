using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodServer.Models
{
    public class FoodEntries
    {
        public int id { get; set; }

        public int student { get; set; }
        public DateTime date { get; set; }

        public bool is_breakfast { get; set; }

        public bool is_lunch { get; set; }

        public bool is_after_lunch { get; set; }

        public bool is_dinner { get; set; }

        public bool is_breakfast_competition { get; set; }

        public bool is_lunch_competition { get; set; }

        public bool is_after_lunch_competition { get; set; }

        public bool is_dinner_competition { get; set; }
    }
}
