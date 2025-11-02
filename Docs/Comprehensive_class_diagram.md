# Trolls et Paillettes - Comprehensive Class Diagram

**Project**: Unity 2D RPG Game  
**Team**: Élodie, Louis & Sergey  
**Date**: November 2024  
**Architecture**: Complete System Overview (All Classes)

---

## Mermaid Class Diagram

```mermaid
classDiagram
    %% ========================================
    %% CORE MANAGERS (Singletons)
    %% ========================================
    class TeamManager {
        <<MonoBehaviour>>
        <<Singleton>>
        +static TeamManager Instance
        +List~CharacterData~ availableCharacters
        +List~TeamMember~ currentTeam
        +int activeCharacterIndex
        +int maxTeamSize
        +Awake() void
        +Start() void
        +AddCharacterToTeam(characterData) bool
        +RemoveCharacterFromTeam(characterData) bool
        +GetActiveCharacter() TeamMember
        +HasAliveCharacters() bool
        +ResetTeam() void
        +ResetTeamForNewBattle() void
    }
    
    class SaveManager {
        <<MonoBehaviour>>
        <<Singleton>>
        +static SaveManager Instance
        +string saveFileName
        +PlayerController playerController
        +Inventory playerInventory
        +MessageDisplay messageDisplay
        +bool autoSaveOnVictory
        -string SaveFilePath
        -float playTime
        -static bool skipAutoLoad
        +Awake() void
        +Start() void
        +Update() void
        +SaveGame() void
        +LoadGame() void
        +AutoSave() void
        +DeleteSave() void
        +DeleteSave(showMessage) void
        +SaveFileExists() bool
        +GetSaveFilePath() string
        +static SetSkipAutoLoad() void
        -ShowMessage(message) void
    }
    
    class AudioManager {
        <<MonoBehaviour>>
        <<Singleton>>
        +static AudioManager Instance
        +AudioSource musicSource
        +AudioSource sfxSource
        +AudioClip mainSoundtrack
        +AudioClip attackSound
        +AudioClip hitSound
        +AudioClip superAttackSound
        +AudioClip victorySound
        +AudioClip defeatSound
        +Awake() void
        +Start() void
        +PlayMainSoundtrack() void
        +PlayAttackSound() void
        +PlayHitSound() void
        +PlaySuperAttackSound() void
        +PlayVictorySound() void
        +PlayDefeatSound() void
        +UpdateVolumes() void
        +StopMusic() void
        +ResumeMusic() void
        -SetupAudioSources() void
        -ApplyVolumeSettings() void
        -PlaySFX(clip, soundName) void
    }
    
    %% ========================================
    %% BATTLE SYSTEM
    %% ========================================
    class BattleManager {
        <<MonoBehaviour>>
        +BattleCharacter playerCharacter
        +BattleCharacter enemyCharacter
        +TeamManager teamManager
        +int currentPlayerIndex
        +Button attackButton
        +Button superAttackButton
        +Button switchCharacterButton
        +TextMeshProUGUI currentCharacterText
        +bool isPlayerTurn
        +bool battleEnded
        +float enemyTurnDelay
        +SpriteRenderer battleBackground
        +SpriteRenderer enemySpriteRenderer
        +Sprite battleground1
        +Sprite battleground2
        +Sprite battleground3
        +Sprite troll1Sprite
        +Sprite troll2Sprite
        +Sprite troll3Sprite
        +MessageDisplay messageDisplay
        +CameraShake cameraShake
        +ParticleSystem hitEffect
        +ParticleSystem superAttackEffect
        +ParticleSystem victoryEffect
        +string gameOverSceneName
        +Start() void
        -ShowMessage(message) void
        -InitializeBattle() void
        -SetupButtonListeners() void
        -SetupTeamManagement() void
        -LoadPlayerData() void
        -LoadEnemyData() void
        +OnAttackButtonClicked() void
        +OnSuperAttackButtonClicked() void
        +OnSwitchCharacterClicked() void
        -StartPlayerTurn() void
        -SetButtonsEnabled(enabled) void
        -StartEnemyTurn() IEnumerator
        -PerformEnemyAction() void
        -CheckBattleEnd() void
        -EndBattle() void
        -ShowGameOver() void
        -ReinitializeGameOverAfterLoad() IEnumerator
        -SwitchToNextCharacter() void
        -SwitchToCharacter(characterIndex) void
        -SaveCurrentCharacterStats() void
        -UpdateCurrentCharacterDisplay() void
        -CheckTeamDefeat() void
        -GetTrollName(index) string
    }
    
    class BattleCharacter {
        <<MonoBehaviour>>
        +string characterName
        +int maxHealth
        +int currentHealth
        +int attackPower
        +int defense
        +int maxMana
        +int currentMana
        +Sprite characterSprite
        +Slider healthBar
        +Slider manaBar
        +bool isPlayer
        +bool isAlive
        +DamageFlash damageFlash
        +AttackAnimation attackAnimation
        +Start() void
        +InitializeStats() void
        +TakeDamage(damage) void
        +BasicAttack() int
        +SuperAttack() int
        +Heal(healAmount) void
        +RestoreMana(amount) void
        +CanSuperAttack() bool
        +GetHealthPercentage() float
        +GetManaPercentage() float
        +UpdateSprite(newSprite) void
    }
    
    %% ========================================
    %% MAP SYSTEM
    %% ========================================
    class PlayerController {
        <<MonoBehaviour>>
        +float moveSpeed
        +Animator animator
        +SpriteRenderer spriteRenderer
        +BoxCollider2D playerCollider
        +float bushAlpha
        +Inventory inventory
        +int maxHealth
        +int currentHealth
        +int maxMana
        +int currentMana
        +MessageDisplay messageDisplay
        -bool hasTriggeredBattle
        -string lastInteractedZone
        -float originalAlpha
        -bool isInBush
        -HashSet~string~ collectedObjects
        -bool statsInitialized
        +Start() void
        +Update() void
        +TriggerBattle(zoneName, enemyIndex, bgIndex) void
        +GetCollectedObjects() List~string~
        +SetCollectedObjects(objects) void
        +GetTrollIndexFromZone(zoneName) int
        -ShowMessage(message) void
        -HandleMovement() void
        -HandleInteraction() void
        -OnTriggerEnter2D(collision) void
        -OnTriggerExit2D(collision) void
        -UpdateStatsFromBattle() void
    }
    
    class BattleZone {
        <<MonoBehaviour>>
        +int zoneIndex
        +OnTriggerEnter2D(other) void
    }
    
    %% ========================================
    %% CHARACTER DATA SYSTEM
    %% ========================================
    class CharacterData {
        <<ScriptableObject>>
        +string characterName
        +string description
        +Sprite characterSprite
        +Sprite characterPortrait
        +int baseHealth
        +int baseMana
        +int baseAttack
        +int baseDefense
        +string specialAbilityName
        +string specialAbilityDescription
        +int specialAbilityCost
        +int specialAbilityDamage
        +CharacterType characterType
        +string[] characterQuotes
    }
    
    class TeamMember {
        <<Serializable>>
        +CharacterData characterData
        +int currentHealth
        +int currentMana
        +bool isDefeated
        +TeamMember(data)
    }
    
    %% ========================================
    %% INVENTORY SYSTEM
    %% ========================================
    class Inventory {
        <<MonoBehaviour>>
        +List~Item~ items
        +int startingHealthPotions
        +int startingManaPotions
        +Action~Item~ OnItemAdded
        +Action~Item~ OnItemUsed
        -bool hasInitialized
        +Start() void
        +AddItem(newItem) void
        +UseItem(item) bool
        +GetItem(itemName) Item
        +GetItemsOfType(type) List~Item~
        +HasHealthPotion() bool
        +HasManaPotion() bool
        +UseHealthPotion() int
        +UseManaPotion() int
        +GetTotalItemCount() int
        +GetItemQuantity(itemType) int
        +MarkAsInitialized() void
    }
    
    class Item {
        <<Serializable>>
        +string name
        +string description
        +ItemType type
        +int quantity
        +int value
        +Item(itemName, desc, itemType, itemValue, qty)
    }
    
    %% ========================================
    %% UI SYSTEM
    %% ========================================
    class CharacterSelectionUI {
        <<MonoBehaviour>>
        +GameObject characterSelectionPanel
        +GameObject teamManagementPanel
        +Transform characterButtonParent
        +GameObject characterButtonPrefab
        +Transform teamMemberParent
        +GameObject teamMemberPrefab
        +TextMeshProUGUI characterNameText
        +TextMeshProUGUI characterDescriptionText
        +Image characterPortraitImage
        +TextMeshProUGUI healthText
        +TextMeshProUGUI manaText
        +TextMeshProUGUI attackText
        +TextMeshProUGUI defenseText
        +TextMeshProUGUI specialAbilityNameText
        +TextMeshProUGUI specialAbilityDescriptionText
        +TextMeshProUGUI teamSizeText
        +Button startBattleButton
        +Button addCharacterButton
        +Button removeCharacterButton
        +Button confirmTeamButton
        -List~GameObject~ characterButtons
        -CharacterData selectedCharacter
        +Awake() void
        +Start() void
        -InitializeUI() void
        -CreateCharacterButtons() void
        -RefreshTeamDisplay() void
        -SelectCharacter(character) void
        -AddCharacterToTeam() void
        -RemoveCharacterFromTeam() void
        -ConfirmTeam() void
        -StartBattle() void
        -UpdateCharacterInfoDisplay() void
    }
    
    class MainMenuManager {
        <<MonoBehaviour>>
        +Button newGameButton
        +Button loadGameButton
        +Button settingsButton
        +Button quitButton
        +GameObject mainMenuPanel
        +GameObject settingsPanel
        +Slider masterVolumeSlider
        +Slider musicVolumeSlider
        +Slider sfxVolumeSlider
        +Button backToMenuButton
        +AudioSource backgroundMusicSource
        +string mapSceneName
        +string characterSelectionSceneName
        +Start() void
        +OnNewGameClicked() void
        +OnLoadGameClicked() void
        +OnSettingsClicked() void
        +OnQuitClicked() void
        +static GetMusicVolume() float
        +static GetSFXVolume() float
        +static GetMasterVolume() float
        -SetupButtonListeners() void
        -SetupSettingsListeners() void
        -InitializeAudioSettings() void
        -OnMasterVolumeChanged(value) void
        -OnMusicVolumeChanged(value) void
        -OnSFXVolumeChanged(value) void
        -OnBackToMenuClicked() void
        -ShowMainMenu() void
        -ShowSettings() void
        -PlayBackgroundMusic() void
    }
    
    class GameOverManager {
        <<MonoBehaviour>>
        +GameObject gameOverPanel
        +TextMeshProUGUI gameOverTitle
        +TextMeshProUGUI defeatMessage
        +TextMeshProUGUI statisticsText
        +Image backgroundOverlay
        +Button restartBattleButton
        +Button mainMenuButton
        +Button quitGameButton
        +string mainMenuSceneName
        +Start() void
        +OnRestartBattleClicked() void
        +OnMainMenuClicked() void
        +MainMenuButtonClicked() void
        +GoToMainMenu() void
        +LoadMainMenu() void
        +OnQuitGameClicked() void
        +TestButtonClick() void
        +ShowGameOver(battleZoneName, defeatedTrollsCount) void
        +ForceReinitialize() void
        -UpdateStatistics() void
        -SetupButtons() void
        -GetCleanZoneName(originalName) string
        -CheckEventSystem() void
        -ForceButtonClickable() void
    }
    
    class MessageDisplay {
        <<MonoBehaviour>>
        +TextMeshProUGUI messageText
        +Image messageBackground
        +float displayDuration
        +float fadeInDuration
        +float fadeOutDuration
        +int maxMessages
        -Queue~string~ messageQueue
        -bool isDisplaying
        +Start() void
        +ShowMessage(message) void
        +ClearMessages() void
        -DisplayMessages() IEnumerator
        -ShowSingleMessage(message) IEnumerator
        -FadeIn() IEnumerator
        -FadeOut() IEnumerator
    }
    
    class ItemCounter {
        <<MonoBehaviour>>
        +TextMeshProUGUI healthPotionText
        +TextMeshProUGUI manaPotionText
        +Inventory playerInventory
        +string healthPotionLabel
        +string manaPotionLabel
        +Start() void
        +Update() void
        +ForceUpdate() void
        -UpdateItemCounters() void
    }
    
    %% ========================================
    %% VISUAL EFFECTS
    %% ========================================
    class DamageFlash {
        <<MonoBehaviour>>
        +float flashDuration
        +Color flashColor
        +int flashCount
        -SpriteRenderer spriteRenderer
        -Color originalColor
        -bool isFlashing
        +Start() void
        +Flash() void
        +StopFlash() void
        -FlashCoroutine() IEnumerator
    }
    
    class AttackAnimation {
        <<MonoBehaviour>>
        +float attackMoveDistance
        +float attackMoveDuration
        +float returnDuration
        -Vector3 originalPosition
        -bool isAnimating
        +Start() void
        +PlayAttackAnimation(isPlayer) void
        +PlaySuperAttackAnimation(isPlayer) void
        -AttackAnimationCoroutine(isPlayer) IEnumerator
        -SuperAttackAnimationCoroutine(isPlayer) IEnumerator
    }
    
    class CameraShake {
        <<MonoBehaviour>>
        +float shakeDuration
        +float shakeMagnitude
        +float dampingSpeed
        -Vector3 initialPosition
        -float currentShakeDuration
        +Start() void
        +Update() void
        +TriggerShake() void
        +TriggerShake(duration, magnitude) void
    }
    
    class CameraFollow {
        <<MonoBehaviour>>
        +Transform target
        +float smoothSpeed
        +Vector3 offset
        +LateUpdate() void
    }
    
    %% ========================================
    %% DATA CLASSES
    %% ========================================
    class BattleData {
        <<static>>
        +static int enemyToFightIndex
        +static int battleBackgroundIndex
        +static string battleZoneName
        +static int playerCurrentHealth
        +static int playerMaxHealth
        +static int playerCurrentMana
        +static int playerMaxMana
        +static int healthPotionCount
        +static int manaPotionCount
        +static SetupBattle(enemy, background, zoneName) void
        +static UpdatePlayerStats(health, maxHealth, mana, maxMana) void
        +static ResetBattleData() void
    }
    
    class SaveData {
        <<Serializable>>
        +int playerHealth
        +int playerMaxHealth
        +int playerMana
        +int playerMaxMana
        +float playerPositionX
        +float playerPositionY
        +int healthPotionCount
        +int manaPotionCount
        +List~int~ defeatedTrolls
        +List~string~ collectedObjects
        +string saveTime
        +float playTime
        +SaveData()
    }
    
    class GameProgress {
        <<static>>
        -static HashSet~int~ defeatedTrolls
        -static bool gameCompleted
        +static DefeatTroll(trollIndex) void
        +static IsTrollDefeated(trollIndex) bool
        +static IsGameCompleted() bool
        +static GetDefeatedTrollCount() int
        +static GetDefeatedTrolls() List~int~
        +static ResetProgress() void
        +static GetVictoryMessage(trollIndex) string
        -static GetTrollName(trollIndex) string
    }
    
    %% ========================================
    %% TEST & DEBUG CLASSES
    %% ========================================
    class GameOverTest {
        <<MonoBehaviour>>
        +Start() void
        -TestGameOverManager() void
    }
    
    class GameOverTester {
        <<MonoBehaviour>>
        +Button testDefeatButton
        +Button testVictoryButton
        +Button resetProgressButton
        +string gameOverSceneName
        +Start() void
        -SetupTestButtons() void
        -SimulateDefeat() void
        -SimulateVictory() void
        -ResetProgress() void
    }
    
    class SimpleGameOverManager {
        <<MonoBehaviour>>
        +Button mainMenuButton
        +string mainMenuSceneName
        +Start() void
        +OnMainMenuClicked() void
    }
    
    class TestScript {
        <<MonoBehaviour>>
        +Start() void
        +Update() void
    }
    
    class AudioManagerSetup {
        <<MonoBehaviour>>
        +Start() void
    }
    
    %% ========================================
    %% UNITY EDITOR (ReadOnly/Tutorial)
    %% ========================================
    class ReadmeEditor {
        <<UnityEditor>>
        +OnEnable() void
        +OnGUI() void
        +DrawHeader(readme) void
        +DrawSection(section) void
    }
    
    class Readme {
        <<ScriptableObject>>
        +Texture2D icon
        +string title
        +Section[] sections
    }
    
    %% ========================================
    %% ENUMERATIONS
    %% ========================================
    class CharacterType {
        <<enumeration>>
        Warrior
        Mage
        Rogue
    }
    
    class ItemType {
        <<enumeration>>
        HealthPotion
        ManaPotion
    }
    
    %% ========================================
    %% RELATIONSHIPS - CORE MANAGERS
    %% ========================================
    TeamManager *-- TeamMember : manages
    TeamManager o-- CharacterData : references
    SaveManager o-- PlayerController : saves/loads
    SaveManager o-- Inventory : saves/loads
    SaveManager ..> SaveData : uses
    SaveManager ..> GameProgress : uses
    SaveManager o-- MessageDisplay : displays
    AudioManager ..> MainMenuManager : reads settings
    
    %% ========================================
    %% RELATIONSHIPS - BATTLE SYSTEM
    %% ========================================
    BattleManager o-- BattleCharacter : controls
    BattleManager o-- TeamManager : uses
    BattleManager ..> BattleData : reads
    BattleManager ..> GameProgress : updates
    BattleManager o-- MessageDisplay : displays
    BattleManager o-- CameraShake : triggers
    BattleManager ..> SaveManager : auto-saves
    BattleManager ..> AudioManager : plays sounds
    BattleCharacter o-- DamageFlash : uses
    BattleCharacter o-- AttackAnimation : uses
    
    %% ========================================
    %% RELATIONSHIPS - MAP SYSTEM
    %% ========================================
    PlayerController *-- Inventory : owns
    PlayerController o-- MessageDisplay : displays
    PlayerController ..> BattleData : sets up
    PlayerController ..> GameProgress : checks
    BattleZone ..> BattleData : triggers
    CameraFollow ..> PlayerController : follows
    
    %% ========================================
    %% RELATIONSHIPS - CHARACTER DATA
    %% ========================================
    TeamMember o-- CharacterData : references
    CharacterData ..> CharacterType : uses
    
    %% ========================================
    %% RELATIONSHIPS - INVENTORY
    %% ========================================
    Inventory *-- Item : contains
    Item ..> ItemType : uses
    ItemCounter o-- Inventory : displays
    
    %% ========================================
    %% RELATIONSHIPS - UI
    %% ========================================
    CharacterSelectionUI ..> TeamManager : manages
    CharacterSelectionUI ..> CharacterData : displays
    MainMenuManager ..> SaveManager : triggers
    MainMenuManager ..> TeamManager : resets
    MainMenuManager ..> GameProgress : resets
    MainMenuManager ..> AudioManager : controls
    GameOverManager ..> GameProgress : reads
    GameOverManager ..> BattleData : reads
    
    %% ========================================
    %% RELATIONSHIPS - TEST CLASSES
    %% ========================================
    GameOverTest ..> GameOverManager : tests
    GameOverTester ..> GameProgress : simulates
    SimpleGameOverManager ..> GameOverManager : alternative
    AudioManagerSetup ..> AudioManager : tests
    
    %% ========================================
    %% RELATIONSHIPS - EDITOR
    %% ========================================
    ReadmeEditor ..> Readme : renders
```

