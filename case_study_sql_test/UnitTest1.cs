using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Npgsql;
using case_study_sql_core;

namespace case_study_sql_test
{
    //[TestClass]
    //public class UnitTest1
    //{
    //    [TestMethod]
    //    public void TestCallFizzbuzzCalc()
    //    {

    //        ////INSERT values to test_table.
    //        ////TODO improve how to insert values.
    //        //cmd_str = @"INSERT INTO test_data(first_name, last_name, expected) VALUES
    //        //  ('Sayaka', 'Ishihara', TRUE),
    //        //  ('彩歌', '石原', FALSE),
    //        //  ('aaaaaaaaaaaaaaaaaaaaa', 'aaaaaaaaaaaaaaaaaaaaa', FALSE),
    //        //  ('1', 'aaa', FALSE),
    //        //  ('aaa', '#', FALSE),
    //        //  ('Radboud', 'Visser', TRUE),
    //        //  ('Jeffrey', 'Bijkerk', TRUE),
    //        //  ('abn', 'amro', TRUE)
    //        //  ; ";
    //        //cmd = new NpgsqlCommand(cmd_str, conn);
    //        //cmd.ExecuteNonQuery();
    //    }
    //}

    [TestClass]
    public class UnitTestCallFizzbuzzCalc
    {
        const string mockFirstName = "First_name";
        const string mockLastName = "Last_name";
        const string tableName = "tested_fizzbuzz_like_table";

        [TestMethod]
        public void TestCallFizzbuzzCalc()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["caseStudyDB"].ConnectionString;

            using (NpgsqlConnection conn = new NpgsqlConnection(connectionString))
            {
                conn.Open();

                //2. Calculate the fizzbuzz_like logic using stored procedure and output the results to the table.

                //Create table using stored procedure.

                var cmdStr = "CALL create_table('" + tableName + "')";
                using (var cmd = new NpgsqlCommand(cmdStr, conn))
                    cmd.ExecuteNonQuery();

                //Enter 1 to 100 to the table and calculate using stored procedure.

                //Make a table using the function.
                Program.CallFizzbuzzCalc(tableName, mockFirstName, mockLastName, conn);

                ////Check the table is exactly the same as this expected table.
                //cmdStr = "select * from " + tableName + " order by num";
                //using (var cmd = new NpgsqlCommand(cmdStr, conn))
                //using (var reader = cmd.ExecuteReader())
                //{
                //    while (reader.Read())
                //    {
                //        //Console.Write(reader.GetInt32(0));
                //        //Console.Write(" ");
                //        Console.WriteLine(reader.GetString(1));
                //    }
                //}

                //Assert.IsTrue(CallTestStoredFunction(mockFirstName, mockLastName);


            }
        }
    }
}
