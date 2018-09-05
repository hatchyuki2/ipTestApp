using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpTestApp
{
    class DataListViewModel : MyPropertyChanged
    {
        public ObservableCollection<Data> _Data = new ObservableCollection<Data>();
        public ObservableCollection<Data> ViewData
        {
            get { return _Data; }
            set
            {
                if (Equals(_Data, value)) return;
                // _Data = value;
                if (RaisePropertyChangedIfSet(ref _Data, value))
                    RaisePropertyChanged(nameof(FullName));
            }
        }

        public string FullName => $"Anders {ViewData}";

        public DataListViewModel()
        {
            for (int i = 0; i < 10; i++)
            {
                _Data.Add(new Data()
                {
                    _IsActive = true,
                    _Number   = i + 1,
                    _Offset   = i * 4,
                    _Name     = "データ" + i,
                    _Type     = "int",
                    _Value    = (3 + i).ToString(),
                });
            }
        }
    }

    public class Data
    {
        public bool _IsActive { get; set; }
        public int _Number { get; set; }
        public int _Offset { get; set; }
        public string _Name { get; set; }
        public string _Type { get; set; }
        public string _Value { get; set; }
    }

    public enum Vendor
    {
        取引先A,
        取引先B
    }


    class UdpViewModel : MyPropertyChanged
    {
        UDP udp = new UDP();

        private byte[] _SendData;
        public byte[] SendData
        {
            set { _SendData = value;}
        }

        private int _ByteOffset;
        public int ByteOffset
        {
            set { _ByteOffset = value; }
        }


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
                    udp.SendPortInit(MyIP, SendPort, SendPort);
                    udp.StartSend(_SendData, _ByteOffset);
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
                RaisePropertyChangedIfSet(ref _MyIP, value);
            }
        }

        private int _SendPort = 9006;
        public int SendPort
        {
            get { return _SendPort; }
            set
            {
                if (Equals(_SendPort, value)) return;
                RaisePropertyChangedIfSet(ref _SendPort, value);
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