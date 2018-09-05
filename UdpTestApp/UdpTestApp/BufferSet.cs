using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpTestApp
{
    class BufferSet
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

        enum NUMERIC_TYPE
        {
            INT,
            UINT,
            SHORT,
            USHORT,
            FLOAT,
            DOUBLE
        };

        public ObservableCollection<Data> ViewDataList { get; } = new ObservableCollection<Data>();
            //= new ObservableCollection<Data>();


        // Csvファイルから読んだICDを送信用データバッファとしてセットする
        public void SetData(string filePath)
        {

            GetDataFromCsv csvData = new GetDataFromCsv();
            csvData.SetCsvStreamData(filePath);

            var DataList = csvData.DataList;

            int counter = 0;

            // string[] Dataの並びは以下とする
            // [0]：番号，[1]：オフセット，[2]：データ名，[3]：データ型，[4]：値，[5]：単位，[6]：配列数
            foreach (string[] Data in DataList)
            {
                try
                {
                    int nArraySize = 1;
                    int.TryParse(Data[6], out nArraySize);

                    SetDataForEachType(Data[3], Data[4], nArraySize);

                    ViewDataList.Add(SetViewData(Data, ++counter));
                }
                catch (System.Exception e)
                {
                    System.Console.WriteLine(e.Message);
                    break;
                }
            }
        }


        public Data SetViewData(string[] Data, int counter)
        {
            Data viewData = new Data();

            viewData._IsActive = true;
            viewData._Number = counter++;
            viewData._Name  = Data[2];
            viewData._Type  = Data[3];
            viewData._Value = Data[4];

            return viewData;
        }

        // 数値（int, float, double）のデータをバッファに追加する
        public void AddNumData(dynamic numData, int nStructSize)
        {
            byte[] tempBuff = BitConverter.GetBytes(numData);
            tempBuff.CopyTo(buffer, byteOffset);

            var byteLength = tempBuff.Length;
            byteOffset += byteLength;
        }

        // 文字列（char）のデータをバッファに追加する
        // 文字列サイズによらずnStructSize分の領域を取る。
        public void AddCharData(string charData, int nStructSize)
        {
            var tempBuff = System.Text.Encoding.GetEncoding("shift-jis").GetBytes(charData);
            tempBuff.CopyTo(buffer, byteOffset);

            byteOffset += nStructSize;
        }


        // データタイプごとに判別して，変換を行い，データバッファにセットする。
        private void SetDataForEachType(string dataTypeString, string dataNum, int nArraySize)
        {
            foreach (NUMERIC_TYPE typeValue in Enum.GetValues(typeof(NUMERIC_TYPE)))
            {
                if (dataTypeString == Enum.GetName(typeof(NUMERIC_TYPE), typeValue))
                {
                    Type dataType;

                    switch (typeValue)
                    {
                        case NUMERIC_TYPE.INT:
                            dataType = typeof(int);
                            break;

                        case NUMERIC_TYPE.UINT:
                            dataType = typeof(uint);
                            break;

                        case NUMERIC_TYPE.SHORT:
                            dataType = typeof(short);
                            break;

                        case NUMERIC_TYPE.USHORT:
                            dataType = typeof(ushort);
                            break;

                        case NUMERIC_TYPE.FLOAT:
                            dataType = typeof(float);
                            break;

                        case NUMERIC_TYPE.DOUBLE:
                            dataType = typeof(double);
                            break;

                        default:
                            dataType = typeof(int);
                            break;
                    }

                    AddNumData(Convert.ChangeType(dataNum, dataType), nArraySize);
                    return;
                }
            }

            // NUMERIC_TYPEで定義した数値型以外はすべて文字列とする
            AddCharData(dataNum, nArraySize);   
        }

    }


    // csvの中身を1行ずつすべて文字列としてList<string[]>に取得する
    // カンマ区切りはstringの配列になる
    class GetDataFromCsv
    {
        private List<string[]> StringDataList = new List<string[]>();
        public List<string[]> DataList
        {
            get { return StringDataList; }
        }

        public void SetCsvStreamData(string fileName)
        {
            try
            {
                using (System.IO.StreamReader streamReader = new System.IO.StreamReader(fileName, System.Text.Encoding.GetEncoding("shift-jis")))
                {
                    ReadStreamAsCsv(streamReader);
                }
            }
            catch (System.Exception e)
            {
                System.Console.WriteLine(e.Message);
            }
        }

        public void ReadStreamAsCsv(System.IO.StreamReader streamReader)
        {
            while (!streamReader.EndOfStream)
            {
                var oneLineString = streamReader.ReadLine();
                if (oneLineString[0] == '#') continue;

                var lineDataList = oneLineString.Split(',');

                StringDataList.Add(lineDataList);
            }
        }
    }
}