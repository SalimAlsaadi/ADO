using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using Microsoft.Identity.Client;

namespace BANK_SYSTEM_WITH_DATABASE
{
    internal class Transaction
    {
       public long ChoosAccNum { get; set; }
       //public double amountCheck { get; set; }

        public Transaction()
        {

        }



        public void ControlTransaction()
        {
            //which database on which server to connect
            string connectionString = "Data Source=(local);Initial Catalog=BANK_SYSTEM; Integrated Security=true";

            // create new object of the main class which will connect us to database 
            SqlConnection sqlConnection = new SqlConnection(connectionString);

            Console.WriteLine();
            Console.WriteLine("1-Add New Account");
            Console.WriteLine("2-Deposite");
            Console.WriteLine("3-Withdraw");
            Console.WriteLine("4-Transfer");
            Console.WriteLine("5-History for yor transaction");
            Console.WriteLine();
            Console.Write("Please choose transaction type: ");
            int i = int.Parse(Console.ReadLine());

            switch (i)
            {
                case 1://Deposite
                    AddAccounts add = new AddAccounts();
                    add.newAccount();
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine();
                    Console.WriteLine("*-*-*-*-*-*-*-*-*-*-*Congratulation for new Account*-*-*-*-*-*-*-*-*-*-*");
                    Console.WriteLine();
                    Thread.Sleep(1000);
                    Login login = new Login();
                    login.checkLogin();
                    break;
                case 2:
                    Deposite();
                    //Console.WriteLine("Deposite");
                    break;

                case 3://Withdraw
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine();
                    withDrawwithdraw();
                    //Console.WriteLine("Withdraw");
                    break;

                case 4://Transfer
                    Console.Clear();
                    Console.WriteLine();
                    Console.WriteLine("     *-*-*-*-*-*-*-*-*-*-Transfer Page*-*-*-*-*-*-*-*-*-*-*-*-*-*-");
                    Console.WriteLine();
                    transfer();
                    //Console.WriteLine("Transfer");
                    break;

                case 5://History for transactions
                    historyTransaction();
                    //Console.WriteLine("History for transaction");
                    break;

                default:
                    Console.WriteLine("Sorry you are choosing wrong, please choose again");
                    Console.Clear();
                    ControlTransaction();
                    break;
            }
        }

        private void checkAcount()
        {
            //which database on which server to connect
            string connectionString = "Data Source=(local);Initial Catalog=BANK_SYSTEM; Integrated Security=true";

            // create new object of the main class which will connect us to database 
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            string returnCurrentAccString = $"select distinct(Accounts.Cust_Account),Accounts.currentBalance from dbo.Accounts,dbo.Users where Acc_Number={Login.Cust_ID}";
            SqlCommand Command2 = new SqlCommand(returnCurrentAccString, sqlConnection);
            SqlDataReader reader2 = Command2.ExecuteReader();

            if (reader2.HasRows)
            {
                Console.WriteLine();

                while (reader2.Read())
                {


                    Console.WriteLine();
                    Console.WriteLine("************************************************ACCOUNT***********************************************");
                    Console.WriteLine("   Account: " + reader2["Cust_Account"].ToString() + " have balance: " + reader2["currentBalance"].ToString());
                }
                Console.WriteLine();
                Console.WriteLine("************************************************ACCOUNT****************************************************");
                Console.WriteLine();

               


                Command2.Dispose();
                reader2.Close();

            }


            else
            {
                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine($"                      {Login.CustomerName}  unforsurently you don't have account");
                Thread.Sleep( 1000 );
                Console.Clear();
                ControlTransaction();
            }


        }

