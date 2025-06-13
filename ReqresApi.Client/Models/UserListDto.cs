using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqresApi.Client.Models
{
    public class UserListDto
    {
        public int Page { get; set; }
        public int TotalPages { get; set; }
        public List<UserDto> Data { get; set; } = new();
    }
}
