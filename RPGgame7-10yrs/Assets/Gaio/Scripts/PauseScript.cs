using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class PauseScript : MonoBehaviour
{
    public GameObject PauseScreen;



    public List<GameObject> Arrows = new List<GameObject>();
    public int CurrentArrow;

    private bool IsPaused;

    void Start()
    {
        IsPaused = false;
        Time.timeScale = 1f;
    }

   
    void PauseUpdate()
    {
        //ARROW INPUT
        if (Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.S))
        {
            CurrentArrow++;
            if (CurrentArrow > 2)
            {
                CurrentArrow = 2;
            }
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.W))
        {
            CurrentArrow--;
            if (CurrentArrow < 0)
            {
                CurrentArrow = 0;
            }
        }

        //INPUT
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.Return))
        {
            if (CurrentArrow == 0)
            {
                IsPaused = false;
                PauseScreen.SetActive(false);
                Time.timeScale = 1;
            }
            else if (CurrentArrow == 1)
            {

            }
            else if (CurrentArrow == 2)
            {
                SceneManager.LoadScene(1);
            }
        }

        //UPDATE
        for (int i = 0; i < Arrows.Count; i++)
        {
            if (i == CurrentArrow)
            {
                Arrows[i].SetActive(true);
            }
            else
            {
                Arrows[i].SetActive(false);
            }
        }
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
                CurrentArrow = 2;
                Time.timeScale = 0;
            }
        }

        if (IsPaused)
        {
            PauseUpdate();
        }
    }
}
