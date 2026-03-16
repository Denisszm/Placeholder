using NUnit.Framework;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectionScript : MonoBehaviour
{
    public SpriteRenderer background;

    public Sprite b1;
    public Sprite b2;

    public List<GameObject> characters;
    private int currentCharacter;

    public Vector3 singlePosition;
    public Vector3 multiPosition;

    private bool firstIsChoosing;
    private bool secondIsChoosing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if (GameManagerScript.instance.playerCount == 1)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                characters[i].transform.position = singlePosition;
            }
            background.sprite = b1;
        }
        else if (GameManagerScript.instance.playerCount == 2)
        {
            for (int i = 0; i < characters.Count; i++)
            {
                characters[i].transform.position = multiPosition;
            }
            background.sprite = b2;
        }
        firstIsChoosing = true;
    }

    // Update is called once per frame
    void SingleUpdate()
    {
        LoadCharacter();
    }
    void MultiUpdate()
    {
        LoadCharacter();
    }
    void LoadCharacter()
    {
        for (int i = 0; i < characters.Count; i++)
        {
            if (i == currentCharacter)
            {
                characters[i].gameObject.SetActive(true);
            }
            else
            {
                characters[i].gameObject.SetActive(false);
            }
        }
    }
    void PlayerInput()
    {
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            CharacterMinus();
        }
        if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            CharacterPlus();
        }
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space))
        {
            if (firstIsChoosing)
            {
                firstIsChoosing = false;
                secondIsChoosing = true;
            }
            else
            {
                SceneManager.LoadScene(4);
            }
        }
        if (Input.GetKeyDown(KeyCode.Backspace) || Input.GetKeyDown(KeyCode.Escape))
        {
            if (secondIsChoosing)
            {
                secondIsChoosing = false;
                firstIsChoosing = true;
            }
            if (firstIsChoosing)
            {
                SceneManager.LoadScene(1);
            }
        }
    }
    void CharacterPlus()
    {
        currentCharacter++;
        if (currentCharacter > (characters.Count - 1))
        {
            currentCharacter = 0;
        }
    }
    void CharacterMinus()
    {
        currentCharacter--;
        if (currentCharacter < 0)
        {
            currentCharacter = (characters.Count - 1);
        }
    }
    void Update()
    {
        PlayerInput();

        if (GameManagerScript.instance.playerCount == 1)
        {
            SingleUpdate();
        }
        else if (GameManagerScript.instance.playerCount == 2)
        {
            MultiUpdate();
        }
    }
}
