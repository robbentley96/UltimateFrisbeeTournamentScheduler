# Ultimate Frisbee Tournament Scheduler
## Overview
This app has been designed to schedule ultimate frisbee tournaments. Tournaments can be created and teams can be added to the tournament. The intention is to have 16 teams which will be sorted into 4 pools of 4 and game times allocated accordingly.
## Program Structure
The below diagram illustrates the classes in this project and their relation to each other:

![image](Screenshots/ClassDiagram.PNG)

## How to Use
### Creating a Tournament
The first step to scheduling your tournament is to create one. This can be done on the home page by typing a name in the box and clicking the "Add Tournament" button. By selecting the newly created tournament, you can change its name by altering in the box and clicking the "Update Tournament" button. A selected tournament can also be deleted using "Remove Torunament".
### Adding Teams to a Tournament
You can now view your tournament. Teams can be added to the tournament by typing a name in the box and clicking "Add Team". Teams can be updated and removed in the same way as tournaments.
Teams are added in seed order; they are given a number from 1 to 16 where 1 is top seed. This seed can be altered by selecting a team and using the "+" and "-" buttons.
### Scheduling the Tournament
Once you have 16 teams and are satisifed with their seed order, you can click "Schedule Tournament" to produce the tournament schedule for the first day. This will also open a window containing the four pools allocated by seed.


