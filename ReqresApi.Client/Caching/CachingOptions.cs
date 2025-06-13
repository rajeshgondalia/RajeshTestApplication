using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqresApi.Client.Caching
{
    public class CachingOptions
    {
        public int UserCacheDurationSeconds { get; set; } = 300; // default 5 minutes
    }
}
