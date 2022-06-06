using EnigmaApi.Interfaces;
using EnigmaApi.Extensions;
using Microsoft.AspNetCore.Mvc;
using Enigma.Common.Entities;
using Enigma.Common.Models;

namespace EnigmaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RotorController : ControllerBase
    {
        private readonly IRotorRepository _RotorRepository;

        public RotorController(IRotorRepository rotorRepository)
        {
            _RotorRepository = rotorRepository;
        }

        [HttpGet]
        public async Task<IEnumerable<RotorEntity>> GetAllAsync()
        {
            return await _RotorRepository.GetAllAsync();
        }

        [HttpGet("{id}")]
        public async Task<RotorEntity> GetByIdAsync(int id) => await _RotorRepository.GetByIdAsync(id);


        [HttpPut]
        public async Task UpdateRotor(RotorModel rotor) => await _RotorRepository.UpdateRotor(rotor.ToEntity());
    }
}
