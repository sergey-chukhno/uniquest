using UnityEngine;
using UnityEngine.SceneManagement;

public class BattleManager : MonoBehaviour
{
    [Header("Battle Characters")]
    public BattleCharacter playerCharacter;
    public BattleCharacter enemyCharacter;
    
    [Header("Team Management")]
    public TeamManager teamManager;
    public int currentPlayerIndex = 0;
    
    [Header("Battle UI")]
    public UnityEngine.UI.Button attackButton;
    public UnityEngine.UI.Button superAttackButton;
    
    [Header("Battle State")]
    public bool isPlayerTurn = true;
    public bool battleEnded = false;
    
    [Header("Enemy AI")]
    public float enemyTurnDelay = 2f; // Delay before enemy acts
    
    [Header("Battle Visuals")]
    public SpriteRenderer battleBackground;
    public SpriteRenderer enemySpriteRenderer;
    
    [Header("Battle Backgrounds")]
    public Sprite battleground1;
    public Sprite battleground2;
    public Sprite battleground3;
    
    [Header("Enemy Sprites")]
    public Sprite troll1Sprite;
    public Sprite troll2Sprite;
    public Sprite troll3Sprite;
    
    [Header("UI Messages")]
    public MessageDisplay messageDisplay;
    
    [Header("Team UI")]
    public UnityEngine.UI.Button switchCharacterButton;
    public TMPro.TextMeshProUGUI currentCharacterText;
    
    [Header("Visual Effects")]
    public CameraShake cameraShake;
    public ParticleSystem hitEffect;
    public ParticleSystem superAttackEffect;
    public ParticleSystem victoryEffect;
    
    void Start()
    {
        // Auto-find TeamManager if not assigned
        if (teamManager == null)
        {
            teamManager = TeamManager.Instance;
            Debug.Log($"BattleManager: Auto-found TeamManager: {teamManager != null}");
        }
        
        // Initialize battle based on BattleData
        InitializeBattle();
        
        // Setup button listeners
        SetupButtonListeners();
        
        // Setup team management
        SetupTeamManagement();
        
        // Start player's turn
        StartPlayerTurn();
        
        // Initialize message display if not assigned
        if (messageDisplay == null)
        {
            messageDisplay = FindObjectOfType<MessageDisplay>();
        }
        
        ShowMessage($"[BATTLE] {playerCharacter.characterName} vs {enemyCharacter.characterName}!");
        
        // Warn if starting with low or no mana
        if (playerCharacter.currentMana < 20)
        {
            ShowMessage($"[WARNING] Low mana! Current MP: {playerCharacter.currentMana}/50 - Cannot use Super Attack!");
        }
        
        ShowMessage("[YOUR TURN] Choose your action!");
    }
    
    void ShowMessage(string message)
    {
        // Show message on screen if available
        if (messageDisplay != null)
        {
            messageDisplay.ShowMessage(message);
        }
        
        // Also log to console
        Debug.Log(message);
    }
    
    void InitializeBattle()
    {
        // Load enemy data from BattleData
        LoadEnemyData();
        
        // Load player data from BattleData
        LoadPlayerData();
        
        // Set up battle visuals (background and enemy sprite)
        SetupBattleVisuals();
        
        // Update UI to show initial state
        UpdateBattleUI();
        
        Debug.Log($"Battle initialized: {playerCharacter.characterName} vs {enemyCharacter.characterName}");
    }
    
