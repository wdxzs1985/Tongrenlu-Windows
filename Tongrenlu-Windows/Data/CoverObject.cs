using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Tongrenlu_Windows.Data
{
    public interface CoverObject
    {
        string CoverPath
        {
            get;
        }

        string CoverUrl
        {
            get;
        }

        string CoverPlaceholder
        {
            get;
        }
        
    }
}
