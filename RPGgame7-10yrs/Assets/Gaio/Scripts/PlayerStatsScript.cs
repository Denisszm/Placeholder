using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStatsScript : MonoBehaviour
{
    //PlayerStats
    public int maxHP;
    public int currentHP;

    public int score;

    //PlayerUI
    public Image hpBar;
    public TextMeshPro scoreText;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
        float percent = currentHP / maxHP;
        hpBar.fillAmount = percent;
    }

    void UpdateScore()
    {
        scoreText.text = $"{score}";
    }

    // Update is called once per frame
    void Update()
    {
        TakeDamage(1);
        AddScore(1);

        UpdateHealth();
        UpdateScore();
    }
}
