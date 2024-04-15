using ServiceCenterLibrary.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceCenterLibrary.Services
{
	public interface IDataService<T>
	{
        Task<IEnumerable<T>?> GetAllAsync();
		Task<T?> GetByIdAsync(int id) => null!;
		Task<T?> CreateAsync(T obj);
        Task<T?> UpdateAsync(T obj);
        Task<T?> DeleteAsync(int id);
	}
}
