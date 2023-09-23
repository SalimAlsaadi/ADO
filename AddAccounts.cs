using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
namespace BANK_SYSTEM_WITH_DATABASE

{
    internal class AddAccounts
    {
        Random random = new Random();
        private long NewAccountNumber { get; set; }
        private double currentBalance { get; set; }
        
        public AddAccounts()
        {

        }

       

       public void newAccount()
        {
            NewAccountNumber = random.Next();
            Console.WriteLine("your new Acount Number is: " + NewAccountNumber);
            Console.Write("please Enter balance for new account: ");
            currentBalance = double.Parse(Console.ReadLine());

            //which database on which server to connect
            string connectionString = "Data Source=(local);Initial Catalog=BANK_SYSTEM; Integrated Security=true";

            // create new object of the main class which will connect us to database 
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //connect our code base to database server
            sqlConnection.Open();

            try
            {
                string AddNewAccountString = $"insert into dbo.Accounts values({NewAccountNumber},'{Login.CustomerName}',{currentBalance},{Login.Cust_ID})";
                SqlCommand command = new SqlCommand(AddNewAccountString, sqlConnection);

                int returns = command.ExecuteNonQuery();
                Console.WriteLine("Number of affecte rows = " + returns);
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }



        }
    }
}
