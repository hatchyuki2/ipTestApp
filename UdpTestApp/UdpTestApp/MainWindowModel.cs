using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpTestApp
{
    class MainWindowModel
    {
        public static List<Data> _Data { get; set; } = new List<Data>();
        public List<Data> ViewData
        {
            get { return _Data; }
            set
            {
                if (Equals(_Data, value)) return;
                _Data = value;
            }
        }


        public MainWindowModel()
        {
            for (int i = 0; i < 10; i++)
            {
                _Data.Add(new Data()
                {
                    _IsActive = true,
                    _Number = i + 1,
                    _Name   = "データ" + i,
                    _Type   = "int",
                    _Value  = (3 + i).ToString(),
                    _Vender = Vendor.取引先A,
                });
            }
        }
    }

    public class Data
    {
        public bool _IsActive { get; set; }
        public int _Number { get; set; }
        public string _Name { get; set; }
        public string _Type { get; set; }
        public string _Value { get; set; }
        public Vendor _Vender { get; set; }
    }

    public enum Vendor
    {
        取引先A,
        取引先B
    }

    unsafe struct Buffer
    {

    }

    // 送信バッファクラス
    public class SendBuffer
    {
        private int byteOffset;
        public int ByteOffset
        {
            get { return byteOffset; }
        }

        private byte[] buffer = new byte[1500];
        public byte[] ST_Data
        {
            get { return buffer; }
        }


        public void SetData()
        {
            //MainWindowModel model = new MainWindowModel();
            //List<Data> a = model.ViewData;


            int b = 1;
            float c = (float)1.0;

            char[] d = new char[10];
            d = "aaaaaaaa".ToCharArray();

            AddNumData(b, 0);
            AddNumData(c, 0);
            AddCharData(d, 10);
            AddCharData("abcde".ToCharArray(), 5);
        }

        // 数値（int, float, double用）
        public void AddNumData(dynamic numData, int nStructSize) 
        {
            byte[] tempBuff = BitConverter.GetBytes(numData);
            tempBuff.CopyTo(buffer, byteOffset);

            var byteLength = tempBuff.Length;
            byteOffset += byteLength;
        }


        public void AddCharData(char[] charData, int nStructSize)
        {
            var tempBuff = System.Text.Encoding.GetEncoding("shift-jis").GetBytes(charData);
            tempBuff.CopyTo(buffer, byteOffset);

            byteOffset += nStructSize;
        }
    }


    class UdpModel
    {
        static UDP udp = new UDP();

        private bool _Send;
        public bool Send
        {
            get { return _Send; }
            set
            {
                if (Equals(_Send, value)) return;
                _Send = value;

                if (_Send == true)
                {
                    SendBuffer sendBuffer = new SendBuffer();
                    sendBuffer.SetData();

                    udp.SendPortInit(MyIP, SendPort, SendPort);
                    udp.StartSend(sendBuffer.ST_Data, sendBuffer.ByteOffset);
                }
                else
                {
                    udp.IsStopSend = true;
                }
            }
        }

        private string _MyIP = "172.0.0.1";
        public string MyIP
        {
            get { return _MyIP; }
            set
            {
                if (Equals(_MyIP, value)) return;
                _MyIP = value;
            }
        }

        private int _SendPort = 9006;
        public int SendPort
        {
            get { return _SendPort; }
            set
            {
                if (Equals(_SendPort, value)) return;
                _SendPort = value;
            }
        }

        private int _RecvPort = 9007;
        public int RecvPort
        {
            get { return _RecvPort; }
            set
            {
                if (Equals(_RecvPort, value)) return;
                _RecvPort = value;
            }
        }
    }
}