---

## Complete Class Inventory

### Production Classes (25)

#### 🎯 **Core Managers (3 Singletons)**
1. `TeamManager` - Manages character team selection and active character
2. `SaveManager` - Handles save/load game data with JSON serialization
3. `AudioManager` - Controls music and sound effects throughout game

#### ⚔️ **Battle System (2)**
4. `BattleManager` - Orchestrates turn-based combat, manages battle flow
5. `BattleCharacter` - Represents combatants with stats, health, mana

#### 🗺️ **Map System (2)**
6. `PlayerController` - Handles player movement, collisions, battle triggers
7. `BattleZone` - Defines battle trigger zones on map

#### 👥 **Character Data System (2)**
8. `CharacterData` - ScriptableObject with character stats and abilities
9. `TeamMember` - Runtime instance of character with current state

#### 🎒 **Inventory System (2)**
10. `Inventory` - Manages list of items, handles usage
11. `Item` - Represents a single item with type and quantity

#### 🖼️ **UI System (5)**
12. `CharacterSelectionUI` - Team selection screen before battle
13. `MainMenuManager` - Main menu with New Game, Load, Settings, Quit
14. `GameOverManager` - Game over screen with statistics
15. `MessageDisplay` - Shows temporary messages with fade effects
16. `ItemCounter` - Displays item counts on HUD

