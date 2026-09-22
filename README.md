# MissionSystem README

This portfolio showcases an NPC-based mission system built with Unity 6 and C#. The project demonstrates clean code architecture rather than a complete game. It is an architectural port of the [Unreal Engine 5 / C++ version](https://github.com/inhopark/MissionSystem_UE5) of the same system.

## Core Flow

The system follows a four-stage cycle:
1. Player overlaps with NPC → dialogue displays
2. Upon acceptance → quarter-view defense mini-game starts with monster waves
3. Player survives timer → success; HP reaches zero → failure
4. Results screen → player returns to original location with restored HP

## Automated Testing

A `MissionAutoCycleController` enables regression testing without manual gameplay. Pressing **F8** toggles this feature, which calls existing public methods on `MissionManager` — mirroring actual player input while leaving core logic untouched.

## Design Patterns Applied

| Pattern | Location |
|---|---|
| State Machine | Ready → InProgress → Succeeded/Failed states (`BaseMission`) |
| Factory Method | Mission type mapping via `MissionUnique` (`MissionFactory`) |
| Template Method | `DefenseMinigameMission` base class for defense game variants |
| Object Pool | Monster deactivation/reuse instead of destroy (`MonsterSpawner`) |
| Facade | `MissionManager` is the single hub NPCs and widgets talk to |

## Technology Stack

- Unity 6000.3.10f1, C#
- Input System package (action-based input, no legacy Input Manager)
- uGUI, driven by a small set of typed widget components (`MainMissionWidget`, `MissionResultWidget`, `HPWidget`, `MissionTimerWidget`)

## Build Instructions

Clone the repository, open the project folder with Unity Hub (Unity 6000.3.10f1 or a later 6.x release), open `Assets/Scenes/SampleScene.unity`, and press Play.
