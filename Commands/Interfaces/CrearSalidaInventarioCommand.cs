using glissvinyls_plus.Context;
using glissvinyls_plus.Models;

namespace glissvinyls_plus.Commands.Interfaces
{
    public class CrearSalidaInventarioCommand : ICommand
    {
        private readonly AppDbContext _context;
        private readonly InventoryExit _inventoryExit;

        public CrearSalidaInventarioCommand(AppDbContext context, InventoryExit inventoryExit)
        {
            _context = context;
            _inventoryExit = inventoryExit;
        }

        public async Task ExecuteAsync()
        {
            _context.InventoryExits.Add(_inventoryExit);
            await _context.SaveChangesAsync();
        }
    }
}
