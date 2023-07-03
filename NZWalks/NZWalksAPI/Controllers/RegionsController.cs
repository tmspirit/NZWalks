using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NZWalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
    {
        private readonly IRegionRepository regionRepository;
        private readonly IMapper mapper;

        public RegionsController(IRegionRepository regionRepository, IMapper mapper)
        {
            this.regionRepository = regionRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRegionsAsync()
        {
           var regions = await regionRepository.GetAllAsync();

            //// return DTO regions
            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //        Id = region.Id,
            //        Code = region.Code,
            //        Name = region.Name,
            //        Area = region.Area,
            //        Lat = region.Lat,
            //        Long = region.Long,
            //        Population = region.Population,
            //    };
            //    regionsDTO.Add(regionDTO);
            //});

            var regionsDTO= mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]//asi le desimos que solo puede recibir guid values
        [ActionName("GetRegionAsync")] //Para poder llamarlo en el CreatedAtAction
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region=await regionRepository.GetAsync(id);

            if (region == null)
            {
                return NotFound();//Devuelve un 404 NOT FOUND
            }
            var regionDTO=mapper.Map<Models.DTO.Region>(region);

            return Ok(regionDTO);

        }

        [HttpPost]
        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {

            //VAlidamos el modelo del request
            if (!ValidateAddRegionAsync(addRegionRequest))
            {
                return BadRequest(ModelState);
            }

            //Request(DTO) to Domanin Model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Long = addRegionRequest.Long,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population
            };

            //Pass details to repository
            region= await regionRepository.AddAsync(region);

            //Convertir otra vez a DTO
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };

            //Asi creamos una respuesta 201 Creadted response,
            //primero pasamos el metodo para saber que hemos creado
            //como segundo parametro e lid de la llamada del metyodo que usamos
            //por ultimo la rerspuesta que queremos devolver, un objeto con toda la info añadida en nuestro caso
            return CreatedAtAction(nameof(GetRegionAsync),new {id=regionDTO.Id}, regionDTO);

        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteRegionAsync(Guid id)
        {
            // Get region from database
            var region = await regionRepository.DeleteRegion(id);
            // If null NotFound
            if (region == null)
            {
                return NotFound();
            }
            // Convert response back to DTO

            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Name = region.Name,
                Population = region.Population
            };
            // return OK response

            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateRegionAsync([FromRoute] Guid id,[FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            //Validate the incoming request
            if(ValidateUpdateRegionAsync(updateRegionRequest))
            {
                return BadRequest(ModelState);
            }

            // Convert DTO to Domain Model
            var region = mapper.Map<Models.Domain.Region>(updateRegionRequest);

            // Update Region using repository
            region = await regionRepository.UpdateAsync(id, region);
            // If  null then NotFound
            if (region == null) {
                return NotFound();
            }
            // Convert Doamin back to DTO
            var regionRequestDTO = mapper.Map<Models.DTO.UpdateRegionRequest>(region);
            //Return OK response
            return Ok(regionRequestDTO);
        }

        #region Private Methods

        private bool ValidateAddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            if (addRegionRequest==null)
            {
                ModelState.AddModelError(nameof(addRegionRequest), $"{nameof(addRegionRequest)} Los datos son necesarios");
                return false;
            }

            if (string.IsNullOrEmpty(addRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Code), $"{nameof(addRegionRequest.Code)} no puede ser vacio");
            }
            if (string.IsNullOrEmpty(addRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(addRegionRequest.Name), $"{nameof(addRegionRequest.Name)} no puede ser vacio");
            }
            if (addRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Area), $"{nameof(addRegionRequest.Area)} no puede ser menos o igual a 0");
            }
            if (addRegionRequest.Lat <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Lat), $"{nameof(addRegionRequest.Lat)} no puede ser menos o igual a 0");
            }
            if (addRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Long), $"{nameof(addRegionRequest.Long)} no puede ser menos o igual a 0");
            }
            if (addRegionRequest.Population <= 0)
            {
                ModelState.AddModelError(nameof(addRegionRequest.Population), $"{nameof(addRegionRequest.Population)} no puede ser menos a 0");
            }
            //Asi comprobamos que ha pasado correctamente todas las comprobaciones anteriores en vez de hacer el return en cada una
            if (ModelState.ErrorCount > 0)
            {
                return false; 
            }

            return true;
        }
        private bool ValidateUpdateRegionAsync(Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            if (updateRegionRequest==null)
            {
                ModelState.AddModelError(nameof(updateRegionRequest), $"{nameof(updateRegionRequest)} Los datos son necesarios");
                return false;
            }

            if (string.IsNullOrEmpty(updateRegionRequest.Code))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Code), $"{nameof(updateRegionRequest.Code)} no puede ser vacio");
            }
            if (string.IsNullOrEmpty(updateRegionRequest.Name))
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Name), $"{nameof(updateRegionRequest.Name)} no puede ser vacio");
            }
            if (updateRegionRequest.Area <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Area), $"{nameof(updateRegionRequest.Area)} no puede ser menos o igual a 0");
            }
            if (updateRegionRequest.Lat <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Lat), $"{nameof(updateRegionRequest.Lat)} no puede ser menos o igual a 0");
            }
            if (updateRegionRequest.Long <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Long), $"{nameof(updateRegionRequest.Long)} no puede ser menos o igual a 0");
            }
            if (updateRegionRequest.Population <= 0)
            {
                ModelState.AddModelError(nameof(updateRegionRequest.Population), $"{nameof(updateRegionRequest.Population)} no puede ser menos a 0");
            }
            //Asi comprobamos que ha pasado correctamente todas las comprobaciones anteriores en vez de hacer el return en cada una
            if (ModelState.ErrorCount > 0)
            {
                return false; 
            }

            return true;
        }
        #endregion
    }
}
