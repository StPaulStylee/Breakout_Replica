#Breakout Replica

In an attemt to solidify the basics of game development
in Unity, I will be recreating the 1976 arcade classic, Breakout.

### October 22, 2019
* Update logic that disables the bricks colliders so that it behaves appropriately in all collsion instances
* Decrease the size of the collider on the ball in order to reduce multiple collisions at "once"
* Learned how to call methods from another game objects script

### October 23, 2019
* I found how to get a gameObjects parent gameObject
* I realized that when the ball reaches max speed the behavior of the ball on collisions is not working correctly, it sticks
* The velocity on x does not remain constant... I am not sure what logic determines the x velocity but it seems to vary

### October 24, 2019
* I was running into issues with ball and paddle collisions once the ball had a high velocity (8/-8 in this instance). I have partially fixed the problems by moving all velocity calculations into the FixedUpdate method. I still see some weird behavior in that the collisions appear to caluclate in different spots - sometimes right above the paddle sometimes right on it (as expected) and sometimes below it. Also, when going this fast collisions with the bricks aren't always fired off correctly.
* Removed the paddleBounds calculations out of every collision and into Start as it only needs to be calculated once

### October 25, 2019
I need to change how the paddle moves - use velocity instead of position. I think this because I need a way to chane the balls x velocity on collision with the paddle in a dynamic way - if the paddle slides in quickly it should increase the balls x velocity. I am having trouble doing this in a way that keeps the paddle movement consistent to the way it is now. I tried using RigidBody.MovePosition but that doesn't appear to be changing the velocity value of the rigidbody... but I need to spend a little more time with it. This method does keep the movement consistent to current functionality, which is good. But as I said, I need a velocity value and it doesn't appear to have one. More investigation is in order.
