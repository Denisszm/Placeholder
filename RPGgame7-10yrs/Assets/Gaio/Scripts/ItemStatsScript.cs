using UnityEngine;

public class ItemStatsScript : MonoBehaviour
{
    public ItemStats Item;

    public string description;
    public Sprite sprite;

    private void Awake()
    {
        description = Item.Description;
        sprite = Item.Image;
        Item.Item = ItemStats.ItemEnum.Boots;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
