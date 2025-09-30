# UniQuest - Development Strategy (2-Week MVP)

## 🎯 Project Overview

**Goal:** Create a working 2D RPG in Unity with C# in 2 weeks  
**Team:** 3-4 students with minimal Unity experience  
**Scope:** MVP (Minimum Viable Product) - core gameplay only  
**Platform:** Unity 2022.3 LTS (Long Term Support)

---

## 📅 Week-by-Week Breakdown

### **Week 1: Core Logic & Systems (No Unity UI)**
**Goal:** Get all game logic working with console output

#### **Day 1-2: Foundation Classes**
**Priority:** Core character and data classes

**Tasks:**
1. **Create Character base class**
   - Properties: name, level, maxHP, currentHP, maxMP, currentMP, attack, defense, speed, currentXP, isAlive
   - Methods: TakeDamage(), Heal(), RestoreMP(), GainXP(), LevelUp(), IsDead()
   - **Test:** Create character, damage it, verify HP decreases

2. **Create PlayerCharacter and EnemyCharacter**
   - PlayerCharacter: inherits from Character
   - EnemyCharacter: inherits from Character + xpReward property
   - **Test:** Create both types, verify inheritance works

3. **Create Team class**
   - Fixed array of 3 PlayerCharacter instances
   - Methods: InitializeTeam(), GetActiveCharacter(), SwitchToNext(), GetAliveMembers(), IsTeamWiped()
   - **Test:** Create team, damage one character, verify switching works

**Deliverable:** All character classes working with console tests

---

#### **Day 3-4: Combat System**
**Priority:** Battle logic without UI

**Tasks:**
1. **Create BattleManager (MonoBehaviour)**
   - Properties: currentPlayer, currentEnemy, playerTeam, state
   - Methods: StartBattle(), PlayerAttack(), PlayerSuperAttack(), EnemyTurn(), EndBattle()
   - **Damage formula:** `baseDamage = attacker.attack - (defender.defense / 2)`
   - **Test:** Create battle, execute attacks, verify damage calculation

2. **Create EnemyDatabase**
   - Static methods: CreateTroll1(), CreateTroll2(), CreateTroll3(), GetTrollByZone()
   - **Test:** Create each troll, verify stats are correct

3. **Implement simple AI**
   - Enemy randomly chooses normal attack (50%) or super attack (50% if has MP)
   - **Test:** Run multiple battles, verify AI makes decisions

**Deliverable:** Combat system working with Debug.Log output

---

#### **Day 5: Inventory & Items**
**Priority:** Item management system

**Tasks:**
1. **Create Item class**
   - Properties: name, type, value
   - Methods: Use() - abstract method

2. **Create Inventory class**
   - Dictionary<string, int> items
   - Methods: AddItem(), UseHealthPotion(), UseManaPotion(), GetItemCount(), HasItem()
   - **Test:** Add items, use potions, verify HP/MP changes

3. **Create ItemDatabase**
   - Constants: HEALTH_POTION_HEAL = 30, MANA_POTION_RESTORE = 20, SUPER_ATTACK_COST = 15

4. **Integrate items into BattleManager**
   - Add PlayerUseItem() method
   - **Test:** Use items during battle, verify effects

**Deliverable:** Inventory system working with console output

---

#### **Day 6-7: Map & Battle Zones**
**Priority:** Map movement and zone-based encounters

**Tasks:**
1. **Create MapController (MonoBehaviour)**
   - Properties: playerTransform, moveSpeed
   - Methods: MovePlayer(), Update() for input handling
   - **Test:** Player moves with arrow keys

2. **Create BattleZone (MonoBehaviour)**
   - Properties: zoneIndex, zoneCollider
   - Methods: OnTriggerEnter2D(), GetTrollForZone()
   - **Test:** Walk into zone, trigger battle

3. **Create GameManager (Singleton)**
   - Properties: Instance, team, inventory, currentState
   - Methods: GetTeam(), GetInventory(), TransitionToBattle(), TransitionToMap()
   - **Test:** GameManager accessible from anywhere

4. **Connect all systems**
   - MapController → GameManager → BattleManager
   - **Test:** Full flow: move → enter zone → battle → return to map

**Deliverable:** Complete game loop working (move → battle → return)

---

### **Week 2: Unity Integration & Polish**
**Goal:** Add UI, scenes, and make it playable

#### **Day 8-9: Battle UI**
**Priority:** Battle interface and controls

**Tasks:**
1. **Create Battle scene**
   - New scene: File → New Scene → 2D
   - Add Canvas (UI → Canvas)
   - **Test:** Scene loads correctly

2. **Create BattleUI (MonoBehaviour)**
   - UI Elements: HP bars, attack buttons, battle log text
   - Methods: UpdateUI(), ShowBattleLog(), EnableButtons()
   - **Test:** UI displays character stats

3. **Connect UI to BattleManager**
   - Button onClick events call BattleManager methods
   - **Test:** Click buttons, execute battle actions