#### ✨ **Visual Effects (4)**
17. `DamageFlash` - Flashes sprite when taking damage
18. `AttackAnimation` - Animates character movement during attacks
19. `CameraShake` - Shakes camera for impact effects
20. `CameraFollow` - Smoothly follows player on map

#### 💾 **Data Classes (3)**
21. `BattleData` - Static class for passing data between scenes
22. `SaveData` - Serializable class for JSON save files
23. `GameProgress` - Static class tracking defeated trolls and completion

#### 🏷️ **Enumerations (2)**
24. `CharacterType` - Warrior, Mage, Rogue
25. `ItemType` - HealthPotion, ManaPotion

---

### Test & Debug Classes (5)

26. `GameOverTest` - Tests GameOverManager initialization
27. `GameOverTester` - UI tester for simulating defeat/victory
28. `SimpleGameOverManager` - Simplified alternative for testing
29. `TestScript` - Generic test script
30. `AudioManagerSetup` - Tests AudioManager setup

---

### Unity Editor Classes (2)

31. `ReadmeEditor` - Custom inspector for Readme ScriptableObject
32. `Readme` - Tutorial/readme ScriptableObject

---

## **TOTAL: 32 Classes**

---

## Design Patterns Used

### 1. **Singleton Pattern** (3 classes)
- **TeamManager**: `static Instance`, `DontDestroyOnLoad`, manages team across scenes
- **SaveManager**: `static Instance`, `DontDestroyOnLoad`, persistent save system
- **AudioManager**: `static Instance`, `DontDestroyOnLoad`, persistent audio

