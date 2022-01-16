﻿using System;
using System.Linq;
using System.Threading.Tasks;
using DicomViewer3.Entities;
using DicomViewer3.Models;

namespace DicomViewer3.Repositories
{
    public interface IUserRepository
    {
        Task<User> GetById(long id);
        Task<User> GetByEmail(string email);
        Task Add(User user);
        Task<bool> ExistsByEmail(string email);
        Task<Page<User>> GetAllPaged(
            int pageNumber,
            int pageSize,
            Func<IQueryable<User>, IQueryable<User>> filter = null,
            Func<IQueryable<User>, IOrderedQueryable<User>> sort = null
        );
    }
}