    void LoadEnemyData()
    {
        // Set enemy stats based on BattleData.enemyToFightIndex
        switch (BattleData.enemyToFightIndex)
        {
            case 1: // Troll 1 (Tent)
                enemyCharacter.characterName = "Troll 1";
                enemyCharacter.maxHealth = 80;
                enemyCharacter.attackPower = 15;
                enemyCharacter.defense = 8;
                enemyCharacter.maxMana = 30;
                break;
                
            case 2: // Troll 2 (Round Castle)
                enemyCharacter.characterName = "Troll 2";
                enemyCharacter.maxHealth = 120;
                enemyCharacter.attackPower = 20;
                enemyCharacter.defense = 12;
                enemyCharacter.maxMana = 40;
                break;
                
            case 3: // Troll 3 (Square Castle)
                enemyCharacter.characterName = "Troll 3";
                enemyCharacter.maxHealth = 150;
                enemyCharacter.attackPower = 25;
                enemyCharacter.defense = 15;
                enemyCharacter.maxMana = 50;
                break;
                
            default:
                enemyCharacter.characterName = "Unknown Enemy";
                enemyCharacter.maxHealth = 100;
                enemyCharacter.attackPower = 18;
                enemyCharacter.defense = 10;
                enemyCharacter.maxMana = 35;
                break;
        }
        
        // Initialize enemy health and mana to max (enemies always start fresh)
        enemyCharacter.currentHealth = enemyCharacter.maxHealth;
        enemyCharacter.currentMana = enemyCharacter.maxMana;
        enemyCharacter.isPlayer = false;
        enemyCharacter.isAlive = true;
        
        // Initialize the character's UI and log stats
        enemyCharacter.InitializeStats();
    }
    
    void LoadPlayerData()
    {
        // Load from team if available, otherwise fall back to BattleData
        if (teamManager != null && teamManager.currentTeam.Count > 0)
        {
            // Reset to first alive character
            currentPlayerIndex = 0;
            teamManager.activeCharacterIndex = 0;
            
            // Find first alive character
            for (int i = 0; i < teamManager.currentTeam.Count; i++)
            {
                if (!teamManager.currentTeam[i].isDefeated)
                {
                    currentPlayerIndex = i;
                    teamManager.activeCharacterIndex = i;
                    break;
                }
            }
            
            TeamMember activeCharacter = teamManager.GetActiveCharacter();
            if (activeCharacter != null)
            {
                playerCharacter.characterName = activeCharacter.characterData.characterName;
                playerCharacter.maxHealth = activeCharacter.characterData.baseHealth;
                playerCharacter.currentHealth = activeCharacter.currentHealth;
                playerCharacter.maxMana = activeCharacter.characterData.baseMana;
                playerCharacter.currentMana = activeCharacter.currentMana;
                playerCharacter.attackPower = activeCharacter.characterData.baseAttack;
                playerCharacter.defense = activeCharacter.characterData.baseDefense;
                playerCharacter.isPlayer = true;
                playerCharacter.isAlive = true;
                
                Debug.Log($"Loaded team character: {playerCharacter.characterName} - HP: {playerCharacter.currentHealth}/{playerCharacter.maxHealth}, MP: {playerCharacter.currentMana}/{playerCharacter.maxMana}");
                Debug.Log($"Current player index: {currentPlayerIndex}");
            }
        }
        else
        {
            // Fallback to BattleData
            Debug.Log($"LoadPlayerData - BattleData before load: HP {BattleData.playerCurrentHealth}/{BattleData.playerMaxHealth}, MP {BattleData.playerCurrentMana}/{BattleData.playerMaxMana}");
            
            playerCharacter.characterName = "Warrior Girl";
            playerCharacter.maxHealth = BattleData.playerMaxHealth;
            playerCharacter.attackPower = 20;
            playerCharacter.defense = 10;
            playerCharacter.maxMana = BattleData.playerMaxMana;
            playerCharacter.isPlayer = true;
            playerCharacter.isAlive = true;
            
            playerCharacter.currentHealth = BattleData.playerCurrentHealth;
            playerCharacter.currentMana = BattleData.playerCurrentMana;
        }
        
        Debug.Log($"LoadPlayerData - After setting values: HP {playerCharacter.currentHealth}/{playerCharacter.maxHealth}, MP {playerCharacter.currentMana}/{playerCharacter.maxMana}");
        
        // Initialize the character's UI and log stats
        playerCharacter.InitializeStats();
        
        Debug.Log($"Battle started with player stats: HP {playerCharacter.currentHealth}/{playerCharacter.maxHealth}, MP {playerCharacter.currentMana}/{playerCharacter.maxMana}");
    }
    
