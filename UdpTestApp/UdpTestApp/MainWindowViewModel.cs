using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UdpTestApp
{
    namespace MainWindowViewModel
    {
        class ViewModel
        {
            public MainWindowModel _MainWindowModel { get; } = new MainWindowModel();

            public UdpModel _UdpModel { get; } = new UdpModel();
        }


    }


}