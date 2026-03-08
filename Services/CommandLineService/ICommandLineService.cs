using SalesDW.API.Models.ProductioDB;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SalesDW.API.Services.CommandLineService;

public interface ICommandLineService
{
    Task<IEnumerable<CommandLineDto>> GetAllAsync();
    Task<CommandLineDto?> GetByIdAsync(int id);
    Task<IEnumerable<CommandLineDto>> GetByCommandIdAsync(int commandId);
    Task<CommandLine> CreateAsync(CommandLine line);
    Task<CommandLine?> UpdateAsync(int id, CommandLine line);
    Task<bool> DeleteAsync(int id);
}
