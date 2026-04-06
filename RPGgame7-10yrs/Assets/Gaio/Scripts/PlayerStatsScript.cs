using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsScript : MonoBehaviour
{
    //PlayerStats
    public CharacterStats stats;

    public int maxHP;
    public int currentHP;

    public int score;

    //PlayerUI
    public Image hpBar;
    public Image easeHpBar;
    public float easeSpeed;
    public TextMeshProUGUI scoreText;
    public Image playerIcon;
    public Image itemIcon;

    private float flashTimer;

    void Start()
    {
        if (gameObject.GetComponent<PlayerInput>().PlayerNumber == 1)
        {
            hpBar = GameObject.Find("HealthBar").GetComponent<Image>();
            scoreText = GameObject.Find("ScoreText").GetComponent<TextMeshProUGUI>();
            playerIcon = GameObject.Find("PlayerIcon").GetComponent<Image>();
            itemIcon = GameObject.Find("ItemIcon").GetComponent<Image>();
            easeHpBar = GameObject.Find("EaseHealthBar").GetComponent<Image>();
        }
        else if (gameObject.GetComponent<PlayerInput>().PlayerNumber == 2)
        {
            hpBar = GameObject.Find("HealthBar2").GetComponent<Image>();
            scoreText = GameObject.Find("ScoreText2").GetComponent<TextMeshProUGUI>();
            playerIcon = GameObject.Find("PlayerIcon2").GetComponent<Image>();
            itemIcon = GameObject.Find("ItemIcon2").GetComponent<Image>();
            easeHpBar = GameObject.Find("EaseHealthBar2").GetComponent<Image>();
        }

        playerIcon.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
        itemIcon.sprite = gameObject.GetComponent<ItemStatsScript>().Item.Image;


        if (stats.Health == CharacterStats.HealthEnum.Tank)
        {
            maxHP = 10000;
        }
        else if (stats.Health == CharacterStats.HealthEnum.Average)
        {
            maxHP = 7500;
        }
        else if (stats.Health == CharacterStats.HealthEnum.Weak)
        {
            maxHP = 5000;
        }

        easeSpeed = 0.05f;
        currentHP = maxHP;
    }

    public void TakeDamage(int amount)
    {
        currentHP -= amount;

        if (currentHP <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Debug.Log(gameObject.name + "died");
    }

    public void Heal(int amount)
    {
        currentHP += amount;
        if (currentHP >= maxHP)
        {
            currentHP = maxHP;
        }
    }

    public void AddScore(int amount)
    {
        score += amount;
    }

    void UpdateHealth()
    {
        float percent = (float)currentHP / maxHP;
        hpBar.fillAmount = percent;

        if (easeHpBar.fillAmount != hpBar.fillAmount)
        {
            easeHpBar.fillAmount = Mathf.Lerp(easeHpBar.fillAmount, hpBar.fillAmount, easeSpeed);
        }
    }

    void UpdateScore()
    {
        scoreText.text = $"{score}";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            TakeDamage(500);
        }

        UpdateHealth();
        UpdateScore();

        flashTimer -= Time.deltaTime;
        if (flashTimer < 0)
        {
            flashTimer = 1;
        }

        float percent = (float)currentHP / maxHP;
        if (flashTimer < 0.5 && percent < 0.4)
        {
            hpBar.color = Color.white;
        }
        else
        {
            hpBar.color = new Color32(80, 0, 0, 255);
        }
    }
}