        private void Deposite()
        {
            DateTime date = DateTime.Now; // will give the date for today
            
            Random random = new Random();
            int transactionID = random.Next();
            string transactionType = "Deposite";

            Console.Write("Enter Account Number: ");
            long beneficiaryAcc = long.Parse(Console.ReadLine());
            Console.WriteLine();

            //which database on which server to connect
            string connectionString = "Data Source=(local);Initial Catalog=BANK_SYSTEM; Integrated Security=true";

            // create new object of the main class which will connect us to database 
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();

            string checkAcc = $"select Cust_Account, AccHolderName from dbo.Accounts where Cust_Account= {beneficiaryAcc}";
            SqlCommand command = new SqlCommand(checkAcc, sqlConnection);
            SqlDataReader readerAcc = command.ExecuteReader();

            if (readerAcc.HasRows)
            {
                while (readerAcc.Read())
                {
                    Console.WriteLine(" Account: " + readerAcc["Cust_Account"] + " for Customer " + readerAcc["AccHolderName"]);
                }
            }
            else
            {
                Console.WriteLine("This Account " + beneficiaryAcc + " No Exist");
                Thread.Sleep(1500);
                Console.Clear();
                Console.WriteLine();
                Deposite();
            }

            command.Dispose();
            readerAcc.Close();

            try
            {
                Console.Write("Please Enter Money: ");
                Double money = double.Parse(Console.ReadLine());
                string DepositeAmount = $"update dbo.Accounts set currentBalance=currentBalance+{money} where Cust_Account= {beneficiaryAcc}";
                string updateTransactionTable = $"insert dbo.User_Transaction (TransId,Trans_Timestamp,trans_type,rans_amount,TargetAccNO,Acc_Number) values ({transactionID},'{date}','{transactionType}',{money},{beneficiaryAcc},{beneficiaryAcc})";
                SqlCommand commandDepositeAmount = new SqlCommand(DepositeAmount, sqlConnection);
                SqlCommand commandUpdateTransactionTable = new SqlCommand(updateTransactionTable, sqlConnection);
                commandDepositeAmount.ExecuteNonQuery();
                commandUpdateTransactionTable.ExecuteNonQuery();
                commandDepositeAmount.Dispose();
                commandUpdateTransactionTable.Dispose();
                
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            
            

        }

        private void withDrawwithdraw()
        {
            
                checkAcount();
                /////////////////////////////////////////////////open connection with SQL////////////////////////////////////////////////
                //which database on which server to connect
                string connectionString = "Data Source=(local);Initial Catalog=BANK_SYSTEM; Integrated Security=true";

                // create new object of the main class which will connect us to database 
                SqlConnection sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

                ////////////////////////Data requered////////////////////////////////////////////////////////////////////////////////
                Console.Write(" which account want to Withdraw from: ");
                ChoosAccNum = long.Parse(Console.ReadLine());
                DateTime date = DateTime.Now; // will give the date for today
                Random random = new Random();
                int transactionID = random.Next();
                string transactionType = "WithDraw";
                double amountCheck=0.0;
                //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////


                ///////////////////////////////////////////////check amount in account in database///////////////////////////////////
            Console.Write("Please Enter amount:");
            double amount = double.Parse(Console.ReadLine());
            //string checkAmountwith = $"select AccHolderName from Accounts where Accounts.Cust_Account={ChoosAccNum}";
            //SqlCommand commandCheckAmount = new SqlCommand(checkAmountwith, sqlConnection);

           // SqlDataReader reader = commandCheckAmount.ExecuteReader();
           // amountCheck = double.Parse(reader["currentBalance"].ToString());
            //string name=commandCheckAmount.ExecuteReader().ToString();
           // commandCheckAmount.Dispose();
            //Console.WriteLine(name);
            //reader.Close();


            //commandCheckAmount.Dispose();
            //reader.Close();
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            //if (amount > amountCheck)
            //{

            //    Console.WriteLine("your current balance is not enough.");
            //}
            //else
            //{


                string withdrawAction = $"update dbo.Accounts set currentBalance=currentBalance-{amount} where Accounts.Cust_Account={ChoosAccNum}";
                string updateTransaction = $"insert dbo.User_Transaction (TransId,Trans_Timestamp,trans_type,rans_amount,SrcAccNO,Acc_Number) values({transactionID},'{date}','{transactionType}',{amount},{ChoosAccNum},{ChoosAccNum});";
                SqlCommand commandWithdrawAction = new SqlCommand(withdrawAction, sqlConnection);
                SqlCommand commandUpdateTransaction = new SqlCommand(updateTransaction, sqlConnection);
                commandWithdrawAction.ExecuteNonQuery();
                commandUpdateTransaction.ExecuteNonQuery();
                commandWithdrawAction.Dispose();
                commandUpdateTransaction.Dispose();
           // }






        }

        private void transfer()
        {
            checkAcount();


            Console.WriteLine();
            Console.WriteLine();
            Console.Write("which account want to tranfer from: ");
            long srcAcc=long.Parse(Console.ReadLine());
            Console.WriteLine();
            Console.Write("Enter account want to tranfer to: ");
            long targetcAcc = long.Parse(Console.ReadLine());
            Console.WriteLine();
            Console.WriteLine("Enter amount to transfer");
            Console.WriteLine();
            double amount = double.Parse(Console.ReadLine());
            DateTime date = DateTime.Now; // will give the date for today
            Random random = new Random();
            int transactionIDsrc = random.Next();
            int transactionIDtrg = random.Next();
            string transactionTypeSrc = "Transfer";
            string transactionTypeTarget = "Deposite";

            /////////////////////////////////////////////////open connection with SQL////////////////////////////////////////////////
            //which database on which server to connect
            string connectionString = "Data Source=(local);Initial Catalog=BANK_SYSTEM; Integrated Security=true";

            // create new object of the main class which will connect us to database 
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

            string srcTransfer = $"update accounts set currentBalance=currentBalance-{amount} where Cust_Account={srcAcc}";
            string targetTransfer = $"update accounts set currentBalance=currentBalance+{amount} where Cust_Account={targetcAcc}";
            string updateSrcAcc = $"insert User_Transaction (TransId,Trans_Timestamp,trans_type,rans_amount,TargetAccNO,Acc_Number) values ({transactionIDsrc},'{date}','{transactionTypeSrc}',{amount},{targetcAcc},{srcAcc})";
            string updateTrg = $"insert User_Transaction (TransId,Trans_Timestamp,trans_type,rans_amount,SrcAccNO,Acc_Number) values ({transactionIDtrg},'{date}','{transactionTypeTarget}',{amount},{srcAcc},{targetcAcc})";

            SqlCommand command1=new SqlCommand(srcTransfer, sqlConnection);
            SqlCommand command2=new SqlCommand(targetTransfer, sqlConnection);
            SqlCommand command3=new SqlCommand(updateSrcAcc, sqlConnection);
            SqlCommand command4=new SqlCommand(updateTrg, sqlConnection);
            command1.ExecuteNonQuery();
            command2.ExecuteNonQuery();
            command3.ExecuteNonQuery();
            command4.ExecuteNonQuery();

        }

        private void historyTransaction()
        {
            checkAcount();
            Console.WriteLine();
            Console.Write("which account would you like to find out the history: ");
            long acc=long.Parse(Console.ReadLine());
            Console.Clear();
            Console.WriteLine();
            Console.WriteLine("    ==================History Bage For Account: " + acc+"===================");
            Console.WriteLine();
            Console.WriteLine("Please Choose History limite as Below:");
            Console.WriteLine();
            Console.WriteLine("1-Last three Tansaction.");
            Console.WriteLine("2-Last Month.");
            Console.WriteLine("3-specific date.");
            Console.WriteLine();
            Console.Write("History in: ");
            int history=int.Parse(Console.ReadLine());
            Console.WriteLine();
            /////////////////////////////////////////////////open connection with SQL////////////////////////////////////////////////
            //which database on which server to connect
            string connectionString = "Data Source=(local);Initial Catalog=BANK_SYSTEM; Integrated Security=true";

            // create new object of the main class which will connect us to database 
            SqlConnection sqlConnection = new SqlConnection(connectionString);
            sqlConnection.Open();
            /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
            switch (history)
            {
                case 1:
                    string getHistory1 = $"select top (3) * from User_Transaction where Acc_Number={acc} order by Trans_Timestamp desc";
                    SqlCommand command1=new SqlCommand(getHistory1, sqlConnection);
                    SqlDataReader reader1 = command1.ExecuteReader();
                   if (reader1.HasRows)
                    {
                        while (reader1.Read())
                        {
                            Console.WriteLine("transaction ID: " + reader1["TransId"] + "|| transaction Date: " + reader1["Trans_Timestamp"] + "\n transaction type: " + reader1["trans_type"] + "|| transaction amount: " + reader1["rans_amount"] + "|| tranaction SrcAcc: " + reader1["SrcAccNO"] + "|| transaction TragetAcc: " + reader1["TargetAccNO"]);
                            Console.WriteLine();
                        }

                    }
                    else
                    {
                        Console.WriteLine("Sorry there is no history details for your Account: " + acc);
                    }
                    break;
               case 2:
                    string getHistory2 = $"select * from User_Transaction where User_Transaction.Acc_Number={acc} and Trans_Timestamp < DATEADD(MONTH, -1, GETDATE());";
                    SqlCommand command2 = new SqlCommand(getHistory2, sqlConnection);
                    SqlDataReader reader2 = command2.ExecuteReader();
                    if (reader2.HasRows)
                    {
                        while (reader2.Read())
                        {
                            Console.WriteLine("transaction ID: " + reader2["TransId"] + "|| transaction Date: " + reader2["Trans_Timestamp"] + "\n transaction type: " + reader2["trans_type"] + "|| transaction amount: " + reader2["rans_amount"] + "|| tranaction SrcAcc: " + reader2["SrcAccNO"] + "|| transaction TragetAcc: " + reader2["TargetAccNO"]);
                            Console.WriteLine();
                        }

                    }
                    else
                    {
                        Console.WriteLine("Sorry there is no history details last month for your Account: " + acc);
                    }
                    break;
                case 3:
                    break;

                default: 
                    Console.WriteLine("wrong choose, please choose again");
                    Thread.Sleep(1000);
                    Console.Clear();
                    historyTransaction();
                    break;
            }

        }




    } 
}
