﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tongrenlu_Windows.Data
{
    public class TrackBean
    {
        public long id { get; set; }
        public long articleId { get; set; }
        public string checksum { get; set; }
        public string title { get; set; }
        public string artist { get; set; }
        public int rate { get; set; }

        public bool xfd { get; set; }
    }


    public class TrackResultModel
    {
        public List<TrackBean> playlist { get; set; }
    }
}
