using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using RollbarAutoAssignment.Models;
using System.Text;

namespace RollbarAutoAssignment.Services
{
    public class rollbarService
    {
        private HttpClient _httpClient;

        public async Task<String> ReturnProjectAccessToken(string accountAccessToken, int projectId)
        {
            _httpClient = new HttpClient();

            //Rollbar API Endpoint
            string apiUrl = $"https://api.rollbar.com/api/1/project/{projectId}/access_tokens";

            //Rollbar Account Access Token
            _httpClient.DefaultRequestHeaders.Add("X-Rollbar-Access-Token", accountAccessToken);

            //Get Response
            var response = await _httpClient.GetAsync(apiUrl);
            string objResponse = await response.Content.ReadAsStringAsync();
            string strResponse = "";

            //Check Status Code
            if (response.StatusCode == System.Net.HttpStatusCode.OK) {

                //Response into Object we can work with
                rollbarProjectAccessTokensModel _resultObject = JsonSerializer.Deserialize<rollbarProjectAccessTokensModel>(objResponse);

                if (_resultObject is not null)
                {

                    foreach (rollbarProjectTokensModel objItem in _resultObject.result)
                    {

                        if (objItem.status == "enabled")
                        {

                            if (objItem.scopes[0] == "write")
                            {
                                //This is the right token to use
                                strResponse = objItem.access_token;

                            }
                        }

                    }

                }

                return strResponse;

            } else {

                //Some Sort of Error Occured... read message
                //Response into Object we can work with
                rollbarErrorModel _resultObject = JsonSerializer.Deserialize<rollbarErrorModel>(objResponse);

                return "Error, " + _resultObject.message;

            }

        }

        public async Task<Object> ReturnUsers(string accountAccessToken)
        {
            _httpClient = new HttpClient();

            //Rollbar API Endpoint
            string apiUrl = $"https://api.rollbar.com/api/1/users";

            //Rollbar Account Access Token
            _httpClient.DefaultRequestHeaders.Add("X-Rollbar-Access-Token", accountAccessToken);

            //Get Response
            var response = await _httpClient.GetAsync(apiUrl);
            string objResponse = await response.Content.ReadAsStringAsync();
            //string strResponse = "";

            //Check Status Code
            if (response.StatusCode == System.Net.HttpStatusCode.OK)
            {

                //Response into Object we can work with
                rollbarUsers _resultObject = JsonSerializer.Deserialize<rollbarUsers>(objResponse);

                //if (_resultObject is not null)
                //{

                //    foreach (rollbarUser objItem in _resultObject.result.users)
                //    {



                //    }

                //}

                return _resultObject;

            }
            else
            {

                //Some Sort of Error Occured... read message
                //Response into Object we can work with
                rollbarErrorModel _resultObject = JsonSerializer.Deserialize<rollbarErrorModel>(objResponse);

                return "Error, " + _resultObject.message;

            }

        }

