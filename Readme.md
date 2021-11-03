# Breakout Replica

In an attemt to solidify the basics of game development in Unity, I have recreated the 1976 arcade classic, Breakout. The game is in an MVP state and it may stay this way so I can move onto more advanced Unity projects - TBD, I suppose.

### What's here
This clone is a near replication of the original cabinet game, including...
  * Random starting ball position and velocity on the X axis at the start of the turn
  * 6 different possible ball velocities depending on various game states
  * Point differentials between the various colored bricks
    * Yellow - 1 point
    * Green - 3 points
    * Orange - 5 points
    * Red - 7 points
  * Shrinking player paddle when "Breakout" is acheived
  * A UI that indicates the current player, their score (that blinks just like the original!), and their current turn count
  * Complete in game audio

### What's missing and what can be improved
This isn't a perfect replication and I don't intend it to be. Here are some things I know aren't perfect...
  * It is only built out for a single player
  * There is only one set of bricks (in the original there is two)
  * If the player clears all the bricks, nothing happens (I am going to fix this one as it will be very quick)
  * The grid layout for the bricks isn't completely symmetric
  * The ball doesn't change colors when traversing a play area that once had a brick or when hitting the paddle (in the original I believe the color for the bricks and panel was created by adding some sort of semi-transparent and colored foil/tape/adhesive to the screen)
  * There is a bug that allows the ball to occassionally escape the play area (I'm going to fix this one once I can figure out what is going on!!!)
  * The size of the score text isn't large enough to match what it is in the original
  * The alebedo and layers of the background and game objects isn't exactly right

The game can be demoed at my personal website,  [jeffreymiller.dev](https://jeffreymiller.dev/portfolio/BreakoutCloneWebGl/index.html). If you find anything I missed above, drop it in the issues. I probably won't fix it but it would be could to know as this was a learning exercise.
