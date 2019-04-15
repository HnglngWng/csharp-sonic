using System;
using System.Text.RegularExpressions;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace SimpleSonic
{

    public abstract class Channel
    {

        private Socket socket;

        private StreamReader streamReader;

        private StreamWriter streamWriter;

        private string password;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="address">Address of Sonic server</param>
        /// <param name="port">port Port of Sonic server</param>
        /// <param name="password">auth_password of Sonic server</param>
        /// <param name="connectionTimeout">Connection timeout in milliseconds</param>
        /// <param name="readTimeout">Read timeout in milliseconds</param>
        public Channel(string address, int port, string password,
                int connectionTimeout, int readTimeout)

        {
            this.password = password;
            this.socket = new Socket(AddressFamily.InterNetwork,
            SocketType.Stream, ProtocolType.Tcp);
            IPAddress ip = IPAddress.Parse(address);
            this.socket.Connect(ip, port);
            this.socket.ReceiveTimeout = readTimeout;
            NetworkStream networkStream = new NetworkStream(socket, true);
            this.streamReader = new StreamReader(networkStream);
            this.streamWriter = new StreamWriter(networkStream);

            this.AssertPrompt("^CONNECTED");
        }

        protected void Send(String command)
        {
            streamWriter.Write(command + "\r\n");
            streamWriter.Flush();
        }

        protected String ReadLine()
        {
            return this.streamReader.ReadLine();

        }

        protected void AssertOK()
        {
            this.AssertPrompt("^OK\r\n$");
        }

        protected int AssertResult()
        {
            string prompt = this.ReadLine();
            Regex reg = new Regex("^RESULT ([0-9]+)$");
            bool isMatch = reg.IsMatch(prompt);
            if (!isMatch)
            {
                throw new SonicException("unexpected prompt: " + prompt);
            }
            Match matche = reg.Match(prompt);
            string s = matche.Groups[0].Value;
            return int.Parse(s);
        }

        protected void AssertPrompt(string regexp)
        {
            string prompt = this.ReadLine();
            if (Regex.IsMatch(prompt, regexp))
            {
                throw new SonicException("unexpected prompt: " + prompt);
            }
        }

        public void Start(Mode mode)
        {
            this.Send(String.Format("START %s %s", mode.ToString(), this.password));
            this.AssertPrompt("^STARTED");
        }

        public void Ping()
        {
            this.Send("PING");
            this.AssertPrompt("^PONG\r\n$");
        }

        public void Quit()
        {
            this.Send("QUIT");
            this.AssertPrompt("^ENDED\r\n$");
            this.socket.Close();
        }
    }
}