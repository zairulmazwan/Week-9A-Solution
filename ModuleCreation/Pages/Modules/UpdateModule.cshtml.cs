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

        public IActionResult OnGet(int? Id) //int? int Id = the Id from the table for the record
        {
            Mod = new Module();
            Mod.ModuleYear = new List<bool>{ false, false, false, false}; //we set the checkboxes as false first

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
                    Mod.Id = reader.GetInt32(0);
                    Mod.ModuleCode = reader.GetString(1);
                    Mod.ModuleName = reader.GetString(2);
                    Mod.ModuleLevel = reader.GetInt32(3);
                    Mod.Year = reader.GetString(4);
                    Mod.Course = reader.GetString(5);
                    Mod.ModuleOfferStatus = reader.GetString(6);
                }

            }

            //Getting Year from string to array format
            string[] year = Mod.Year.Split(","); //this variable is string from database. Split the string into an array

            //Getting Course from string to array format
            string[] course = Mod.Course.Split(","); //this variable is string from database. Split the string into an array
            Console.WriteLine("Len " + year.Length);

            //each year item indicates the index of the checkbox
            for (int i=0; i<year.Length-1; i++)//ignore the last element = 1,2,3,
            {
                int index = Int32.Parse(year[i]); //getting the year which represent the position of checkbox
                Console.WriteLine(index);
                Mod.ModuleYear.Insert((index-1), true); //set the checkbox list as true for the index (module year)
            }

            //reading the course from the list
            for (int i=0; i<Course.Count(); i++)
            {
                for (int j = 0; j < course.Length; j++) //reading the course from the table
                {
                    if (course[j] == Course[i].Value)//if the same course found from the table
                    {
                        Course[i].Selected = true; //set the item as true (selected) if the item is found from Db
                    }
                }
            }

            return Page();
        }




        public IActionResult OnPost()
        {
            string StringYear = "";
            for (int i = 0; i < Mod.ModuleYear.Count; i++)
            {
                if (Mod.ModuleYear[i] == true)
                {
                    StringYear += Year[i] + ",";
                }
            }


            if (!ModelState.IsValid)
            {
                Mod.ModuleCourse.Clear();
                return Page();
            }

            string Course = "";
            foreach (var course in Mod.ModuleCourse)
            {
                Course += course + ",";
            }

            Console.WriteLine("::OnPost::");
            Console.WriteLine("Id : " + Mod.Id);
            Console.WriteLine("Code : " + Mod.ModuleCode);
            Console.WriteLine("Name : " + Mod.ModuleName);
            Console.WriteLine("Level : " + Mod.ModuleLevel);
            Console.WriteLine("Year : " + StringYear);
            Console.WriteLine("Course : " + Course);
            Console.WriteLine("Status : " + Mod.ModuleOfferStatus);


            DBConnection db = new DBConnection();
            string DbConnection = db.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = @"UPDATE Module SET ModuleCode=@ModCode, ModuleName=@ModName, ModuleLevel=@ModLevel, ModuleYear=@ModYear, ModuleCourse=@ModCourse, ModuleOfferStatus=@ModStat WHERE Id = @Id";

                command.Parameters.AddWithValue("@Id", Mod.Id);
                command.Parameters.AddWithValue("@ModCode", Mod.ModuleCode);
                command.Parameters.AddWithValue("@ModName", Mod.ModuleName);
                command.Parameters.AddWithValue("@ModLevel", Mod.ModuleLevel);
                command.Parameters.AddWithValue("@ModYear", StringYear);
                command.Parameters.AddWithValue("@ModCourse", Course);
                command.Parameters.AddWithValue("@ModStat", Mod.ModuleOfferStatus);

                command.ExecuteNonQuery();
            }

            return RedirectToPage("/Index");
        }
    }
}
