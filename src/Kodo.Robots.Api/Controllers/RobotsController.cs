using AutoMapper;
using AutoMapper.QueryableExtensions;
using Kodo.Robots.Api.ViewModels;
using Kodo.Robots.Domain.Entities;
using Kodo.Robots.Domain.Interfaces.Repositories;
using Kodo.Robots.Domain.Interfaces.UoW;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Kodo.Robots.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RobotsController : ControllerBase
    {
        private readonly IRobotRepository _repository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _uow;

        private readonly Action<IMappingOperationOptions> _mappingOperationOptions = opt =>
        {
            opt.Items["Route"] = "robots";
            opt.Items["Type"] = "robotData";
            opt.Items["Actions"] = new List<dynamic>
            {
                new { Method = "GET", Action = "get" },
                new { Method = "POST", Action = "create" },
                new { Method = "PUT", Action = "update" },
                new { Method = "DELETE", Action = "delete" }
            };
        };

        public RobotsController(
                            IRobotRepository repository,
                            IMapper mapper,
                            IUnitOfWork uow
                            )
        {
            _repository = repository;
            _mapper = mapper;
            _uow = uow;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RobotSimplifiedViewModel>>> GetRobots()
        {
            return await _repository.Query(wh => !wh.IsDeleted)
                                    .ProjectTo<RobotSimplifiedViewModel>(_mapper.ConfigurationProvider)
                                    .ToListAsync();
        }

        [HttpGet("{name}/history")]
        public async Task<ActionResult<RobotViewModel>> GetHistoryByRobotName(string name)
        {
            Robot robot = await _repository.FindAsync(wh => !wh.IsDeleted && string.Equals(wh.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (robot == null)
            {
                return NotFound();
            }

            return _mapper.Map<RobotViewModel>(robot, _mappingOperationOptions);
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<RobotViewModel>> GetRobotByName(string name)
        {
            Robot robot = await _repository.FindAsync(wh => !wh.IsDeleted && string.Equals(wh.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (robot == null)
            {
                return NotFound();
            }

            return _mapper.Map<RobotViewModel>(robot, _mappingOperationOptions);
        }

        [HttpPut("{name}")]
        public async Task<IActionResult> PutRobot(string name, JObject robotData)
        {
            if (!_repository.RobotExists(name))
            {
                _repository.Create(new Robot(robotData.ToString()));
                await _uow.CommitAsync();
                return Ok();
            }

            Robot _robot = await _repository.GetByName(name);
            JObject _existingRobotData = JObject.Parse(_robot.Data);

            foreach (JProperty attribute in robotData.Properties())
                _existingRobotData[attribute.Name] = attribute.Value;

            _robot.UpdateData(_existingRobotData.ToString());
            
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<Robot>> PostRobot(RobotViewModel robot)
        {
            Robot _newRobot = _mapper.Map<Robot>(robot);

            _repository.Create(_newRobot);
            await _uow.CommitAsync();

            return CreatedAtAction("GetRobotByName", robot.Name, _newRobot);
        }
    }
}
