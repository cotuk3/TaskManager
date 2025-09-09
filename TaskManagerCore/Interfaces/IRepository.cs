using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManager.Core.Interfaces;
public interface IRepository<T> where T : class
{
    Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken); // отримання всіх об'єктів
    Task<T?> GetByIdAsync(string id, CancellationToken cancellationToken); // отримання одного об'єкта за id
    Task AddAsync(T item, CancellationToken cancellationToken); // створення об'єкта
    Task<bool> UpdateAsync(T item, CancellationToken cancellationToken); // оновлення об'єкта
    Task Delete(string id, CancellationToken cancellationToken); // видалення об'єкта за id
}
