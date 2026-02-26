using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseScript : MonoBehaviour
{
    public GameObject PauseScreen;

    public GameObject Arrow;
    public int CurrentArrowPosition;
    public Vector3[] ArrowPositions = new Vector3[3];

    private bool IsPaused;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        IsPaused = false;
    }

    // Update is called once per frame
   
    void PauseUpdate()
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

        //INPUT
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (CurrentArrowPosition == 0)
            {
                SceneManager.LoadScene(1);
            }
            else if (CurrentArrowPosition == 1)
            {

            }
            else if (CurrentArrowPosition == 2)
            {
                IsPaused = false;
                PauseScreen.SetActive(false);
                Time.timeScale = 1;
            }
        }

        //UPDATE
        Arrow.transform.position = ArrowPositions[CurrentArrowPosition];
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (IsPaused)
            {
                IsPaused = false;
                PauseScreen.SetActive(false);

                Time.timeScale = 1;
       
            }
            else
            {

                
                IsPaused = true;
                PauseScreen.SetActive(true);
                CurrentArrowPosition = 2;
                Time.timeScale = 0;
            }
        }

        if (IsPaused)
        {
            PauseUpdate();
        }
    }
}