    void SetupBattleVisuals()
    {
        // Set battle background based on BattleData.battleBackgroundIndex
        if (battleBackground != null)
        {
            switch (BattleData.battleBackgroundIndex)
            {
                case 1:
                    if (battleground1 != null) battleBackground.sprite = battleground1;
                    break;
                case 2:
                    if (battleground2 != null) battleBackground.sprite = battleground2;
                    break;
                case 3:
                    if (battleground3 != null) battleBackground.sprite = battleground3;
                    break;
                default:
                    if (battleground1 != null) battleBackground.sprite = battleground1; // Default to battleground 1
                    break;
            }
            Debug.Log($"Battle background set to Battleground {BattleData.battleBackgroundIndex}");
        }
        
        // Set enemy sprite based on BattleData.enemyToFightIndex
        if (enemySpriteRenderer != null)
        {
            switch (BattleData.enemyToFightIndex)
            {
                case 1:
                    if (troll1Sprite != null) enemySpriteRenderer.sprite = troll1Sprite;
                    break;
                case 2:
                    if (troll2Sprite != null) enemySpriteRenderer.sprite = troll2Sprite;
                    break;
                case 3:
                    if (troll3Sprite != null) enemySpriteRenderer.sprite = troll3Sprite;
                    break;
                default:
                    if (troll1Sprite != null) enemySpriteRenderer.sprite = troll1Sprite; // Default to troll 1
                    break;
            }
            Debug.Log($"Enemy sprite set to Troll {BattleData.enemyToFightIndex}");
        }
    }
    
    void SetupButtonListeners()
    {
        // Clear existing listeners first to prevent duplicates
        if (attackButton != null)
        {
            attackButton.onClick.RemoveAllListeners();
            attackButton.onClick.AddListener(OnAttackButtonClicked);
            Debug.Log("Attack button listener added!");
        }
        
        if (superAttackButton != null)
        {
            superAttackButton.onClick.RemoveAllListeners();
            superAttackButton.onClick.AddListener(OnSuperAttackButtonClicked);
            Debug.Log("Super Attack button listener added!");
        }
    }
    
    void StartPlayerTurn()
    {
        isPlayerTurn = true;
        battleEnded = false;
        
        // Enable player buttons
        SetButtonsEnabled(true);
        
        ShowMessage("[YOUR TURN] Choose your action!");
        Debug.Log($"Player's turn - choose your action! Attack button enabled: {attackButton?.interactable}, Super Attack button enabled: {superAttackButton?.interactable}");
    }
    
    void StartEnemyTurn()
    {
        isPlayerTurn = false;
        
        // Disable player buttons
        SetButtonsEnabled(false);
        
        ShowMessage("[ENEMY TURN] Prepare for attack!");
        Debug.Log("Enemy's turn - prepare for attack!");
        
        // Start enemy AI after delay
        Invoke(nameof(ExecuteEnemyTurn), enemyTurnDelay);
    }
    
    void ExecuteEnemyTurn()
    {
        if (!battleEnded && enemyCharacter.isAlive)
        {
            // Simple enemy AI: 70% basic attack, 30% super attack (if enough mana)
            bool useSuperAttack = Random.Range(0f, 1f) < 0.3f && enemyCharacter.CanSuperAttack();
            
            int damage;
            if (useSuperAttack)
            {
                damage = enemyCharacter.SuperAttack();
                ShowMessage($"[SUPER ATTACK] {enemyCharacter.characterName} unleashes Super Attack!");
            }
            else
            {
                damage = enemyCharacter.BasicAttack();
                ShowMessage($"[ATTACK] {enemyCharacter.characterName} uses Basic Attack!");
            }
            
            // Apply damage to player
            if (damage > 0)
            {
                playerCharacter.TakeDamage(damage);
                ShowMessage($"[HIT] {playerCharacter.characterName} takes {damage} damage!");
                
                // Trigger camera shake on hit
                if (cameraShake != null)
                {
                    // Bigger shake for super attacks
                    if (useSuperAttack)
                    {
                        cameraShake.TriggerShake(0.3f, 0.25f);
                    }
                    else
                    {
                        cameraShake.TriggerShake();
                    }
                }
                
                // Play hit particle effect at player position
                if (hitEffect != null)
                {
                    hitEffect.transform.position = playerCharacter.transform.position;
                    hitEffect.Play();
                }
                
                // Play super attack effect if enemy used super attack
                if (useSuperAttack && superAttackEffect != null)
                {
                    superAttackEffect.transform.position = enemyCharacter.transform.position;
                    superAttackEffect.Play();
                }
            }
            
            // Check for battle end
            CheckBattleEnd();
            
            // If battle continues, start player's turn
            if (!battleEnded)
            {
                StartPlayerTurn();
            }
        }
    }
    