**Benefits**:
- Global access from any script
- Persist across scene changes
- Single point of control

### 2. **Observer Pattern** (1 class)
- **Inventory**: `Action<Item> OnItemAdded`, `Action<Item> OnItemUsed`
- **UI components** (ItemCounter): Subscribe to inventory changes

**Benefits**:
- Loose coupling between inventory and UI
- Automatic UI updates

### 3. **Static Utility Classes** (3 classes)
- **BattleData**: Transfer battle configuration between scenes
- **GameProgress**: Global progress tracking without instance
- **No GameManager**: Distributed responsibility pattern

**Benefits**:
- Simple data passing between scenes
- No need for DontDestroyOnLoad for data
- Clear single responsibility

### 4. **ScriptableObject Pattern** (2 classes)
- **CharacterData**: Data-driven character configuration
- **Readme**: Tutorial content storage

**Benefits**:
- Easy iteration and balancing
- Inspector-editable data
- Reusable assets

### 5. **Component Pattern** (MonoBehaviour-heavy)
- **18 MonoBehaviour classes** for game logic
- Leverages Unity's component system
- Separation of visual effects, UI, and logic

**Benefits**:
- Unity's built-in lifecycle (Start, Update, OnTrigger)
- Inspector integration
- Modular and reusable

### 6. **Coroutine Pattern** (Used in multiple classes)
- `BattleManager.StartEnemyTurn()` - Delayed enemy actions
- `MessageDisplay.DisplayMessages()` - Timed message display
- `DamageFlash.FlashCoroutine()` - Flash animation
- `AttackAnimation.*Coroutine()` - Movement animations

