using MiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mono=Mono.Data.Sqlite;
using System.Data;
using NT=System.Data.SQLite;

namespace CorePlugins
{
    public class DataBase
    {
        public static ModelUser NotUser = new ModelUser { id = -1 };

        private bool _isMono;
        private DataBaseProvider _provider;

        public DataBase()
        {
            _isMono = MiNetServer.IsRunningOnMono();

            if(_isMono)
            {
                _provider = new DataBaseMono();
            }
            else
            {
                _provider = new DataBaseNT();
            }

            //NT.SQLiteConnection.CreateFile("db.sqlite");
            //NT.SQLiteConnection m_dbConnection = new NT.SQLiteConnection("Data Source=db.sqlite;Version=3;");
            //m_dbConnection.Open();
            //string sql = "CREATE TABLE users (id INTEGER PRIMARY KEY AUTOINCREMENT, nick VARCHAR(64), password VARCHAR(32), role INT)";
            //NT.SQLiteCommand command = new NT.SQLiteCommand(sql, m_dbConnection);
            //command.ExecuteNonQuery();
        }

        public bool HasUser(string nick)
        {
            return _provider.Query("select id from users where nick == @0", new List<string> { "id" }, nick).Count > 0;
        }

        public bool HasUser(string nick, out ModelUser user)
        {
            var res = _provider.Query("select * from users where nick == @0", new List<string> { "id", "nick", "password", "role" }, nick);
            if (res.Count == 0)
            {
                user = NotUser;
                return false;
            }

            user = new ModelUser { id = (long)res["id"], nick = res["nick"].ToString(), password = res["password"].ToString(), role = (int)res["role"]};

            return true;
        }

        public void AddUser(string nick, string password)
        {
            _provider.NonQuery("insert into users values (null, @0, @1, 0)", nick, password);
        }

        ~DataBase()
        {
            _provider.Dispose();
        }
    }

    public struct ModelUser
    {
        public long id;
        public int role;
        public string nick, password;

        public override string ToString()
        {
            return string.Format("User {0}; nick: {1}; password: {2}; role: {3}", id, nick, password, role);
        }
    }

    public interface DataBaseProvider
    {
        Dictionary<string, object> Query(string data, List<string> fields, params object[] parameters);
        void NonQuery(string data, params object[] parameters);
        void Dispose();
    }

    public class DataBaseNT : DataBaseProvider
    {
        private NT.SQLiteConnection m_dbConnection;

        public DataBaseNT()
        {
            m_dbConnection = new NT.SQLiteConnection("Data Source=db.sqlite;Version=3;");
            m_dbConnection.Open();
        }

        public Dictionary<string, object> Query(string data, List<string> fields, params object[] parameters)
        {
            NT.SQLiteCommand cmd = new NT.SQLiteCommand(data, m_dbConnection);
            int i = 0;
            foreach(var param in parameters)
            {
                cmd.Parameters.Add(new NT.SQLiteParameter(i++.ToString(), param));
            }
            NT.SQLiteDataReader reader = cmd.ExecuteReader();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            while (reader.Read())
                foreach (string field in fields)
                    dict.Add(field, reader[field]);
            reader.Close();
            cmd.Dispose();
            return dict;
        }

        public void NonQuery(string data, params object[] parameters)
        {
            NT.SQLiteCommand cmd = new NT.SQLiteCommand(data, m_dbConnection);
            int i = 0;
            foreach (var param in parameters)
            {
                cmd.Parameters.Add(new NT.SQLiteParameter(i++.ToString(), param));
            }
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        public void Dispose()
        {
            m_dbConnection.Dispose();
        }
    }
    public class DataBaseMono : DataBaseProvider
    {
        public DataBaseMono()
        {

        }

        public Dictionary<string, object> Query(string data, List<string> fields, params object[] parameters)
        {
            return new Dictionary<string, object>();
        }

        public void NonQuery(string data, params object[] parameters)
        {
            
        }

        public void Dispose()
        {

        }
    }
}
