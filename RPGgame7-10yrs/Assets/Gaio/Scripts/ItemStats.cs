using UnityEngine;

[CreateAssetMenu(fileName = "ItemStats", menuName = "Scriptable Objects/ItemStats")]
public class ItemStats : ScriptableObject
{
    public string Description;

    public Sprite Image;

    public enum ItemEnum {Bag, Book, Boots, Helmet, Poison, Robe, Shield, Skull }

    public ItemEnum Item;
}
