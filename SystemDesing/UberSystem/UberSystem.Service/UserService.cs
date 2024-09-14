using System;
using UberSystem.Domain.Entities;
using UberSystem.Domain.Interfaces;
using UberSystem.Domain.Interfaces.Services;
namespace UberSystem.Service
{
	public class UserService : IUserService
	{
        private readonly IUnitOfWork _unitOfWork;
        public UserService(IUnitOfWork unitOfWork)
		{
            _unitOfWork = unitOfWork;
		}

        public async Task<int> Add(User user)
        {
            try
            {
                await _unitOfWork.BeginTransaction();
                var UserRepos = _unitOfWork.Repository<User>();
                var objUser = await UserRepos.GetAllAsync();
                if (objUser.Any(c => c.Email.Equals(user.Email)))
                    return 0;
                var rnd = new Random();
                user.Id = long.Parse(rnd.Next().ToString());
                user.Role = 0;
                await UserRepos.InsertAsync(user);
                await _unitOfWork.CommitTransaction();
                return 1;
            }
            catch
            {
                await _unitOfWork.RollbackTransaction();
                return -1;
            }
        }

        public Task CheckPasswordAsync(User user)
        {
            throw new NotImplementedException();
        }

        public Task<User> FindByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public async Task<List<User>> GetAll()
        {
            try
            {
                var userRepo = _unitOfWork.Repository<User>();
                var objUsers = await userRepo.GetAllAsync();
                return objUsers != null && objUsers.Count > 0 ? objUsers.ToList() : new List<User>();
            }
            catch
            {
                return null;
            }
        }

        public async Task<bool> Login(User user)
        {
            try{
                await _unitOfWork.BeginTransaction();
                var UserRepos = _unitOfWork.Repository<User>();
                var objUser = await UserRepos.FindAsync(user.Email);
                if (objUser == null)
                    return false;
                if (objUser.Password != user.Password)
                    return false; 
                else 
                    return true;
            }catch{
                return false;
            }
        }

        public Task Update(User user)
        {
            throw new NotImplementedException();
        }
    }
}

