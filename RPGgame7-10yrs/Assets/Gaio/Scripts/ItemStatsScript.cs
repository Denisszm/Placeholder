using UnityEngine;

public class ItemStatsScript : MonoBehaviour
{
    public ItemStats Item;

    public string description;
    public Sprite sprite;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        description = Item.Description;
        sprite = Item.Image;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
