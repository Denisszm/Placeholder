using TMPro;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    public GameObject Arrow;
    public int CurrentArrowPosition;
    public Vector3[] ArrowPositions = new Vector3[4];
    public Text Press;
    private float PressTimer;
    public int Phase;

    public GameObject First;
    public GameObject Second;

    void Start()
    {
        CurrentArrowPosition = 0;
        Phase = 1;
        PressTimer = 2;
    }

    void FirstInput()
    {
        if (Keyboard.current.anyKey.isPressed)
        {
            Phase = 2;
            Press.text = "";
        }
    }
    void FirstUpdate()
    {
        First.SetActive(true);

        PressTimer -= Time.deltaTime;
        if (PressTimer > 1)
        {
            Press.text = "Press Any Key to Continue";
        }
        else if(PressTimer > 0)
        {
            Press.text = "";
        }
        else
        {
            PressTimer = 2;
        }
    }


    void SecondInput()
    {
        //Arrow Input
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            CurrentArrowPosition++;
            if (CurrentArrowPosition > 3)
            {
                CurrentArrowPosition = 3;
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
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (CurrentArrowPosition == 0)
            {
                GameManagerScript.instance.playerCount = 1;
                SceneManager.LoadScene(3);
            }
            else if (CurrentArrowPosition == 1)
            {
                GameManagerScript.instance.playerCount = 2;
                SceneManager.LoadScene(3);
            }
            else if (CurrentArrowPosition == 2)
            {
                SceneManager.LoadScene(2);
            }
            else if (CurrentArrowPosition == 3)
            {
                Application.Quit();
            }
        }
    }
    void SecondUpdate()
    {
        First.SetActive(false);
        Second.SetActive(true);

        Arrow.transform.position = ArrowPositions[CurrentArrowPosition];
    }

    void Update()
    {
        if (Phase == 1)
        {
            FirstInput();
            FirstUpdate();
        }
        else if (Phase == 2)
        {
            SecondInput();
            SecondUpdate();
        }
    }
}