    public void OnAttackButtonClicked()
    {
        Debug.Log($"OnAttackButtonClicked called! isPlayerTurn={isPlayerTurn}, battleEnded={battleEnded}, playerCharacter.isAlive={playerCharacter.isAlive}");
        
        if (isPlayerTurn && !battleEnded && playerCharacter.isAlive)
        {
            Debug.Log("All conditions met, executing basic attack!");
            ExecutePlayerAction("basic");
        }
        else
        {
            Debug.LogWarning($"Attack button clicked but conditions not met: isPlayerTurn={isPlayerTurn}, battleEnded={battleEnded}, playerCharacter.isAlive={playerCharacter.isAlive}");
        }
    }
    
    public void OnSuperAttackButtonClicked()
    {
        if (isPlayerTurn && !battleEnded && playerCharacter.isAlive)
        {
            // Check if player has enough mana before executing
            if (!playerCharacter.CanSuperAttack())
            {
                ShowMessage($"[NO MANA] Not enough mana! Need 20 MP, have {playerCharacter.currentMana} MP");
                Debug.Log($"Cannot use Super Attack - Need 20 MP, have {playerCharacter.currentMana} MP");
                return;
            }
            
            ExecutePlayerAction("super");
        }
    }
    
    void ExecutePlayerAction(string actionType)
    {
        Debug.Log($"ExecutePlayerAction called with actionType: {actionType}");
        int damage = 0;
        
        switch (actionType)
        {
            case "basic":
                damage = playerCharacter.BasicAttack();
                ShowMessage($"[ATTACK] {playerCharacter.characterName} uses Basic Attack!");
                break;
                
            case "super":
                damage = playerCharacter.SuperAttack();
                ShowMessage($"[SUPER ATTACK] {playerCharacter.characterName} unleashes Super Attack!");
                
                // Play super attack particle effect
                if (superAttackEffect != null)
                {
                    superAttackEffect.transform.position = playerCharacter.transform.position;
                    superAttackEffect.Play();
                }
                break;
        }
        
        // Apply damage to enemy
        if (damage > 0)
        {
            enemyCharacter.TakeDamage(damage);
            ShowMessage($"[HIT] {enemyCharacter.characterName} takes {damage} damage!");
            
            // Trigger camera shake on hit
            if (cameraShake != null)
            {
                // Bigger shake for super attacks
                if (actionType == "super")
                {
                    cameraShake.TriggerShake(0.3f, 0.25f);
                }
                else
                {
                    cameraShake.TriggerShake();
                }
            }
            
            // Play hit particle effect at enemy position
            if (hitEffect != null)
            {
                hitEffect.transform.position = enemyCharacter.transform.position;
                hitEffect.Play();
            }
        }
        
        // Save current character stats
        SaveCurrentCharacterStats();
        
        // Update UI
        UpdateBattleUI();
        
        // Check for battle end
        CheckBattleEnd();
        
        // If battle continues, start enemy's turn
        if (!battleEnded)
        {
            StartEnemyTurn();
        }
    }
    
    void CheckBattleEnd()
    {
        // Check if current character is defeated
        if (playerCharacter.currentHealth <= 0)
        {
            Debug.Log("Character defeated! Starting character switch process...");
            
            // Mark current character as defeated
            if (teamManager != null && currentPlayerIndex < teamManager.currentTeam.Count)
            {
                teamManager.currentTeam[currentPlayerIndex].isDefeated = true;
                teamManager.currentTeam[currentPlayerIndex].currentHealth = 0;
                ShowMessage($"[DEFEAT] {teamManager.currentTeam[currentPlayerIndex].characterData.characterName} is defeated!");
            }
            
            // Try to switch to next alive character
            SwitchToNextCharacter();
            
            // Check if team has any alive characters
            CheckTeamDefeat();
            
            // If team is not defeated, ensure it's still the player's turn
            if (!battleEnded)
            {
                Debug.Log("Team not defeated, ensuring player's turn continues...");
                StartPlayerTurn();
            }
        }
        else if (enemyCharacter.currentHealth <= 0)
        {
            // Enemy defeated
            battleEnded = true;
            ShowMessage($"[VICTORY] {enemyCharacter.characterName} defeated!");
            EndBattle(true); // true = player won
        }
    }
    
