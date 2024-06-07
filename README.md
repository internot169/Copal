

# Welcome to Copal

## Table of contents

-   Guide to Codebase
    
-   Setup
    
-   Tutorial
    
-   Contributors
    
-   Acknowledgements for Art
    

## Guide to Codebase

Go to Assets/Scripts to see our code

Ignore .meta files

 
/Data

-   This stores the class definitions for the data we read from our flask server and from our trivia file
    
-   Question.cs - Class that defines object for question data from JSON file
    
-   Score.cs - Class that defines object for score data from server
    

/Enemies

-   This stores definitions for enemy objects. It also stores a definition for the physical manifestation of the wumpus
    
-   Enemy.cs - Handles enemy movement, HP, effects, and triggers shooting
    
-   EnemyGun.cs - Handles enemy shooting and raycasts
    
-   EnemySpawner.cs - Spawns Enemies. What did you expect?
    
-   Wumpus.cs - Handles basic Wumpus mechanics such as HP + death
    
-   WumpusAI.cs - Handles movement and the 4 attacks. Analogous to Enemy.cs
    
-   WumpusCollisionScript.cs - Handles collision with the Wumpus body
    
-   WumpusDroneGun.cs - Handles shooting by the Wumpus' overhead drones. Analogous to EnemyGun.cs
    

/GameManagement

-   Stores teleporters between rooms (also carry hazards)

-   Teleporter.cs - Parent objects for all teleporters
    
-   BatTeleporter.cs - Triggers when Bat hazard assigned to room
    
-   BossTeleporter.cs - Triggers when Wumpus assigned to room
    
-   PitTeleporter.cs - Triggers when Pit hazard assigned to a room
    
-   AugmentManager.cs - Manages state of augments currently on player
    
-   RoomGenerator.cs - Generates rooms and their links randomly. Assigns hazard
    
-   GameManager.cs - Manages state of score objects. Enables pausing (timescale = 0) and testing (info from MenuInfo).
    

/Player

-   Stores all scripts attached to the Player object. Includes audio and movement.
    
-   AudioLoop.cs - Loops through soundtracks in game
    
-   MouseLook.cs - First person mouse look script for player
    
-   PlayerInfo.cs - Information about Player Health and augment manifestations
    
-   PlayerMovement.cs - Movement script for player
    

/Traps

-   Stores scripts for all traps and interactables for the game.
    
-   Coin.cs - Code for coin prefab - increases coins count
    
-   MovingPlatform.cs - Defines moving platforms
    
-   SpikeTrap.cs - Defines spike traps
    
-   Trap.cs - Parent class for all traps excluding coin and moving platform.
    

/UI Elements

-   arrowShoot.cs - Code to allow player to shoot arrow in UI
    
-   Logger.cs - Logs events in the game. Called in various places.
    
-   Menu.cs - Code for main menu. Loads leaderboard, tutorial, and plays game
    
-   MenuInfo.cs - Stores name from main menu and passes to GameManager
    
-   scoreUI.cs - Class that defines UI for the score on the leaderboard
    
-   Shop.cs - Takes input from buttons to purchase items in shop
    
-   Trivia.cs - Loads data for trivia and displays trivia questions to screen
    
/Combat

-   This stores player combat scripts, which allow it to fight enemies
- 
  

## Setup

When playing the game, make sure to run app.py in the same folder as scores.json!

  

Unity - Install Unity version 2022.3.18f1 with C# and Windows Build Tools

  

Python - install python version 3.10 or greater, run 

    pip install -r requirements.txt

 To start the server for the data when playing the game, run
 

    python app.py

## Tutorial

Make note of this - I won’t let you see it again >:). Your learning curve better be sharper than mine!

The object of this game is to find and kill the Wumpus (me), before I can take over the world with my algorithms! There is only one way to win: you must discover which room I am hiding in and surprise it by shooting an arrow (not your gun!) into that room from an adjacent room. Then, you will enter that room and destroy me!

### Controls

-   W, A, S, D to move
    
-   Left click is normal shoot
    
-   Right click is wide radius shoot
    
-   Middle is grapple - press space to go fast
    
-   Esc to pause and open the pause menu
    

-   This allows you to shoot the special arrows to kill the wumpus with
    
-   It also allows you to open the shop, to buy more arrows, buy secrets, or buy a random augment

Good luck!! I’m going to go make a cup of coffee…

(Psst.. hero.. it’s me - human intelligence) I’ll help you out.

### Traversing the Game

-   To traverse the game, you will need to find teleporter objects. They look like this:
    

(see game)

  

These teleporters will tell you which room (of the 30 possible rooms) they lead to. When you touch one of these objects, you will teleport to that room.

  

### Obstacles

  

Excluding the parkour and traps in the rooms, you will face numerous obstacles on your journey.

  

Bottomless Pits - Two rooms have bottomless pits in them. If you go there, you fall into the pit. You can save yourself from crushing defeat and get out of the pit by getting at least 2 out of three trivia questions right. If you get out of the pit, you will be placed back where you started the game.

Super Bats - Two other rooms have super bats. If you go there, a bat grabs you and takes you to some other room at random. After the bats drop you into a new room, they will fly away to another random room in the cave.

  

## Contributors

Yile Shen - yshen@eastsideprep.org

Angad Singh Josan - ajosan25@eastsideprep.org

Anmol Singh Josan - ajosan@eastsideprep.org

Ryan Li - rli@eastsideprep.org

Rishay Puri - rpuri@eastsideprep.org

Jack Goetzmann - jgoetzmann*eastsideprep.org

  
  

## Acknowledgements for Art

For music: White Records - [https://pixabay.com/users/white_records-32584949/](https://pixabay.com/users/white_records-32584949/)

Skybox: LaFinca - [https://assetstore.unity.com/publishers/66779](https://assetstore.unity.com/publishers/66779)
