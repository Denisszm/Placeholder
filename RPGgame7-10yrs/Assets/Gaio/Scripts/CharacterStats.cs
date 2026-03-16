using UnityEngine;

[CreateAssetMenu(fileName = "CharacterStats", menuName = "Scriptable Objects/CharacterStats")]
public class CharacterStats : ScriptableObject
{
    public enum ClassEnum { Swordsman, Brute, Archer, Sorcerer }

    public ClassEnum Class;

    public enum DamageEnum { Powerful, Average, Lacking }

    public DamageEnum Damage;

    public enum SpeedEnum { Swift, Average, Clumsy }

    public SpeedEnum Speed;

    public enum HealthEnum { Tank, Average, Weak }

    public HealthEnum Health;

    public enum ArtifactEnum { Shield, Boots, Poison, Skull, CoinBag, Helmet, Robe, Book }

    public ArtifactEnum Artifact;
}
