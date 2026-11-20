using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using DG.Tweening;

public class Health : MonoBehaviour
{
    public int health = 1;
    int maxHealth;
    public float timer = 0f;
    public bool isPlayer = false;
    public Image healthBar;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        maxHealth = health;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(timer >= 0)
        {
            timer -= Time.fixedDeltaTime;
        }
    }

    public void Damage(int damage, float cooldown)
    {
        if (timer <= 0)
        {
            health -= damage;
            if (healthBar) healthBar.DOFillAmount((float)health / maxHealth, 0.5f);
            Debug.Log("Damage hit " + gameObject.name + " for " + damage + " damage. Remaining HP: " + health);

            timer = cooldown;
        }

        if (health <= 0)
        {
            if (isPlayer) 
            {
                //GameState.Instance.GameLose();
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.TryGetComponent<Bullet>(out Bullet bullet)) //Kalau ada component Bullet(Script), ciptakan variabel "Bullet bullet"
        {
            if (!isPlayer)
            {
                Damage(bullet.damage, 0);
                Destroy(bullet.gameObject);
            }
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (isPlayer && hit.gameObject.CompareTag("Enemy"))
        {
            Damage(1, 2f);
        }
    }
}
