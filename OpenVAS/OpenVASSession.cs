using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace OpenVAS
{

    //This class is the class for OpenVAS in which the authorization operations are performed.
    //Bu sınıf OpenVAS için yetkilendirme ve okuma-yazma işlemlerinin gerçekleştirildiği sınıftır.

    public class OpenVASSession : IDisposable
    {
       
        private SslStream _stream;

        public string Username { get; set; }

        public string Password { get; set; }

        public IPAddress ServerIPAddress { get; set; }

        public int ServerPort { get; set; }

        public SslStream Stream
        {
            get
            {
                if (_stream == null || !_stream.IsAuthenticated)
                    GetStream();

                return _stream;
            }

            set
            {
                _stream = value;
            }

        }


       
        public OpenVASSession(string username, string password, string host, int port = 9390)
        {
            this.ServerIPAddress = IPAddress.Parse(host);
            this.ServerPort = port;
            if (this.Authenticate(username, password) == false)
            {
                Username = null;
                Password = null;
            }
        }   


       
        //Bu Fonksiyon ile, OpenVAS'a giriş işlemi gerçekleştirilir.
        //With this function, To Openvas is performed login operation.
        public bool Authenticate(string username, string password)
        {
            ASCIIEncoding enc = new ASCIIEncoding();

            if (_stream == null || !_stream.CanRead)
                this.GetStream();

            if (_stream == null || !_stream.CanRead)
                return false;

            this.Username = username;
            this.Password = password;

            XDocument authXML = new XDocument(
                                    new XElement("authenticate",
                                        new XElement("credentials",
                                            new XElement("use" +
                                            "rname", username),
                                            new XElement("password", password)
                                        )));

            this.Stream.Write(enc.GetBytes(authXML.ToString()));

            string response = ReadMessage(this.Stream);

            if (response.Count() == 0)
            {
                Console.WriteLine("Openvasmd is not running on Remote Server. Please, try to running this code: openvasmd --listen='your-remote-server-ip' --port='any-port' ");
                return false;
            }

            XDocument doc = XDocument.Parse(response);

            if (doc.Root.Attribute("status").Value != "200")
            {
                Console.WriteLine("Authentication Failed. Input valid username and password.");
                return false;
            }


            return true;
        }

        //Bu fonksiyon ile, OpenVAS Protokol (API) komutları (request) gönderilir ve cevabı (response) okunur.
        //With this function, OpenVAS Protokol (API) commands (request) is sent and response is read.
        public XDocument ExecuteCommand(XDocument doc, bool requiresAuthentication = false)
        {
            ASCIIEncoding enc = new ASCIIEncoding();

            if (requiresAuthentication)
            {
                if (this.Username == null || this.Password == null)
                    throw new Exception("Username or password null");

                this.Authenticate(this.Username, this.Password);
            }

            if (_stream == null || !_stream.CanRead)
                this.GetStream();

            string xml = doc.ToString();
            string response = string.Empty;
            try
            {
                this.Stream.Write(enc.GetBytes(xml), 0, xml.Length);

                response = ReadMessage(this.Stream);

                this.Stream = null;

                return XDocument.Parse(response);
            }
            catch (Exception ex)
            {
                this.Stream = null;
                throw ex;
            }
        }


        private void GetStream()
        {
            if (_stream == null || !_stream.CanRead)
            {
                TcpClient c;
                try
                {
                    c = new TcpClient(this.ServerIPAddress.ToString(), this.ServerPort);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Service isn't running on Server \n" + ex.Message);
                    return;
                    //throw;
                }

                SslStream s;
                try
                {
                    s = new SslStream(c.GetStream(), false, new RemoteCertificateValidationCallback (ValidateServerCertificate),
                        (sender, targetHost, localCertificates, remoteCertificate, acceptableIssuers) => null);

                    s.AuthenticateAsClient("OpenVAS", null, System.Security.Authentication.SslProtocols.Tls, false);

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Connection blocked due to Firewall. Error message is \" " + ex.Message + " \" ");
                    return;
                }

                _stream = s;
            }
        }

        private string ReadMessage(SslStream sslStream)
        {
            byte[] buffer = new byte[2048];
            StringBuilder messageData = new StringBuilder();
            int bytes = -1;
            do
            {
                bytes = sslStream.Read(buffer, 0, buffer.Length);

                Decoder decoder = Encoding.ASCII.GetDecoder();
                char[] chars = new char[decoder.GetCharCount(buffer, 0, bytes)];
                decoder.GetChars(buffer, 0, bytes, chars, 0);
                messageData.Append(chars);

                // Check for EOF.
                if (bytes < buffer.Length)
                {
                    bytes = 0;
                    return messageData.ToString();
                }

                buffer = new byte[2048]; //clear cruft
            } while (bytes != 0);

            return messageData.ToString();
        }

        private static bool ValidateServerCertificate(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        }

        public void Dispose()
        {
            if (_stream != null)
            {
                _stream.Flush();
                _stream = null;
            }
        }

        /*
         * Servisin çalışıp çalışmadığını test eder.
         * 
         */
        public  void TestStream()
        {
           
                TcpClient c;
                try
                {
                    c = new TcpClient(this.ServerIPAddress.ToString(), this.ServerPort);

                if (c.Connected)
                    Console.WriteLine("***Connection was established.***");
                else
                    Console.WriteLine("***Connection was not established.***");
                

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Service isn't running on Server \n" + ex.Message);
                    return;
                    //throw;
                }

                //SslStream s;
                //try
                //{
                //    s = new SslStream(c.GetStream(), false, new RemoteCertificateValidationCallback(ValidateServerCertificate),
                //        (sender, targetHost, localCertificates, remoteCertificate, acceptableIssuers) => null);

                //    s.AuthenticateAsClient("OpenVAS", null, System.Security.Authentication.SslProtocols.Tls, false);

                //}
                //catch (Exception ex)
                //{
                //    Console.WriteLine("Connection blocked due to Firewall. Error message is \" " + ex.Message + " \" ");
                //    return;
                //}

                //_stream = s;
            
        }
    }
}
