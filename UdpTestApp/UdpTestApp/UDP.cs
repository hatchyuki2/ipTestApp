using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net; // for UDP 
using System.Net.Sockets; // for UDP 
using System.Threading; // for Interlocked 

namespace UdpTestApp
{
    public class UDP
    {
        private UdpClient udpForSend;              // Client for sending 
        private string remoteHost = " localhost "; // Destination IP address 
        private int remotePort;                    // destination port 
        private UdpClient udpForReceive;           // Client for receiving 
        public string rcvMsg = " ini ";            // for storing received messages 
        private System.Threading.Thread rcvThread; // Receiving thread 

        private bool _IsStopSend;
        public bool IsStopSend
        {
            set { _IsStopSend = value; }
        }

        private bool _IsStopRecv;
        public bool IsStopRecv
        {
            set { _IsStopRecv = value; }
        }


        public UDP()
        {
        }

        // UDP setting (Receiving thread is generated while opening / 
        public bool SendPortInit(string ip_snd, int port_snd, int port_to)
        {
            try
            {
                udpForSend = new UdpClient(port_snd); // port for sending 

                remoteHost = ip_snd;
                remotePort = port_to; // Destination port 
                return true;
            }
            catch
            {
                return false;
            }
        }

        // UDP setting (Receiving thread is generated while opening / 
        public bool RecvPortInit(int port_rcv)
        {
            try
            {
                udpForReceive = new UdpClient(port_rcv); // reception port 
                return true;
            }
            catch
            {
                return false;
            }
        }

        // Send a string from the sending port to the destination port 
        public void Send(byte[] sendBytes, int bufferSize)
        {
            try
            {
                while (!_IsStopSend)
                {
                    udpForSend.Send(sendBytes, bufferSize, remoteHost, remotePort);
                }
            }
            catch { }
        }

        public void Receive() // a function executed in the receive thread 
        {
            int recvCounter = 0;

            IPEndPoint remoteEP = null; // Receive data from any source 
            while (true)
            {
                try
                {
                    byte[] rcvBytes = udpForReceive.Receive(ref remoteEP);
                    Interlocked.Exchange(ref rcvMsg, Encoding.ASCII.GetString(rcvBytes));
                }
                catch { }

                if (sizeof(byte) > 0)
                {
                    recvCounter++;
                }
            }
        }


        public async void Start_receive() // start receiving thread 
        {
            try
            {
                await Task.Run(() => this.Receive());
            }
            catch { }
        }


        public void Stop_receive() // stop receiving thread 
        {
            try
            {
                _IsStopRecv = false;
                rcvThread.Interrupt();
            }
            catch { }
        }


        public async void StartSend(byte[] sendBytes, int bufferSize) // start receiving thread 
        {
            try
            {
                _IsStopSend = false;
                await Task.Run(() => this.Send(sendBytes, bufferSize));
            }
            catch { }
        }


        public void End() // Closing the sending / receiving port and discontinue the receiving thread 
        {
            try
            {
                udpForReceive.Close();
                udpForSend.Close();
                rcvThread.Abort();
            }
            catch { }
        }
    }
}
