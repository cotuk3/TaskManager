using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Core.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Domain.Exceptions.DomainExceptions;
using TaskManager.Infrastracture.Data;
using TaskManager.Infrastracture.Identity;

namespace TaskManager.Infrastracture.Repositories;
public class UserRepository : IRepository<User>
{
    private readonly TaskManagerDbContext _context;
    private readonly DbSet<User> _dbSet;

    public UserRepository(TaskManagerDbContext context)
    {
        _context = context;
        _dbSet = context.Users;
    }
    public async Task AddAsync(User item, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(item, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(string id, CancellationToken cancellationToken)
    {
        User? user = await _dbSet
            .Where(u => u.Id == id.ToString())
            .SingleOrDefaultAsync(cancellationToken);

        if(user != null)
        {
            _dbSet.Remove(user);
        }
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<User>> GetAllAsync(CancellationToken cancellationToken)
    {
        var users = await _dbSet.ToArrayAsync(cancellationToken);
        return users;
    }

    public async Task<User?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        User? user = await _dbSet
            .Where(u => u.Id == id)
            .SingleOrDefaultAsync(cancellationToken);

        return user;
    }

    public async Task<bool> UpdateAsync(User item, CancellationToken cancellationToken)
    {
        User user = await _dbSet
            .Where(u => u.Id == item.Id)!
            .SingleOrDefaultAsync(cancellationToken)!;

        if(item.UserName != null) 
        {
            user.UserName = item.UserName;
        }

        if(item.PhoneNumber != null) 
        {
            user.PhoneNumber = item.PhoneNumber;
        }   

        if(item.Email != null) 
        {
            user.Email = item.Email;
        }

        int rowsAffacted = await _context.SaveChangesAsync(cancellationToken);

        return rowsAffacted > 0;
    }
}
