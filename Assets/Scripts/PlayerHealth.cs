using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] int maxHealth = 3;
    [SerializeField] float invulnerability = 1f;
    [SerializeField] Image[] hearts;
    [SerializeField] LevelResult result;

    int health;
    float hurtTimer;
    SpriteRenderer[] sprites;
    Color[] colors;

    public bool IsAlive => health > 0;

    void Awake()
    {
        health = maxHealth;
        sprites = GetComponentsInChildren<SpriteRenderer>();
        colors = new Color[sprites.Length];
        for (int i = 0; i < sprites.Length; i++) colors[i] = sprites[i].color;
        RefreshHearts();
    }

    void Update()
    {
        hurtTimer = Mathf.Max(0f, hurtTimer - Time.deltaTime);
        for (int i = 0; i < sprites.Length; i++)
            sprites[i].color = hurtTimer > 0f ? colors[i] * new Color(1f, .55f, .55f) : colors[i];
    }

    public void TakeDamage(int damage)
    {
        if (!IsAlive || hurtTimer > 0f || result.IsFinished) return;
        health = Mathf.Max(0, health - damage);
        hurtTimer = invulnerability;
        RefreshHearts();
        if (!IsAlive) result.Finish(false);
    }

    void RefreshHearts()
    {
        for (int i = 0; i < hearts.Length; i++)
            hearts[i].color = i < health ? Color.white : new Color(1f, 1f, 1f, .2f);
    }
}
