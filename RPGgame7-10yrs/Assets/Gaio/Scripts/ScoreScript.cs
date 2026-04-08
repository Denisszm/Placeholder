using NUnit.Framework;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class ScoreScript : MonoBehaviour
{
    public Transform entryContainer;
    public Transform entryTemplate;
    private List<Transform> highscoreEntryTransforms;

    private bool isPlayer1;
    private bool isPlayer2;

    public List<GameObject> characters;
    private int currentCharacter;

    public Text scoreText;

    private class Highscores
    {
        public List<HighscoreEntry> highscoreEntries;
    }

    [System.Serializable]
    private class HighscoreEntry
    {
        public int score;
        public string name;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (isPlayer1)
            {
                if (GameManagerScript.instance.playerCount == 2)
                {
                    isPlayer1 = false;
                    isPlayer2 = false;

                    currentCharacter = GameManagerScript.instance.player2index;
                    AddHighscoreEntry(GameManagerScript.instance.score2, "P2");
                    string jsonString = PlayerPrefs.GetString("highscoreTable");
                    Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

                    ClearHighscoreEntries();
                    highscoreEntryTransforms = new List<Transform>();

                    for (int i = 0; i < highscores.highscoreEntries.Count; i++)
                    {
                        for (int j = 0; j < highscores.highscoreEntries.Count; j++)
                        {
                            if (highscores.highscoreEntries[i].score > highscores.highscoreEntries[j].score)
                            {
                                HighscoreEntry temporary = highscores.highscoreEntries[i];
                                highscores.highscoreEntries[i] = highscores.highscoreEntries[j];
                                highscores.highscoreEntries[j] = temporary;
                            }
                        }
                    }

                    if (highscores.highscoreEntries.Count > 10)
                    {
                        for (int h = highscores.highscoreEntries.Count; h > 10; h--)
                        {
                            highscores.highscoreEntries.RemoveAt(10);
                        }
                    }

                    for (int i = 0; i < highscores.highscoreEntries.Count; i++)
                    {
                        CreateHighscoreEntryTransform(highscores.highscoreEntries[i], entryContainer, highscoreEntryTransforms);
                    }
                }
                else
                {
                    SceneManager.LoadScene(1);
                }
            }
            else
            {
                SceneManager.LoadScene(1);
            }
        }

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

        if (isPlayer1)
        {
            scoreText.text = $"SCORE: {GameManagerScript.instance.score1}";
        }
        else
        {
            scoreText.text = $"SCORE: {GameManagerScript.instance.score2}";
        }
    }

    private void Awake()
    {
        currentCharacter = GameManagerScript.instance.player1index;

        AddHighscoreEntry(GameManagerScript.instance.score1, "P1");

        isPlayer1 = true;
        isPlayer2 = false;

        entryTemplate.gameObject.SetActive(false);

        //AddHighscoreEntry(2000, "BOB");

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);


        ClearHighscoreEntries();
        highscoreEntryTransforms = new List<Transform>();

        for (int i = 0; i < highscores.highscoreEntries.Count; i++)
        {
            for (int j = 0; j < highscores.highscoreEntries.Count; j++)
            {
                if (highscores.highscoreEntries[i].score > highscores.highscoreEntries[j].score)
                {
                    HighscoreEntry temporary = highscores.highscoreEntries[i];
                    highscores.highscoreEntries[i] = highscores.highscoreEntries[j];
                    highscores.highscoreEntries[j] = temporary;
                }
            }
        }

        if (highscores.highscoreEntries.Count > 10)
        {
            for (int h = highscores.highscoreEntries.Count; h > 10; h--)
            {
                highscores.highscoreEntries.RemoveAt(10);
            }
        }

        for (int i = 0; i < highscores.highscoreEntries.Count; i++)
        {
            CreateHighscoreEntryTransform(highscores.highscoreEntries[i], entryContainer, highscoreEntryTransforms);
        }
    }

    private void CreateHighscoreEntryTransform(HighscoreEntry highscoreEntry, Transform container, List<Transform> transforms)
    {
        int rank = transforms.Count + 1;
        int score = highscoreEntry.score;
        string name = highscoreEntry.name;

        Transform entryTransform = Instantiate(entryTemplate, container);
        RectTransform entryRectTransfrom = entryTransform.GetComponent<RectTransform>();
        entryRectTransfrom.anchoredPosition = new Vector2(15 * transforms.Count, -80 * transforms.Count);
        entryTransform.gameObject.SetActive(true);

        entryTransform.Find("RankText").GetComponent<Text>().text = rank.ToString();
        entryTransform.Find("ScoreText").GetComponent<Text>().text = score.ToString();
        entryTransform.Find("NameText").GetComponent<Text>().text = name;

        transforms.Add(entryTransform);
    }

    private void AddHighscoreEntry(int score, string name)
    {
        HighscoreEntry highscoreEntry = new HighscoreEntry { score = score, name = name };

        string jsonString = PlayerPrefs.GetString("highscoreTable");
        Highscores highscores = JsonUtility.FromJson<Highscores>(jsonString);

        highscores.highscoreEntries.Add(highscoreEntry);

        string json = JsonUtility.ToJson(highscores);
        PlayerPrefs.SetString("highscoreTable", json);
        PlayerPrefs.Save();
    }

    private void ClearHighscoreEntries()
    {
        foreach (Transform child in entryContainer)
        {
            if (child != entryTemplate)
            {
                Destroy(child.gameObject);
            }
        }
    }
}
