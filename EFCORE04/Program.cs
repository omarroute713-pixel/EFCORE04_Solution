using EFCORE04.Context;
using EFCORE04.Enums;
using EFCORE04.Enums.NonEntityEnums;
using EFCORE04.Models;
using Microsoft.EntityFrameworkCore;
using System.Net;

namespace EFCORE04
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using BankDbContext db = new BankDbContext();

            db.Database.Migrate();


            while (true)
            {

                Console.Clear();
                ShowMenu();

                int choice = ReadInt("Enter choice: ");

                Console.Clear();

                switch (choice)
                {
                    case 1:
                        AddCustomer(db);
                        Pause();
                        break;

                        case 2:
                        OpenAccount(db);
                        Pause();
                        break;

                    case 3:
                        UpdateAccountStatus(db);
                        Pause();
                        break;

                    case 4:
                        RemoveAccountFromCustomer(db);
                        Pause();
                        break;

                    case 5:
                        ListCustomers(db);
                        Pause();
                        break;



                    case 0:
                        Console.WriteLine("Connection closed. Goodbye.");
                        return;

                    default:
                        Console.WriteLine("Invalid choice.");
                        Pause();
                        break;
                }


                


            }
              
        }






        #region Helper Methodes

        static void ShowMenu()
        {
            Console.WriteLine("========================================");
            Console.WriteLine("        National Bank - Management       ");
            Console.WriteLine("========================================");
            Console.WriteLine("1) Add a new Customer");
            Console.WriteLine("2) Open a new Account for a Customer");
            Console.WriteLine("3) Update Account Status (Active / Closed)");
            Console.WriteLine("4) Remove an Account from a Customer");
            Console.WriteLine("5) List all Customers (with accounts)");
            Console.WriteLine("0) Exit");
            Console.WriteLine("----------------------------------------");
        }
        static int ReadInt(string message)
        {
            int value;

            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();

                if (int.TryParse(input, out value))
                {
                    return value;
                }

                Console.WriteLine("Invalid number. Please try again.");
            }
        }

        static void Pause()
        {
            Console.WriteLine();
            Console.WriteLine("Press any key to return to the menu...");
            Console.ReadKey();
        }

        static string ReadString(string message)
        {
            string? input;

            do
            {
                Console.Write(message);
                input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input cannot be empty.");
                }

            } while (string.IsNullOrWhiteSpace(input));

            return input.Trim();
        }


        static DateTime ReadDate(string message)
        {
            DateTime value;

            while (true)
            {
                Console.Write(message);
                string? input = Console.ReadLine();

                if (DateTime.TryParse(input, out value))
                {
                    return value;
                }

                Console.WriteLine("Invalid date. Please use yyyy-MM-dd format.");
            }
        }


        #endregion

        #region Add Customer Methode
        static void AddCustomer(BankDbContext db)
        {
            Console.WriteLine("--- Add New Customer ---");

            string fullName = ReadString("Full Name: ");
            string nationalId = ReadString("National ID: ");
            DateTime dob = ReadDate("Date of Birth (yyyy-MM-dd): ");
            string email = ReadString("Email: ");
            string phone = ReadString("Phone: ");
            string address = ReadString("Address: ");

            Console.WriteLine("Customer Type:");
            Console.WriteLine("1) Individual");
            Console.WriteLine("2) Business");

            int typeChoice = ReadInt("Choice: ");

            if (typeChoice != 1 && typeChoice != 2)
            {
                Console.WriteLine("Invalid customer type.");
                return;
            }

            bool nationalIdExists = db.Customers.Any(c => c.NationalId == nationalId);
            if (nationalIdExists)
            {
                Console.WriteLine("A customer with this National ID already exists.");
                return;
            }

            bool emailExists = db.Customers.Any(c => c.Email == email);
            if (emailExists)
            {
                Console.WriteLine("A customer with this Email already exists.");
                return;
            }

            Customer customer = new Customer
            {
                Address = address,
                DateOfBirth = dob,
                FullName = fullName,
                Email = email,
                PhoneNumber = phone,
                NationalId = nationalId,
                CustomerType = (CustomerType) typeChoice,
            };

            db.Customers.Add(customer);
            db.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"Customer created successfully. CustomerId = {customer.Id}");
            Console.ResetColor();

        }
        #endregion


        #region Open a new Account for a Customer
        static void OpenAccount(BankDbContext db)
        {
            var Tran = db.Database.BeginTransaction();
            try
            {
                Console.WriteLine("--- Open New Account ---");

                int accountNumber = ReadInt("Account Number: ");

                Console.WriteLine("Account Type:");
                Console.WriteLine("1) Savings");
                Console.WriteLine("2) Current");
                Console.WriteLine("3) Business");

                int accountTypeChoice = ReadInt("Choice: ");

                if (accountTypeChoice < 1 || accountTypeChoice > 3)
                {
                    Console.WriteLine("Invalid account type.");
                    return;
                }

                string branchCode = ReadString("Branch Code: ");
                int customerId = ReadInt("Customer Id: ");

                Console.WriteLine("Ownership Role:");
                Console.WriteLine("1) Primary");
                Console.WriteLine("2) CoHolder");

                int ownershipChoice = ReadInt("Choice: ");

                if (ownershipChoice != 1 && ownershipChoice != 2)
                {
                    Console.WriteLine("Invalid ownership role.");
                    return;
                }
            Console.ForegroundColor = ConsoleColor.Cyan;

            Console.WriteLine($"Validating branch '{branchCode}' and customer #{customerId}...");
            Console.ResetColor();
                Branch? branch = db.Branchs.FirstOrDefault(b => b.Code == branchCode);
                if (branch == null)
                {
                    Console.WriteLine("Branch does not exist.");
                    return;
                }

                Customer? customer = db.Customers.Find(customerId);
                if (customer == null)
                {
                    Console.WriteLine("Customer does not exist.");
                    return;
                }

                bool accountExists = db.Accounts.Any(a => a.AccountNumber == accountNumber);
                //Account Exists So Add Customer To It
                if (accountExists)
                {
                    //Check That CusomerId Is Not Duplicated For Same AccountId
                    var AllcustomerAccountStatus = db.CustomerAccounts
                        .Where(ca => ca.CustomerId == customerId && ca.AccountId == accountNumber);
                    if (AllcustomerAccountStatus.Count() > 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("This Customer Has Linked For That Account Before");
                        Console.ResetColor();
                        return;
                    }


                    #region Different Status For Each Customer For Same Account
                    CustomerAccount customerAccount = new CustomerAccount
                    {
                        CustomerId = customerId,
                        AccountId = accountNumber,
                        OwnershipType = (OwnershipType)ownershipChoice,
                        OwnershipStartDate = DateTime.Now,
                        AccountStatus = AccountStatus.Active
                    };

                    db.CustomerAccounts.Add(customerAccount);
                    db.SaveChanges();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Account '{accountNumber}' " +
                            $"created and linked to customer {customerId} " +
                            $"as {(OwnershipType)ownershipChoice} owner."
                        );
                    Console.ResetColor();
                    #endregion

                    #region Same Status For Each Customer For Same Account

                    //Get Status Of Existing Link Status
                    //var ExistscustomerAccountStatus = db.CustomerAccounts
                    //    .Where(ca => ca.AccountId == accountNumber);
                    //if (ExistscustomerAccountStatus is null)
                    //    return;

                    //CustomerAccount customerAccount = new CustomerAccount
                    //{
                    //    CustomerId = customerId,
                    //    AccountId = accountNumber,
                    //    OwnershipType = (OwnershipType)ExistscustomerAccountStatus.First().OwnershipType,
                    //    OwnershipStartDate = DateTime.Now,
                    //    AccountStatus = ExistscustomerAccountStatus.First().AccountStatus
                    //};

                    //db.CustomerAccounts.Add(customerAccount);
                    //db.SaveChanges();

                    //Console.ForegroundColor = ConsoleColor.Green;
                    //Console.WriteLine($"Account '{accountNumber}' " +
                    //        $"created and linked to customer {customerId} " +
                    //        $"as {(OwnershipType)ExistscustomerAccountStatus.First().OwnershipType} owner."
                    //    );
                    //Console.ResetColor();
                    #endregion

                } // Not Exists
                else
                {

                    Account account = new Account
                    {
                        AccountNumber = accountNumber,
                        AccountType = (AccountType)accountTypeChoice,
                        OpeningDate = DateTime.Now,
                        CurrentBalance = 0,
                        Code = branchCode
                    };
                    db.Accounts.Add(account);
                    db.SaveChanges();

                    CustomerAccount customerAccount = new CustomerAccount
                    {
                        CustomerId = customerId,
                        AccountId = accountNumber,
                        OwnershipType = (OwnershipType)ownershipChoice,
                        OwnershipStartDate = DateTime.Now,
                        AccountStatus = AccountStatus.Active
                    };

                    db.CustomerAccounts.Add(customerAccount);
                    db.SaveChanges();

                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Account '{accountNumber}' " +
                            $"created and linked to customer {customerId} " +
                            $"as {(OwnershipType)ownershipChoice} owner."
                        );
                    Console.ResetColor();

                }

                

                Tran.Commit();

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Tran.Rollback();

            }


        }
        #endregion


        #region Update Account Status
        static void UpdateAccountStatus(BankDbContext db)
        {
            Console.WriteLine("--- Update Account Status ---");

            int accountNumber = ReadInt("Account Number: ");

            int customerId = ReadInt("Customer Id: ");

            #region One Customer Afect All Joint Accounts For All Customers
            var customerAccountAffectll = db.CustomerAccounts
               .Where(ca => ca.AccountId == accountNumber);

            if (customerAccountAffectll.Count() == 0)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Account Number Not Found");
                Console.ResetColor();
                return;
            }
           
            foreach (var account in customerAccountAffectll)
            {
                account.AccountStatus = account.AccountStatus == AccountStatus.Active ?
                AccountStatus.Closed : AccountStatus.Active;
            }
            db.SaveChanges();

            Console.ForegroundColor = ConsoleColor.Green;

            Console.WriteLine($"Status updated For All Customers.");
            Console.ResetColor();

            #endregion

            #region Each Customer Has Its Own Status For The Same Account

            //CustomerAccount? customerAccount = db.CustomerAccounts
            //   .FirstOrDefault(ca => ca.AccountId == accountNumber && ca.CustomerId == customerId);

            //if (customerAccount is null)
            //{
            //    Console.WriteLine("No ownership link found for this customer and account.");
            //    return;
            //}

            //customerAccount.AccountStatus = customerAccount.AccountStatus == AccountStatus.Active ?
            //    AccountStatus.Closed : AccountStatus.Active;

            //db.SaveChanges();

            //Console.ForegroundColor = ConsoleColor.Green;

            //Console.WriteLine($"Status updated to {customerAccount.AccountStatus}.");
            //Console.ResetColor();

            #endregion


        }
        #endregion

        #region  Remove an Account From a Customer -- Need More Review 
        static void RemoveAccountFromCustomer(BankDbContext db)
        {
            Console.WriteLine("--- Remove Account From Customer ---");
            int accountNumber = ReadInt("Account Number: ");

            int customerId = ReadInt("Customer Id: ");

            CustomerAccount? customerAccount = db.CustomerAccounts.Include(ca => ca.Account)
               .FirstOrDefault(ca => ca.AccountId == accountNumber && ca.CustomerId == customerId);

            if (customerAccount is null)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("No ownership link found for this customer and account.");
                Console.ResetColor();
                return;
            }
            Account account = customerAccount.Account;

            db.CustomerAccounts.Remove(customerAccount);
            db.SaveChanges();

            bool hasOtherOwners = db.CustomerAccounts.Any(ca => ca.AccountId == account.AccountNumber);
            if (!hasOtherOwners)
            {
                db.Accounts.Remove(account);
                db.SaveChanges();
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Ownership link deleted.");
                Console.WriteLine($"That was the last owner — account '{accountNumber}' was also removed.");
                Console.ResetColor();

            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Ownership link deleted.");
                Console.ResetColor();
            }




        }

        #endregion


        #region  List all Customers (with accounts)
        static void ListCustomers(BankDbContext db)
        {
            Console.WriteLine("--- All Customers ---");

            var customers = db.Customers
            .Include(c => c.CustomerAccounts)
                .ThenInclude(ca => ca.Account)
                    .ThenInclude(a => a.Branch)
            .ToList();

            foreach (var customer in customers)
            {
                Console.WriteLine($"#{customer.Id} {customer.FullName} ({customer.CustomerType})");

                if (!customer.CustomerAccounts.Any())
                {
                    Console.WriteLine("   (no accounts)");
                    continue;
                }

                foreach (var ca in customer.CustomerAccounts)
                {
                    Console.WriteLine(
                    $"   {ca.Account.AccountNumber}   {ca.Account.AccountType}   Balance: {ca.Account.CurrentBalance:C}   {ca.OwnershipType}   {ca.AccountStatus}   @ {ca.Account.Branch.Name}"
                    );
                }

            }
        }


        #endregion
    }

}
