using MiNET;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Linux=Mono.Data.Sqlite;
using System.Data;
using NT=System.Data.SQLite;

namespace CorePlugins
{
    public class DataBase
    {
        public static ModelUser NotUser = new ModelUser { id = -1 };
        public static Dictionary<long, PermRow> Perms = new Dictionary<long, PermRow>();

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

            LoadAllPerms();

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
            var res = _provider.Query("select * from users where nick == @0", new List<string> { "id", "nick", "password", "role", "perm_id", "perms" }, nick);
            if (res.Count == 0)
            {
                user = NotUser;
                return false;
            }

            user = new ModelUser { id = (long)res["id"], nick = res["nick"].ToString(), password = res["password"].ToString(), role = (int)res["role"], perm_id = (long)res["perm_id"], perms = res["perms"].ToString()};

            return true;
        }

        public void AddUser(string nick, string password)
        {
            _provider.NonQuery("insert into users values (null, @0, @1, 0, 1, '')", nick, password);
        }

        public void ChangeUserRole(string nick, int role)
        {
            _provider.NonQuery("update users set role = @1 where nick = @0", nick, role);
        }

        public void ChangeUserPermGroupId(string nick, int perm_id)
        {
            _provider.NonQuery("update users set perm_id = @1 where nick = @0", nick, perm_id);
        }

        public void ChangeUserCustomPerms(string nick, string perms)
        {
            _provider.NonQuery("update users set perms = @1 where nick = @0", nick, perms);
        }

        public void ChangeUserPassword(string nick, string pass)
        {
            _provider.NonQuery("update users set password = @1 where nick = @0", nick, pass);
        }

        public void ChangePermsValue(long id, string perms)
        {
            _provider.NonQuery("update perms set perms = @1 where id = @0", id, perms);
        }

        public void ChangePermsParent(long id, long parent)
        {
            _provider.NonQuery("update perms set parent = @1 where id = @0", id, parent);
        }

        public void ChangePermsName(long id, string name)
        {
            _provider.NonQuery("update perms set name = @1 where id = @0", id, name);
        }

        public PermRow LoadUserCustomPerms(string nick)
        {

            return PermRow.Serialize(_provider.Query("select perms from users where nick == @0", new List<string> { "perms" }, nick)["perms"].ToString());
        }

        public PermRow LoadUserGroupPerms(string nick)
        {
            return Perms[(long)_provider.Query("select perm_id from users where nick == @0", new List<string> { "perm_id" }, nick)["perm_id"]];
        }

        public PermRow LoadUserPerms(string nick)
        {
            var custom = _provider.Query("select perm_id, perms from users where nick == @0", new List<string> { "perms", "perm_id" }, nick);
            var customPerms = PermRow.Serialize(custom["perms"].ToString());
            customPerms.Parent = Perms[(long)custom["perm_id"]].Copy();
            return customPerms;
        }

        public void LoadAllPerms()
        {
            Perms = _provider.QueryAll("select * from perms", new List<string> { "id", "name", "perms", "parent" }).ToDictionary(x => (long)x["id"], x => PermRow.Serialize(x["perms"].ToString(), x["name"].ToString(), (long)x["parent"]));
            int count = Perms.Count;
            for (long i = 1; i <= count; i++)
            {
                if(Perms[i].parent > 0)
                    Perms[i].Parent = Perms[Perms[i].parent];
            }
        }

