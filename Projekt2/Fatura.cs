using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates;

namespace Projekt2
{
    public partial class Fatura : Form
    {
        String connectionstring = System.Configuration.ConfigurationSettings.AppSettings["connstring"];
        private MySqlConnection connection;
        private RSACryptoServiceProvider rsa;
        private X509Certificate2 certificate;
        private USER user = null;
        private Socket derguesi;

        private DESCryptoServiceProvider des = null;
        public Fatura()
        {
            InitializeComponent();
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void txttvsh_TextChanged(object sender, EventArgs e)
        {
           
        }

        private void Fatura_Load(object sender, EventArgs e)
        {
            txtmuaji.Items.Add("Janar");
            txtmuaji.Items.Add("Shkurt");
            txtmuaji.Items.Add("Mars");
            txtmuaji.Items.Add("Prill");
            txtmuaji.Items.Add("Maj");
            txtmuaji.Items.Add("Qershor");
            txtmuaji.Items.Add("Korrik");
            txtmuaji.Items.Add("Gusht");
            txtmuaji.Items.Add("Shtator");
            txtmuaji.Items.Add("Tetor");
            txtmuaji.Items.Add("Nentor");
            txtmuaji.Items.Add("Dhjetor");

            txtuser.Text = finduserid();

        }

        private void savebtn_Click(object sender, EventArgs e)
        {
            if (txtuser.Text.Length != 0 && txtlloji.Text.Length != 0 && txtviti.Text.Length != 0 && txtmuaji.Text.Length != 0 && txtqmimi.Text.Length != 0)
            {
                double tvsh = (Convert.ToDouble(txtqmimi.Text) * 18) / 100;
                double qmimimetvsh = Convert.ToDouble(txtqmimi.Text) + tvsh;
                txttvsh.Text = qmimimetvsh.ToString();
               
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
                        String user = finduserid();
                        String lloji = txtlloji.Text;
                        String viti = txtviti.Text;
                        String muaji = txtmuaji.Text;
                        String qmimi = txtqmimi.Text;


                        sendMessage("Fatura*" + user +"*"+ lloji +"*"+ viti +"*"+ muaji +"*"+ qmimi);

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
            else
            {
                MessageBox.Show("Pleas fill out the form");
            }

        }
        public string finduserid()
        {
            String iduser=null;
            Form1 fromf1 = new Form1();
            String user= fromf1.Get_form1text();
            try
            {
                connection = new MySqlConnection(connectionstring);
                connection.Open();
                MySqlCommand cmd = new MySqlCommand("Select * from users where Username= @Username", connection);
                cmd.Parameters.AddWithValue("@Username", user);
                MySqlDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    iduser = reader["Id"].ToString();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return iduser;
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

        private void txtlloji_TextChanged(object sender, EventArgs e)
        {

        }

        private void certificatelabel_Click(object sender, EventArgs e)
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
