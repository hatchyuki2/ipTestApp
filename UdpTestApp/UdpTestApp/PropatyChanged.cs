using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace UdpTestApp
{
    public class MyPropertyChanged : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected void RaisePropertyChanged([CallerMemberName]string propertyName = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

        /// <summary>
        /// 前と値が違うなら変更してイベントを発行する
        /// </summary>
        /// <typeparam name="TResult">プロパティの型</typeparam>
        /// <param name="source">元の値</param>
        /// <param name="value">新しい値</param>
        /// <param name="propertyName">プロパティ名/param>
        /// <returns>値の変更有無</returns>
        protected bool RaisePropertyChangedIfSet<TResult>(ref TResult source,
           TResult value, [CallerMemberName]string propertyName = null)
        {
            //値が同じだったら何もしない
            if (EqualityComparer<TResult>.Default.Equals(source, value))
                return false;

            source = value;
            //イベント発行
            RaisePropertyChanged(propertyName);

            return true;
        }
    }
}
