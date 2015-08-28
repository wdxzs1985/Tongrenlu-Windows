using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tongrenlu_Windows.Data
{

    public class PageSupport
    {
        public int pageNumber { get; set; }
        public int pageSize { get; set; }
        public int pageCount { get; set; }
        public int itemCount { get; set; }
        public bool first { get; set; }
        public bool last { get; set; }
    }
}
