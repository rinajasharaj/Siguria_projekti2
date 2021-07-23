using MySql.Data.MySqlClient;
using Projekt2;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;

namespace Server
{
    public partial class Server : Form
    {
        String connectionstring = System.Configuration.ConfigurationSettings.AppSettings["connstring"];
        private Socket serveri;
        private MySqlConnection connection;
        private RSACryptoServiceProvider rsa;
        private DESCryptoServiceProvider des;
        X509Certificate2 serverCertificate;
        public Server()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            
        }

        public bool finduser(string username)
        {
            bool res = false;

            try
            {
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from users where Username= @Username", connection);
                cmd.Parameters.AddWithValue("@Username", username);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read()) res = true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            finally
            {
                connection.Close();
            }
            return res;

        }

        public string authenticate(string username, string password)
        {
            string outs = null;

            try
            {
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from users where Username= @Username", connection);
                cmd.Parameters.AddWithValue("@Username", username);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (reader["Username"] == username)
                    {
                        if (reader["Password"] == hashPassword(password + reader["Salt"]))
                        {
                            outs = reader["Id"].ToString();
                        }
                    }
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
            return outs;
        }

        public string hashPassword(string saltedpwd)
        {
            byte[] byteSaltPassword = Encoding.UTF8.GetBytes(saltedpwd);

            SHA1CryptoServiceProvider objHash = new SHA1CryptoServiceProvider();
            byte[] byteSaltedHashPassword = objHash.ComputeHash(byteSaltPassword);
            return Convert.ToBase64String(byteSaltedHashPassword);

        }

        public void registerUser(string username, string pwd)
        {
            USER user = new USER(username, pwd);

            try
            {
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                String query = "insert into users(UserName,Password,Salt) values (" +
                 "'" + user.usernameGetSet + "','" + user.passwordi + "','" + user.saltGetSet + "')";
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

        public string decryptMessage(string message)
        {
            rsa = (RSACryptoServiceProvider)serverCertificate.PrivateKey;
            byte[] fullMsgData = Convert.FromBase64String(message);
            byte[] iv = new byte[8];
            Array.Copy(fullMsgData, iv, 8);

            byte[] enDesKey = new byte[128];
            byte[] desKey = new byte[8];
            Array.Copy(fullMsgData, iv.Length, enDesKey, 0, 128);
            desKey = rsa.Decrypt(enDesKey, true);
            // Console.WriteLine("iv length="+iv.Length+" iv value="+Convert.ToBase64String(iv)+" deskey  length="+enDesKey.Length+"  des encrytpted="+Convert.ToBase64String(enDesKey)+" des decrypted="+Convert.ToBase64String(desKey));

            des = new DESCryptoServiceProvider();
            des.IV = iv;
            des.Key = desKey;
            des.Mode = CipherMode.CBC;
            des.Padding = PaddingMode.PKCS7;

            byte[] encryptedMessage = new byte[fullMsgData.Length - iv.Length - enDesKey.Length];

            Array.Copy(fullMsgData, iv.Length + enDesKey.Length, encryptedMessage, 0, fullMsgData.Length - iv.Length - enDesKey.Length);

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(encryptedMessage, 0, encryptedMessage.Length);
            cryptoStream.FlushFinalBlock();
            byte[] decryptedMsg = memoryStream.ToArray();

            string decryptedData = Encoding.ASCII.GetString(decryptedMsg);

            return decryptedData;
        }

        public string encryptMessage(string message)
        {
            des.GenerateIV();
            byte[] ivPrim = des.IV;
            des.Mode = CipherMode.CBC;
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(Encoding.ASCII.GetBytes(message), 0, Encoding.ASCII.GetBytes(message).Length);
            cryptoStream.FlushFinalBlock();
            byte[] encryptedMsg = memoryStream.ToArray();

            byte[] fullMsg = new byte[ivPrim.Length + encryptedMsg.Length];

            ivPrim.CopyTo(fullMsg, 0);
            encryptedMsg.CopyTo(fullMsg, ivPrim.Length);

            return Convert.ToBase64String(fullMsg);
        }

        private void txtStatus_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            X509Store objStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            objStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection =
                                                        X509Certificate2UI.SelectFromCollection(objStore.Certificates,
                                                        "Zgjedh certifikaten per nenshkrim", "Zgjedh certifikaten",
                                                        X509SelectionFlag.SingleSelection);
            try
            {
                serverCertificate = certCollection[0];
                if (serverCertificate.HasPrivateKey)
                {
                    MessageBox.Show("   Certifikata permbane cels privat!   ");

                    string strContent = "";
                    strContent += "Subject: " + serverCertificate.Subject + "\n";
                    strContent += "Issuer: " + serverCertificate.Issuer + "\n";
                    strContent += "Thumbprint: " + serverCertificate.Thumbprint;
                    MessageBox.Show("  Te dhenat e certifikates:\n\n " + strContent);
                }
            }
            catch
            {
                MessageBox.Show("   Certifikata ne indeksin 0 nuk ekziston!   ");
            }

        }
        public void responseToClient(Socket clientSocket)
        {
            
                
                while (true)
                {
                    byte[] bytes = new Byte[1024];
                    string data = null;
                    int numByte = clientSocket.Receive(bytes);

                    data += Encoding.ASCII.GetString(bytes, 0, numByte);

                    string decryptedData = decryptMessage(data);
                    

                    string command = decryptedData.Split('*')[0];

                    Console.WriteLine(command);
                    if (data == "Exit")
                        break;
                    else if (command == "Register")
                    {
                        if (finduser(decryptedData.Split('*')[1]))
                        {
                            byte[] response = Encoding.ASCII.GetBytes("ERROR");
                            clientSocket.Send(response);
                        }
                        else
                        {
                            registerUser(decryptedData.Split('*')[1], decryptedData.Split('*')[2]);
                            MessageBox.Show("Yout account is created succesfully");
                            byte[] response = Encoding.ASCII.GetBytes(encryptMessage("OK"));
                            clientSocket.Send(response);
                        }
                    }
                    else if (command == "Login")
                    {
                        string userId = authenticate(decryptedData.Split('*')[1], decryptedData.Split('*')[2]);
                        if (userId != null)
                        {
                            connection = new MySqlConnection(connectionstring);
                            connection.Open();
                            MySqlCommand cmd = new MySqlCommand("select u.username,f.lloji,f.viti, f.muaji, f.qmimi, f.qmimitvsh " +
                                "from users u inner join fatura f where @u.Id=f.UserId", connection);
                            cmd.Parameters.AddWithValue("@u.Id", userId);
                            MySqlDataReader reader = cmd.ExecuteReader();
                            while (reader.Read())
                            {
                                String username = reader["Username"].ToString();
                                String lloji = reader["lloji"].ToString();
                                String viti = reader["viti"].ToString();
                                String muaji = reader["muaji"].ToString();
                                String cmimi = reader["qmimi"].ToString();
                                String cmimitvsh = reader["qmimitvsh"].ToString();
                                createAndSignXml(username, lloji, viti, muaji, cmimi, cmimitvsh);

                            }
                        }
                        else
                        {
                            byte[] response = Encoding.ASCII.GetBytes(encryptMessage("ERROR"));
                            clientSocket.Send(response);
                        }
                    }
                    else if (command == "Fatura")
                    {
                        USER user = USER.getfromdb(Convert.ToInt32(decryptedData.Split(' ')[1]));
                        user.addfatura(decryptedData.Split(' ')[2], decryptedData.Split(' ')[3], decryptedData.Split(' ')[4], decryptedData.Split(' ')[5], decryptedData.Split(' ')[6]);
                        byte[] response = Encoding.ASCII.GetBytes(encryptMessage("OK"));
                        clientSocket.Send(response);
                    }
                    else
                    {
                        Console.WriteLine("Something went worng");
                    }
                }
                Console.WriteLine("Text received -> {0} ", data);
                byte[] message = Encoding.ASCII.GetBytes("Test Server");

            

            clientSocket.Shutdown(SocketShutdown.Both);
            clientSocket.Close();
        }

        public void listen()
        {
            serveri.Listen(10);

            while (true)
            {
                Socket client = serveri.Accept();
                Thread thread = new Thread(() => this.responseToClient(client));
                thread.Start();
            }
        }
       
        private void startbtn_Click(object sender, EventArgs e)
        {
            IPHostEntry ipHost = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHost.AddressList[0];
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 11100);


            try
            {
                serveri = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                serveri.Bind(localEndPoint);
                txtStatus.AppendText("Waiting connection ... ");
                listen();


            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        XmlDocument objXml = new XmlDocument();

        private string createAndSignXml(string username, string lloji, string viti, string muaji, string cmimi, string cmimitvsh)
        {
            if (File.Exists(username + ".xml") == false)
            {
                XmlTextWriter xmlTw = new XmlTextWriter(username + ".xml", Encoding.UTF8);
                xmlTw.WriteStartElement("User");
                xmlTw.Flush();
                xmlTw.Close();
            }

            objXml.Load(username + ".xml");

            XmlElement rootNode = objXml.DocumentElement;

            XmlElement usernameNode = objXml.CreateElement("Username");
            XmlElement llojiNode = objXml.CreateElement("Lloji");
            XmlElement vitiNode = objXml.CreateElement("Viti");
            XmlElement muajiNode = objXml.CreateElement("Muaji");
            XmlElement cmimiNode = objXml.CreateElement("Cmimi");
            XmlElement cmimitvshNode = objXml.CreateElement("Cmimitvsh");

            usernameNode.InnerText = username;
            llojiNode.InnerText = lloji;
            vitiNode.InnerText = viti;
            muajiNode.InnerText = muaji;
            cmimiNode.InnerText = cmimi;
            cmimitvshNode.InnerText = cmimitvsh;

            rootNode.AppendChild(usernameNode);
            rootNode.AppendChild(llojiNode);
            rootNode.AppendChild(vitiNode);
            rootNode.AppendChild(muajiNode);
            rootNode.AppendChild(cmimiNode);
            rootNode.AppendChild(cmimitvshNode);
            objXml.Save(username + ".xml");



            objXml.Load(username + ".xml");

            RSACryptoServiceProvider privateKeyProvider = (RSACryptoServiceProvider)serverCertificate.PrivateKey;

            SignedXml objSignedXml = new SignedXml(objXml);

            Reference referenca = new Reference();
            referenca.Uri = "";

            XmlDsigEnvelopedSignatureTransform transform = new XmlDsigEnvelopedSignatureTransform();
            referenca.AddTransform(transform);

            objSignedXml.AddReference(referenca);

            KeyInfo kI = new KeyInfo();
            kI.AddClause(new RSAKeyValue(privateKeyProvider));

            objSignedXml.KeyInfo = kI;

            objSignedXml.SigningKey = (RSACryptoServiceProvider)serverCertificate.PrivateKey;

            objSignedXml.ComputeSignature();

            XmlElement signatureNode = objSignedXml.GetXml();

            rootNode = objXml.DocumentElement;
            rootNode.AppendChild(signatureNode);



            objXml.Save(username + "_nenshkruar.xml");
            return objXml.InnerXml;
        }

    }
}

    

