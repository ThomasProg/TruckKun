using UnityEngine;

[CreateAssetMenu(fileName = "CraneGamePrize", menuName = "ScriptableObjects/CraneGame", order = 1)]
public class CraneGamePrizeData : ScriptableObject
{
    public string DisplayName;
    public string Description;
    public Sprite Icon;
}