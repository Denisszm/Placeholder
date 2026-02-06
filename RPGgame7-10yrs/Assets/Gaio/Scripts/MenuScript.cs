using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuScript : MonoBehaviour
{
    public GameObject Arrow;
    public int CurrentArrowPosition;
    public Vector3[] ArrowPositions = new Vector3[3];

    void Start()
    {
        CurrentArrowPosition = 0;
    }

    void Update()
    {
        //INPUT

        //Arrow Input
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            CurrentArrowPosition++;
            if (CurrentArrowPosition > 2)
            {
                CurrentArrowPosition = 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            CurrentArrowPosition--;
            if (CurrentArrowPosition < 0)
            {
                CurrentArrowPosition = 0;
            }
        }
        
        //Select Input
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            if (CurrentArrowPosition == 0)
            {
                SceneManager.LoadScene(3);
            }
            else if (CurrentArrowPosition == 1)
            {
                SceneManager.LoadScene(2);
            }
            else if (CurrentArrowPosition == 2)
            {
                Application.Quit();
            }
        }


        //UPDATE
        Arrow.transform.position = ArrowPositions[CurrentArrowPosition];
    }
}