    void EndBattle(bool playerWon)
    {
        // Disable all buttons
        SetButtonsEnabled(false);
        
        // Update BattleData with player's current stats
        Debug.Log($"Battle ending - Saving stats: HP {playerCharacter.currentHealth}/{playerCharacter.maxHealth}, MP {playerCharacter.currentMana}/{playerCharacter.maxMana}");
        BattleData.UpdatePlayerStats(
            playerCharacter.currentHealth,
            playerCharacter.maxHealth,
            playerCharacter.currentMana,
            playerCharacter.maxMana
        );
        
        if (playerWon)
        {
            // Mark troll as defeated
            GameProgress.DefeatTroll(BattleData.enemyToFightIndex);
            ShowMessage($"[VICTORY] Battle in {BattleData.battleZoneName} won! Troll {BattleData.enemyToFightIndex} defeated!");
            Debug.Log($"[VICTORY] Battle in {BattleData.battleZoneName} won! Troll {BattleData.enemyToFightIndex} defeated!");
            
            // Play victory particle effect
            if (victoryEffect != null)
            {
                victoryEffect.transform.position = playerCharacter.transform.position;
                victoryEffect.Play();
            }
            
            // Wait a moment then return to map
            Invoke(nameof(ReturnToMap), 2f);
        }
        else
        {
            // Player lost - show game over
            ShowMessage($"[GAME OVER] Defeated in {BattleData.battleZoneName}!");
            Debug.Log($"[GAME OVER] Defeated in {BattleData.battleZoneName}!");
            Invoke(nameof(ShowGameOver), 2f);
        }
    }
    
    void ReturnToMap()
    {
        // Trigger auto-save before returning to map (will be handled by SaveManager in MapScene)
        // The SaveManager will auto-save when it detects a troll was defeated
        
        // Return to the map scene
        SceneManager.LoadScene("MapScene"); // Updated to match renamed scene
    }
    
    void ShowGameOver()
    {
        // Reset game progress
        GameProgress.ResetProgress();
        
        // Reset player stats to full
        BattleData.ResetBattleData();
        
        // Return to map scene (player will start fresh)
        SceneManager.LoadScene("MapScene");
        
        Debug.Log("🔄 Game Over! Progress reset. Starting fresh adventure!");
    }
    
    void SetButtonsEnabled(bool enabled)
    {
        Debug.Log($"SetButtonsEnabled({enabled}): Attack={attackButton != null}, SuperAttack={superAttackButton != null}");
        
        if (attackButton != null)
        {
            attackButton.interactable = enabled;
            Debug.Log($"Attack button set to {enabled}");
        }
        
        if (superAttackButton != null)
        {
            superAttackButton.interactable = enabled;
            Debug.Log($"Super Attack button set to {enabled}");
        }
    }
    
    void UpdateBattleUI()
    {
        // Update health bars (handled automatically by BattleCharacter)
        // Update mana bars manually to ensure they reflect current values
        if (playerCharacter.manaBar != null)
        {
            playerCharacter.manaBar.value = playerCharacter.currentMana;
            playerCharacter.manaBar.maxValue = playerCharacter.maxMana;
        }
        
        if (enemyCharacter.manaBar != null)
        {
            enemyCharacter.manaBar.value = enemyCharacter.currentMana;
            enemyCharacter.manaBar.maxValue = enemyCharacter.maxMana;
        }
        
        // Update button states based on mana
        if (superAttackButton != null)
        {
            bool canUseSuperAttack = playerCharacter.CanSuperAttack();
            superAttackButton.interactable = canUseSuperAttack;
            
            // Log when button becomes disabled
            if (!canUseSuperAttack && isPlayerTurn)
            {
                Debug.Log($"Super Attack disabled - Need 20 MP, have {playerCharacter.currentMana} MP");
            }
        }
    }
    
