using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class TeamMember
{
    public CharacterData characterData;
    public int currentHealth;
    public int currentMana;
    public bool isDefeated;

    public TeamMember(CharacterData data)
    {
        characterData = data;
        currentHealth = data.baseHealth;
        currentMana = data.baseMana;
        isDefeated = false;
    }
}

public class TeamManager : MonoBehaviour
{
    [Header("Team Configuration")]
    public List<CharacterData> availableCharacters = new List<CharacterData>();
    public int maxTeamSize = 3;

    [Header("Current Team")]
    public List<TeamMember> currentTeam = new List<TeamMember>();
    public int activeCharacterIndex = 0;

    public static TeamManager Instance { get; private set; }

    void Awake()
    {
        Debug.Log("TeamManager Awake() called!");
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("TeamManager Instance set!");
        }
        else
        {
            Debug.LogWarning("Duplicate TeamManager found, destroying!");
            Destroy(gameObject);
        }
    }

    void Start()
    {
        Debug.Log("TeamManager Start() called!");
        Debug.Log($"Available characters count: {availableCharacters.Count}");
        
        // Log each character
        for (int i = 0; i < availableCharacters.Count; i++)
        {
            if (availableCharacters[i] != null)
            {
                Debug.Log($"Character {i}: {availableCharacters[i].characterName}");
            }
        }
    }

    public bool AddCharacterToTeam(CharacterData characterData)
    {
        if (characterData == null)
        {
            Debug.LogWarning("Cannot add null character to team!");
            return false;
        }

        if (currentTeam.Count >= maxTeamSize)
        {
            Debug.LogWarning("Team is full!");
            return false;
        }

        if (currentTeam.Any(member => member.characterData == characterData))
        {
            Debug.LogWarning($"{characterData.characterName} is already in the team!");
            return false;
        }

        currentTeam.Add(new TeamMember(characterData));
        Debug.Log($"{characterData.characterName} added to team! Current team size: {currentTeam.Count}");
        return true;
    }

    public bool RemoveCharacterFromTeam(CharacterData characterData)
    {
        if (characterData == null)
        {
            Debug.LogWarning("Cannot remove null character from team!");
            return false;
        }

        TeamMember memberToRemove = currentTeam.FirstOrDefault(member => member.characterData == characterData);
        if (memberToRemove != null)
        {
            currentTeam.Remove(memberToRemove);
            Debug.Log($"{characterData.characterName} removed from team! Current team size: {currentTeam.Count}");
            return true;
        }

        Debug.LogWarning($"{characterData.characterName} not found in team!");
        return false;
    }

    public TeamMember GetActiveCharacter()
    {
        if (currentTeam.Count > 0 && activeCharacterIndex < currentTeam.Count)
        {
            return currentTeam[activeCharacterIndex];
        }
        return null;
    }

    public bool HasAliveCharacters()
    {
        return currentTeam.Any(member => !member.isDefeated);
    }

    public void ResetTeam()
    {
        currentTeam.Clear();
        activeCharacterIndex = 0;
        Debug.Log("Team reset!");
    }
    
    public void ResetTeamForNewBattle()
    {
        // Clear team for fresh selection each battle
        currentTeam.Clear();
        activeCharacterIndex = 0;
        Debug.Log("Team reset for new battle - ready for character selection!");
    }
}
