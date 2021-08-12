using Microsoft.AspNetCore.Mvc;
using RollbarAutoAssignment.Models;
using RollbarAutoAssignment.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace RollbarAutoAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class rollbarController : ControllerBase
    {
        //Rollbar Service
        private rollbarService _rollbarService;
        private asssignmentrulesService _assignmentRules;

        // POST api/<rollbarController>
        [HttpPost("{accountAccessToken}")]
        public async void Post(string accountAccessToken, [FromBody] object value)
        {

            //Get the services we need to use
            _rollbarService = new rollbarService();
            _assignmentRules = new asssignmentrulesService();

            //Read Json from Body to get the Variables we need
            //Response into Object we can work with
            rollbarEventModel _eventObject = JsonSerializer.Deserialize<rollbarEventModel>(value.ToString());

            if (_eventObject is not null)
            {

                //Load Assignment Rules to use
                List<assignmentModel> _rules = await _assignmentRules.ReturnAssignmentRules();

                //Get the Access Token for the specific project
                string projectToken = await _rollbarService.ReturnProjectAccessToken(accountAccessToken, _eventObject.data.item.project_id);

                //If we have some sort of error
                if (projectToken.StartsWith("Error"))
                {

                    ////Do nothing.. just log it to file
                    try {
                        var logPath = $"Data\\temp" + CurrentTimestamp().ToString() + ".txt";
                        System.IO.File.WriteAllText(logPath, projectToken + System.Environment.NewLine + "Token:" + accountAccessToken + System.Environment.NewLine + value.ToString());
                    } catch { 
                    
                    }
                    

                }
                else
                {

                    //Get the Users for this Account, used to match up teh username in the rules setup
                    rollbarUsers _users = (rollbarUsers)await _rollbarService.ReturnUsers(accountAccessToken);

                    //Execute Auto Assignment Rules
                    var strResult = await _rollbarService.RunAutoAssignment(projectToken, _eventObject.data.item.id.ToString(), _eventObject, _users, _rules);

                    ////Log it to file and execute the assignment
                    try {
                        var logPath = $"Data\\temp" + CurrentTimestamp().ToString() + ".txt";
                        System.IO.File.WriteAllText(logPath, value.ToString());
                    } catch {
                    
                    }

                }

            }
            else { 
            
                //Nothing to work with as the object is NULL

            }          

        }

        static long CurrentTimestamp()
        {
            return (long)(DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds * 1000);
        }

    }
}