    void SetupTeamManagement()
    {
        Debug.Log($"SetupTeamManagement: teamManager={teamManager != null}, switchButton={switchCharacterButton != null}, currentText={currentCharacterText != null}");
        
        if (switchCharacterButton != null)
        {
            switchCharacterButton.onClick.AddListener(SwitchToNextCharacter);
            Debug.Log("Switch character button listener added!");
        }
        else
        {
            Debug.LogWarning("Switch character button is null!");
        }
        
        UpdateCurrentCharacterDisplay();
    }
    
    void SwitchToNextCharacter()
    {
        Debug.Log($"SwitchToNextCharacter: teamManager={teamManager != null}, teamCount={teamManager?.currentTeam.Count ?? 0}");
        
        if (teamManager != null && teamManager.currentTeam.Count > 1)
        {
            // Find next alive character
            int startIndex = currentPlayerIndex;
            int nextIndex = (currentPlayerIndex + 1) % teamManager.currentTeam.Count;
            
            Debug.Log($"Starting search from index {startIndex}, looking for next alive character...");
            
            while (nextIndex != startIndex)
            {
                TeamMember nextMember = teamManager.currentTeam[nextIndex];
                Debug.Log($"Checking character {nextIndex}: {nextMember.characterData.characterName}, defeated: {nextMember.isDefeated}");
                
                if (!nextMember.isDefeated)
                {
                    Debug.Log($"Found alive character: {nextMember.characterData.characterName} at index {nextIndex}");
                    SwitchToCharacter(nextIndex);
                    return;
                }
                nextIndex = (nextIndex + 1) % teamManager.currentTeam.Count;
            }
            
            Debug.Log("No other alive characters found!");
            ShowMessage("[TEAM] No other alive characters to switch to!");
        }
        else
        {
            Debug.Log("Cannot switch: teamManager is null or team has only 1 character");
        }
    }
    
    void SwitchToCharacter(int characterIndex)
    {
        if (teamManager != null && characterIndex < teamManager.currentTeam.Count)
        {
            TeamMember newCharacter = teamManager.currentTeam[characterIndex];
            
            if (newCharacter.isDefeated)
            {
                ShowMessage($"[TEAM] {newCharacter.characterData.characterName} is defeated and cannot fight!");
                return;
            }
            
            currentPlayerIndex = characterIndex;
            teamManager.activeCharacterIndex = characterIndex;
            
            // Update player character stats
            playerCharacter.characterName = newCharacter.characterData.characterName;
            playerCharacter.maxHealth = newCharacter.characterData.baseHealth;
            playerCharacter.currentHealth = newCharacter.currentHealth;
            playerCharacter.maxMana = newCharacter.characterData.baseMana;
            playerCharacter.currentMana = newCharacter.currentMana;
            playerCharacter.attackPower = newCharacter.characterData.baseAttack;
            playerCharacter.defense = newCharacter.characterData.baseDefense;
            playerCharacter.isAlive = true; // Ensure the new character is marked as alive
            
            // Update UI
            UpdateBattleUI();
            UpdateCurrentCharacterDisplay();
            
            // Ensure it's still the player's turn after switching
            StartPlayerTurn();
            
            ShowMessage($"[TEAM] Switched to {newCharacter.characterData.characterName}!");
        }
    }
    
    void UpdateCurrentCharacterDisplay()
    {
        if (currentCharacterText != null && teamManager != null)
        {
            if (currentPlayerIndex < teamManager.currentTeam.Count)
            {
                TeamMember currentMember = teamManager.currentTeam[currentPlayerIndex];
                currentCharacterText.text = $"Active: {currentMember.characterData.characterName}";
            }
        }
    }
    
    void SaveCurrentCharacterStats()
    {
        if (teamManager != null && currentPlayerIndex < teamManager.currentTeam.Count)
        {
            TeamMember currentMember = teamManager.currentTeam[currentPlayerIndex];
            currentMember.currentHealth = playerCharacter.currentHealth;
            currentMember.currentMana = playerCharacter.currentMana;
        }
    }
    
    void CheckTeamDefeat()
    {
        if (teamManager != null)
        {
            bool hasAliveCharacters = teamManager.HasAliveCharacters();
            if (!hasAliveCharacters)
            {
                ShowMessage("[DEFEAT] All team members are defeated!");
                EndBattle(false);
            }
        }
    }
}
