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
room. quit stops play mode in the editor and closes a build.

`Assets/Scripts/MenuButtons.cs` uses the play and quit examples from assignment 3,
with the scene name changed to `Prison`. `SafeArea.cs` uses the lecture 8-9 example.
the menu canvas uses 1920 x 1080, match height, and centred anchors inside SafeArea.

the hud shows three hearts, an initial `items 0` counter and an on-screen stick.
move with wasd, arrow keys, a gamepad left stick or by dragging the stick handle.
walls keep the player inside the room. there is no collection or damage system yet.

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
guards, item collection and the escape objective are outside this setup stage.

## repository

[the-escapes-mobile](https://github.com/Yernur-git/the-escapes-mobile)
