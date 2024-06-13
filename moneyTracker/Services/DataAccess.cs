using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Data.Sqlite;
using moneyTracker.Core.Models;
using Windows.Storage;

namespace moneyTracker.Services
{
    public static class DataAccess
    {
        private static string dbFileName = "moneyTrackerDB.db";
        public async static void InitializeDatabase()
        {
            await ApplicationData.Current.LocalFolder.CreateFileAsync(dbFileName, CreationCollisionOption.OpenIfExists);
            string dbpath = Path.Combine(ApplicationData.Current.LocalFolder.Path, dbFileName);
            
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                String columnAddon = "NVARCHAR(2048) NULL";
                String columns = "Date" + columnAddon;// + ", Description"+columnAddon+", Withdrawals"+columnAddon+", Deposits"+columnAddon+", Category"+columnAddon+", Balance"+columnAddon;


                //Todo: change from strings types to more accurate types. this is important for querying.
                String tableCommand = "CREATE TABLE IF NOT EXISTS Transactions " +
                    "(Primary_Key INTEGER PRIMARY KEY, " +
                    "Date NVARCHAR(2048) NULL, " +
                    "Description NVARCHAR(2048) NULL, " +
                    "Amount NVARCHAR(2048) NULL, " +
                    "Category NVARCHAR(2048) NULL)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db);

                createTable.ExecuteReader();
            }
        }

        //Todo: move transactions class somewhere seperate (models??). also in datagridpage

        //Todo: next step bulk insert sql statement.
        public static void AddData(FinancialTransaction transaction)
        {
            string dbpath =  Path.Combine(ApplicationData.Current.LocalFolder.Path,dbFileName);
            using (SqliteConnection db =
              new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                SqliteCommand insertCommand =  new SqliteCommand();
                insertCommand.Connection = db;




                // Use parameterized query to prevent SQL injection attacks
                insertCommand.CommandText = "INSERT INTO Transactions VALUES (NULL, @Date, @Description, @Amount, @Category);";
                insertCommand.Parameters.AddWithValue("@Date", transaction.Date);
                insertCommand.Parameters.AddWithValue("@Description", transaction.Description);
                insertCommand.Parameters.AddWithValue("@Amount", transaction.Amount);
                insertCommand.Parameters.AddWithValue("@Category", transaction.Category);


                insertCommand.ExecuteReader();
            }
        }

        public async static Task<List<FinancialTransaction>> GetData()
        {
            List<FinancialTransaction> entries = new List<FinancialTransaction>();

            string dbpath =  Path.Combine(ApplicationData.Current.LocalFolder.Path, dbFileName);
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT Date, Description, Amount, Category from Transactions", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(new FinancialTransaction(query.GetString(0), query.GetString(1), query.GetString(2), query.GetString(3)));
                }
            }
            await Task.CompletedTask;
            return entries;
        }
    }
}
