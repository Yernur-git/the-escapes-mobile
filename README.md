# the escapes

a planned top-down 2d prison escape game for mobile, in landscape orientation.
this repository starts with the project setup for assignment 8-9.

## setup

unity version: 6000.6.0f1.

add this folder through unity hub. unity creates the local cache on first import.
the project uses force text serialization and visible meta files.

## current stage

open `Assets/Scenes/MainMenu.unity` and press play.
the menu has play, settings and quit buttons. play opens level select.
quit stops play mode in the editor and closes a build.

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
it on a wall hit or after four seconds. the prefab references its balance
asset in `Assets/Data/EnemyBullet.asset`.

## assignment 10, third stage

the green charger guards the lower route. its states are idle, wind-up, dash
and stunned. a wide red lane warns for 0.7 seconds. the dash then follows a
fixed direction at speed 11 for up to six units. step sideways to dodge.

`Charger.cs` follows lecture 10's charge sequence. the guard stops on contact,
rests for one second, then walks back to its post before charging again.
an attack cooldown prevents immediate repeat charges. a collider cast stops
fast movement before a wall; player contact during the dash deals one damage.
the uniform dims while stunned. all attack values are set in the inspector.

the third data level uses all three enemy types. the canvas submission asks for the
project or repository link, the enemy scripts and one narrated 2–3 minute mp4
showing the result, main steps, problems and solutions. AI_LOG.txt is the log
named by the canvas brief; DECISIONS.md and separate phone playtest clips are
not listed there.

## assignment 10 layout revision

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

## assignment 11, balance data

`Assets/Data` holds five ScriptableObject assets: Patroller, Shooter, Charger,
Player and EnemyBullet. select an asset to edit its values in the inspector.
the matching stats classes are in `Assets/Scripts/Data` and use lecture 11's
CreateAssetMenu pattern.

the enemy scripts read their movement, sight and attack values from these assets.
PlayerMovement and PlayerHealth share the Player asset. EnemyBullet reads its
speed, damage and lifetime from the EnemyBullet asset through the prefab.
current health, state and timers stay on each scene object; gameplay does not
write them into shared data. the values match the previous level setup.

## assignment 11, saved progress

the main menu shows successful escapes and the best number of hearts left at
the exit. winning adds one escape and keeps the higher heart result. getting
caught does not add progress. restart starts a new attempt with full health.

`Assets/Scripts/Data/SaveData.cs` holds the version and progress fields.
`SaveSystem.cs` uses lecture 11's JsonUtility and persistentDataPath examples.
it writes `save.tmp`, then moves it on the first save or replaces `save.json`
on later saves. a missing, empty or broken file loads default progress.

`SaveManager.cs` loads in Awake and saves after a win and when the app goes
into the background. each scene has its own manager, so returning to the menu
reads the saved result. `LevelResult.cs` reports a victory once and passes
PlayerHealth's remaining hearts and the selected level to the manager.

on windows, the file is `%USERPROFILE%/AppData/LocalLow/Yernur/the escapes/save.json`.
progress is local to the device; it is not a cloud save or a mid-run checkpoint.

## assignment 11, levels from data

`Assets/Data/Levels` contains Level_01, Level_02 and Level_03. each references
its own layout prefab in `Assets/Prefabs/Levels`, including room floors, walls
and solid cover. all three use the reach-exit goal. there are no key pickups,
switch objectives or inventory requirements yet.

- night shift: cells, infirmary, laundry, yard and storage. two patrollers and
  one shooter; 90 seconds.
- loading sector: storage, workshop, loading lanes and service rooms. two
  patrollers, two shooters and one charger; 120 seconds.
- high security: nine connected rooms with a central canteen and side routes.
  three patrollers, two shooters and two chargers; 150 seconds.

`LevelData.cs` stores the goal, time, player and exit positions, enemy spawn
points, layout and map size. each wave references an enemy prefab and holds
its count, delay, starting direction and patrol points. every patroller has its
own route. `LevelLoader.cs` follows lecture 11's coroutine and wave
loops. it reads `GameSession.levelIndex` and builds the selected level in the
same Prison scene. `GameTimer.cs` counts down the asset's time limit, stops on
pause or a result, and ends the attempt if time runs out. `CameraFollow.cs`
keeps the player in view and clamps the camera to the loaded map boundaries.

