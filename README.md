# Ultimate Frisbee Tournament Scheduler
## Overview
This app has been designed to schedule ultimate frisbee tournaments. Tournaments can be created and teams can be added to the tournament. Tournaments must consist of 16 teams which will be automatically assigned to pools according to seed and games are scheduled based on these pools.
## Program Structure
The below diagram illustrates the classes in this project and their relation to each other:

![image](Screenshots/ClassDiagram.PNG)

## How to Use
### Creating a Tournament
The first step to scheduling your tournament is to create one. This can be done on the home page by typing a name in the box and clicking the "Add Tournament" button. By selecting the newly created tournament, you can change its name by altering in the box and clicking the "Update Tournament" button. A selected tournament can also be deleted using "Remove Tournament".

![image](Screenshots/CreatingTournament.PNG)

### Adding Teams to a Tournament
You can now view your tournament. Teams can be added to the tournament by typing a name in the box and clicking "Add Team". 

![image](Screenshots/AddedTeam.PNG)

Teams can be updated and removed in the same way as tournaments.
Teams are added in seed order; they are given a number from 1 to 16 where 1 is top seed. This seed can be altered by selecting a team and using the "+ Seed" and "- Seed" buttons.

![image](Screenshots/AllTeams.PNG)

### Scheduling the Tournament
Once you have 16 teams and are satisifed with their seed order, you can click "Schedule Tournament" to produce the tournament schedule for the first day.

![image](Screenshots/ScheduleSat.PNG)

This will also open a window containing the four pools allocated by seed.

![image](Screenshots/Pools.PNG)

The "View Day 2" Button will enable you to view the seeding games on the second day of the tournament.

![image](Screenshots/Day2.PNG)

## Future Features
This project has many features but there are some additional features which will be added in a future revision. These features include the ability to start the tournament at a particular time, have variable match length and be able to update the tournament scores so that you can see which teams are performing well.

## Project Management
This project was managed in a series of sprints as would be expected for a Scrum project. The retrospectives of each of the three sprints can be seen here:
[Retrospectives](SprintRetrospectives.pdf)
