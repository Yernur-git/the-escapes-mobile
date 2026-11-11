# the escapes

a planned top-down 2d prison escape game for mobile, in landscape orientation.
this repository starts with the project setup for assignment 8-9.

## setup

unity version: 6000.6.0f1.

add this folder through unity hub. unity creates the local cache on first import.
the project uses force text serialization and visible meta files.

## current stage

open `Assets/Scenes/MainMenu.unity` and press play.
the menu has play and quit buttons. play loads `Prison`, a small movement test
level. quit stops play mode in the editor and closes a build.

`Assets/Scripts/MenuButtons.cs` uses the play and quit examples from assignment 3,
with the scene name changed to `Prison`. `SafeArea.cs` uses the lecture 8-9 example.
the menu canvas uses 1920 x 1080, match height, and centred anchors inside SafeArea.

the hud shows three hearts, the exit objective and an on-screen stick.
move with wasd, arrow keys, a gamepad left stick or by dragging the stick handle.
walls keep the player inside the level and block the guard's sight.

`PlayerMovement.cs` uses a7 `PlayerInput` and `OnMove(InputValue)`, adapted to
two-axis top-down movement. the screen stick sends `<Gamepad>/leftStick` input.
the top-right pause button opens resume, restart and menu. pause sets
`Time.timeScale` to 0. resume restores it to 1; restart and menu restore time
before loading their scene. `PauseMenu.cs` follows the lecture 8-9 pause
example and the scene-loading examples from assignment 3.

## assignment 8-9 scope

- main menu: game title, play and quit.
- hud: hearts, score, pause and on-screen controls.
- pause: resume, restart and menu, with game time stopped.
- safe-area layout tested on a 16:9 phone, a notch phone and a 4:3 tablet.

`the-escapes-gdd.pdf` describes the planned game for the following weeks.
## assignment 10, first stage

reach the green exit while avoiding the patroller. the guard follows six
waypoints, chases a player inside its vision cone and stops before a melee hit.
the red ring warns for 0.6 seconds. move out of reach to dodge. after a strike,
the guard rests for 0.6 seconds and has a 1.5 second attack cooldown.

`Patroller.cs` adapts lecture 10's enum, switch, state timer, waypoint movement
and 2d distance, angle and raycast checks. its obstacle mask contains only Walls.
movement uses Rigidbody2D.MovePosition in FixedUpdate. select the guard to see
the vision range and cone in the scene view. the yellow dot marks its facing.

`PlayerHealth.cs` updates the hearts and gives one second of immunity after a hit.
`PrisonExit.cs` calls `LevelResult.cs` when the player reaches the exit.
zero health shows caught; reaching the exit shows you escaped. both screens
offer restart and menu using the existing PauseMenu scene-loading methods.

## assignment 10, second stage

the purple shooter uses lecture 10's idle, keep distance, aim and cooldown
states. it tries to stay about 3.6 units from the player. a red line warns
for 0.6 seconds, then a slow bullet travels at five units per second. the
shooter waits 1.5 seconds before preparing another shot.

`Shooter.cs` controls distance, aiming and firing. walls block detection and
stop the aim line. it cannot start an attack outside the camera view.
`EnemyBullet.cs` moves the projectile, damages the player on contact and removes
it on a wall hit or after four seconds. its speed, damage and lifetime are
set on `Assets/Prefabs/EnemyBullet.prefab`.

## assignment 10, third stage

the green charger guards the lower route. its states are idle, wind-up, dash
and stunned. a wide red lane warns for 0.7 seconds. the dash then follows a
fixed direction at speed 11 for up to six units. step sideways to dodge.

`Charger.cs` follows lecture 10's charge sequence. the guard stops on contact,
rests for one second, then walks back to its post before charging again.
an attack cooldown prevents immediate repeat charges. a collider cast stops
fast movement before a wall; player contact during the dash deals one damage.
the uniform dims while stunned. all attack values are set in the inspector.

all three enemy types share the same level. the canvas submission asks for the
project or repository link, the enemy scripts and one narrated 2–3 minute mp4
showing the result, main steps, problems and solutions. AI_LOG.txt is the log
named by the canvas brief; DECISIONS.md and separate phone playtest clips are
not listed there.

## level revision

the route goes through the cell block, watch post and service wing. the first
divider opens at the bottom, the second at the top. the exit is at the bottom
of the service wing, so the old straight run along the upper edge is blocked.
the middle cover can block the shooter's sight and bullets.

player speed is 2.8. the shooter stays within one unit of its post, while trying
to keep its preferred distance. when a chase ends, the patroller selects the
nearest reachable waypoint. a collider cast checks that its body can follow
the path, including at wall corners.

these changes followed laptop feedback and automated movement checks. the author
played the revised level several times and lost once. no phone-test result is claimed.
item collection, inventory and the two full escape routes remain future work.

## repository

[the-escapes-mobile](https://github.com/Yernur-git/the-escapes-mobile)
