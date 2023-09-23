namespace BANK_SYSTEM_WITH_DATABASE
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("              =======================Welcome To Bank Muscat=======================");

            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Please choose one option ");
            Console.WriteLine();
            Console.WriteLine("1-New Registration");
            Console.WriteLine("2_Login");
            Console.WriteLine("3-view current exchange rates");
            Console.WriteLine();
            Console.WriteLine();
            Console.Write("Service Number:");
            int choice=int.Parse(Console.ReadLine());
            switch (choice)
            {
                //New Registration
                case 1:
                    Registration registration = new Registration();
                    registration.NewUser();

                    //Console.WriteLine("New Registration");
                    break;


                //Login
                case 2:
                    Login login = new Login();                    
                    login.checkLogin();
                    Transaction transaction = new Transaction();
                    transaction.ControlTransaction();
                   
                   // Console.WriteLine("Login");
                    break;



                //view current exchange rates
                case 3:
                    Console.WriteLine("view current exchange rates");
                    break;

            }


        }
    }
}