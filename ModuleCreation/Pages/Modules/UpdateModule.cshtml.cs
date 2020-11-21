using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModuleCreation.Models;

namespace ModuleCreation.Pages.Modules
{
    public class UpdateModuleModel : PageModel
    {
        [BindProperty]
        public Module Mod { get; set; }

        public List<int> Level = new List<int> { 4, 5, 6, 7 }; //initialise the list 
        public List<string> Year = new List<string> { "1", "2", "3", "4" }; //initialise the array - uses checkbox

        public List<SelectListItem> Course { get; set; } = new List<SelectListItem>
        {
            new SelectListItem { Text = "Computer Science", Value = "Computer Science"},
            new SelectListItem { Text = "Software Engineering", Value = "Software Engineering"},
            new SelectListItem { Text = "Network", Value = "Network"},
            new SelectListItem { Text = "Artificial Intelligence", Value = "Artificial Intelligence"}
        };

        public string[] StatusOfModule = new string[2] { "Active", "Not-Active" };

        public IActionResult OnGet(int? Id) 
        {
            Mod = new Module();
            Mod.ModuleYear = new List<bool>{ false, false, false, false};

            DBConnection db = new DBConnection();
            string DbConnection = db.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM Module WHERE Id = @ID";
                command.Parameters.AddWithValue("@ID", Id);

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    Mod.ModuleCode = reader.GetString(1);
                    Mod.ModuleName = reader.GetString(2);
                    Mod.ModuleLevel = reader.GetInt32(3);
                    Mod.Year = reader.GetString(4);
                    Mod.Course = reader.GetString(5);
                    Mod.ModuleOfferStatus = reader.GetString(6);
                }

            }

            //Getting Year from string to array format
            string[] year = Mod.Year.Split(",");

            //Getting Course from string to array format
            string[] course = Mod.Course.Split(",");
            Console.WriteLine("Len " + year.Length);

        
            for (int i=0; i<year.Length-1; i++)//ignore the last element = 1,2,3,
            {
                int index = Int32.Parse(year[i]); //getting the year which represent the position of checkbox
                Console.WriteLine(index);
                Mod.ModuleYear.Insert(index, true);
            }

           
            for (int i=0; i<Course.Count(); i++)
            {
                for (int j = 0; j < course.Length; j++)
                {
                    if (course[j] == Course[i].Value)
                    {
                        Course[i].Selected = true;
                    }
                }
            }
           

            /*
            foreach (var item in selectList.Items)
            {
                if (item.Value == selectedValue)
                {
                    item.Selected = true;
                    break;
                }
            }*/


            return Page();
        }
    }
}
