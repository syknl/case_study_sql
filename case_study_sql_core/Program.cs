using System;
using System.Configuration;
using Npgsql;
using System.Text.RegularExpressions;

namespace case_study_sql_core
{
    public class Program
    {
        public static void Main()
        {
            const int nameLengthMax = 20;

            //1. Receive name from the console
            string firstName = GetInput("What is your first name?", nameLengthMax);
            string lastName = GetInput("What is your last name?", nameLengthMax);

            //Define variables for SQL.
            string cmdStr = null;

            //Connection variable
            var connectionString = ConfigurationManager.ConnectionStrings["caseStudyDB"].ConnectionString;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                //2. Calculate the fizzbuzz_like logic using stored procedure and output the results to the table.

                //Create table using stored procedure.
                var tableName = "fizzbuzz_like_table";
                cmdStr = "CALL create_table('" + tableName + "')";
                
                using (var cmd = new NpgsqlCommand(cmdStr, conn))
                    cmd.ExecuteNonQuery();

                //Enter 1 to 100 in the table and calculate using stored procedure.
                CallFizzbuzzCalc(tableName, firstName, lastName, conn);

                //3. Show the results.
                cmdStr = "select * from " + tableName + " order by num";
                using (var cmd = new NpgsqlCommand(cmdStr, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Console.WriteLine(reader.GetString(1));
                    }
                }
            }

        }

        public static string GetInput(string message, int lengthLimit)
        {
            Console.WriteLine(message);
            string name = Console.ReadLine();

            //Checking the length and character type, only alphabets are accepted.
            if (!IsLessThanLimit(name, lengthLimit) || !IsAlphabet(name))
            {
                Environment.Exit(0);
            }

            return name;
        }

        public static bool IsLessThanLimit(string name, int lengthLimit)
        {
            bool boolV = true;
            var len = name.Length;
            if (len > lengthLimit)
            {
                var str = "Please enter " + $"{lengthLimit}" + " or less number of characters.";
                Console.WriteLine(str);
                boolV = false;
            }
            return boolV;
        }

        public static bool IsAlphabet(string name)
        {
            bool boolV = true;
            if (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
            {
                Console.WriteLine("Please enter only alphabets.");
                boolV = false;
            }
            return boolV;
        }


        public static void CallFizzbuzzCalc(string tableName, string firstName, string lastName, NpgsqlConnection conn)
        {
            var cmdStr = "CALL fizzbuzz_calc('" + tableName + "', '" + firstName + "', '" + lastName + "')";
            using (var cmd = new NpgsqlCommand(cmdStr, conn))
                cmd.ExecuteNonQuery();
        }

    }
}

