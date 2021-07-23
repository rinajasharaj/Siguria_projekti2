using System;
using System.Security.Cryptography;
using System.Xml;
using System.Text;
using MySql.Data.MySqlClient;

namespace Projekt2
{
    class USER
    {
        private int id;
        private string username;
        private string password;
        private string salt;
        String connectionstring = System.Configuration.ConfigurationSettings.AppSettings["connstring"];
        private static MySqlConnection connection;

        public USER(int id, string username, string pwd, string salt)
        {
            this.id = id;
            this.username = username;
            this.password = pwd;
            this.salt = salt;
        }

        public USER(string username, string pwd)
        {
            this.id = getNewId();
            this.username = username;
            this.salt = generateSalt();
            this.password = hashPassword(pwd);
        }

        public static USER getfromdb(int id, String connstring)
        {
            USER user = null;

            try
            {
                connection = new MySqlConnection(connstring);
                connection.Open();

                MySqlCommand cmd = new MySqlCommand("Select * from Register ", connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["Id"].ToString() == Convert.ToString(id))
                        user = new USER(Convert.ToInt32(reader["Id"]), reader["Username"].ToString(), reader["Password"].ToString(), reader["Salt"].ToString());
                    return user;
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                connection.Close();
            }
            return user;
        }


        public void printUserInfo()
        {
            Console.WriteLine("------------User Info------------");
            Console.WriteLine("Username=" + this.usernameGetSet);
            Console.WriteLine("Id=" + this.idGetSet);
            Console.WriteLine("---------------------------------");
        }

        public int idGetSet
        {
            get
            {
                return this.id;
            }
            set
            {
                this.id = value;
            }
        }

        public void addfatura(string type, string value, string year, string month)
        {
            try
            {
                String connectionstring = System.Configuration.ConfigurationSettings.AppSettings["connstring"];
                double tvsh = (Convert.ToDouble(value) * 18) / 100;
                double qmimimetvsh = Convert.ToDouble(value) + tvsh;
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                String query = "insert into fatura(lloji,viti,muaji,qmimi,qmimitvsh) values (" +
                 "'" + type + "','" + year + "','" +month+ "','" + value + "','" + qmimimetvsh + "')";
                MySqlCommand comand = new MySqlCommand(query, connection);
                MySqlDataReader reader = comand.ExecuteReader();
                reader.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                connection.Close();
            }
        }

        public string usernameGetSet
        {
            get
            {
                return this.username;
            }
            set
            {
                this.username = value;
            }
        }

        public string passwordi
        {
            get
            {
                return this.password;
            }
            set
            {
                this.password = hashPassword(value + this.saltGetSet);
            }
        }

        public string saltGetSet
        {
            get
            {
                return this.salt;
            }
            set
            {
                this.salt = generateSalt();
            }
        }

        public string hashPassword(string saltedpwd)
        {
            byte[] byteSaltPassword = Encoding.UTF8.GetBytes(saltedpwd);

            SHA1CryptoServiceProvider objHash = new SHA1CryptoServiceProvider();
            byte[] byteSaltedHashPassword = objHash.ComputeHash(byteSaltPassword);
            
            return Convert.ToBase64String(byteSaltedHashPassword);
        }

        public string generateSalt()
        {
            byte[] bytes = new byte[12];
            RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider();
            rngCsp.GetNonZeroBytes(bytes);

            return Convert.ToBase64String(bytes);
        }

        public int getNewId()
        {
            int ret = 1;

            try
            {
                String connectionstring = System.Configuration.ConfigurationSettings.AppSettings["connstring"];
                connection = new MySqlConnection(connectionstring);
                connection.Open();

                MySqlCommand cmd = new MySqlCommand("SELECT * from users ", connection);
                MySqlDataReader reader = cmd.ExecuteReader();
                
                if (reader.HasRows == false)
                {
                    ret = 1;
                }
                else
                {
                    MySqlCommand com = new MySqlCommand("SELECT LAST_INSERT_ID() from users ", connection);
                    MySqlDataReader mdr = com.ExecuteReader();
                    if (mdr != null && mdr.Read())
                    {
                        long iddb = reader.GetInt64(0);

                        ret= (int)(iddb + 1);
                    }
                    mdr.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                connection.Close();
            }
            return ret;
        }
    }
}

