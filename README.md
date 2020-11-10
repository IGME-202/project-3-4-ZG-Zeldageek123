Name: Cathryn Szulczewski
Section: IGME 202.05S1
Project Title: Project 3 - HvZ Pt. 1 - “Blocky HvZ!”
IMPORTANT NOTE: I AM USING MY GRACE PERIOD (2-DAY EXTENSION) ON THIS ASSIGNMENT.
Description:
	This project exists to showcase my skills in implementing force-based movement and my own written collision detection in a 3D worldspace, as well as logic about game states, GUI, and interactions across different GameObjects/scripts through a manager. This project also does well to showcase my proficiency with Unity at this point in time. I used force-based movement to simulate the on-campus classic Humans vs. Zombies game, utilizing seek and flee vectors to move the humans (seeking out treasure, fleeing from zombies near them) and seek vectors to move the zombies toward the humans. Collision was detected using circle detection and units’ radii. I also dabbled in debug lines for the first time, and used them to display the forward and right vectors of each object, as well as to show the connection between a zombie and its target human. Because this is a debug / prototype build, the masses/max speeds of the game objects may change. In the final build, zombies should be slower/heavier than humans so that the humans can outrun them.
	In HvZ Pt. 1, 3 humans and 3 zombies are instantiated on start in the same position, as well as a piece of treasure. The humans will seek out the treasure until a zombie comes within its detection range. They will then flee all of the zombies in range in addition to trying to seek out that treasure. Upon start, all debug lines will be displayed by default. The user can press d to toggle them on and off. When a human collides with a piece of treasure, the treasure is “picked up” and a “new” (really the same” treasure is spawned at a random location in bounds. When a zombie collides with a human, it bites that human and turns them into a zombie that behaves just like the others! Zombies only target the closest human, and can switch their target mid-pursuit because they’re aware of which one is closer. When all humans have been caught, all zombies stop and the simulation is over.
	The Vehicle script is responsible for the base movement of the game objects.It has methods for seeking, fleeing, and keeping the object in the bounds of the game using a force to seek the center when nearing the edge of the terrain. It also utilizes an abstract method to calculate all steering forces acting on an object that a child script is attached to. 
	The Human script inherits from Vehicle and sets human specific masses / max speed values. It applies flee forces from every zombie in the scene using the manager object’s list of zombies if they’re close enough, and if they aren’t close enough the humans just seek out the treasure. Human also checks every frame for a collision with treasure, and if it does collide with treasure, it triggers that treasure’s ongrab method which respawns it at a random place in the bounds of the terrain.
	The Zombie script inherits from Vehicle and sets zombie specific masses / max speed values. By default, every zombie’s target human is set to the manager’s humans list at index 0, because that will always exist until there are no longer any humans. The zombie seeks its target human every frame unless that human is inactive, which will make them stop and their velocity/acceleration = 0. The target human’s distance is also compared to all of the other humans’ distances to the current zombie in the manager’s list. If a different human is closer, then the target human will switch and the zombie will seek that human instead. In the case that a target human is a null reference pointer (when a human is caught and transformed into a zombie), the target human is set to humans[0] which is arbitrary because target human is updated every frame. The algorithm would break if targethuman was ever an empty pointer, though.
	The Treasure script has a single method - OnGrab, which is triggered when a human collides with treasure. It simply respawns at some random point in bounds that the human will continue to chase again.
	The TerrainGen script is only responsible for displaying GUI to the user to press d to toggle debug lines and to create a terrain of the proper size of 100x100.
	The Manager script is responsible for keeping track of all gameobjects on the scene in a humans list and a zombies list. It also handles collision between a human and a zombie, where that human is set to inactive, a zombie is spawned at the point it was, that human is removed from the humans list and that zombie is added to the zombies list. Manager is also responsible for managing game states for debug lines. The game state is toggled between on and off with a press of d, and keeps the presence of debug lines consistent across all game objects, which display debug kines only under the condition that their manager has it set to active.
	
User Responsibilities / Functionality: 
This is meant to be a simulation, not a game. There is no functionality beyond toggling debug lines.
You can toggle debug lines by pressing “d” on your keyboard.
You can watch the humans attempt to get treasure without getting caught, and then eventually all get eaten and turned into zombies.
Nothing happens when the simulation ends. You can’t even toggle the debug lines anymore.

Caveats or Known Issues: 
For some reason, the last human that dies and turns into a zombie will be the only one that you can toggle debug lines for at the end. I don’t know why.

Requirements Not Completed:
None! I have them all done. I realize I was probably supposed to use a different algorithm for keeping game objects in bounds, but it’s a force that’s applied and steers the vehicle so I’m happy with it.

Sources:
All of my game objects are primitives. No sources for models.

Notes:
I AM USING MY GRACE PERIOD (2 DAY EXTENSION) ON THIS PROJECT 
Something is up with debug lines. I don’t know why you can only toggle the last instantiated zombie’s debug lines in the end game state.

