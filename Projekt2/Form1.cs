using MySql.Data.MySqlClient;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;

namespace Projekt2
{
    public partial class Form1 : Form
    {
        String connectionstring = System.Configuration.ConfigurationSettings.AppSettings["connstring"];
        private MySqlConnection connection;
        private RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
        X509Certificate2 certificate;
        private USER user = null;
        private Socket derguesi;
        private DESCryptoServiceProvider des = null;
       
        public Form1()
        {
            InitializeComponent();
        }

       
       

        public string Get_form1text()
        {
            return txtuser.Text;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
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
                byte[] desKey = rsa.Encrypt(des.Key, true);
                // Console.WriteLine("iv length="+iv.Length+" iv value="+Convert.ToBase64String(des.IV)+" deskey  length="+desKey.Length+"  des encrytpted="+Convert.ToBase64String(desKey)+" des decrypted="+Convert.ToBase64String(des.Key));

                des.Mode = CipherMode.CBC;
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

        private void lblExit_Click_1(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            new Form2().Show(); // per regjistrim shfaqet Register
            this.Hide();
        }

        private void loginbutton_Click_1(object sender, EventArgs e)
        {
            if (txtuser.Text.Length != 0 && txtPass.Text.Length != 0)
            {
                try
                {
                    IPHostEntry ipHost = Dns.GetHostEntry("localhost");
                    IPAddress ipAddr = ipHost.AddressList[0];
                    IPEndPoint localEndPoint = new IPEndPoint(ipAddr, 11100);
                    derguesi = new Socket(ipAddr.AddressFamily,
                               SocketType.Stream, ProtocolType.Tcp);

                    try
                    {
                        derguesi.Connect(localEndPoint);
                        Console.WriteLine("Socket connected to -> {0} ",
                                     derguesi.RemoteEndPoint.ToString());

                        // Creation of messagge that
                        // we will send to Server
                        String user = txtuser.Text;
                        String pass = txtPass.Text;

                        sendMessage("Login*" + user + "*" + pass);

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

                    new Fatura().Show();
                    this.Hide();

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
            }
            else
            {
                MessageBox.Show("Pleas fill out the form");
            }
        }

        private void lblClearFields_Click(object sender, EventArgs e)
        {
            txtuser.Clear();
            txtPass.Clear();
            txtuser.Focus();
        }

        private void label2_Click(object sender, EventArgs e)
        {
            X509Store objStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            objStore.Open(OpenFlags.ReadOnly);
            X509Certificate2Collection certCollection =
                                                        X509Certificate2UI.SelectFromCollection(objStore.Certificates,
                                                        "Zgjedh certifikaten per nenshkrim", "Zgjedh certifikaten",
                                                        X509SelectionFlag.SingleSelection);
            try
            {
                certificate= certCollection[0];
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