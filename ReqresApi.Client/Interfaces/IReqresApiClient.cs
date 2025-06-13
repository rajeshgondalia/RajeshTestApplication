using ReqresApi.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqresApi.Client.Interfaces
{
    public interface IReqresApiClient
    {
        Task<UserDto?> GetUserByIdAsync(int userId);
        Task<IEnumerable<UserDto>> GetAllUsersAsync();
    }
}
