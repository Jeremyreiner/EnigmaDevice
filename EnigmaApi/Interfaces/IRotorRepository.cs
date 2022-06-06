using Enigma.Common.Entities;

namespace EnigmaApi.Interfaces
{
    public interface IRotorRepository
    { 

        Task<IEnumerable<RotorEntity>> GetAllAsync();

        Task<RotorEntity> GetByIdAsync(int id);

        Task UpdateRotor(RotorEntity rotor);

    }
}
