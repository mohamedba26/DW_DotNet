using SalesDW.API.Models.ProductioDB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesDW.API.Services.CommandService;

public interface ICommandService
{
    Task<IEnumerable<CommandDto>> GetAllAsync();
    Task<CommandDto?> GetByIdAsync(int id);
    Task<Command> CreateAsync(Command cmd);
    Task<Command?> UpdateAsync(int id, Command cmd);
    Task<bool> DeleteAsync(int id);
}