4. **Add battle animations (basic)**
   - Character sprites shake when hit
   - **Test:** Visual feedback during combat

**Deliverable:** Playable battle with UI

---

#### **Day 10-11: Map Scene & Integration**
**Priority:** Map interface and scene transitions

**Tasks:**
1. **Create Map scene**
   - Add background sprite or tilemap
   - Add player sprite with movement
   - **Test:** Player moves smoothly on map

2. **Create MapUI (MonoBehaviour)**
   - Display team HP/status
   - **Test:** UI updates when team changes

3. **Implement scene transitions**
   - Map → Battle → Map
   - Use SceneManager.LoadScene()
   - **Test:** Seamless transitions between scenes

4. **Add player sprite and movement**
   - Import player character sprites
   - **Test:** Visual movement on map

**Deliverable:** Complete map-to-battle flow

---

#### **Day 12-13: Balance & Polish**
**Priority:** Game balance and visual polish

**Tasks:**
1. **Playtesting and balance**
   - Test all 3 trolls, adjust stats if needed
   - **Test:** Each troll provides appropriate challenge

2. **Add visual polish**
   - Character sprites for all 3 heroes and 3 trolls
   - Item icons for potions
   - **Test:** Game looks professional

3. **Add sound effects (optional)**
   - Attack sounds, hit sounds, level up sound
   - **Test:** Audio feedback enhances gameplay

4. **Add game over/victory screens**
   - Game over when team wiped
   - Victory screen after beating all trolls
   - **Test:** Clear win/lose conditions

**Deliverable:** Polished, balanced game

---

#### **Day 14: Final Testing & Presentation**
**Priority:** Bug fixes and demo preparation

**Tasks:**
1. **Full playthrough testing**
   - Complete game from start to finish
   - **Test:** No crashes, all features work

2. **Bug fixes**
   - Fix any critical issues found
   - **Test:** Stable gameplay

3. **Demo preparation**
   - Create save file for demo
   - Prepare presentation slides
   - **Test:** Smooth demo flow

4. **Documentation**
   - Update README with controls
   - **Test:** Others can run the game

**Deliverable:** Presentation-ready game

---

## 👥 Team Collaboration Strategy

### **Recommended Team Roles (3 People)**

#### **Person A: Core Systems Developer**
**Responsibilities:**
- Character, Team, Inventory classes
- BattleManager logic
- EnemyDatabase
- Unit tests for all logic classes

**Git Workflow:**
- Create feature branch: `feature/core-systems`
- Work on POCO classes first (no Unity dependencies)
- Commit frequently with descriptive messages
- Create pull request when feature complete

**Time Allocation:**
- Day 1-2: Character classes
- Day 3-4: BattleManager
- Day 5: Inventory integration
- Day 6-7: Testing and integration

---

#### **Person B: Unity Integration Developer**
**Responsibilities:**
- MapController, BattleZone
- GameManager singleton
- Scene management
- Unity-specific MonoBehaviour classes

**Git Workflow:**
- Create feature branch: `feature/unity-integration`
- Focus on MonoBehaviour classes
- Test in Unity frequently
- Coordinate with Person A for integration

**Time Allocation:**
- Day 1-2: Study Unity basics
- Day 3-4: MapController and movement
- Day 5-6: BattleZone and GameManager
- Day 7: Integration testing

---

#### **Person C: UI & Polish Developer**
**Responsibilities:**
- BattleUI, MapUI
- Scene setup and transitions
- Visual assets and polish
- Final testing and presentation

**Git Workflow:**
- Create feature branch: `feature/ui-polish`
- Start with basic UI, add polish later
- Coordinate with others for UI integration
- Handle final presentation preparation

**Time Allocation:**
- Day 1-5: Study Unity UI system
- Day 6-9: Battle UI implementation
- Day 10-11: Map UI and scenes
- Day 12-14: Polish and presentation

---

### **Alternative: Parallel Development Strategy**

#### **Week 1: Parallel Development**
- **Person A:** Focus on POCO classes (Character, Team, Inventory)
- **Person B:** Focus on MonoBehaviour classes (BattleManager, MapController)
- **Person C:** Study Unity, prepare assets, create basic scenes

#### **Week 2: Integration & Polish**
- **Person A:** Help with integration and testing
- **Person B:** Focus on scene transitions and GameManager
- **Person C:** Lead UI implementation and polish

---

## 🔧 Git Workflow & Collaboration

### **Repository Setup**
```bash
# Initialize repository
git init
git remote add origin [your-github-repo-url]

# Create main branch
git checkout -b main
git push -u origin main
```

### **Branch Strategy**
```bash
# Main branches
main                    # Production-ready code
develop                 # Integration branch

# Feature branches (each person)
feature/core-systems    # Person A
feature/unity-integration # Person B  
feature/ui-polish       # Person C

# Hotfix branches (if needed)
hotfix/critical-bug
```

