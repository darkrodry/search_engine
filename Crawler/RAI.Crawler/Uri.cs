using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RAI.Crawler.Data
{
    [System.Diagnostics.DebuggerDisplay("Id={Id}, Uri={AbsoluteUri}, Parent={Parent}, Cache={Cache}, Status={Status}")]
    public partial class DataUri
    {
        public DataUri(string absoluteUri, Data.DataUri parent, string cache, bool? status):this()
        {
            this.AbsoluteUri = absoluteUri;
            this.ParentUri = parent;
            this.Cache = cache;
            this.Status = status;
        }
        public bool IsSeed
        {
            get { return this.ParentUri == null; }
        }
    }
}
