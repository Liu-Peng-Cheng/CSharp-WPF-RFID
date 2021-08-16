using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PublicResource.SQLite3;
namespace CK_C001TestDemo.SQLite3
{
    public class SQLite3DataBase
    {
        string dbFile = "Display";
        public SQLiteHelper UserSQLite;
        public SQLiteHelper PasswordSQLite;
        //public SQLiteHelper OperateSQLite;

        public  SQLite3DataBase()
        {
            UserSQLite = new SQLiteHelper(dbFile, "UserMsg");
            PasswordSQLite = new SQLiteHelper(dbFile, "Password");
        }

        public void CreatNewTable()
        {
            string dbColumnUser = "UserName TEXT,HFNum TEXT,DigitaliVenaID TEXT,IsAdmin INTEGER,PRIMARY KEY('HFNum','DigitaliVenaID')";
            UserSQLite.NewDbTable(dbColumnUser);
            string dbColumnPassword = "Password TEXT,PRIMARY KEY('Password')";
            PasswordSQLite.NewDbTable(dbColumnPassword);
            //try {
            //    UserSQLite.InsertData("UserName", "'TestUser'");
            //}  catch { }
        }
    }
}
