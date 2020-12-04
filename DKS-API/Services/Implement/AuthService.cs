using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using DFPS.API.Data.Interface;
using DFPS.API.DTOs;
using DFPS.API.Models;
using DFPS_API.Helpers;
using Microsoft.EntityFrameworkCore;

namespace DFPS_API.Services.Implement
{
    public class AuthService : IAuthService
    {
        private readonly IUserDAO _userDAO;
        private readonly IMapper _mapper;
        public AuthService(IUserDAO userDAO, IMapper mapper)
        {
            _userDAO = userDAO;
            _mapper = mapper;
        }

        public Task<bool> Add(User model)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Delete(object id)
        {
            throw new System.NotImplementedException();
        }

        public async Task<List<User>> GetAllAsync()
        {
            return await _userDAO.GetAll().ToListAsync();
        }

        public User GetById(object id)
        {
           return _userDAO.FindById(id);
        }

        public Task<PagedList<User>> GetWithPaginations(PaginationParams param)
        {
            throw new System.NotImplementedException();
        }

        public Task<PagedList<User>> Search(PaginationParams param, object text)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> Update(User model)
        {
            _userDAO.Update(model);
            return  _userDAO.SaveAll();
        }
    }
}