using UnityEngine;

[CreateAssetMenu(fileName = "UnitType", menuName = "Scriptable Objects/UnitType")]
public class UnitType : ScriptableObject
{
    public string unitName = "New Unit";
    public int health = 0;
    public int attack = 0;
    public int manaCost = 0;

    public Sprite cardSprite;
    public Sprite unitSprite;
    
}