**Benefits**:
- Asynchronous operations
- Timed sequences
- Animation control

---

## Detailed Method Counts

### Classes with Most Methods

| Class | Methods | Type | Complexity |
|-------|---------|------|------------|
| `BattleManager` | 25+ | MonoBehaviour | ⭐⭐⭐⭐⭐ High |
| `SaveManager` | 15+ | MonoBehaviour | ⭐⭐⭐⭐ Medium-High |
| `PlayerController` | 15+ | MonoBehaviour | ⭐⭐⭐⭐ Medium-High |
| `Inventory` | 13 | MonoBehaviour | ⭐⭐⭐ Medium |
| `MainMenuManager` | 15+ | MonoBehaviour | ⭐⭐⭐ Medium |
| `CharacterSelectionUI` | 12+ | MonoBehaviour | ⭐⭐⭐ Medium |
| `BattleCharacter` | 11 | MonoBehaviour | ⭐⭐ Low-Medium |
| `GameOverManager` | 13+ | MonoBehaviour | ⭐⭐⭐ Medium |

---

## Key Dependencies

### High-Level Dependency Graph

```
┌─────────────────────────────────────────┐
│         CORE SINGLETONS                  │
│  TeamManager │ SaveManager │ AudioManager│
└──────────────┬──────────────┬────────────┘
               │              │
        ┌──────┴──────┐   ┌──┴───────┐
        │             │   │          │
   ┌────▼────┐   ┌───▼──────┐   ┌──▼────┐
   │ Battle  │   │   Map    │   │  UI   │
   │ System  │   │  System  │   │System │
   └─────────┘   └──────────┘   └───────┘
        │              │             │
   ┌────▼────┐   ┌────▼────┐   ┌───▼────┐
   │Character│   │Inventory│   │Message │
   │  Data   │   │ System  │   │Display │
   └─────────┘   └─────────┘   └────────┘
        │              │
   ┌────▼────────────┬─┴─────────┐
   │ Visual Effects  │ Data Classes │
   └─────────────────┴──────────────┘
```

