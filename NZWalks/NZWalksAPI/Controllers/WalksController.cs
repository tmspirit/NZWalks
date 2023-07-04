using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class WalksController : ControllerBase
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;
        private readonly IRegionRepository regionRepository;

        public WalksController(IWalkRepository walkRepository, IMapper mapper, IRegionRepository regionRepository)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
            this.regionRepository = regionRepository;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllWalks()
        {
            //Fetch data from database
            var walksDomain = await walkRepository.GetAllAsync();
            //Convert domain walks to dto walks
            var walksDTO= mapper.Map<List<Models.DTO.Walk>>(walksDomain);

            //return response
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]//asi le desimos que solo puede recibir guid values
        [ActionName("GetWalkAsync")] //Para poder llamarlo en el CreatedAtAction
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            var walk = await walkRepository.GetAsync(id);

            if (walk == null)
            {
                return NotFound();//Devuelve un 404 NOT FOUND
            }
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            return Ok(walkDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Validate the incoming request
            if (!await ValidateAddWalkAsync(addWalkRequest))
            {
                return BadRequest(ModelState);
            }

            var walkDomain = mapper.Map<Models.Domain.Walk>(addWalkRequest);

            walkDomain = await walkRepository.AddAsync(walkDomain);

            var walkDTO= mapper.Map<Models.DTO.Walk>(walkDomain);

            return CreatedAtAction(nameof(GetWalkAsync), new {id= walkDTO.Id}, walkDTO);

        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Convert DTO to domain object
            var walkDomain= mapper.Map<Models.Domain.Walk>(updateWalkRequest);

            //Pass details to Repository
            walkDomain=await walkRepository.UpdateAsync(id, walkDomain);

            //handle Null
            if (walkDomain==null)
            {
                return NotFound("This id was not found");
            }

            //Convert back Domain to DTO
            var walkDTO= mapper.Map<Models.DTO.UpdateWalkRequest>(walkDomain);

            //return response

            return Ok(walkDTO);



        }
        
        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync(Guid id)
        {
            var walkDomain=walkRepository.DeleteAsync(id);
            if (walkDomain==null)
            {
                return NotFound();
            }

            var walkDTO= mapper.Map<Models.DTO.Walk>(walkDomain);

            return Ok(walkDTO);
        }

        #region Private Methods
        private async Task<bool> ValidateAddWalkAsync(Models.DTO.AddWalkRequest addWalkRequest)
        {
            var region = await regionRepository.GetAsync(addWalkRequest.RegionId);
            if (region == null)
            {
                ModelState.AddModelError(nameof(addWalkRequest.RegionId), 
                    $"{nameof(addWalkRequest.RegionId)} no es valido");
            }

            if (ModelState.ErrorCount > 0)
            {
                return false;
            }
            return true;
        }
        #endregion
    }
}
