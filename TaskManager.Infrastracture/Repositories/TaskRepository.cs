using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using TaskManager.Core.Interfaces;
using TaskManager.Domain.Entities;
using TaskManager.Infrastracture.Data;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace TaskManager.Infrastracture.Repositories;
public class TaskRepository : IRepository<TaskItem> 
{
    private readonly TaskManagerDbContext _context;
    private readonly DbSet<TaskItem> _dbSet;

    public TaskRepository(TaskManagerDbContext context)
    {
        _context = context;
        _dbSet = context.Tasks;
    }

    public async Task AddAsync(TaskItem item, CancellationToken cancellationToken)
    {
        await _dbSet.AddAsync(item, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task Delete(string id, CancellationToken cancellationToken)
    {
        TaskItem? taskItem = await _dbSet
            .Where(u => u.Id.ToString() == id)
            .SingleOrDefaultAsync(cancellationToken);

        if(taskItem != null) 
        {
            _dbSet.Remove(taskItem);
        }
        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<IEnumerable<TaskItem>> GetAllAsync(CancellationToken cancellationToken)
    {
        var tasks = await _dbSet.ToArrayAsync(cancellationToken);
        return tasks;
    }

    public async Task<TaskItem?> GetByIdAsync(string id, CancellationToken cancellationToken)
    {
        TaskItem? taskItem = await _dbSet
             .AsNoTracking()
             .Where(u => u.Id.ToString() == id)
             .Select(todoItem => new TaskItem
             {
                 Id = todoItem.Id,
                 UserId = todoItem.UserId,
                 Description = todoItem.Description,
                 DueDate = todoItem.DueDate,
                 Labels = todoItem.Labels,
                 IsCompleted = todoItem.IsCompleted,
                 CreatedAt = todoItem.CreatedAt,
                 CompletedAt = todoItem.CompletedAt
             })
             .SingleOrDefaultAsync(cancellationToken);

        return taskItem;
    }

    public async Task<bool> UpdateAsync(TaskItem item, CancellationToken cancellationToken)
    {
        var task = await _dbSet
            .SingleOrDefaultAsync(u => u.Id == item.Id, cancellationToken);

        if(task is null)
            return false;

        task.IsCompleted = item.IsCompleted; // тепер оновлюється завжди

        if(item.Description != null)
            task.Description = item.Description;

        if(item.DueDate != null)
            task.DueDate = item.DueDate;

        if(item.Labels != null)
            task.Labels = item.Labels;

        if(item.CreatedAt != null)
            task.CreatedAt = item.CreatedAt;

        if(item.CompletedAt != null)
            task.CompletedAt = item.CompletedAt;

        if(item.Priority != null)
            task.Priority = item.Priority;

        var rowsAffected = await _context.SaveChangesAsync(cancellationToken);
        return rowsAffected > 0;
    }
}