        ~DataBase()
        {
            _provider.Dispose();
        }
    }

    public struct ModelUser
    {
        public long id, perm_id;
        public int role;
        public string nick, password, perms;

        public override string ToString()
        {
            return string.Format("User {0}; nick: {1}; password: {2}; role: {3}", id, nick, password, role);
        }
    }
    public struct ModelPerm
    {
        public long id;
        public string perms;

        public override string ToString()
        {
            return string.Format("Perm {0}; perms: {1}", id, perms);
        }
    }

    public class PermRow
    {
        public Dictionary<string, bool> perms = new Dictionary<string, bool>();
        public string Name = "UserPerms";
        public PermRow Parent = null;
        public long parent = 0;

        public PermRow Copy()
        {
            return new PermRow { Name = Name, Parent = Parent, perms = perms, parent = parent };
        }

        public bool CheckPerm(string perm)
        {
            return perms.ContainsKey(perm) ? perms[perm] : (Parent != null ? Parent.CheckPerm(perm) : false);
        }

        public void SetPerm(string perm, bool value = true)
        {
            perms[perm] = value;
        }

        public void RemovePerm(string perm)
        {
            if(perms.ContainsKey(perm))
                perms.Remove(perm);
        }

        public override string ToString()
        {
            return Deserialize(this);
        }

        public static PermRow Serialize(string perms, string name = "UserPerms", long parent = 0)
        {
            return new PermRow { perms = perms.Split(';').ToDictionary(x => x.TrimStart('-'), x => x.Length > 0 ? x[0] != '-' : false), Name = name, parent = parent };
        }

        public static string Deserialize(PermRow perms)
        {
            return string.Join(";", perms.perms.Select(x => { return (!x.Value ? "-" : "") + x.Key; }));
        }
    }

    public interface DataBaseProvider
    {
        Dictionary<string, object> Query(string data, List<string> fields, params object[] parameters);
        void NonQuery(string data, params object[] parameters);
        void Dispose();
        List<Dictionary<string, object>> QueryAll(string data, List<string> fields, params object[] parameters);
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
            foreach (var param in parameters)
            {
                cmd.Parameters.Add(new NT.SQLiteParameter(i++.ToString(), param));
            }
            NT.SQLiteDataReader reader = cmd.ExecuteReader();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            while (reader.Read())
                foreach (string field in fields)
                    dict[field] = reader[field];
            reader.Close();
            cmd.Dispose();
            return dict;
        }
        public List<Dictionary<string, object>> QueryAll(string data, List<string> fields, params object[] parameters)
        {
            NT.SQLiteCommand cmd = new NT.SQLiteCommand(data, m_dbConnection);
            int i = 0;
            foreach (var param in parameters)
            {
                cmd.Parameters.Add(new NT.SQLiteParameter(i++.ToString(), param));
            }
            NT.SQLiteDataReader reader = cmd.ExecuteReader();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            while (reader.Read())
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                foreach (string field in fields)
                    dict[field] = reader[field];
                list.Add(dict);
            }
            reader.Close();
            cmd.Dispose();
            return list;
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
        private Linux.SqliteConnection m_dbConnection;

        public DataBaseMono()
        {
            m_dbConnection = new Linux.SqliteConnection("URI=file:db.sqlite,version=3");
            m_dbConnection.Open();
        }

        public Dictionary<string, object> Query(string data, List<string> fields, params object[] parameters)
        {
            Linux.SqliteCommand cmd = new Linux.SqliteCommand(data, m_dbConnection);
            int i = 0;
            foreach (var param in parameters)
            {
                cmd.Parameters.Add(new Linux.SqliteParameter(i++.ToString(), param));
            }
            Linux.SqliteDataReader reader = cmd.ExecuteReader();
            Dictionary<string, object> dict = new Dictionary<string, object>();
            while (reader.Read())
                foreach (string field in fields)
                    dict[field] = reader[field];
            reader.Close();
            cmd.Dispose();
            return dict;
        }

        public List<Dictionary<string, object>> QueryAll(string data, List<string> fields, params object[] parameters)
        {
            Linux.SqliteCommand cmd = new Linux.SqliteCommand(data, m_dbConnection);
            int i = 0;
            foreach (var param in parameters)
            {
                cmd.Parameters.Add(new Linux.SqliteParameter(i++.ToString(), param));
            }
            Linux.SqliteDataReader reader = cmd.ExecuteReader();
            List<Dictionary<string, object>> list = new List<Dictionary<string, object>>();
            while (reader.Read())
            {
                Dictionary<string, object> dict = new Dictionary<string, object>();
                foreach (string field in fields)
                    dict[field] = reader[field];
                list.Add(dict);
            }
            reader.Close();
            cmd.Dispose();
            return list;
        }

        public void NonQuery(string data, params object[] parameters)
        {
            Linux.SqliteCommand cmd = new Linux.SqliteCommand(data, m_dbConnection);
            int i = 0;
            foreach (var param in parameters)
            {
                cmd.Parameters.Add(new Linux.SqliteParameter(i++.ToString(), param));
            }
            cmd.ExecuteNonQuery();
            cmd.Dispose();
        }

        public void Dispose()
        {
            m_dbConnection.Dispose();
        }
    }
}
