using Microsoft.AspNetCore.Mvc;
using RollbarAutoAssignment.Models;
using RollbarAutoAssignment.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RollbarAutoAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class assignmentrulesController : ControllerBase
    {

        //Rollbar Service
        private rollbarService _rollbarService;
        private asssignmentrulesService _assignmentRules;

        // GET: api/<assignmentrulesController>
        [HttpGet]
        public async Task<List<assignmentModel>> GetAsync()
        {
            //Get the services we need to use
            _assignmentRules = new asssignmentrulesService();

            //Load Assignment Rules to use
            List<assignmentModel> _rules = await _assignmentRules.ReturnAssignmentRules();

            return _rules;
        }

        // POST api/<assignmentrulesController>
        [HttpPost("{accountAccessToken}")]
        public void Post(string accountAccessToken, [FromBody] object value)
        {

            //Get the services we need to use
            _rollbarService = new rollbarService();
            _assignmentRules = new asssignmentrulesService();

            //Need to try an API Command to see if we have a valid accountaccessToken.. as a method to auth


            //Write New Json Value to Json File
            var rulesPath = "AssignmentRules\\assignmentrules.json";
            System.IO.File.WriteAllText(rulesPath, value.ToString());

        }

    }
}
