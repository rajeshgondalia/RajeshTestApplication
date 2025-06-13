using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqresApi.Client.Configuration
{
    public class CacheSettings
    {
        public int UserCacheDurationSeconds { get; set; } = 300;
        public int AllUsersCacheDurationSeconds { get; set; } = 600;
    }
}
