using UnityEngine;

[CreateAssetMenu(fileName = "SkillTree", menuName = "Scriptable Objects/SkillTree")]
public class SkillTree : ScriptableObject
{
    static int currentSPoints;
    static int spentSPoints;
    static int totalSPoints = currentSPoints + spentSPoints;
}
