using Enigma.Common.Entities;
using EnigmaApi.Infrastructure.SQL;
using EnigmaApi.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace EnigmaApi.Repositories
{
    public class RotorRepository : IRotorRepository
    {
        private readonly ApplicationDbContext _DbContext;

        //class injection of applicationdb into rotorrepop
        public RotorRepository(ApplicationDbContext dbContext)
        {
            _DbContext = dbContext;
        }

        public async Task<IEnumerable<RotorEntity>> GetAllAsync() => await _DbContext.Rotors.ToListAsync();

        public async Task<RotorEntity> GetByIdAsync(int RotorId) => await _DbContext.Rotors.FirstOrDefaultAsync(r => r.RotorId == RotorId);

        public async Task UpdateRotor(RotorEntity rotor)
        {
            _DbContext.Rotors.Update(rotor);

            await _DbContext.SaveChangesAsync();
        }
    }
}