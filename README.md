This character controller takes inspiration from the new example Unity First Person and Third Person character controller assets. I have rebuilt them (for the most part) in Unity Visual Scripting. There are three main graphs:
- Movement Handler
- Player Controller
- Push Rigidbodies


I recommend not changing the graph variable values unless you really need to experiment.

Custom Events List:
- OnJump: called when a jump is processed
- OnLanded: called when the character touches the ground after being in the air
- OnFalling: called every frame the character is in free-fall
- OnFP: called when the camera switches to First Person View
- OnTP: called when the camera switches to Third Person View
- OnRBPush: called every frame where the player controller is colliding with a non-kinematic rigidbody that can be pushed

Custom Units:
- Check Layer Mask: Takes a layer (an int from Game Object - Get Layer, for example) and a LayerMask and checks if the layer is in the layer mask and returns true if so
- Update Cinemachine Follow Distance: ONLY WORKS ON A 3RD PERSON FOLLOW BODY COMPONENT - sets the camera distance for a Cinemachine Virtual Camera with a 3rd Person Follow component on the Body of the camera. If you want similar units for other components, the code is very copy-pastable and I may add more upon request. Please feel free to send me a merge request with any updated Cinemachine units.

