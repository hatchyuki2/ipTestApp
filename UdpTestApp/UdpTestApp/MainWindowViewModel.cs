using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace UdpTestApp
{
    namespace MainWindowViewModel
    {
        class ViewModel : ViewModelBase
        {
            public DataListViewModel _DataListViewModel { get; } = new DataListViewModel();

            public UdpViewModel _UdpViewModel { get; } = new UdpViewModel();

            public BufferSet _BufferSet = new BufferSet();

            public ViewModel()
            {
                UpdateCommand = CreateCommand(param => MyUpdateCommand());
                SendCommand = CreateCommand(param => MySendCommand(param));
            }

            public ICommand UpdateCommand { get; private set; }
            public ICommand SendCommand { get; private set; }

            private string _FilePath;
            public string FilePath
            {
                get { return _FilePath; }
                set
                {
                    if (_FilePath != value)
                    {
                        _FilePath = value;
                        RaisePropertyChanged(nameof(FilePath));
                    }
                }
            }

            // ファイルを読み込みリストの表示と送信バッファを更新する
            public void MyUpdateCommand()
            {
                Data data = new Data();

                var filePath = OpenFile();

                FilePath = filePath;

                _BufferSet.SetData(filePath);

                UpdateListView(_BufferSet.ViewDataList);
                UpdateSendBuff(_BufferSet.ST_Data, _BufferSet.ByteOffset);
            }


            private void UpdateListView(ObservableCollection<Data> ViewDataList)
            {
                _DataListViewModel.ViewData = ViewDataList;
            }

            private void UpdateSendBuff(byte[] Buff, int BuffSize)
            {
                _UdpViewModel.SendData = Buff;
                _UdpViewModel.ByteOffset = BuffSize;
            }


            public void MySendCommand(object param)
            {
                UDP udp = new UDP();

                bool IsSend = (bool)param;

                if (IsSend == true)
                {
                    udp.SendPortInit(_UdpViewModel.MyIP, _UdpViewModel.SendPort, _UdpViewModel.SendPort);
                    udp.StartSend(_BufferSet.ST_Data, _BufferSet.ByteOffset);
                }
                else
                {
                    udp.IsStopSend = true;
                }
            }


            private string OpenFile()
            {
                // ダイアログのインスタンスを生成
                var dialog = new OpenFileDialog
                {
                    ReadOnlyChecked = true,

                    // ファイルの種類を設定
                    Filter = "テキストファイル (*.csv)|*.csv|全てのファイル (*.*)|*.*"
                };

                // ダイアログを表示する
                if (dialog.ShowDialog() == true)
                {
                    // 選択されたファイル名 (ファイルパス) をメッセージボックスに表示
                    return dialog.FileName;
                }

                return "File  not found.";
            }
        }
    }
}