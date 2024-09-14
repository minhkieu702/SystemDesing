using System;
using UberSystem.Domain.Entities;

namespace UberSystem.Domain.Interfaces.Services
{
	public interface IUserService
	{
        Task<List<User>> GetAll();
        Task<User> FindByEmail(string  email);
        Task Update(User user);
        Task<int> Add(User user);
        Task<bool> Login(User user);
        Task CheckPasswordAsync(User user);
    }
}

