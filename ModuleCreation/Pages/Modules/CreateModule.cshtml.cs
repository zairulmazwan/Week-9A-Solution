using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using ModuleCreation.Models;

namespace ModuleCreation.Pages.Modules
{
    public class CreateModuleModel : PageModel
    {
        [BindProperty]
        public Module ModuleObject { get; set; }

        public List<int> Level = new List<int> { 4, 5, 6, 7 }; //initialise the list 
        public List<string> Year = new List<string>{ "1", "2", "3", "4" }; //initialise the array - uses checkbox
     
        public List<SelectListItem> Course = new List<SelectListItem>
        {
            new SelectListItem { Text = "Computer Science", Value = "Software Engineering"},
            new SelectListItem { Text = "Software Engineering", Value = "Software Engineering"},
            new SelectListItem { Text = "Network", Value = "Network", Selected = true },
            new SelectListItem { Text = "Artificial Intelligence", Value = "Artificial Intelligence"}
        };

        public string[] StatusOfModule = new string[2] { "Active", "Not-Active" };

        /*
        [BindProperty]
        public List<bool> ModYearIsCheck { get; set; } = new List<bool>() {false, false, false, false };

        [BindProperty]
        public List<string> ModCourseIsSelect { get; set; } = new List<string>();
        */

        public void OnGet()
        {
            Console.WriteLine("Onget loaded");
          

        }

        public IActionResult OnPost()
        {
            string StringYear = "";
            for (int i = 0; i < ModuleObject.ModuleYear.Count; i++)
            {
                if (ModuleObject.ModuleYear[i] == true)
                {
                    StringYear += Year[i] + ",";
                }
            }


            if (!ModelState.IsValid)
            {
                ModuleObject.ModuleCourse.Clear();
                return Page();
            } 

            string Course = "";
            foreach (var course in ModuleObject.ModuleCourse)
            {
                Course += course + ",";
            }

            Console.WriteLine("Code : " + ModuleObject.ModuleCode);
            Console.WriteLine("Name : " + ModuleObject.ModuleName);
            Console.WriteLine("Level : " + ModuleObject.ModuleLevel);
            Console.WriteLine("Year : " + StringYear);
            Console.WriteLine("Course : " + Course);
            Console.WriteLine("Status : " + ModuleObject.ModuleOfferStatus);



            string DbConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\ZAIRU\SOURCE\REPOS\MODULECREATION\MODULECREATION\DATA\CREATEMODULE.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand ())
            {
                command.Connection = conn;
                command.CommandText = @"INSERT INTO Module (ModuleCode, ModuleName, ModuleLevel, ModuleYear, ModuleCourse, ModuleOfferStatus) VALUES (@ModCode, @ModName, @ModLevel, @ModYear, @ModCourse, @ModStat)";

                command.Parameters.AddWithValue("@ModCode",ModuleObject.ModuleCode);
                command.Parameters.AddWithValue("@ModName", ModuleObject.ModuleName);
                command.Parameters.AddWithValue("@ModLevel", ModuleObject.ModuleLevel);
                command.Parameters.AddWithValue("@ModYear", StringYear);
                command.Parameters.AddWithValue("@ModCourse", Course);
                command.Parameters.AddWithValue("@ModStat", ModuleObject.ModuleOfferStatus);

                command.ExecuteNonQuery();
            }

           return RedirectToPage("/Index");
        }

    }
}
