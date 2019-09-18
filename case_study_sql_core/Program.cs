﻿using System;
using Npgsql;
using System.Text.RegularExpressions;
using System.Transactions;
using System.Data;


namespace case_study_sql_core
{
    public class Program
    {
        //static void Main(string[] args)
        //{
        //    //SQL procedure is called
        //    Console.WriteLine("Hello World!");
        //}
        static void Main(String[] args)
        {

            // 1. Receive name from the console
            // Limit of name length.
            const int length_limit = 20;

            string first_name = Get_input("What is your first name?", length_limit);
            string last_name = Get_input("What is your last name?", length_limit);

            // 2. Calculate the fizzbuzz_like logic using stored procedure.



            //SQL処理で用いる変数を予め宣言
            NpgsqlCommand cmd = null;
            string cmd_str = null;
            DataTable dt = null;
            NpgsqlDataAdapter da = null;
            NpgsqlCommand reader = null;

            //接続文字列
            string conn_str = "Server=localhost;Port=5432;User ID=sayaka;Database=postgres;Password=postgres;Enlist=true";


            //TransactionScopeの利用
            //using (TransactionScope ts = new TransactionScope())
            //{
                //接続その1
                using (NpgsqlConnection conn = new NpgsqlConnection(conn_str))
                {
                    //PostgreSQLへ接続後、INSERT、DELETE処理を実施し、SELECT結果を取得
                    conn.Open();


                    //2. ストアドプロシージャで1から100まで入れる
                    //Create Table;
                    var table_name = "fizzbuzz_like_table";

                    cmd_str = "CALL create_table('" + table_name + "')";
                    Console.WriteLine(cmd_str); //DEBUG

                //using (var cmd = new NpgsqlCommand(cmd_str, conn))
                //using (var reader = cmd.ExecuteReader())
                //{
                //    while (reader.Read())
                //        Console.WriteLine(reader.GetString(0));
                //}



                    cmd = new NpgsqlCommand(cmd_str, conn);
                    cmd.ExecuteNonQuery();

                    //Calculate
                    cmd_str = "CALL fizzbuzz_calc('" + table_name + "', '" + first_name +"', '" + last_name + "')";
                    cmd = new NpgsqlCommand(cmd_str, conn);
                    cmd.ExecuteNonQuery();



                //3. アウトプットもストアドプロシージャで。すなわちselect文で順番に並べる。
                    cmd_str = "select * from " + table_name + " order by num";
                    cmd = new NpgsqlCommand(cmd_str, conn);
                    reader = cmd.ExecuteReader();


                ////Delete test_table if exists.
                //cmd_str = "DROP TABLE IF EXISTS test_data";
                //cmd = new NpgsqlCommand(cmd_str, conn);
                //cmd.ExecuteNonQuery();

                ////Create test_table;
                //cmd_str = @"CREATE TABLE test_data(
                //  id SERIAL,
                //  first_name text NOT NULL,
                //  last_name text NOT NULL,
                //  expected boolean
                //);
                //";
                //cmd = new NpgsqlCommand(cmd_str, conn);
                //cmd.ExecuteNonQuery();

                ////INSERT values to test_table.
                ////TODO improve how to insert values.
                //cmd_str = @"INSERT INTO test_data(first_name, last_name, expected) VALUES
                //  ('Sayaka', 'Ishihara', TRUE),
                //  ('彩歌', '石原', FALSE),
                //  ('aaaaaaaaaaaaaaaaaaaaa', 'aaaaaaaaaaaaaaaaaaaaa', FALSE),
                //  ('1', 'aaa', FALSE),
                //  ('aaa', '#', FALSE),
                //  ('Radboud', 'Visser', TRUE),
                //  ('Jeffrey', 'Bijkerk', TRUE),
                //  ('abn', 'amro', TRUE)
                //  ; ";
                //cmd = new NpgsqlCommand(cmd_str, conn);
                //cmd.ExecuteNonQuery();


                //////test対象のテーブルをリセット(全データDELETE)
                ////cmd_str = "DELETE FROM test";
                ////cmd = new NpgsqlCommand(cmd_str, conn);
                ////cmd.ExecuteNonQuery();

                //////INSERT処理
                ////cmd_str = "INSERT INTO test VALUES(1, 'AAA'), (2, 'BBB'), (3, 'CCC')";
                ////cmd = new NpgsqlCommand(cmd_str, conn);
                ////cmd.ExecuteNonQuery();

                //////DELETE処理
                ////cmd_str = "DELETE FROM test WHERE col1 % 2 = 0";
                ////cmd = new NpgsqlCommand(cmd_str, conn);
                ////cmd.ExecuteNonQuery();

                ////SELECT処理
                //dt = new DataTable();
                //cmd_str = "SELECT * FROM test_data";
                //cmd = new NpgsqlCommand(cmd_str, conn);
                //da = new NpgsqlDataAdapter(cmd);
                //da.Fill(dt);

                ////SELECT結果表示
                //for (int i = 0; i < dt.Rows.Count; i++)
                //{
                //    Console.WriteLine("(first_name, last_name) = (" + dt.Rows[i][1] + ", " + dt.Rows[i][2] + ")");
                //}

            }
            //トランザクション完了
            //    ts.Complete();
            //}
            //using (NpgsqlConnection conn = new NpgsqlConnection(conn_str))
            //{
            //    //PostgreSQLへ接続
            //    conn.Open();
            //    conn.GetSchema();
            //    Console.WriteLine("Connection success!");

            //    ////SELECT処理
            //    //dt = new System.Data.DataTable();
            //    //cmd_str = "SELECT * FROM test";
            //    //cmd = new NpgsqlCommand(cmd_str, conn);
            //    //da = new NpgsqlDataAdapter(cmd);
            //    //da.Fill(dt);

            //    ////SELECT結果表示
            //    //for (int i = 0; i < dt.Rows.Count; i++)
            //    //{
            //    //    Console.WriteLine("(col1, col2) = (" + dt.Rows[i][0] + ", " + dt.Rows[i][1] + ")");
            //    //}

            //}



            //using (var cmd = new NpgsqlCommand("INSERT INTO test_data first_name VALUES (@p)", conn))
            //{
            //    cmd.Parameters.AddWithValue("conn_test", "some_value");
            //    cmd.ExecuteNonQuery();
            //}
        }

        public static string Get_input(string message, int length_limit)
        {
            Console.WriteLine(message);
            string name = Console.ReadLine();

            //Checking length of the names and exiting if too long.
            int len = name.Length;
            if (len > length_limit)
            {
                string str = "Please enter " + $"{length_limit}" + " or less number of characters.";
                Console.WriteLine(str);
                Environment.Exit(0);

            }

            if (!Regex.IsMatch(name, @"^[a-zA-Z]+$"))
            {
                Console.WriteLine("Please enter only alphabets.");
                Environment.Exit(0);
            }

            return name;
        }

    }
}

