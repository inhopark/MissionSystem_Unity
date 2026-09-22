# MissionSystem README

이 포트폴리오는 Unity 6와 C#으로 구현한 NPC 기반 미션 시스템을 보여줍니다. 완성된 게임이 아니라 클린 코드 아키텍처를 보여주는 것이 목적이며, 동일 시스템의 [Unreal Engine 5 / C++ 버전](https://github.com/inhopark/MissionSystem_UE5)을 아키텍처 그대로 이식한 프로젝트입니다.

## 핵심 흐름 (Core Flow)

시스템은 4단계 사이클로 동작합니다:
1. 플레이어가 NPC와 접촉 → 대화창 표시
2. 수락 시 → 쿼터뷰 디펜스 미니게임 시작, 몬스터 웨이브 등장
3. 타이머 동안 생존하면 성공, HP가 0이 되면 실패
4. 결과 화면 표시 → 플레이어가 원래 위치로 복귀하며 HP 회복

## 자동 테스트 (Automated Testing)

`MissionAutoCycleController`는 수동 플레이 없이 회귀 테스트를 할 수 있게 해줍니다. **F8** 키를 누르면 이 기능이 토글되며, `MissionManager`에 이미 존재하는 public 메서드들을 그대로 호출합니다 — 실제 플레이어 입력을 그대로 모방하면서 핵심 로직은 건드리지 않습니다.

## 적용된 디자인 패턴 (Design Patterns Applied)

| 패턴 | 위치 |
|---|---|
| State Machine | Ready → InProgress → Succeeded/Failed 상태 (`BaseMission`) |
| Factory Method | `MissionUnique`를 통한 미션 타입 매핑 (`MissionFactory`) |
| Template Method | 디펜스 게임 변형을 위한 `DefenseMinigameMission` 베이스 클래스 |
| Object Pool | 몬스터를 destroy 대신 비활성화 후 재사용 (`MonsterSpawner`) |
| Facade | NPC와 위젯이 소통하는 단일 창구 역할의 `MissionManager` |

## 기술 스택 (Technology Stack)

- Unity 6000.3.10f1, C#
- Input System 패키지 (레거시 Input Manager 미사용, 액션 기반 입력)
- uGUI, 타입이 명확한 소수의 위젯 컴포넌트(`MainMissionWidget`, `MissionResultWidget`, `HPWidget`, `MissionTimerWidget`)로 구동

## 빌드 방법 (Build Instructions)

저장소를 clone한 뒤 Unity Hub로 프로젝트 폴더를 열고(Unity 6000.3.10f1 이상의 6.x 버전), `Assets/Scenes/SampleScene.unity`를 연 다음 Play를 누르세요.
