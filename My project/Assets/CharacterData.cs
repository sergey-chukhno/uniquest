using UnityEngine;

[CreateAssetMenu(fileName = "New Character", menuName = "RPG/Character Data")]
public class CharacterData : ScriptableObject
{
    [Header("Character Info")]
    public string characterName;
    public string description;
    public Sprite characterSprite;
    public Sprite characterPortrait;

    [Header("Base Stats")]
    public int baseHealth = 100;
    public int baseMana = 50;
    public int baseAttack = 20;
    public int baseDefense = 10;

    [Header("Special Abilities")]
    public string specialAbilityName;
    public string specialAbilityDescription;
    public int specialAbilityCost = 20;
    public int specialAbilityDamage = 35;

    [Header("Character Traits")]
    public CharacterType characterType;
    public string[] characterQuotes;
}

public enum CharacterType
{
    Warrior,    // Balanced stats, good all-around
    Mage,       // High mana, powerful spells, low health
    Rogue       // High attack, low defense, fast
}
