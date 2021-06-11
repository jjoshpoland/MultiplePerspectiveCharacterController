![](MVCCPreview.gif)

This character controller takes inspiration from the new example Unity First Person and Third Person character controller assets. I have rebuilt them (for the most part) in Unity Visual Scripting. As of writing this, there are not a lot of great Visual Scripting examples or assets, so I wanted to share my work. There are a couple custom Visual Scripting units that you are free to pull from as well.

From a design perspective, using a controller like this has multiple advantages:
- Many players have a strong preference on third vs first person, this allows you to accomodate both
- Even in a first person game, many people want to admire their character from many angles
- Forcing the player to switch between first and third person in some situations is necessary for mood or mechanics. For example, switching from first to third person for ultimate abilities in (see Destiny 2) or for story/cut scenes where the player is not in control of their character. Conversely, switching from third to first person when using a precise aiming ability, in tight spaces where the camera is constrained, or for those scenes where immersion is more important.

![](MVCC-FP.gif)

The variables and logic are containerized so that the movement and rotation of the character can be assigned by an AI and can be synced in multiplayer if needed.

Right now the first person view is working with the Unity single-mesh robot character, so it will sometimes clip through the camera. In your first-person game, parts of the mesh should be hidden to prevent this. There is also a multi-camera, multi-layer setup that some people use for first person view. If your game is primarily third person, hiding the character mesh during first person view is the easiest and best option.

Primary Variables:
- OrientationReference: This is orientation from which the character is rotating. The Player object never rotates, only the character avatar rotates. This is currently the Camera, as the player's orientation is based on the Camera. For an AI character, setting this to the Player object would allow for a move direction to be calculated based on the angle between the Player object and the desired position.
- Move: This is the assigned from the InputSystem Move action, a vector2 with x and y from -1 to 1.
- Speed: Move speed of the character
- LookSpeed: Governs camera rotation
- SprintSpeedModifier: The multiplier for speed when sprinting
- JumpForce: How high the character should jump
- LockFacing: IMPORTANT - when true, the character will always move while facing the Orientation Reference. This allows for strafing behavior and is automatically set to true during First Person view. If you wanted to implement a lock-on mechanic during third person, this bool is required.
- Character Object: The character avatar being animated and rotated based on movement. Should be a child object of the Player object where this is attached.
- CM-Pivot: A reference transform for both first person and third person cameras. This is rotated so that the Cinemachine virtual camera follows it.
- LookSpeedMultiplier: This is the multiplier for the look-rotation input speed
- TP VCM: The Cinemachine virtual camera used for Third Person view. Disabled and enabled for view switching.
- FP VCM: The Cinemachine virtual camera used for First Person view. Disabled and enabled for view switching.
- Jumping: Set to true if conditions are met and then set to false if the player cannot jump or if the jump has been processed. The boolean is easier to sync across multiplayer than a custom event or animation trigger.

![](graph.png)

There are three main graphs:
- Movement Handler: Moves the player relative to the Orientation Reference (camera by default). Handles jumping, falling, landing, turning the character, rotating the camera, and setting animator parameters.
  - The RelativeMovement variable is the result of converting the input from an absolute input vector into an angle away from the current OrientationReference. 
  - Forced Movement is a vector manipulated by jumping and gravity. If you wanted to push your character back or have them move forward when performing an attack, for example, this is the variable to use. Right now, the controller only decays and clamps with gravity, so you would need to add logic to decrement and clamp the x and z length of this vector over time if you change those.
  - Gravity is a constant, only change if you want a low-grav situation
  - Grounded is set from the CharacterController IsGrounded property
  - FallTimeout is the amount of time the character should be ungrounded before starting a free-fall. Setting this too short results in the character "falling" down slopes and stairs
  - FallTimeoutDelta is used for tracking against the FallTimeout
- Player Controller: Sets object variables and fires events based on the InputSystem inputs. Also responsible for Camera Zoom and View Switching.
  - FirstPersonCameraDistanceThreshold is the distance of the camera from the player where the controller should switch to first person view.
  - MaxZoom is the maximum distance the camera should be from the player
  - ZoomVelocity is used for smooth damping the camera zoom
  - Current Zoom is set to the camera distance. Should be set to the same distance as the TP Virtual Camera in the inspector to ensure alignment.
  - ZoomSpeed is how much the the zoom input should move the camera distance
  - ZoomTarget is the targeted camera distance set by the input - the smoothdamp will move toward this target
- Push Rigidbodies: Pushes away rigidbodies from the character when the CharacterController collides with a rigidbody on the PushLayers mask
  - CanPush - this character will only push rigidbodies if this is set to true
  - PushLayers is a LayerMask that is used to determine if the rigidbody hit is in a pushable layer. Make sure any rigidbodies you want to push are set to a layer that is selected in this mask.
  - Strength is the force multiplier applied to the object from the force of the character's movement

![](MVCC-RB.gif)

Change graph variables at your own risk.

Custom Events List:
- OnJump: called when a jump is processed
- OnLanded: called when the character touches the ground after being in the air
- OnFalling: called every frame the character is in free-fall
- OnFP: called when the camera switches to First Person View
- OnTP: called when the camera switches to Third Person View
- OnRBPush: called every frame where the player controller is colliding with a non-kinematic rigidbody that can be pushed

Custom Units:
- Check Layer Mask: Takes a layer (an int from Game Object => Get Layer, for example) and a LayerMask and checks if the layer is in the layer mask and returns true if so
- Update Cinemachine Follow Distance: ONLY WORKS ON A 3RD PERSON FOLLOW BODY COMPONENT - sets the camera distance for a Cinemachine Virtual Camera with a 3rd Person Follow component on the Body of the camera. If you want similar units for other components, the code is very copy-pastable and I may add more upon request. Please feel free to send me a merge request with any updated Cinemachine units.

