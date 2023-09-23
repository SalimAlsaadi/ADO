using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data;

namespace BANK_SYSTEM_WITH_DATABASE
{
    internal class Login
    {
        private string Email { get; set; }
        private string Password { get; set; }
        public static int Cust_ID;
        public static string CustomerName;

        public Login()
        {

        }

        public Login(string Email, string Password)
        {
            this.Email = Email;
            this.Password = Password;
        }

        public void checkLogin()
        {
            Console.Write("Please Enter Your Email: ");
            Email = Console.ReadLine();
            Console.WriteLine();
            Console.Write("Please Enter Your Password: ");
            Password = Console.ReadLine();

            //which database on which server to connect
            string connectionString = "Data Source=(local);Initial Catalog=BANK_SYSTEM; Integrated Security=true";

            // create new object of the main class which will connect us to database 
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            //connect our code base to database server
            sqlConnection.Open();
            try
            {

                
                string LoginString = $"select Customer_Name,Customer_Email, Customer_Password,customer_ID from Users where Customer_Email='{Email}' and Customer_Password='{Password}'";
                 SqlCommand Command = new SqlCommand(LoginString, sqlConnection);
                SqlDataReader reader = Command.ExecuteReader();

                if (reader.HasRows)
                {
                    
                    Console.WriteLine("Login successful");
                    Thread.Sleep(1500);
                    Console.Clear();
                    Console.WriteLine();
                    
                    while (reader.Read())
                    {

                        
                        CustomerName = (reader["Customer_name"].ToString());
                        Cust_ID = int.Parse(reader["customer_ID"].ToString());

                        Console.WriteLine($"    ========================================== Welcome {CustomerName} =========================================");
                       
                    }

                    Command.Dispose();
                    reader.Close();

                    string returnCurrentAccString = $"select distinct(Accounts.Cust_Account),Accounts.currentBalance from dbo.Accounts,dbo.Users where Acc_Number={Cust_ID}";
                    SqlCommand Command2 = new SqlCommand(returnCurrentAccString, sqlConnection);
                    SqlDataReader reader2 = Command2.ExecuteReader();
                   
                        if (reader2.HasRows)
                        {
                            while (reader2.Read())
                            {
                            Console.WriteLine();
                            Console.WriteLine("                          ********************ACCOUNT*********************");
                                Console.WriteLine("                             Account: " + reader2["Cust_Account"].ToString() + " have balance: " + reader2["currentBalance"].ToString()+
                                    "\n                          ************************************************");
                            }
                          Command2.Dispose();
                          reader2.Close();
                        }


                        else
                        {
                           Console.WriteLine();
                           Console.WriteLine();
                           Console.WriteLine($"                      {CustomerName}  unforsurently you don't have account");
                        }

                   


                }
                else
                {
                    Console.WriteLine();
                    Console.WriteLine("Login not Successfull");
                    Thread.Sleep(2000);
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("              =======================Login HomePage=======================");
                    Console.WriteLine();
                    Console.WriteLine();
                    checkLogin();
                }
            }
            catch (Exception e)
            {
                //catch the exception message if any occurs
                Console.WriteLine(e.Message);

            }
            finally
            {
                //after all we need to close the connection with database
                sqlConnection.Close();
            }
        }
    }
}
