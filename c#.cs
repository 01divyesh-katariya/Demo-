using System;
using System.Data;
using System.IO;


class BankSystem
{
    static DataTable accountsTable = new DataTable();

    static void Main() 
    {
        accountsTable.Columns.Add("AccountID", typeof(int));
        accountsTable.Columns.Add("AccountHolder", typeof(string));
        accountsTable.Columns.Add("Balance", typeof(decimal));
        int nextAccountId = 1;

        while (true)
        {
            Console.WriteLine("\n1. Create Account\n2. Deposit Money\n3. Withdraw Money\n4. Close Account\n5. Exit Bank");
            Console.Write("Choose an option: ");
            string? choice = Console.ReadLine(); 

            switch (choice)
            {
                case "1": CreateAccount(ref nextAccountId); break;
                case "2": DepositMoney(); break;
                case "3": WithdrawMoney(); break;
                case "4": CloseAccount(); break;
                case "5": CloseBank(); return;
                default: Console.WriteLine("Invalid choice."); break;
            }
        }
    }

    static void CreateAccount(ref int nextAccountId)
    {
        Console.Write("Enter Account Holder Name: ");
        string? name = Console.ReadLine();
        accountsTable.Rows.Add(nextAccountId, name ?? "Unknown", 0m); 
        Console.WriteLine($"Account created: ID {nextAccountId}, Holder {name}");
        nextAccountId++;
    }

    static void DepositMoney()
    {
        try
        {
            Console.Write("Enter Account ID: ");
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int accountId)) 
            {
                DataRow[] rows = accountsTable.Select($"AccountID = {accountId}");

                if (rows.Length > 0)
                {
                    Console.Write("Enter amount to deposit: ");
                    input = Console.ReadLine();
                    if (decimal.TryParse(input, out decimal amount) && amount > 0)
                    {
                        rows[0]["Balance"] = (decimal)rows[0]["Balance"] + amount;
                        Console.WriteLine($"Deposited {amount:C} into Account ID {accountId}. New Balance: {rows[0]["Balance"]:C}");
                    }
                    else
                    {
                        Console.WriteLine("Invalid deposit amount.");
                    }
                }
                else
                {
                    Console.WriteLine("Account not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Account ID.");
            }
        }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
    }
     static void WithdrawMoney()
    {
        try
        {
            Console.Write("Enter Account ID: ");
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int accountId))
            {
                DataRow[] rows = accountsTable.Select($"AccountID = {accountId}");

                if (rows.Length > 0)
                {
                    Console.Write("Enter amount to withdraw: ");
                    input = Console.ReadLine();
                    if (decimal.TryParse(input, out decimal amount) && amount > 0)
                    {
                        if ((decimal)rows[0]["Balance"] >= amount)
                        {
                            rows[0]["Balance"] = (decimal)rows[0]["Balance"] - amount;
                            Console.WriteLine($"Withdrew {amount:C} from Account ID {accountId}. New Balance: {rows[0]["Balance"]:C}");
                        }
                        else
                        {
                            Console.WriteLine("Insufficient funds.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid withdrawal amount.");
                    }
                }
                else
                {
                    Console.WriteLine("Account not found.");
                }
            }
            else
            {
                Console.WriteLine("Invalid Account ID.");
            }
        }
        catch (Exception ex) { Console.WriteLine($"Error: {ex.Message}"); }
    }

    static void CloseAccount()
    {
        Console.Write("Enter Account ID to close: ");
        string? input = Console.ReadLine();
        if (int.TryParse(input, out int accountId))
        {
            DataRow[] rows = accountsTable.Select($"AccountID = {accountId}");
            if (rows.Length > 0)
            {
                accountsTable.Rows.Remove(rows[0]);
                Console.WriteLine($"Account ID {accountId} closed successfully.");
            }
            else
            {
                Console.WriteLine("Account not found.");
            }
        }
        else
        {
            Console.WriteLine("Invalid Account ID.");
        }
    }

    static void CloseBank()
    {
        using (StreamWriter writer = new StreamWriter("AccountDetails.txt"))
        {
            foreach (DataRow row in accountsTable.Rows)
            {
                writer.WriteLine($"AccountID: {row["AccountID"]}, AccountHolder: {row["AccountHolder"]}, Balance: {row["Balance"]:C}");
            }
        }
        Console.WriteLine("Account details saved to AccountDetails.txt.");
    }
}

    

