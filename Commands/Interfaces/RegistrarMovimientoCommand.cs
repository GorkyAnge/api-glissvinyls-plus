using glissvinyls_plus.Context;
using glissvinyls_plus.Models;

namespace glissvinyls_plus.Commands.Interfaces
{
    public class RegistrarMovimientoCommand : ICommand
    {
        private readonly AppDbContext _context;
        private readonly MovementHistory _movement;

        public RegistrarMovimientoCommand(AppDbContext context, MovementHistory movement)
        {
            _context = context;
            _movement = movement;
        }

        public async Task ExecuteAsync()
        {
            _context.MovementHistories.Add(_movement);
            await _context.SaveChangesAsync();
        }
    }
}