### Critical Dependencies

1. **BattleManager** depends on:
   - TeamManager (team management)
   - BattleCharacter (player/enemy)
   - BattleData (scene transfer)
   - GameProgress (troll tracking)
   - SaveManager (auto-save)
   - AudioManager (sound effects)
   - MessageDisplay (UI feedback)
   - CameraShake (visual feedback)

2. **SaveManager** depends on:
   - PlayerController (stats to save)
   - Inventory (items to save)
   - GameProgress (progress to save)
   - SaveData (serialization format)

3. **PlayerController** depends on:
   - Inventory (item management)
   - MessageDisplay (feedback)
   - BattleData (battle setup)
   - GameProgress (troll checking)

4. **CharacterSelectionUI** depends on:
   - TeamManager (team building)
   - CharacterData (character info)

---

## Scene Architecture

### Scene Flow

```
MainMenuScene
    ├─ MainMenuManager
    ├─ AudioManager (spawned)
    ├─ TeamManager (spawned)
    └─ SaveManager (spawned)
         │
         ├─→ New Game → MapScene
         └─→ Load Game → MapScene

MapScene
    ├─ PlayerController
    │   └─ Inventory
    ├─ CameraFollow
    ├─ ItemCounter
    ├─ MessageDisplay
    ├─ BattleZone (×3)
    └─ SaveManager (persistent)
         │
         └─→ Trigger Battle → BattleScene

BattleScene (doubles as Character Selection)
    ├─ BattleManager
    ├─ BattleCharacter (×2)
    │   ├─ DamageFlash
    │   └─ AttackAnimation
    ├─ CharacterSelectionUI
    ├─ CameraShake
    ├─ MessageDisplay
    └─ TeamManager (persistent)
         │
         ├─→ Victory → MapScene
         └─→ Defeat → GameOverScene

GameOverScene
    ├─ GameOverManager
    └─ BattleData (static data)
         │
         ├─→ Main Menu → MainMenuScene
         └─→ Restart Battle → BattleScene
```

