using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Data.SqlTypes;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace PublicResource.SQLite3
{
    public class SQLiteHelper
    {
        SQLiteConnection dbConObj;
        string dbConStr;
        string dbPath;
        string dbTable;
        object obj;
        //事务
        SQLiteTransaction tsc;//事务对象
        bool _IsRunTrans;//事务运行标志
        bool _AutoCmmit;//事务在自动提交标志


        public SQLiteHelper(string dbFileName, string TableName)
        {
            obj = new object();
            this.dbConStr = "Data Source=" + AppDomain.CurrentDomain.BaseDirectory + dbFileName;
            this.dbPath = AppDomain.CurrentDomain.BaseDirectory + dbFileName;
            this.dbTable = TableName;
            if (!File.Exists(this.dbPath))
            {
                SQLiteConnection.CreateFile(this.dbPath);
            }
        }

        /// <summary>
        /// 创建新表       列名栗子：Num INTEGER PRIMARY KEY, UserName varchar NOT NULL,UserID varchar,PRIMARY KEY('Num','UserName')
        /// </summary>
        /// <param name="tableName">创建表的名称</param>
        /// <param name="columnName">创建表的列名 </param>
        public void NewDbTable( string columnName)
        {
            /*
             * 储存的数据类型有以下5种：
                存储类                	描述
                NULL	            一个NULL值
                INTERGER	    带符号的整数，根据值的大小，自动存储为1,2,3,4,5,8字节6种
                REAL	            浮点数，存储为IEEE 8byte浮点数
                TEXT	                文本字符串，缺省的编码为utf-8
                BLOB	            blob数据，不定长 (数组)
             */
            lock (obj)
            {
                if (!OpenDb())
                    return;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = dbConObj;
                    cmd.CommandText = "CREATE TABLE  IF NOT EXISTS " + this.dbTable + " (" + columnName + ");";
                    cmd.ExecuteNonQuery();
                    this.CloseDb();
                }
               System.Threading.Monitor.Pulse(obj);
            }
        }
  
        /// <summary>
        /// 增
        /// </summary>
        /// <param name="columns">栗子：id, name, age, address, data</param>
        /// <param name="value">栗子：'1000','张三','20', '中国-广东深圳坂田'</param>
        public int InsertData(string columns,string value)
        {
            lock (obj)
            {
                try
                {
                    int ret = -1;
                    string sql;
                    if (!OpenDb())
                        return -2;
                    using (SQLiteCommand cmd = new SQLiteCommand())
                    {
                        cmd.Connection = dbConObj;
                        sql = "INSERT INTO " + this.dbTable + "(" + columns + ") VALUES (" + value + ");";
                        cmd.CommandText = sql;
                        //插入的   ret  行
                        ret = cmd.ExecuteNonQuery();
                        this.CloseDb();
                        System.Threading.Monitor.Pulse(obj);
                        return ret;
                    }
                }
                catch(Exception e) { throw new Exception(e.Message); }
            }
        }

        /// <summary>
        /// 替换  （主键值相同替换，否则插入）
        /// </summary>
        /// <param name="ColumnName">列名  栗子："id,age"</param>
        /// <param name="insertValue">替换的值  栗子："'5','Test'"</param>
        public int Replace(string ColumnName, object insertValue)
        {
            lock (obj)
            {
                int ret = -1;
                if (!OpenDb())
                    return -2;
                using (SQLiteCommand cmd = new SQLiteCommand()) {
                    cmd.Connection = dbConObj;
                    cmd.CommandText = "REPLACE INTO " + this.dbTable + "(" + ColumnName + ") VALUES(" + insertValue + ");";
                    ret = cmd.ExecuteNonQuery();
                    this.CloseDb();
                    System.Threading.Monitor.Pulse(obj);
                    return ret;
                }                
            }
        }

        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="ColumnName_NewValue">栗子："id=24,name='ergou'"</param>
        /// <param name="keyValue">匹配的主键的值：栗子:"id=12" --- "id>30" --- "id=12 OR age=30" </param>
        public int Update(string ColumnName_NewValue, string keyValue)
        {
            lock (obj)
            {
                int ret = -1;
                if (!OpenDb())
                    return -2;
                using (SQLiteCommand cmd = new SQLiteCommand()) {
                    cmd.Connection = dbConObj;
                    cmd.CommandText = "UPDATE " + this.dbTable + " SET " + ColumnName_NewValue + " WHERE " + keyValue;
                    ret = cmd.ExecuteNonQuery();
                    System.Threading.Monitor.Pulse(obj);
                    this.CloseDb();
                    return ret;
                }                
            }
        }


        /// <summary>
        /// 删
        /// </summary>
        /// <param name="keyValue">主键</param>
        public int Delete(  string keyValue)
        {
            lock (obj)
            {
                int ret = -1;
                if (!OpenDb())
                    return -2;
                using (SQLiteCommand cmd = new SQLiteCommand()) { 
                    cmd.Connection = dbConObj;
                    cmd.CommandText = "DELETE FROM " + this.dbTable + " WHERE " + keyValue;
                    ret = cmd.ExecuteNonQuery();
                    this.CloseDb();
                    System.Threading.Monitor.Pulse(obj);
                    return ret;
                }
            }     
        }
        /// <summary>
        /// 查
        /// </summary>
        /// <param name="KeyName">查找的主键</param>
        /// <returns></returns>
        public SQLiteDataReader Search( string KeyValue)
        {
            lock (obj)
            {
                if (!OpenDb())
                return null;
                using (SQLiteCommand cmd = new SQLiteCommand())
                {
                    cmd.Connection = dbConObj;
                    cmd.CommandText = "SELECT * FROM " + this.dbTable + " WHERE " + KeyValue;
                    //cmd.CommandText =  "SELECT * FROM sqlite_master WHERE type='table' and name='" + tableName + "';";
                    SQLiteDataReader ret  = cmd.ExecuteReader();
                    this.CloseDb();
                    System.Threading.Monitor.Pulse(obj);
                    return ret;
                }
            }
        }
        /// <summary>
        /// 查看字段是否存在
        /// </summary>
        /// <param name="ColumnName">列名</param>
        /// <param name="ColumnIndex">序号</param>
        /// <param name="KeyValue">要对比的字段</param>
        /// <returns>返回第一列</returns>
        public object IsFieldExist(string ColumnName, string Value)
        {
            try
            {
                lock (obj)
                {
                    string sql = "SELECT * FROM " + this.dbTable + " WHERE " + ColumnName + "='" + Value + "'";
                    if (!OpenDb())
                        return null;
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConObj))
                    {
                        return cmd.ExecuteScalar();
                    }
                }
            } catch { return null; }
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PrimeKey">主键的列名</param>
        /// <param name="KeyValue">主键值</param>
        /// <param name="ColumnName">要判断是否为空的列</param>
        /// <returns></returns>
        public bool IsFieldNull(string PrimeKey,string KeyValue, string ColumnName)
        {
            lock (obj)
            {
                try
                {
                    string sql = "SELECT * FROM " + this.dbTable + " WHERE " + PrimeKey + "='" + KeyValue + "'";
                    if (!OpenDb())
                        return false;
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, dbConObj))
                    {
                        cmd.ExecuteNonQuery();
                        DataTable dt = new DataTable();
                        SQLiteDataAdapter apt = new SQLiteDataAdapter(cmd);
                        apt.Fill(dt);
                        if (dt.Rows.Count != 1)
                            throw new Exception("Key not exist!");
                        bool ret = string.IsNullOrEmpty(dt.Rows[0][ColumnName].ToString());
                        this.CloseDb();
                        System.Threading.Monitor.Pulse(obj);
                        return ret;
                    }
                }
                catch {
                    return false;
                }
               
            }
        }

        /// <summary>
        /// 获取整张表
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetDbTable()
        {
            lock (obj)
            {
                if (!OpenDb())
                return null;
                using ( SQLiteCommand cmd = new SQLiteCommand("SELECT * FROM " + this.dbTable, dbConObj))
                { 
                    cmd.ExecuteNonQuery();                
                    DataTable dt = new DataTable();
                    SQLiteDataAdapter adp = new SQLiteDataAdapter(cmd);
                    adp.Fill(dt);
                    //adp.Update(dt);
                    this.CloseDb();
                    System.Threading.Monitor.Pulse(obj);
                    return dt;
                }
            }
        }
        
        bool OpenDb()
        {
            lock (obj)
            {
                try
                {
                    if (dbConObj == null)
                    {
                        dbConObj = new SQLiteConnection(this.dbConStr);
                    }
                    if (dbConObj.State != ConnectionState.Open)
                    {
                        dbConObj.Open();
                    }
                    return true;
                }
                catch { return false; }
                finally
                {
                    System.Threading.Monitor.Pulse(obj);
                }
            }
        }

   
        void CloseDb()
        {
            lock (obj)
            {
                try
                {
                    if (this.dbConObj != null && this.dbConObj.State != ConnectionState.Closed)
                    {
                        if (this._IsRunTrans && this._AutoCmmit)
                        {
                            this.Commit();
                        }
                        try
                        {
                            this.dbConObj.Close();
                            GC.Collect();
                            GC.WaitForPendingFinalizers();
                        }
                        catch { }
                    }
                }
                catch { }
                finally
                {
                    System.Threading.Monitor.Pulse(obj);
                }
            }
        }


        #region 事务
        /// <summary>
        /// 开始数据库事务
        /// </summary>
        public void BeginTransaction()
        {
            this.dbConObj.BeginTransaction();
            this._IsRunTrans = true;
        }

        // <summary>
        /// 开始数据库事务
        /// </summary>
        /// <param name="isoLevel">事务锁级别</param>
        public void BeginTransaction(IsolationLevel isoLevel)
        {
            this.dbConObj.BeginTransaction(isoLevel);
            this._IsRunTrans = true;
        }

        /// <summary>
        /// 提交当前挂起的事务
        /// </summary>
        public void Commit()
        {
            if (this._IsRunTrans)
            {
                this.tsc.Commit();
                this._IsRunTrans = false;
            }
        }

        /// <summary>
        /// 用来重新整理整个数据库达到紧凑之用，比如把删除的彻底删掉等等。
        /// </summary>
        public  void VACUUM()
        {
            lock (obj)
            {
                try
                {
                    if (!OpenDb())
                        return;
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = dbConObj;
                    cmd.CommandText = "VACUUM";
                    cmd.ExecuteNonQuery();
                }
                catch { }
                finally
                {
                    this.CloseDb(); System.Threading.Monitor.Pulse(obj);

                }
            }
        }


        /// <summary>
        /// 删除数据库
        /// </summary> 
        public void DeleteDbFile( )
        {
            if (System.IO.File.Exists(this.dbPath))
            {
                System.IO.File.Delete(this.dbPath);
            }
        }

        /// <summary>
        /// 删除表
        /// </summary>
        public void DeleteDbTable( )
        {
            lock (obj)
            {
                try
                {
                    if (!OpenDb())
                        return;
                    SQLiteCommand cmd = new SQLiteCommand();
                    cmd.Connection = this.dbConObj;
                    cmd.CommandText = "DROP TABLE IF EXISTS " + this.dbTable;
                    cmd.ExecuteNonQuery();
                    this.dbTable = null;
                }
                catch { }
                finally
                {
                    this.CloseDb(); System.Threading.Monitor.Pulse(obj);

                }
            }
        }
        #endregion
    }


}
