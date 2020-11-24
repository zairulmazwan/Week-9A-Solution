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
    public class DeleteModel : PageModel
    {
        [BindProperty]
        public List<Module> Mod { get; set; }

        [BindProperty]
        public List<bool> IsSelect { get; set; } //this is needed to allow the user to select the checkbox


        public List<Module> ModToDelete { get; set; } //this variable is a list to collect the selected modules to be deleted

        public IActionResult OnGet()
        {
            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            using (SqlCommand command = new SqlCommand())
            {
                command.Connection = conn;
                command.CommandText = "SELECT * FROM Module";

                SqlDataReader reader = command.ExecuteReader();

                Mod = new List<Module>(); //create an object to collect records
                IsSelect = new List<bool>();
                while (reader.Read())
                {
                    Module rec = new Module();
                    rec.Id = reader.GetInt32(0); //the primary key in the table. This is mandatory to have.
                    rec.ModuleCode = reader.GetString(1);
                    rec.ModuleName = reader.GetString(2);
                    rec.ModuleLevel = reader.GetInt32(3);
                    rec.Year = reader.GetString(4);
                    Mod.Add(rec);
                    IsSelect.Add(false);// every record should have 1 checkbox, but we set to false first!
                    //You can populate all the fields of Module. For this example, we display a few fields.
                }
            }


            return Page();
        }

        public IActionResult OnPost()
        {
            ModToDelete = new List<Module>();//create the object for Module to be deleted
            for (int i = 0; i < Mod.Count; i++) //Read all rows from Module. Each row has a checkbox!
            {
                if (IsSelect[i] == true) //if the checkbox of the row is true
                {
                    ModToDelete.Add(Mod[i]); //collect the item for the row
                }
            }

            Console.WriteLine("Module to be deleted : ");

            for (int i = 0; i < ModToDelete.Count(); i++)
            {
                Console.WriteLine(ModToDelete[i].Id); // we need to write SQL statement to delete the module based on this Id
                Console.WriteLine(ModToDelete[i].ModuleCode); //We can also use this as the key to delete the module (depending on Db design)
                Console.WriteLine(ModToDelete[i].ModuleName);
                //We can have more fields 
            }

            //Starting writing the DB connnection and SQL statement to delete the selected modules.

            DBConnection dbstring = new DBConnection();
            string DbConnection = dbstring.DbString();
            SqlConnection conn = new SqlConnection(DbConnection);
            conn.Open();

            for (int i = 0; i < ModToDelete.Count(); i++)
            {

                using (SqlCommand command = new SqlCommand())
                {
                    command.Connection = conn;
                    command.CommandText = @"DELETE FROM Module WHERE Id = @ModID";
                    command.Parameters.AddWithValue("@ModID", ModToDelete[i].Id);
                    command.ExecuteNonQuery();
                }
            }

            return RedirectToPage("/Modules/ViewModule");
        }
    }
}
