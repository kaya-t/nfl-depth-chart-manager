# .❀。• *₊°。 ❀°。NFL Depth Chart Manager .❀。• *₊°。 ❀°。

This is a console app to manage NFL team depth charts. It is able to add, remove, and view players in the depth chart, as well as print the entire depth chart for NFL teams. 

## Acceptance Criterias
- **Add a player**
  - Add new player to the chart at given depth or at the end of the list.
  - Player team number is unique.
- **Remove a player**
  - Remove player from a depth position or return empty if not found / not valid.
- **Get backups of player**
  - Show the list of backups of a given player.
  - Return empty list if there are no backups or player is not valid.
- **Show depth chart**
  - Print the depth chart of all NFL teams at current state.
- ***Unit tests***
  - Unit tests to cover the above acceptance criterias. 

## Additional Features
- Multiple NFL teams can be added and managed.
- Categories display by NFL position categories.  

## Design Patterns Scalability
- Other sport can be extended from the initial factory pattern implementation -`ISportFactory`.
- Repository pattern is used to abstract data access logic, and would be scalable for API or Database data sources.
- Dependency injection to set dependencies for service classes, to manage lifecycle of services, and better testing with Mocks. 

## Limitations
- Each console session is for one selected team only, to modify or add different NFL team, console app needs to be restarted.
- No capability to go back to previous submitted user input.
- Extended safeguarding of valid players details or NFL team requirements (e.g. players depth chart count per position).
 
## Navigation options - press the following
- "1" to Add player to team
- "2" to Remove player from team
- "3" to Show player's backups
- "4" to Show team/s depth chart
- "5" to Save chart as new file
- "6" to Save and exit program
- Ctrl+C to exit without saving

## Team selection
In the beginning of the console prompts, team selection is shown. 
- If `enter` key is pressed, the default team in the `/nfl_teams` will be selected, which is the first team in the json file.
- To select an existing other teams, enter the the team name value, if it is valid, the existing team will be selected. Otherwise, a new team will be generated for the depth chart. 

## Assumptions
- If a depth value which is higher than current available depth position is set for Add player, the new player will be added at the end of the list.
- An existing NFL chart with team values are set by default as JSON file - see `/nfl_teams`. 

## Run the NFL Depth Chart Manager

***Pre-reqs***
Download .NET 8.0https://dotnet.microsoft.com/en-us/download/dotnet/8.0

```
cd NFLDepthChartManager
dotnet run
```

## Run Unit Tests
```
cd NFLDepthChartManager.Tests
dotnet test
```
