using NUnit.Framework;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SelectionScript : MonoBehaviour
{
    public SpriteRenderer background;

    public Sprite b1;
    public Sprite b2;

    public GameObject player1Icon;
    public GameObject player2Icon;

    public List<GameObject> characters;
    private int currentCharacter;

    public Vector3 singlePosition;
    public Vector3 multiPosition;

    public Vector3 player1Position;
    public Vector3 player2Position;

    private bool firstIsChoosing;
    private bool secondIsChoosing;

    public Image p1ItemImage;
    public Image p2ItemImage;
    public Text p1ItemText;
    public Text p2ItemText;

    public Text p1DamageText;
    public Text p1SpeedText;
    public Text p1HealthText;

    public Text p2DamageText;
    public Text p2SpeedText;
    public Text p2HealthText;

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
        ShowCharacter();
        UpdateSingleStats();
    }
    void MultiUpdate()
    {
        ShowCharacter();
        UpdateMultiStats();
    }
    void ShowCharacter()
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
    void UpdateSingleStats()
    {
        player1Icon.GetComponent<SpriteRenderer>().sprite = characters[currentCharacter].GetComponent<SpriteRenderer>().sprite;

        p1ItemImage.sprite = characters[currentCharacter].GetComponent<ItemStatsScript>().Item.Image;
        p1ItemText.text = characters[currentCharacter].GetComponent<ItemStatsScript>().Item.Description;

        p1DamageText.text = $"Damage: {characters[currentCharacter].GetComponent<PlayerStatsScript>().stats.Damage.ToString()}";
        p1SpeedText.text = $"Speed: {characters[currentCharacter].GetComponent<PlayerStatsScript>().stats.Speed.ToString()}";
        p1HealthText.text = $"Health: {characters[currentCharacter].GetComponent<PlayerStatsScript>().stats.Health.ToString()}";
    }
    void UpdateMultiStats()
    {
        if (firstIsChoosing)
        {
            UpdateSingleStats();
        }
        else if (secondIsChoosing)
        {
            player2Icon.gameObject.SetActive(true);
            player2Icon.GetComponent<SpriteRenderer>().sprite = characters[currentCharacter].GetComponent<SpriteRenderer>().sprite;

            p2ItemImage.gameObject.SetActive(true);
            p2ItemImage.sprite = characters[currentCharacter].GetComponent<ItemStatsScript>().Item.Image;
            p2ItemText.text = characters[currentCharacter].GetComponent<ItemStatsScript>().Item.Description;

            p2DamageText.text = $"Damage: {characters[currentCharacter].GetComponent<PlayerStatsScript>().stats.Damage.ToString()}";
            p2SpeedText.text = $"Speed: {characters[currentCharacter].GetComponent<PlayerStatsScript>().stats.Speed.ToString()}";
            p2HealthText.text = $"Health: {characters[currentCharacter].GetComponent<PlayerStatsScript>().stats.Health.ToString()}";
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
                GameManagerScript.instance.player1 = characters[currentCharacter];

                if (GameManagerScript.instance.playerCount == 1)
                {
                    SceneManager.LoadScene(4);
                }
                else
                {
                    firstIsChoosing = false;
                    secondIsChoosing = true;
                }
            }
            else
            {
                GameManagerScript.instance.player2 = characters[currentCharacter];
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
