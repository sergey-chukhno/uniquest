using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Linq;

public class CharacterSelectionUI : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject characterSelectionPanel;
    public GameObject teamManagementPanel;

    [Header("Character Display")]
    public Transform characterButtonParent;
    public GameObject characterButtonPrefab;

    [Header("Team Display")]
    public Transform teamMemberParent;
    public GameObject teamMemberPrefab;

    [Header("Character Info Display")]
    public TextMeshProUGUI characterNameText;
    public TextMeshProUGUI characterDescriptionText;
    public Image characterPortraitImage;
    public TextMeshProUGUI healthText;
    public TextMeshProUGUI manaText;
    public TextMeshProUGUI attackText;
    public TextMeshProUGUI defenseText;
    public TextMeshProUGUI specialAbilityNameText;
    public TextMeshProUGUI specialAbilityDescriptionText;

    [Header("Team Info")]
    public TextMeshProUGUI teamSizeText;
    public Button startBattleButton;

    [Header("Action Buttons")]
    public Button addCharacterButton;
    public Button removeCharacterButton;
    public Button confirmTeamButton;

    private List<GameObject> characterButtons = new List<GameObject>();
    private CharacterData selectedCharacter;

    void Awake()
    {
        Debug.Log("CharacterSelectionUI Awake() called!");
    }

    void Start()
    {
        Debug.Log("CharacterSelectionUI Start() called!");
        
        // Reset team for fresh selection each battle
        if (TeamManager.Instance != null)
        {
            TeamManager.Instance.ResetTeamForNewBattle();
        }
        
        InitializeUI();
        CreateCharacterButtons();
        RefreshTeamDisplay();
        
        // Auto-select first character
        if (TeamManager.Instance != null && TeamManager.Instance.availableCharacters.Count > 0)
        {
            SelectCharacter(TeamManager.Instance.availableCharacters[0]);
        }
    }

    void InitializeUI()
    {
        Debug.Log("Initializing UI...");
        
        if (startBattleButton != null)
        {
            startBattleButton.onClick.AddListener(StartBattle);
        }
        
        if (addCharacterButton != null)
        {
            addCharacterButton.onClick.AddListener(AddCharacterToTeam);
        }
        
        if (removeCharacterButton != null)
        {
            removeCharacterButton.onClick.AddListener(RemoveCharacterFromTeam);
        }
        
        if (confirmTeamButton != null)
        {
            confirmTeamButton.onClick.AddListener(ConfirmTeam);
        }
    }

    void CreateCharacterButtons()
    {
        Debug.Log("Creating character buttons...");
        
        if (TeamManager.Instance == null)
        {
            Debug.LogError("TeamManager.Instance is null!");
            return;
        }

        Debug.Log($"Available characters count: {TeamManager.Instance.availableCharacters.Count}");

        // Clear existing buttons
        foreach (GameObject button in characterButtons)
        {
            if (button != null) Destroy(button);
        }
        characterButtons.Clear();

        // Create buttons for each character
        for (int i = 0; i < TeamManager.Instance.availableCharacters.Count; i++)
        {
            CharacterData character = TeamManager.Instance.availableCharacters[i];
            Debug.Log($"Creating button for character {i}: {character.characterName}");

            if (characterButtonPrefab != null && characterButtonParent != null)
            {
                GameObject buttonObj = Instantiate(characterButtonPrefab, characterButtonParent);
                characterButtons.Add(buttonObj);

                // Set up the button
                Button button = buttonObj.GetComponent<Button>();
                if (button != null)
                {
                    int index = i; // Capture for closure
                    button.onClick.AddListener(() => SelectCharacter(TeamManager.Instance.availableCharacters[index]));
                }

                // Set character info on the button
                CharacterButtonInfo buttonInfo = buttonObj.GetComponent<CharacterButtonInfo>();
                if (buttonInfo == null)
                {
                    buttonInfo = buttonObj.AddComponent<CharacterButtonInfo>();
                }
                buttonInfo.SetCharacter(character);

                // Position the button
                RectTransform rect = buttonObj.GetComponent<RectTransform>();
                if (rect != null)
                {
                    rect.anchoredPosition = new Vector2(i * 200 - 200, 0);
                }
            }
        }

        Debug.Log($"Created {characterButtons.Count} character buttons");
    }

    public void SelectCharacter(CharacterData character)
    {
        Debug.Log($"Selecting character: {character.characterName}");
        selectedCharacter = character;
        UpdateCharacterInfo(character);
        RefreshTeamDisplay();
    }

    void UpdateCharacterInfo(CharacterData character)
    {
        if (characterNameText != null) characterNameText.text = character.characterName;
        if (characterDescriptionText != null) characterDescriptionText.text = character.description;
        if (characterPortraitImage != null) characterPortraitImage.sprite = character.characterPortrait;
        if (healthText != null) healthText.text = $"Health: {character.baseHealth}";
        if (manaText != null) manaText.text = $"Mana: {character.baseMana}";
        if (attackText != null) attackText.text = $"Attack: {character.baseAttack}";
        if (defenseText != null) defenseText.text = $"Defense: {character.baseDefense}";
        if (specialAbilityNameText != null) specialAbilityNameText.text = character.specialAbilityName;
        if (specialAbilityDescriptionText != null) specialAbilityDescriptionText.text = character.specialAbilityDescription;
    }

    void RefreshTeamDisplay()
    {
        if (TeamManager.Instance == null) return;

        // Update team size text
        if (teamSizeText != null)
        {
            teamSizeText.text = $"Team: {TeamManager.Instance.currentTeam.Count}/{TeamManager.Instance.maxTeamSize}";
        }

        // Update start battle button
        if (startBattleButton != null)
        {
            startBattleButton.interactable = TeamManager.Instance.currentTeam.Count > 0;
        }

        // Update action buttons
        if (addCharacterButton != null)
        {
            bool canAdd = selectedCharacter != null && 
                TeamManager.Instance.currentTeam.Count < TeamManager.Instance.maxTeamSize &&
                !TeamManager.Instance.currentTeam.Any(member => member.characterData == selectedCharacter);
            addCharacterButton.interactable = canAdd;
        }

        if (removeCharacterButton != null)
        {
            bool canRemove = selectedCharacter != null && 
                TeamManager.Instance.currentTeam.Any(member => member.characterData == selectedCharacter);
            removeCharacterButton.interactable = canRemove;
        }
    }

    public void AddCharacterToTeam()
    {
        if (selectedCharacter != null && TeamManager.Instance != null)
        {
            bool success = TeamManager.Instance.AddCharacterToTeam(selectedCharacter);
            if (success)
            {
                RefreshTeamDisplay();
                Debug.Log($"Added {selectedCharacter.characterName} to team!");
            }
            else
            {
                Debug.Log($"Failed to add {selectedCharacter.characterName} to team!");
            }
        }
    }

    public void RemoveCharacterFromTeam()
    {
        if (selectedCharacter != null && TeamManager.Instance != null)
        {
            bool success = TeamManager.Instance.RemoveCharacterFromTeam(selectedCharacter);
            if (success)
            {
                RefreshTeamDisplay();
                Debug.Log($"Removed {selectedCharacter.characterName} from team!");
            }
        }
    }

    public void ConfirmTeam()
    {
        if (TeamManager.Instance != null)
        {
            Debug.Log($"Team confirmed with {TeamManager.Instance.currentTeam.Count} characters!");
        }
    }

    public void StartBattle()
    {
        if (TeamManager.Instance != null && TeamManager.Instance.currentTeam.Count > 0)
        {
            Debug.Log("Starting battle!");
            // Hide character selection UI
            gameObject.SetActive(false);
            
            // Enable BattleManager to start the battle
            BattleManager battleManager = FindObjectOfType<BattleManager>();
            if (battleManager != null)
            {
                battleManager.enabled = true;
                // Don't call Start() directly - just enable the component
                Debug.Log("BattleManager enabled!");
            }
        }
    }
}

// Simple component to hold character info on buttons
public class CharacterButtonInfo : MonoBehaviour
{
    public CharacterData characterData;
    
    public void SetCharacter(CharacterData character)
    {
        characterData = character;
        
        // Update button appearance
        Image image = GetComponent<Image>();
        if (image != null && character.characterPortrait != null)
        {
            image.sprite = character.characterPortrait;
        }
        
        // Update text if there's a text component
        TextMeshProUGUI text = GetComponentInChildren<TextMeshProUGUI>();
        if (text != null)
        {
            text.text = character.characterName;
        }
    }
}
