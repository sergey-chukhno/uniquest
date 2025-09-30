# UniQuest - Unity Tutorial for Beginners

## 🎯 Overview

This tutorial will guide you through creating UniQuest in Unity from scratch. **No Unity experience required!** We'll cover everything step-by-step.

**What you'll learn:**
- Unity basics and interface
- Creating 2D games
- C# scripting in Unity
- UI systems
- Scene management
- Asset management

---

## 📋 Prerequisites

### **Software Needed**
1. **Unity Hub** (free) - Download from unity.com
2. **Unity 2022.3 LTS** (free) - Install through Unity Hub
3. **Visual Studio** (free) - For C# coding
4. **Git** (free) - For version control

### **Hardware Requirements**
- **RAM:** 8GB minimum, 16GB recommended
- **Storage:** 10GB free space
- **Graphics:** Any modern graphics card

---

## 🚀 Step 1: Unity Setup

### **1.1 Install Unity Hub**
1. Go to [unity.com](https://unity.com)
2. Click "Get Unity" → "Personal" (free)
3. Download Unity Hub
4. Install Unity Hub
5. Create Unity ID account (free)

### **1.2 Install Unity Editor**
1. Open Unity Hub
2. Go to "Installs" tab
3. Click "Install Editor"
4. Select **Unity 2022.3 LTS** (Long Term Support)
5. Click "Install"
6. Wait for installation (30-60 minutes)

### **1.3 Create New Project**
1. Open Unity Hub
2. Click "New project"
3. Select **"2D Core"** template
4. Project name: **"UniQuest"**
5. Location: Choose your folder
6. Click "Create project"
7. Wait for Unity to open

---

## 🎮 Step 2: Unity Interface Tour

### **2.1 Main Windows (First Time Setup)**

When Unity opens, you'll see several windows. **Don't panic!** Here's what each does:

#### **Scene View (Center)**
- **What it is:** Your game world
- **What you do:** Place objects, move things around
- **Think of it as:** Your game's stage

#### **Hierarchy (Top Left)**
- **What it is:** List of all objects in your scene
- **What you do:** Select objects, organize them
- **Think of it as:** Table of contents

#### **Inspector (Right Side)**
- **What it is:** Properties of selected object
- **What you do:** Change object settings
- **Think of it as:** Object's control panel

#### **Project (Bottom)**
- **What it is:** All your files (scripts, images, sounds)
- **What you do:** Organize assets, create scripts
- **Think of it as:** Your project's file explorer

#### **Console (Bottom)**
- **What it is:** Messages from your code
- **What you do:** See errors, debug messages
- **Think of it as:** Your code's feedback

### **2.2 Essential Shortcuts**
- **F** - Focus on selected object
- **Ctrl + S** - Save scene
- **Ctrl + Z** - Undo
- **Space** - Play/Stop game
- **Ctrl + Shift + N** - Create new GameObject

---

## 📁 Step 3: Project Structure Setup

### **3.1 Create Folder Structure**

In the **Project window** (bottom), create these folders:

1. **Right-click in Project window**
2. **Create → Folder**
3. **Name it:** `Scripts`
4. **Repeat for:**
   - `Scripts/Characters`
   - `Scripts/Combat`
   - `Scripts/Inventory`
   - `Scripts/Map`
   - `Scripts/Managers`
   - `Scripts/UI`
   - `Sprites`
   - `Sprites/Characters`
   - `Sprites/Enemies`
   - `Sprites/Items`
   - `Sprites/UI`
   - `Scenes`
   - `Prefabs`

### **3.2 Create Scenes**

1. **File → New Scene**
2. **Save as:** `Map` in `Scenes` folder
3. **File → New Scene**
4. **Save as:** `Battle` in `Scenes` folder
5. **File → Save Scene** (save current scene)

---

## 🎨 Step 4: Basic Assets Setup

### **4.1 Create Simple Sprites (Temporary)**

Since you don't have art assets yet, let's create simple colored squares:

#### **Create Player Sprite**
1. **Right-click in Project → Create → 2D → Sprites → Square**
2. **Name it:** `PlayerSprite`
3. **In Inspector, set:**
   - **Color:** Blue
   - **Size:** 1x1

#### **Create Enemy Sprites**
1. **Create 3 more squares:**
   - `Troll1Sprite` (Red)
   - `Troll2Sprite` (Orange)  
   - `Troll3Sprite` (Dark Red)

#### **Create Item Sprites**
1. **Create 2 more squares:**
   - `HealthPotionSprite` (Green)
   - `ManaPotionSprite` (Blue)

### **4.2 Create Simple Background**
1. **Right-click in Project → Create → 2D → Sprites → Square**
2. **Name it:** `MapBackground`
3. **In Inspector, set:**
   - **Color:** Light Green
   - **Size:** 20x15

---

## 🎮 Step 5: Map Scene Setup

### **5.1 Set Up Map Scene**

1. **Open Map scene** (double-click in Project)
2. **Delete default objects** (Main Camera, Directional Light)
3. **Add background:**
   - **Drag `MapBackground` from Project to Scene**
   - **Position:** (0, 0, 0)
   - **Scale:** (20, 15, 1)

### **5.2 Create Player Object**

1. **Right-click in Hierarchy → Create Empty**
2. **Name it:** `Player`
3. **Add components:**
   - **Add Component → Sprite Renderer**
   - **Drag `PlayerSprite` to Sprite field**
   - **Add Component → Box Collider 2D**
   - **Add Component → Rigidbody 2D**
   - **Set Rigidbody 2D:**
     - **Gravity Scale:** 0
     - **Freeze Rotation:** Z

### **5.3 Create Battle Zones**

#### **Create Zone 1 (Troll1)**
1. **Right-click in Hierarchy → Create Empty**
2. **Name it:** `BattleZone1`
3. **Position:** (5, 0, 0)
4. **Add components:**
   - **Add Component → Box Collider 2D**
   - **Check "Is Trigger"**
   - **Size:** (3, 3, 1)
5. **Add visual indicator:**
   - **Add Component → Sprite Renderer**
   - **Create new sprite:** Red square, size 3x3
   - **Color:** Red with 50% transparency

#### **Create Zone 2 (Troll2)**
1. **Repeat above steps**
2. **Name:** `BattleZone2`
3. **Position:** (10, 0, 0)
4. **Color:** Orange

#### **Create Zone 3 (Troll3)**
1. **Repeat above steps**
2. **Name:** `BattleZone3`
3. **Position:** (15, 0, 0)
4. **Color:** Dark Red

### **5.4 Set Up Camera**
1. **Select Main Camera**
2. **In Inspector, set:**
   - **Position:** (0, 0, -10)
   - **Size:** 10
   - **Clear Flags:** Solid Color
   - **Background:** Light Blue

---

## ⚔️ Step 6: Battle Scene Setup

### **6.1 Set Up Battle Scene**

1. **Open Battle scene**
2. **Delete default objects**
3. **Create Canvas:**
   - **Right-click in Hierarchy → UI → Canvas**
   - **Name it:** `BattleCanvas`

### **6.2 Create Battle UI Elements**

#### **Create HP Bars**
1. **Right-click Canvas → UI → Slider**
2. **Name it:** `PlayerHPBar`
3. **Position:** Top left
4. **Add Text:**
   - **Right-click PlayerHPBar → UI → Text**
   - **Name it:** `PlayerHPText`
   - **Text:** "Player HP: 100/100"

5. **Repeat for Enemy:**
   - **Name:** `EnemyHPBar`
   - **Position:** Top right
   - **Add Text:** `EnemyHPText`

#### **Create Action Buttons**
1. **Right-click Canvas → UI → Button**
2. **Name it:** `AttackButton`
3. **Position:** Bottom left
4. **Text:** "Attack"

5. **Create more buttons:**
   - `SuperAttackButton` - "Super Attack"
   - `ItemButton` - "Use Item"
   - `SwitchButton` - "Switch Character"

#### **Create Battle Log**
1. **Right-click Canvas → UI → Text**
2. **Name it:** `BattleLog`
3. **Position:** Bottom center
4. **Text:** "Battle started!"
5. **Font Size:** 14

### **6.3 Create Character Display Areas**

1. **Create empty GameObject:** `PlayerDisplay`
2. **Add Sprite Renderer**
3. **Position:** Left side of screen
4. **Repeat for:** `EnemyDisplay`

---

## 💻 Step 7: Scripting Setup

### **7.1 Create First Script**

1. **Right-click in Project → Create → C# Script**
2. **Name it:** `PlayerController`
3. **Double-click to open in Visual Studio**

### **7.2 Basic Script Template**

```csharp
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    
    void Update()
    {
        // Get input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        // Move player
        Vector2 movement = new Vector2(horizontal, vertical);
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }
}
```

### **7.3 Attach Script to Player**

1. **Select Player object in Hierarchy**
2. **In Inspector, click "Add Component"**
3. **Type:** `PlayerController`
4. **Click on the script**
5. **Set Move Speed:** 5

### **7.4 Test Movement**

1. **Click Play button** (▶️)
2. **Use arrow keys to move**
3. **Click Play again to stop**

---

## 🎯 Step 8: Core Scripts Implementation

### **8.1 Character Script**

1. **Create new script:** `Character.cs` in `Scripts/Characters`
2. **Copy this code:**

```csharp
using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Stats")]
    public string characterName;
    public int level = 1;
    public int maxHP = 100;
    public int currentHP;
    public int maxMP = 30;
    public int currentMP;
    public int attack = 20;
    public int defense = 10;
    public int speed = 5;
    public int currentXP = 0;
    public bool isAlive = true;
    
    void Start()
    {
        currentHP = maxHP;
        currentMP = maxMP;
    }
    
    public void TakeDamage(int amount)
    {
        currentHP -= amount;
        if (currentHP <= 0)
        {
            currentHP = 0;
            isAlive = false;
        }
    }
    
    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP > maxHP)
            currentHP = maxHP;
    }
    
    public void RestoreMP(int amount)
    {
        currentMP += amount;
        if (currentMP > maxMP)
            currentMP = maxMP;
    }
    
    public bool IsDead()
    {
        return !isAlive;
    }
}
```

### **8.2 PlayerCharacter Script**

1. **Create:** `PlayerCharacter.cs`
2. **Copy this code:**

```csharp
using UnityEngine;

public class PlayerCharacter : Character
{
    // PlayerCharacter inherits everything from Character
    // Add player-specific methods here later
}
```

### **8.3 EnemyCharacter Script**

1. **Create:** `EnemyCharacter.cs`
2. **Copy this code:**

```csharp
using UnityEngine;

public class EnemyCharacter : Character
{
    public int xpReward = 25;
    
    public int GetXPReward()
    {
        return xpReward;
    }
}
```

---

## 🎮 Step 9: Game Manager Setup

### **9.1 Create GameManager Script**

1. **Create:** `GameManager.cs` in `Scripts/Managers`
2. **Copy this code:**

```csharp
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    
    [Header("Game Data")]
    public PlayerCharacter[] teamMembers = new PlayerCharacter[3];
    public Inventory playerInventory;
    
    void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    void Start()
    {
        // Initialize game
        InitializeGame();
    }
    
    void InitializeGame()
    {
        // Create team members
        // This will be filled in later
        Debug.Log("Game initialized!");
    }
    
    public void TransitionToBattle(EnemyCharacter enemy)
    {
        SceneManager.LoadScene("Battle");
    }
    
    public void TransitionToMap()
    {
        SceneManager.LoadScene("Map");
    }
}
```

### **9.2 Create GameManager Object**

1. **Right-click in Hierarchy → Create Empty**
2. **Name it:** `GameManager`
3. **Add Component → GameManager**
4. **This object will persist between scenes**

---

## 🎨 Step 10: UI Scripts

### **10.1 BattleUI Script**

1. **Create:** `BattleUI.cs` in `Scripts/UI`
2. **Copy this code:**

```csharp
using UnityEngine;
using UnityEngine.UI;

public class BattleUI : MonoBehaviour
{
    [Header("UI Elements")]
    public Slider playerHPBar;
    public Slider enemyHPBar;
    public Text playerHPText;
    public Text enemyHPText;
    public Text battleLog;
    public Button attackButton;
    public Button superAttackButton;
    public Button itemButton;
    public Button switchButton;
    
    void Start()
    {
        // Set up button listeners
        attackButton.onClick.AddListener(OnAttackClicked);
        superAttackButton.onClick.AddListener(OnSuperAttackClicked);
        itemButton.onClick.AddListener(OnItemClicked);
        switchButton.onClick.AddListener(OnSwitchClicked);
    }
    
    public void UpdateUI(Character player, Character enemy)
    {
        // Update HP bars
        playerHPBar.value = (float)player.currentHP / player.maxHP;
        enemyHPBar.value = (float)enemy.currentHP / enemy.maxHP;
        
        // Update HP text
        playerHPText.text = $"Player HP: {player.currentHP}/{player.maxHP}";
        enemyHPText.text = $"Enemy HP: {enemy.currentHP}/{enemy.maxHP}";
    }
    
    public void ShowBattleLog(string message)
    {
        battleLog.text = message;
    }
    
    void OnAttackClicked()
    {
        Debug.Log("Attack button clicked!");
        // This will connect to BattleManager later
    }
    
    void OnSuperAttackClicked()
    {
        Debug.Log("Super Attack button clicked!");
    }
    
    void OnItemClicked()
    {
        Debug.Log("Item button clicked!");
    }
    
    void OnSwitchClicked()
    {
        Debug.Log("Switch button clicked!");
    }
}
```

### **10.2 Attach UI Script**

1. **Select BattleCanvas in Battle scene**
2. **Add Component → BattleUI**
3. **Drag UI elements to script fields:**
   - **Player HP Bar:** Drag PlayerHPBar
   - **Enemy HP Bar:** Drag EnemyHPBar
   - **Player HP Text:** Drag PlayerHPText
   - **Enemy HP Text:** Drag EnemyHPText
   - **Battle Log:** Drag BattleLog
   - **Attack Button:** Drag AttackButton
   - **Super Attack Button:** Drag SuperAttackButton
   - **Item Button:** Drag ItemButton
   - **Switch Button:** Drag SwitchButton

---

## 🎯 Step 11: Battle Zone Script

### **11.1 Create BattleZone Script**

1. **Create:** `BattleZone.cs` in `Scripts/Map`
2. **Copy this code:**

```csharp
using UnityEngine;

public class BattleZone : MonoBehaviour
{
    public int zoneIndex = 0; // 0, 1, or 2 for different trolls
    
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log($"Player entered zone {zoneIndex}!");
            // This will trigger battle later
            GameManager.Instance.TransitionToBattle(null);
        }
    }
}
```

### **11.2 Attach to Battle Zones**

1. **Select BattleZone1 in Map scene**
2. **Add Component → BattleZone**
3. **Set Zone Index:** 0
4. **Repeat for BattleZone2 (index 1) and BattleZone3 (index 2)**

### **11.3 Tag Player Object**

1. **Select Player object**
2. **In Inspector, find Tag dropdown**
3. **Select "Player" (or create new tag if needed)**

---

## 🎮 Step 12: Testing Your Setup

### **12.1 Test Map Scene**

1. **Open Map scene**
2. **Click Play**
3. **Use arrow keys to move player**
4. **Walk into red zones**
5. **Check Console for messages**

### **12.2 Test Battle Scene**

1. **Open Battle scene**
2. **Click Play**
3. **Click buttons**
4. **Check Console for button messages**

### **12.3 Test Scene Transitions**

1. **In Map scene, walk into zone**
2. **Should transition to Battle scene**
3. **In Battle scene, add button to return to Map**

---

## 🎨 Step 13: Adding Visual Polish

### **13.1 Improve Sprites**

1. **Find free sprites online:**
   - **OpenGameArt.org** - Free game assets
   - **Itch.io** - Free and paid assets
   - **Unity Asset Store** - Free assets

2. **Import sprites:**
   - **Drag sprite files to Project window**
   - **Select sprite in Project**
   - **In Inspector, set Sprite Mode to "Single"**
   - **Click Apply**

### **13.2 Add Animations (Optional)**

1. **Select Player object**
2. **Add Component → Animator**
3. **Create Animator Controller:**
   - **Right-click in Project → Create → Animator Controller**
   - **Name it:** `PlayerAnimator`
   - **Drag to Player's Animator component**

### **13.3 Add Sound Effects (Optional)**

1. **Import sound files to Project**
2. **Add AudioSource component to objects**
3. **Play sounds in scripts:**
   ```csharp
   GetComponent<AudioSource>().Play();
   ```

---

## 🚀 Step 14: Build and Test

### **14.1 Build Settings**

1. **File → Build Settings**
2. **Add scenes:**
   - **Add Open Scenes** (Map and Battle)
3. **Select Platform:** PC, Mac & Linux Standalone
4. **Click Build**

### **14.2 Test Build**

1. **Run the built game**
2. **Test all features**
3. **Check for bugs**
4. **Fix any issues**

---

## 🎯 Step 15: Final Polish

### **15.1 Game Balance**

1. **Test all 3 trolls**
2. **Adjust stats if needed:**
   - **Select troll objects**
   - **Change HP, Attack, Defense values**
   - **Test again**

### **15.2 UI Polish**

1. **Improve button layouts**
2. **Add better fonts**
3. **Improve colors and contrast**
4. **Add visual feedback**

### **15.3 Final Testing**

1. **Complete playthrough**
2. **Test all features**
3. **Fix any remaining bugs**
4. **Prepare for presentation**

---

## 🎮 Controls Reference

### **Map Controls**
- **Arrow Keys:** Move player
- **Walk into colored zones:** Trigger battles

### **Battle Controls**
- **Attack Button:** Normal attack
- **Super Attack Button:** Powerful attack (costs MP)
- **Item Button:** Use potions
- **Switch Button:** Change team member

---

## 🚨 Common Issues and Solutions

### **Issue: Player doesn't move**
- **Solution:** Check if PlayerController script is attached
- **Check:** Move Speed is set to 5
- **Check:** Input settings in Edit → Project Settings → Input

### **Issue: Battle zones don't trigger**
- **Solution:** Check if BattleZone script is attached
- **Check:** Player has "Player" tag
- **Check:** Zone colliders are set as "Trigger"

### **Issue: UI buttons don't work**
- **Solution:** Check if BattleUI script is attached
- **Check:** Button references are assigned
- **Check:** Button onClick events are set

### **Issue: Scene transitions don't work**
- **Solution:** Check if scenes are added to Build Settings
- **Check:** Scene names match exactly
- **Check:** GameManager script is working

---

## 📚 Additional Resources

### **Unity Learning**
- **Unity Learn** - Official tutorials
- **Brackeys YouTube** - Great Unity tutorials
- **Code Monkey YouTube** - Advanced Unity concepts

### **C# Learning**
- **Microsoft C# Docs** - Official documentation
- **Codecademy C#** - Interactive learning
- **W3Schools C#** - Quick reference

### **Game Development**
- **GameDev.tv** - Comprehensive courses
- **Udemy Unity courses** - Paid but thorough
- **Unity Forums** - Community help

---

## 🎯 Final Checklist

### **Before Presentation**
- [ ] Game runs without crashes
- [ ] All 3 trolls can be defeated
- [ ] Team switching works
- [ ] Items function correctly
- [ ] Leveling system works
- [ ] Game over and victory screens
- [ ] Controls are intuitive
- [ ] Visual polish is adequate
- [ ] Sound effects (if added) work
- [ ] Build runs on target platform

### **Presentation Tips**
- [ ] Have a save file ready for demo
- [ ] Practice the demo flow
- [ ] Prepare to explain technical decisions
- [ ] Be ready to show code
- [ ] Have backup plans for technical issues

---

## 🎮 Congratulations!

You've now learned the basics of Unity game development! This tutorial covered:

✅ **Unity interface and workflow**  
✅ **2D game development**  
✅ **C# scripting in Unity**  
✅ **UI systems**  
✅ **Scene management**  
✅ **Asset management**  
✅ **Game architecture**  

**You're ready to build UniQuest!** 🚀

**Remember:** Start simple, test frequently, and don't be afraid to ask for help. Game development is a journey, and every expert was once a beginner!

**Good luck with your project!** 🎮⚔️