---

## Data Flow Diagrams

### Save/Load Flow

```
SaveManager
    ↓ SaveGame()
    ├─ Read PlayerController stats
    ├─ Read Inventory items
    ├─ Read GameProgress trolls
    ├─ Serialize to SaveData
    └─ Write JSON to disk

SaveManager
    ↓ LoadGame()
    ├─ Read JSON from disk
    ├─ Deserialize to SaveData
    ├─ Restore PlayerController stats
    ├─ Restore Inventory items
    └─ Restore GameProgress trolls
```

### Battle Initialization Flow

```
PlayerController
    ↓ OnTriggerEnter2D(BattleZone)
    ├─ BattleData.SetupBattle(enemy, background, zone)
    ├─ BattleData.UpdatePlayerStats(health, mana)
    └─ SceneManager.LoadScene("BattleScene")
         ↓
    BattleManager.Start()
         ↓
    BattleManager.InitializeBattle()
         ├─ LoadPlayerData() from BattleData
         ├─ LoadEnemyData() from BattleData
         ├─ Setup UI (health bars, buttons)
         └─ StartPlayerTurn()
```

### Team Management Flow

```
CharacterSelectionUI
    ↓ AddCharacterToTeam()
    └─ TeamManager.AddCharacterToTeam(characterData)
         ↓
    TeamManager
         ├─ Check if team is full (maxTeamSize = 3)
         ├─ Check if character already in team
         ├─ Create new TeamMember(characterData)
         └─ Add to currentTeam list
              ↓
    BattleManager.LoadPlayerData()
         ├─ Get TeamManager.GetActiveCharacter()
         ├─ Load stats into BattleCharacter
         └─ Display character sprite
```

---

## Code Metrics

### Lines of Code (Estimated)

| Class | LOC | Complexity |
|-------|-----|------------|
| BattleManager | ~700+ | ⭐⭐⭐⭐⭐ |
| PlayerController | ~600+ | ⭐⭐⭐⭐ |
| SaveManager | ~375+ | ⭐⭐⭐⭐ |
| MainMenuManager | ~300+ | ⭐⭐⭐ |
| GameOverManager | ~250+ | ⭐⭐⭐ |
| CharacterSelectionUI | ~250+ | ⭐⭐⭐ |
| BattleCharacter | ~212 | ⭐⭐ |
| Inventory | ~210 | ⭐⭐⭐ |
| MessageDisplay | ~183 | ⭐⭐ |
| ItemCounter | ~130 | ⭐ |
| TeamManager | ~138 | ⭐⭐ |
| AttackAnimation | ~129 | ⭐⭐ |
| AudioManager | ~177 | ⭐⭐ |
| DamageFlash | ~72 | ⭐ |
| CameraShake | ~62 | ⭐ |
| GameProgress | ~101 | ⭐⭐ |
| BattleData | ~68 | ⭐ |
| SaveData | ~55 | ⭐ |
| Item | ~34 | ⭐ |
| CharacterData | ~27 | ⭐ |
| CameraFollow | ~18 | ⭐ |
| BattleZone | ~16 | ⭐ |

