using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tongrenlu_Windows.Data
{
    public interface IPageSupport
    {
        int pageNumber { get; set; }
        int pageSize { get; set; }
        int pageCount { get; set; }
        int itemCount { get; set; }
        bool first { get; set; }
        bool last { get; set; }
    }
}
