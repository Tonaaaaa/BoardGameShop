using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BoardGameShop.Api.Models.User;

namespace BoardGameShop.Api.Services
{
    public interface IUserService
    {
        Task<UserDTO> RegisterAsync(CreateUserDTO request);
        Task<string> LoginAsync(string username, string password);
        Task<UserDTO> GetByUsernameAsync(string username);
    }
}