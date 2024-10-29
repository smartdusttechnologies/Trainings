﻿using Microsoft.EntityFrameworkCore;
using MultiDatabase.Data;
using MultiDatabase.Models.Entities;
using MultiDatabase.Repository.Interface;

namespace MultiDatabase.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly Application2DbContext _context;

        public UserRepository(Application2DbContext context)
        {
            _context = context;
        }

        public async Task AddUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserById(int id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await GetUserById(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
        }
    }

}
