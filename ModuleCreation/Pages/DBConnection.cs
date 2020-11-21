using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ModuleCreation.Pages
{
    public class DBConnection
    {
        public string DbString ()
        {
            string dbString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\zairu\source\repos\SelectCheckBoxModuleTEST\ModuleCreation\Data\CreateModule.mdf;Integrated Security=True;Connect Timeout=30";
            return dbString;
        }
    }
}
