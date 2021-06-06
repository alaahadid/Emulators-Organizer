using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AHD.EO.Base
{
    public struct SeparateItem
    {
        private string fileName;
        private string data;

        public string FileName { get { return fileName; } set { fileName = value; } }
        public string Data { get { return data; } set { data = value; } }
    }
}