Patroller, Shooter and Charger prefabs reference their existing stats assets.
they start inactive so the loader can assign the player and patrol route before
Awake runs. it then activates them; the shooter and charger record their new
spawn position as home. the scene no longer holds placed guard instances.
a level-one layout preview is visible before play; the loader disables and
removes it before creating the selected runtime layout.

play opens the level-select panel. `LevelSelect.cs` reads level names and time
limits from the three assets. level one is available on a fresh save. later
buttons show which previous level must be finished and stay disabled until it is.

`SaveData.unlockedLevels` stores level numbers starting at one; GameSession uses
array indices starting at zero. `SaveManager.LevelCompleted` opens only the next
existing level and saves immediately. replaying level one can add another escape
but does not open level three. completing the final level never creates level four.
`LevelLoader` checks the saved unlocks before loading, including when Prison is
opened directly. an invalid or locked selection falls back to level one.

old save files keep their escape count and best hearts. a missing, null or empty
unlock list makes level one available; earlier escape counts alone do not prove
which level was completed. the panel's back button returns to the main menu and
restores keyboard/gamepad focus to play.

## assignment 11, settings and reset

settings has two working preferences: guard state labels and the on-screen
joystick. labels are off by default and touch controls are on. `SettingsMenu.cs`
uses the lecture's PlayerPrefs GetInt, SetInt and Save pattern. values are saved
when closing settings and when the open settings panel receives an app pause.
enemy scripts read the label preference when spawned; `GamePreferences.cs`
applies the joystick preference in the game scene. keyboard and gamepad movement
continue to work when the joystick is hidden.

reset progress opens a full-screen confirmation showing the current escape count
and best hearts, and warns that opened levels and settings will be reset. cancel
keeps both progress and preferences. keyboard navigation stays within the current
panel and initially selects cancel in the confirmation.

`ResetProgress.cs` follows lecture 11's reset sequence: delete save.json and the
temporary file, delete the game's guardLabels, touchControls, MusicVol and SFXVol keys, save
PlayerPrefs, clear SaveManager's data in RAM and reload the menu. level one stays
open. an app pause after reset can save defaults but cannot restore old progress.
if file deletion fails, the confirmation stays open with an error message.

## assignment 12, music foundation

`Assets/Audio/GameMix.mixer` has Master, Music, SFX and the Music child Tracks.
the two music sources output to Tracks, so Music controls both together. the
initial music group volume is -10 dB. music imports stay stereo and use Streaming
with Vorbis compression for mobile memory use.

`MusicManager.cs` follows lecture 12's two-source crossfade and DontDestroyOnLoad
pattern. one manager survives scene changes. a repeated request for the current
track keeps its playback position. a one-second crossfade uses unscaled time, so
it continues when game time is stopped. interrupted transitions start from the
current gains and reuse a still-playing track when reversing the fade.

`SceneMusic.cs` is on its own scene object. it requests the menu or level clip in
Start; it stays separate from the duplicate manager that destroys itself on reload.
settings and pause use the same persistent mixer.

## assignment 12, volume and pause

settings has music and sound-effects sliders with a 0.0001-1 range and percentage
labels. `VolumeSlider.cs` follows lecture 12: Log10(value) * 20 converts the value
to decibels, then PlayerPrefs remembers it. half volume is about -6 dB, 10% is
-20 dB and the minimum is -80 dB. left/right adjusts a focused slider; up/down
moves through the settings controls.

`MusicManager.Start` applies both saved values even while settings is hidden.
opening the panel refreshes its positions and labels. closing settings, pausing
the app or quitting saves preferences. the initial mix stays at -10 dB for music
and 0 dB for effects.

`PauseAudio.cs` switches between Normal and Paused snapshots over 0.3 seconds.
Tracks has volume 0 dB and Lowpass Simple cutoff 22000 Hz in Normal, and -6 dB
and 800 Hz in Paused. transitions use UnscaledTime so they finish at timeScale 0.
the Music and SFX parameters stay separate from the snapshot, preserving the
player's volume settings. effects and button clicks remain clear during pause.

`PauseMenu.cs` requests the snapshot on pause/resume and restores Normal before
retry or returning to the menu. `SceneMusic.cs` also restores Normal on scene
entry. reset deletes the two volume keys and immediately reapplies defaults to
the surviving MusicManager, before the menu reloads. unrelated preferences stay.