### **Daily Workflow**
```bash
# Morning: Pull latest changes
git checkout develop
git pull origin develop

# Work on your feature
git checkout feature/your-feature
git merge develop  # Get latest changes

# Work on your tasks...
# Commit frequently
git add .
git commit -m "Add Character.TakeDamage() method"

# End of day: Push your work
git push origin feature/your-feature
```

### **Integration Workflow**
```bash
# When feature is complete
git checkout develop
git merge feature/your-feature
git push origin develop

# Create pull request for review
# After approval, merge to main
```

### **Conflict Resolution**
```bash
# If conflicts occur
git checkout develop
git pull origin develop
git checkout feature/your-feature
git merge develop
# Resolve conflicts in Unity/VS Code
git add .
git commit -m "Resolve merge conflicts"
```

---

## 🎮 Unity Hub & Project Management

### **Unity Hub Setup**
1. **Install Unity Hub**
   - Download from unity.com
   - Create Unity ID account
   - Install Unity 2022.3 LTS

2. **Project Setup**
   - Create new 2D project: "UniQuest"
   - Set up version control (Git)
   - Configure project settings

### **Project Structure**
```
UniQuest/
├── Assets/
│   ├── Scripts/
│   │   ├── Characters/     # Character, PlayerCharacter, EnemyCharacter
│   │   ├── Combat/         # BattleManager, EnemyDatabase
│   │   ├── Inventory/      # Inventory, Item, ItemDatabase
│   │   ├── Map/           # MapController, BattleZone
│   │   ├── Managers/      # GameManager
│   │   └── UI/            # BattleUI, MapUI
│   ├── Sprites/
│   │   ├── Characters/    # Player and enemy sprites
│   │   ├── Items/         # Potion icons
│   │   └── UI/            # UI elements
│   ├── Scenes/
│   │   ├── Map.unity
│   │   └── Battle.unity
│   └── Prefabs/
│       ├── BattleZone.prefab
│       └── Player.prefab
├── Packages/
├── ProjectSettings/
└── README.md
```

### **Unity Collaboration Settings**
1. **Version Control Settings**
   - Edit → Project Settings → Editor
   - Version Control Mode: Visible Meta Files
   - Asset Serialization Mode: Force Text

2. **Git Ignore File**
   ```
   [Ll]ibrary/
   [Tt]emp/
   [Oo]bj/
   [Bb]uild/
   [Bb]uilds/
   [Ll]ogs/
   [Uu]ser[Ss]ettings/
   *.tmp
   *.user
   *.userprefs
   *.pidb
   *.booproj
   *.svd
   *.pdb
   *.mdb
   *.opendb
   *.VC.db
   ```

---

## 📋 Daily Standup Template

### **Daily Questions (15 minutes)**
1. **What did you complete yesterday?**
2. **What are you working on today?**
3. **Are there any blockers or issues?**
4. **Do you need help from other team members?**

### **Weekly Review (30 minutes)**
1. **Review completed features**
2. **Test integration between systems**
3. **Identify any critical issues**
4. **Plan next week's priorities**

---

## 🚨 Risk Management

### **High-Risk Areas**
1. **Unity Learning Curve** - Start with Unity basics early
2. **Integration Issues** - Test integration daily
3. **Scope Creep** - Stick to MVP features only
4. **Git Conflicts** - Communicate changes frequently

### **Mitigation Strategies**
1. **Daily Testing** - Test your code every day
2. **Frequent Commits** - Commit working code frequently
3. **Backup Strategy** - Push to GitHub daily
4. **Fallback Plan** - Have simpler alternatives ready

---

## 📊 Success Metrics

### **Week 1 Goals**
- [ ] All POCO classes working with unit tests
- [ ] BattleManager executes battles correctly
- [ ] Inventory system manages items
- [ ] Map movement and zone triggers work
- [ ] Complete game loop (move → battle → return)

### **Week 2 Goals**
- [ ] Battle UI is functional and responsive
- [ ] Map UI displays team status
- [ ] Scene transitions work smoothly
- [ ] Game is visually polished
- [ ] No critical bugs or crashes

### **Final Deliverable**
- [ ] Playable game from start to finish
- [ ] All 3 trolls can be defeated
- [ ] Team switching works correctly
- [ ] Items and leveling function properly
- [ ] Game over and victory conditions work
- [ ] Presentation-ready demo

---

## 🎯 Final Advice

### **Week 1 Focus**
- **Get logic working first** - UI can wait
- **Test frequently** - Don't wait until the end
- **Keep it simple** - Avoid complex features
- **Communicate daily** - Don't work in isolation

### **Week 2 Focus**
- **Polish what you have** - Don't add new features
- **Test the complete game** - End-to-end testing
- **Prepare for presentation** - Have a demo ready
- **Document everything** - README, controls, etc.

### **Team Success Tips**
- **Help each other** - Share knowledge and resources
- **Ask questions early** - Don't struggle alone
- **Celebrate small wins** - Each working feature is progress
- **Stay focused** - MVP first, polish later

---

**Remember: A working game with basic graphics is better than a beautiful game that doesn't work!** 🎮

**Good luck, team! You've got this! 🚀**
