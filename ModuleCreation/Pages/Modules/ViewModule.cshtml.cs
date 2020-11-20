using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ModuleCreation.Models;

namespace ModuleCreation.Pages.Modules
{
    public class ViewModuleModel : PageModel
    {



        public List<Module> Records { get; set; }

        [BindProperty(SupportsGet = true)]
        public string Status {get; set;}


        public List<string> StatusItems { get; set; } = new List<string> { "Active", "Not-Active","Suspended" };

        public IActionResult OnGet()
        {
            string DbConnection = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=C:\USERS\ZAIRU\SOURCE\REPOS\MODULECREATION\MODULECREATION\DATA\CREATEMODULE.MDF;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM Module";

                if (!string.IsNullOrEmpty(Status) && Status != "All")
                {
                    command.CommandText += " WHERE ModuleOfferStatus = @stat";
                    command.Parameters.AddWithValue("@stat", Status);
                }
               

                SqlDataReader reader = command.ExecuteReader();

                Records = new List<Module>(); //create the object to collect records
                
                while (reader.Read())
                {
                    Module rec = new Module() ;
                    rec.ModuleCode = reader.GetString(1);
                    rec.ModuleName = reader.GetString(2);
                    rec.ModuleLevel = reader.GetInt32(3);
                    rec.Year = reader.GetString(4);
                    rec.Course = reader.GetString(5);
                    rec.ModuleOfferStatus = reader.GetString(6);

                    Records.Add(rec);
                }

                reader.Close();
            }



             return Page();
        }


        
    }
}
