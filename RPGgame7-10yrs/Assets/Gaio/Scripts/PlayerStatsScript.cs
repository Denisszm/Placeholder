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
    public TextMeshProUGUI scoreText;
    public Image playerIcon;
    public Image itemSprite;
    public Image itemIcon;


    void Start()
    {
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

        currentHP = maxHP;

        playerIcon.sprite = gameObject.GetComponent<SpriteRenderer>().sprite;
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
    }

    void UpdateScore()
    {
        scoreText.text = $"{score}";
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        TakeDamage(1);
        AddScore(1);

        UpdateHealth();
        UpdateScore();
    }
}