## assignment 12, sound events

`SfxPlayer.cs` uses the lecture's PlayOneShot pattern with one AudioSource per
object, routed to SFX. steps use three clips and hits use two, without the same
clip twice in a row. repeated effects vary pitch between 0.92 and 1.08.

`PlayerMovement.cs` counts actual distance travelled for footsteps. holding a
direction against a wall does not keep generating steps. `PlayerHealth.cs`
plays a hit only when damage is accepted; a fatal hit plays the capture cue.
`LevelResult.cs` plays win or lose once, including when the result stops game
time. there is no jump or collectible in this top-down game.

Patroller plays its warning on entering Attack, Shooter on entering Aim and
Charger on entering WindUp. each has a different warning and attack clip.
warnings can start within a 15% margin outside the camera view; an offscreen
guard cannot land its melee hit, fire a bullet or start a dash. the existing
wind-up delays are unchanged.

`ButtonSound.cs` is the first onClick action on each menu and game button.
it calls a persistent UI source on MusicManager, so a scene change cannot cut
off the click. this also works with keyboard and gamepad button activation.

## sounds and music

| project file | use | title | author and license |
| --- | --- | --- | --- |
| `Assets/Audio/Music/menu_loop.ogg` | main menu and level selection | [Sneaky Snitch](https://incompetech.com/music/royalty-free/index.html?isrc=USUAN1100772) | Kevin MacLeod, CC BY 4.0 |
| `Assets/Audio/Music/level_loop.ogg` | Prison, all three levels | [Sneaky Adventure](https://incompetech.com/music/royalty-free/index.html?isrc=USUAN1100644) | Kevin MacLeod, CC BY 4.0 |

"Sneaky Snitch" and "Sneaky Adventure" Kevin MacLeod (incompetech.com).
Licensed under [Creative Commons: By Attribution 4.0 License](https://creativecommons.org/licenses/by/4.0/).
the tracks use loop excerpts with a 15 ms blend at the wrap, reduced gain and
stereo OGG Vorbis encoding. exact ranges and download URLs are in
`Assets/Audio/LICENSES.txt`.

15 effects in `Assets/Audio/SFX` cover 12 event types: footsteps, ui clicks,
damage, capture, win, lose, three guard warnings and three guard attacks.
the files come from the owner's purchased library at
`C:/media/SOUND EFFECTS`; the pack title, publisher and license terms have not
been supplied. each original path is listed in `Assets/Audio/LICENSES.txt`.
effects are trimmed mono WAV files, imported with ADPCM and DecompressOnLoad.

all effects below share that owner-supplied source and unconfirmed pack license;
the original filenames are relative to `C:/media/SOUND EFFECTS`.

| project sound | original file | event |
| --- | --- | --- |
| `step_01.wav` | `Knocking Door/Single Footstep Tap 01.wav` | movement |
| `step_02.wav` | `Knocking Door/Single Footstep Tap 02.wav` | movement variant |
| `step_03.wav` | `Knocking Door/Single Footstep Tap 03.wav` | movement variant |
| `ui_click.wav` | `Computer/Click1.wav` | button activation |
| `hurt_01.wav` | `Hit/PUNCH.mp3` | accepted damage |
| `hurt_02.wav` | `Hit/PUNCH #3.mp3` | damage variant |
| `patrol_warning.wav` | `Alerts, Error, Correct/Alert.wav` | Patroller wind-up |
| `shooter_warning.wav` | `Weapons/Gun Click (#1).wav` | Shooter wind-up |
| `charge_warning.wav` | `Whooshs/Whoosh Light Short.wav` | Charger wind-up |
| `melee_swing.wav` | `Whooshs/whoosh_swish_small_01.wav` | Patroller attack |
| `shot.wav` | `Weapons/Shot.wav` | Shooter fires |
| `dash.wav` | `Whooshs/whoosh_swish_high_fast_01.wav` | Charger starts dash |
| `caught.wav` | `Cell Door/Jail Door Closed (1).wav` | player death/capture |
| `win.wav` | `Prize or Achievement/Bells 1 Upward 01.wav` | win |
| `lose.wav` | `Prize or Achievement/Bells 1 Downward.wav` | lose |

## repository

[the-escapes-mobile](https://github.com/Yernur-git/the-escapes-mobile)
