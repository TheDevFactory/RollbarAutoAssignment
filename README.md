# Rollbar Auto Assignment
Auto Assign Items in Rollbar based on custom rules per project for both new OR reactivated items.

Download Docker Image: https://hub.docker.com/repository/docker/nkruger/rollbarautoassignment

# How to configure

1. Get your current Rollbar Account Access Token (read or write) from https://rollbar.com/settings/accounts/ACCOUNT_NAME/access_tokens/
   
   This Access Token will be used to setup the WebHook.
2. Create a new WebHook in your Rollbar project under the notifications.https://rollbar.com/ACCOUNT_NAME/PROJECT_NAME/settings/notifications/
3. Enter the URL that the WebHook will send the payload to.

   Example: https://yourhostedserver/api/rollbar/ACCOUNTACCESSTOKEN
5. Your webhook is no complete and Rollbar will send payloads to this as configured in your notification options.


# How to configure the Auto Assignment Rules

1. The Auto Assignment works based on a set of configured rules (Json format).
2. Rules are executed top down in that order. If a match is found the rule is applied and the process terminates to perform the assignment.

   Rules can be viewed from the API here (GET): https://yourhostedserver/api/assignmentrules
   
   Rules can be updated from the API here (POST): https://yourhostedserver/api/assignmentrules/ACCOUNTACCESSTOKEN the body is the Json Rules.
4. Example of a basic rule:
``` Json
[
  {
    "project_id": 1234,
    "event_name": "new_item",
    "rules": [
      {
        "field": "title",
        "matchtype": "contains",
        "value": "word"
      }
    ],
    "assign_to_username": "usernameA"
  },
  {
    "project_id": 1234,
    "event_name": "reactivated_item",
    "rules": [
      {
        "field": "title",
        "matchtype": "equals",
        "value": "word"
      }
    ],
    "assign_to_username": "usernameB"
  }
]
```

5. Fields available:

   'title', 'root', 'environment', 'framework', 'host', 'language', 'level', 'url'
7. Matchtypes available (string value):

   'contains', 'equals', 'startswith'
9. Values that can be suplied:

   String based value could be a number or any other value (string).
   
10. The assign_to_username needs to be exact as the service performs a lookup to identify the user_id to use for the actual assignment.

# Need help?

Feel free to contact the Rollbar Customer Engineering team or log a Issue in GitHub.

Please feel free to add, improve and expand this Auto Assignment service.
