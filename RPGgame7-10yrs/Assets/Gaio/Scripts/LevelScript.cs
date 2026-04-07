using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelScript : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            GameManagerScript.instance.score1 = GameObject.Find("Player Spawnpoint").GetComponentInChildren<PlayerStatsScript>().score;

            if (GameManagerScript.instance.playerCount == 2)
            {
                GameManagerScript.instance.score2 = GameObject.Find("Player2 Spawnpoint").GetComponentInChildren<PlayerStatsScript>().score;
            }
            SceneManager.LoadScene(8);
        }
    }
}