**Total Production Code**: ~4,500+ LOC  
**Total with Tests**: ~4,750+ LOC

---

## Technical Stack

### Unity Components Used

- **MonoBehaviour**: 26 classes
- **ScriptableObject**: 2 classes (CharacterData, Readme)
- **Serializable**: 3 classes (Item, TeamMember, SaveData)
- **Static**: 2 classes (BattleData, GameProgress)
- **UnityEditor**: 1 class (ReadmeEditor)

### Unity Systems Used

- **Scene Management**: SceneManager.LoadScene
- **UI System**: Button, Slider, TextMeshProUGUI, Image, Canvas
- **Physics 2D**: BoxCollider2D, OnTriggerEnter2D
- **Audio**: AudioSource, AudioClip
- **Animation**: Coroutines for animations
- **Particles**: ParticleSystem for effects
- **Serialization**: JsonUtility for save/load
- **Persistence**: PlayerPrefs (volume settings), File I/O (save files)
- **Lifecycle**: Start, Update, Awake, LateUpdate

### External Dependencies

- **TextMeshPro**: Enhanced text rendering
- **Unity UI**: Button, Slider, Image components
- **System.IO**: File operations for saves
- **System.Collections.Generic**: List, HashSet, Dictionary, Queue

---

## Architectural Strengths

### ✅ **What Works Well**

1. **Singleton Managers**
   - Clean global access
   - Persistent across scenes
   - Single responsibility

2. **Separation of Concerns**
   - Battle logic separate from UI
   - Data separate from presentation
   - Visual effects modular

3. **Data-Driven Design**
   - ScriptableObjects for characters
   - Easy to balance and iterate

4. **Scene Independence**
   - Static classes for data transfer
   - Minimal scene coupling

5. **Modular Visual Effects**
   - Reusable components
   - Easy to add/remove

6. **Event-Driven Inventory**
   - UI auto-updates
   - Loose coupling

---

## Architectural Considerations

### 📌 **Areas of Note**

1. **No Central GameManager**
   - Logic distributed across specialized managers
   - Could make debugging harder for complex interactions

2. **Static Classes for Data**
   - Simple and effective for single-player
   - Would need refactoring for multiplayer

3. **MonoBehaviour-Heavy**
   - Leverages Unity's component system
   - Some logic classes could be POCOs

4. **BattleManager Complexity**
   - 700+ LOC, handles multiple responsibilities
   - Could be split into smaller classes

5. **Test Classes in Production**
   - 5 test/debug scripts in Assets
   - Should move to Tests folder

---

## Files Not Yet Implemented

Based on the original class diagram, the following planned classes are **not yet implemented**:

1. **EnemyDatabase** - Planned factory for creating trolls
   - **Current**: Trolls created directly in BattleManager from BattleData
2. **PlayerDatabase** - Planned factory for creating player characters
   - **Current**: Characters defined as ScriptableObjects (CharacterData)
3. **ItemDatabase** - Planned static constants for item values
   - **Current**: Values hardcoded in Item class and Inventory
4. **MapController** - Planned separate movement controller
   - **Current**: Movement handled directly in PlayerController
5. **BattleUI** - Planned separate battle UI controller
   - **Current**: UI handled directly in BattleManager
6. **MapUI** - Planned separate map UI controller
   - **Current**: UI handled by ItemCounter and MessageDisplay

**Reason**: The implementation evolved to be simpler and more direct, avoiding unnecessary abstraction layers for an MVP.

---

## Summary

### By the Numbers

- **32 Total Classes** (25 production + 5 test + 2 editor)
- **4,750+ Lines of Code**
- **4 Scenes** (MainMenu, Map, Battle, GameOver)
- **3 Singletons** (TeamManager, SaveManager, AudioManager)
- **5 Design Patterns** (Singleton, Observer, Static Utility, ScriptableObject, Component)
- **2 Enumerations** (CharacterType, ItemType)
- **18 MonoBehaviour Classes**
- **10 UI Components**
- **7 Data Classes**

---

**Developed by**: Élodie, Louis & Sergey  
**Framework**: Unity 2D with C#  
**Architecture**: Component-based with Singleton Managers  
**Project**: Trolls et Paillettes RPG Game


