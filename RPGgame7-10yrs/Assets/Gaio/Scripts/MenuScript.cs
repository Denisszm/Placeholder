using UnityEngine;
using UnityEngine.InputSystem;

public class MenuScript : MonoBehaviour
{
    public GameObject Arrow;
    public int CurrentArrowPosition;
    public Vector2[] ArrowPositions = new Vector2[3];

    void Start()
    {
        CurrentArrowPosition = 0;
    }

    void Update()
    {
       
    }
}
