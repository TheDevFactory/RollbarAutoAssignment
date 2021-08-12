using RollbarAutoAssignment.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace RollbarAutoAssignment.Services
{
    public class asssignmentrulesService
    {

        public async Task<List<assignmentModel>> ReturnAssignmentRules()
        {

            ////Example Config:
            ////Rules execute from the top down in order. So if a rule matches it is applied and no other rules are checked.
            ////This allows you to have a final rule (last item in json list) that can act as the default handler if no rules matches it can assign to a user of your choice.

            ////Field(s): 'root', 'environment', 'framework', 'host', 'language', 'level', 'url'
            ////Matchtype(s): 'contains', 'equals', 'startswith'

            //[
            //   {
            //    "project_id": 1234,
            //    "event_name": "new_item",
            //    "rules": [
            //      {
            //        "field": "environment",
            //        "matchtype": "contains",
            //        "value": "production"
            //      }
            //    ],
            //    "assign_to_username": "brian"
            //  },
            //  {
            //    "project_id": 1234,
            //    "event_name": "reactivated_item",
            //    "rules": [
            //      {
            //        "field": "host",
            //        "matchtype": "equals",
            //        "value": "www.domain.com"
            //      }
            //    ],
            //    "assign_to_username": "cory"
            //  },
            //  {
            //    "project_id": 1234,
            //    "event_name": "new_item",
            //    "rules": [
            //      {
            //        "field": "level",
            //        "matchtype": "contains",
            //        "value": "error"
            //      }
            //    ],
            //    "assign_to_username": "brian"
            //  }
            //]


            //Read json config file containing rules
            var rulesPath = "AssignmentRules\\assignmentrules.json";
            
            List<assignmentModel> _rulesObject = JsonSerializer.Deserialize<List<assignmentModel>>(File.ReadAllText(rulesPath));
            
            return _rulesObject;

        }

    }
}
