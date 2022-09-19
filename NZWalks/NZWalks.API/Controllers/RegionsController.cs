using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System.Xml.Schema;
using Region = NZWalks.API.Models.DTO.Region;

namespace NZWalks.API.Controllers
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
        public async Task<IActionResult> GetAllRegions()
        {

            var regions = await regionRepository.GetAllAsync();

            ////Return DTO Regions

            //var regionsDTO = new List<Models.DTO.Region>();
            //regions.ToList().ForEach(region =>
            //{
            //    var regionDTO = new Models.DTO.Region()
            //    {
            //       Id= region.Id,
            //       Code = region.Code,
            //       Name = region.Name,
            //       Area = region.Area,
            //       Lat = region.Lat,
            //       Longitude = region.Longitude,
            //       Population = region.Population,

            //    };
            //    regionsDTO.Add(regionDTO);
            //});
            var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

            return Ok(regionsDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetRegionAsync")]
        public async Task<IActionResult> GetRegionAsync(Guid id)
        {
            var region = await regionRepository.GetAsync(id);
            if (region == null)
            {
                return NotFound();
            }

            var regionDTO = mapper.Map<Models.DTO.Region>(region);
            return Ok(regionDTO);
        }

        [HttpPost]

        public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
        {
            //Request(DTO) to Domain Model
            var region = new Models.Domain.Region()
            {
                Code = addRegionRequest.Code,
                Area = addRegionRequest.Area,
                Lat = addRegionRequest.Lat,
                Longitude = addRegionRequest.Longitude,
                Name = addRegionRequest.Name,
                Population = addRegionRequest.Population

            };

            //Pass details to Repository

            region = await regionRepository.AddAsync(region);

            //Convert back to Repository

            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Longitude = region.Longitude,
                Name = region.Name,
                Population = region.Population
            };

            return CreatedAtAction(nameof(GetRegionAsync), new { id = regionDTO.Id }, regionDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task <IActionResult> DeleteRegionAsync(Guid id)
        {
            //Get the region from Database

          var region = await regionRepository.DeleteAsync(id);
            
            //if not found
            if (region == null)
            {
                return NoContent();
            }

            //if got the data then convert it to DTO
            var regionDTO = new Region()
            {

                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Longitude = region.Longitude,
                Name = region.Name,
                Population = region.Population

            };

            return Ok(regionDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task <IActionResult>UpdateRegionAsync([FromRoute]Guid id,[FromBody] Models.DTO.UpdateRegionRequest updateRegionRequest)
        {
            //Convert DTO to Domain Model
            var region = new Models.Domain.Region()
            {
                Code = updateRegionRequest.Code,
                Area = updateRegionRequest.Area,
                Lat = updateRegionRequest.Lat,
                Longitude = updateRegionRequest.Longitude,
                Name = updateRegionRequest.Name,
                Population = updateRegionRequest.Population


            };

            //Update Region using Repository

          region = await regionRepository.UpdateAsync(id, region);
            
            //if data was not found
            if (region == null)
            {
                return NoContent();
            }

            //Convert DTO to Domain Model
            var regionDTO = new Models.DTO.Region
            {
                Id = region.Id,
                Code = region.Code,
                Area = region.Area,
                Lat = region.Lat,
                Longitude = region.Longitude,
                Name = region.Name,
                Population = region.Population
            };

            return Ok(regionDTO);

        }
    }
}
