using UnityEngine;
using UnityEngine.SceneManagement;

public class OptionsScript : MonoBehaviour
{
    public GameObject Arrow;
    public int CurrentArrowPosition;
    public Vector3[] ArrowPositions = new Vector3[3];
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CurrentArrowPosition = 2;
    }

    // Update is called once per frame
    void Update()
    {
        //ARROW INPUT
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

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (CurrentArrowPosition == 0)
            {
                
            }
            else if (CurrentArrowPosition == 1)
            {
                
            }
            else if (CurrentArrowPosition == 2)
            {
                SceneManager.LoadScene(1);
            }
        }

        //UPDATE
        Arrow.transform.position = ArrowPositions[CurrentArrowPosition];
    }
}
