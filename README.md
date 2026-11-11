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

reach the green exit while avoiding the patroller. the guard follows four
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
states. it tries to stay about five units from the player. a red line warns
for 0.6 seconds, then a slow bullet travels at five units per second. the
shooter waits 1.5 seconds before preparing another shot.

`Shooter.cs` controls distance, aiming and firing. walls block detection and
stop the aim line. it cannot start an attack outside the camera view.
`EnemyBullet.cs` moves the projectile, damages the player on contact and removes
it on a wall hit or after four seconds. its speed, damage and lifetime are
set on `Assets/Prefabs/EnemyBullet.prefab`.

the level now has two enemy types. the charger is the next stage.
item collection, inventory and the two full escape routes remain future work.

## repository

[the-escapes-mobile](https://github.com/Yernur-git/the-escapes-mobile)
