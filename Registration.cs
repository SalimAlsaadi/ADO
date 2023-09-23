using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Data.SqlClient;

namespace BANK_SYSTEM_WITH_DATABASE
{
    internal class Registration
    {
        private string user_name { get; set; }
        private string user_email { get; set; }
        private string user_password { get; set; }
        
        public Registration()
        {

        }

        public Registration(string user_name, string user_email, string user_password)
        {
            this.user_name = user_name;
            this.user_email = user_email;
            this.user_password = user_password;

        }

        public void NewUser()
        {
            Console.Write("Please Enter your name: ");
            user_name = Console.ReadLine();
            Console.WriteLine();
            Console.Write("Please Enter your Email: ");
            user_email = Console.ReadLine();
            Console.WriteLine();
            Console.WriteLine("Please Enter your Password, such that password consist of 8 characters, at least one capital letter and one specail character");
            Console.Write("create Password: ");
            user_password = Console.ReadLine();           

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasMinimum8Chars = new Regex(@".{8,}");
            var isValidated = hasNumber.IsMatch(user_password) && hasUpperChar.IsMatch(user_password) && hasMinimum8Chars.IsMatch(user_password);
            if (isValidated.Equals(true))
            {
                //which database on which server to connect
                string connectionString = "Data Source=(local);Initial Catalog=BANK_SYSTEM; Integrated Security=true";

                // create new object of the main class which will connect us to datbase 
                SqlConnection sqlConnection = new SqlConnection(connectionString);

                try
                {
                    //connect our code base to database server
                    sqlConnection.Open();

                    string insertString = $"insert into dbo.Users values('{user_name}','{user_email}','{user_password}')";
                    SqlCommand Command = new SqlCommand(insertString, sqlConnection);
                    int returns = Command.ExecuteNonQuery();
                    Console.WriteLine("Number of affecte rows = " + returns);


                }

                catch (Exception e)
                {
                    //catch the exception message if any occurs
                    Console.WriteLine(e.Message);
                    NewUser();
                }
                finally
                {
                    //after all we need to close the connection with database
                    sqlConnection.Close();
                }
            }
            else
            {
                Console.WriteLine("invalide password, please try again");
            }
        }
    }
}
