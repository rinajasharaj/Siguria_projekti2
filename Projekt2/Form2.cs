using MySql.Data.MySqlClient;
using System;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;

namespace Projekt2
{
    public partial class Form2 : Form
    {
        String connectionstring = System.Configuration.ConfigurationSettings.AppSettings["connstring"];
        private MySqlConnection connection;
        private RSACryptoServiceProvider rsa ;
        private X509Certificate2 certificate;
        private USER user = null;
        private Socket derguesi;

        private DESCryptoServiceProvider des = null;
        public Form2()
        {
            InitializeComponent();
        }

        public string decryptMessageClient(string message)
        {

            byte[] fullMsgData = Convert.FromBase64String(message);
            byte[] iv = new byte[8];
            Array.Copy(fullMsgData, iv, 8);

            des.IV = iv;
            des.Mode = CipherMode.CBC;

            byte[] encryptedMessage = new byte[fullMsgData.Length - iv.Length];

            Array.Copy(fullMsgData, iv.Length, encryptedMessage, 0, fullMsgData.Length - iv.Length);

            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateDecryptor(), CryptoStreamMode.Write);
            cryptoStream.Write(encryptedMessage, 0, encryptedMessage.Length);
            cryptoStream.FlushFinalBlock();
            byte[] decryptedMsg = memoryStream.ToArray();

            string decryptedData = Encoding.ASCII.GetString(decryptedMsg);

            return decryptedData;
        }

        public void sendMessage(string message)
        {
            try
            {
                   
                    rsa = (RSACryptoServiceProvider)certificate.PublicKey.Key;

                    des = new DESCryptoServiceProvider();
                    des.GenerateIV();
                    des.GenerateKey();
                    byte[] iv = des.IV;
                    byte[] desKey = rsa.Encrypt(des.Key,true);
                    // Console.WriteLine("iv length="+iv.Length+" iv value="+Convert.ToBase64String(des.IV)+" deskey  length="+desKey.Length+"  des encrytpted="+Convert.ToBase64String(desKey)+" des decrypted="+Convert.ToBase64String(des.Key));

                    des.Mode = CipherMode.CBC;
                    des.Padding = PaddingMode.PKCS7;
                    MemoryStream memoryStream = new MemoryStream();
                    CryptoStream cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(Encoding.ASCII.GetBytes(message), 0, Encoding.ASCII.GetBytes(message).Length);
                    cryptoStream.FlushFinalBlock();
                    byte[] encryptedMsg = memoryStream.ToArray();

                    // Console.WriteLine("encrypted msg="+Convert.ToBase64String(encryptedMsg));
                    byte[] fullMsg = new byte[iv.Length + desKey.Length + encryptedMsg.Length];

                    iv.CopyTo(fullMsg, 0);
                    desKey.CopyTo(fullMsg, iv.Length);
                    encryptedMsg.CopyTo(fullMsg, desKey.Length + iv.Length);

                    // Console.WriteLine("Full msg="+Convert.ToBase64String(fullMsg));
                    byte[] msg = Encoding.ASCII.GetBytes(Convert.ToBase64String(fullMsg));
                    derguesi.Send(msg);
            }
            catch (ArgumentNullException ane)
            {
                Console.WriteLine("ArgumentNullException : {0}", ane.ToString());
            }

            catch (SocketException se)
            {

                Console.WriteLine("SocketException : {0}", se.ToString());
            }

            catch (Exception e)
            {
                Console.WriteLine("Unexpected exception : {0}", e.ToString());
            }
        }

        public static bool is_Valid_Password(String password)
        {
            if (password.Length < 8)
            {
                return false;
            }

            int charCount = 0;
            int numCount = 0;
            for (int i = 0; i < password.Length; i++)
            {
                char ch = password[i];

                if (is_Numeric(ch)) numCount++;
                else if (is_Letter(ch)) charCount++;
                else return false;
            }

            return (charCount >= 1 && numCount >= 1);
        }

        public static bool is_Letter(char ch)
        {
            ch = char.ToUpper(ch);
            return (ch >= 'A' && ch <= 'Z');
        }

        public static bool is_Numeric(char ch)
        {
            return (ch >= '0' && ch <= '9');
        }

        private void loggedin_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Form1 login = new Form1();
            login.Show();  
        }

        private void Registerbutton_Click_1(object sender, EventArgs e)
        {
            if (txtusername.Text.Length !=0 && txtpassword.Text.Length != 0 && txtconfirmpass.Text.Length != 0)
            {
                if (txtpassword.Text != txtconfirmpass.Text)
                {
                    MessageBox.Show("Please confirm password");
                }
                else if (is_Valid_Password(txtpassword.Text) == false)
                {
                    MessageBox.Show("Password must contain at least 8 characters, one uppercase letter and one digit");
                }
                else
                {
                    try
                    {
                        IPHostEntry ipHost = Dns.GetHostEntry("localhost");
                        IPAddress ipAddr = ipHost.AddressList[0];
                        Console.WriteLine(ipAddr);
                        IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11100);
                        Console.WriteLine(localEndPoint);
                        derguesi = new Socket(ipAddr.AddressFamily, SocketType.Stream, ProtocolType.Tcp);

                        try
                        {
                            derguesi.Connect(localEndPoint);
                            Console.WriteLine("Socket connected to -> {0} ",
                                         derguesi.RemoteEndPoint.ToString());

                            // Creation of messagge that
                            // we will send to Server
                            String user = txtusername.Text;
                            String pass = txtpassword.Text;

                            sendMessage("Register*" + user + "*" + pass);

                            byte[] messageReceived = new byte[1024];


                            int byteRecv = derguesi.Receive(messageReceived);
                            string response = Encoding.ASCII.GetString(messageReceived, 0, byteRecv);

                            string serverRes = decryptMessageClient(response);
                            Console.WriteLine(serverRes);

                            derguesi.Shutdown(SocketShutdown.Both);
                            derguesi.Close();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.ToString());
                    }

                }
            }
            else
            {
                MessageBox.Show("Pleas fill out the form");
            }
        }

        private void lblClearFields_Click(object sender, EventArgs e)
        {
            txtusername.Clear();
            txtpassword.Clear();
            txtconfirmpass.Focus();
        }

        private void lblExit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

           
        }

        private void vhoosecertificate_Click(object sender, EventArgs e)
        {
            X509Store objStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            objStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection =
                                                        X509Certificate2UI.SelectFromCollection(objStore.Certificates,
                                                        "Zgjedh certifikaten per nenshkrim", "Zgjedh certifikaten",
                                                        X509SelectionFlag.SingleSelection);
            try
            {
                certificate = certCollection[0];
                if (certificate.HasPrivateKey)
                {
                    MessageBox.Show("Certifikata permbane cels privat!");

                    string strContent = "";
                    strContent += "Subject: " + certificate.Subject + "\n";
                    strContent += "Issuer: " + certificate.Issuer + "\n";
                    strContent += "Thumbprint: " + certificate.Thumbprint;
                    MessageBox.Show("Te dhenat e certifikates:\n\n" + strContent);
                }
            }
            catch
            {
                MessageBox.Show("Certifikata ne indeksin 0 nuk ekziston!");
            }
        }
    }
}