        public async Task<string> RunAutoAssignment(string projectAccessToken, string itemId, rollbarEventModel eventObject, rollbarUsers usersObject, List<assignmentModel> rulesObject)
        {

            //Variables required
            bool bolMatchFound = false;
            string assignToUsername = "";
            string assignToUserId = "0";

            //Look at the Rules to see if we have a match and execute
            foreach (assignmentModel objRule in rulesObject) {

                //Work on each rule to see if we can find a match

                //We have a matching rule for this event_name (type)
                //Also matches the project_id we configured
                if (objRule.event_name.ToLower() == eventObject.event_name.ToLower() && objRule.project_id == eventObject.data.item.project_id) {


                    ////Field(s): 'title', 'root', 'environment', 'framework', 'host', 'language', 'level', 'url'
                    ////Matchtype(s): 'contains', 'equals', 'startswith'
                    
                    //*********************************************************************            
                    foreach (assignmentrulesModel objRuleStatement in objRule.rules) {

                        //Exit if a match has already been found
                        if (bolMatchFound == true) { break; }

                        switch (objRuleStatement.field.ToLower())
                        {
                            case "title":
                                if (objRuleStatement.matchtype.ToLower() == "contains")
                                {

                                    if (eventObject.data.item.title.Contains(objRuleStatement.value))
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }
                                if (objRuleStatement.matchtype.ToLower() == "equals")
                                {

                                    if (objRuleStatement.value == eventObject.data.item.title)
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }
                                if (objRuleStatement.matchtype.ToLower() == "startswith")
                                {

                                    if (eventObject.data.item.title.StartsWith(objRuleStatement.value))
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }

                                break;

                            case "root":
                                if (objRuleStatement.matchtype.ToLower() == "contains") {

                                    if (eventObject.data.item.last_occurrence.root.Contains(objRuleStatement.value)) {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }
                                
                                }
                                if (objRuleStatement.matchtype.ToLower() == "equals")
                                {

                                    if (objRuleStatement.value == eventObject.data.item.last_occurrence.root)
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }
                                if (objRuleStatement.matchtype.ToLower() == "startswith")
                                {

                                    if (eventObject.data.item.last_occurrence.root.StartsWith(objRuleStatement.value))
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }

                                break;

                            case "environment":
                                if (objRuleStatement.matchtype.ToLower() == "contains")
                                {

                                    if (eventObject.data.item.last_occurrence.environment.Contains(objRuleStatement.value))
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }
                                if (objRuleStatement.matchtype.ToLower() == "equals")
                                {

                                    if (objRuleStatement.value == eventObject.data.item.last_occurrence.environment)
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }
                                if (objRuleStatement.matchtype.ToLower() == "startswith")
                                {

                                    if (eventObject.data.item.last_occurrence.environment.StartsWith(objRuleStatement.value))
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }

                                break;

                            case "framework":
                                if (objRuleStatement.matchtype.ToLower() == "contains")
                                {

                                    if (eventObject.data.item.last_occurrence.framework.Contains(objRuleStatement.value))
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }
                                if (objRuleStatement.matchtype.ToLower() == "equals")
                                {

                                    if (objRuleStatement.value == eventObject.data.item.last_occurrence.framework)
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }
                                if (objRuleStatement.matchtype.ToLower() == "startswith")
                                {

                                    if (eventObject.data.item.last_occurrence.framework.StartsWith(objRuleStatement.value))
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }

                                break;

                            case "host":
                                if (objRuleStatement.matchtype.ToLower() == "contains")
                                {

                                    if (eventObject.data.item.last_occurrence.host.Contains(objRuleStatement.value))
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }
                                if (objRuleStatement.matchtype.ToLower() == "equals")
                                {

                                    if (objRuleStatement.value == eventObject.data.item.last_occurrence.host)
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }
                                if (objRuleStatement.matchtype.ToLower() == "startswith")
                                {

                                    if (eventObject.data.item.last_occurrence.host.StartsWith(objRuleStatement.value))
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }

                                break;

                            case "language":
                                if (objRuleStatement.matchtype.ToLower() == "contains")
                                {

                                    if (eventObject.data.item.last_occurrence.language.Contains(objRuleStatement.value))
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }
                                if (objRuleStatement.matchtype.ToLower() == "equals")
                                {

                                    if (objRuleStatement.value == eventObject.data.item.last_occurrence.language)
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }
                                if (objRuleStatement.matchtype.ToLower() == "startswith")
                                {

                                    if (eventObject.data.item.last_occurrence.language.StartsWith(objRuleStatement.value))
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }

                                break;

                            case "level":
                                if (objRuleStatement.matchtype.ToLower() == "contains")
                                {

                                    if (eventObject.data.item.last_occurrence.level.ToLower().Contains(objRuleStatement.value.ToLower()))
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }
                                if (objRuleStatement.matchtype.ToLower() == "equals")
                                {

                                    if (objRuleStatement.value.ToLower() == eventObject.data.item.last_occurrence.level.ToLower())
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }
                                if (objRuleStatement.matchtype.ToLower() == "startswith")
                                {

                                    if (eventObject.data.item.last_occurrence.level.ToLower().StartsWith(objRuleStatement.value.ToLower()))
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }

                                break;

                            case "url":
                                if (objRuleStatement.matchtype.ToLower() == "contains")
                                {

                                    if (eventObject.data.item.last_occurrence.request.url.Contains(objRuleStatement.value))
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }
                                if (objRuleStatement.matchtype.ToLower() == "equals")
                                {

                                    if (objRuleStatement.value == eventObject.data.item.last_occurrence.request.url)
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }
                                if (objRuleStatement.matchtype.ToLower() == "startswith")
                                {

                                    if (eventObject.data.item.last_occurrence.request.url.StartsWith(objRuleStatement.value))
                                    {
                                        assignToUsername = objRule.assign_to_username;
                                        bolMatchFound = true;
                                        break;
                                    }

                                }

                                break;

                            default:

                                break;
                        }

                    }
                    //*********************************************************************            



                }
                            
            }


            //Only try to find user if a match was found
            if (bolMatchFound == true) {

                foreach (rollbarUser objUser in usersObject.result.users) {
                    if (objUser.username == assignToUsername) {
                        assignToUserId = objUser.id.ToString();
                        break;
                    }
                }

            }





            //Only Execute the Patch if we have a matching Rule
            if (bolMatchFound == true)
            {

                _httpClient = new HttpClient();

                //Rollbar API Endpoint
                string apiUrl = $"https://api.rollbar.com/api/1/item/{itemId}";

                //Rollbar Account Access Token
                _httpClient.DefaultRequestHeaders.Add("X-Rollbar-Access-Token", projectAccessToken);

                var responseBody = String.Empty;
                //var jsonRequest = JsonSerializer.Serialize("{ ""assigned_user_id"":"""" }");

                var myData = new {
                    assigned_user_id = assignToUserId
                };
                string jsonData = JsonSerializer.Serialize(myData);

                var content = new StringContent(jsonData, Encoding.UTF8, "application/json");
                using (HttpResponseMessage response = await _httpClient.PatchAsync(apiUrl, content))
                {
                    response.EnsureSuccessStatusCode();
                    responseBody = await response.Content.ReadAsStringAsync();

                    //Check Status Code
                    if (response.StatusCode == System.Net.HttpStatusCode.OK)
                    {
                        //All good, nothing to do
                    }
                    else
                    {
                        //Log this in the future with what was the problem and status code
                    }

                }

                return "Success";

            }
            else {

                return "Error, no matcing rule found";

            }
         
        }

    }
}
