using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Events
    // Events
    public delegate void HealthChange(float healthPercent);
    public delegate void Spawn();
    public delegate void Death();

    public event HealthChange OnHealthChange;
    public event Spawn OnSpawn;
    public event Death OnDeath;
    #endregion

    // Health info
    [SerializeField, Min(1)]
    protected int maxHealth;
    protected int curHealth;
    public int Health { get { return curHealth; } }
    public float HealthPercent { get { return curHealth / (float)maxHealth; } }

    // Respawn info
    [SerializeField]
    protected float respawnTime = 5.0f;
    public bool DoesRespawn { get { return respawnTime > 0.0f; } }
    protected bool _isDead = false;
    public bool IsDead { get { return _isDead;} }
    protected Vector2 spawnPos;
    protected Coroutine respawnCoroutine;

    // Components
    protected SpriteRenderer spriteRend;
    protected Rigidbody2D rb;

    private void Start()
    {
        spriteRend = this.GetComponent<SpriteRenderer>();
        rb = this.GetComponent<Rigidbody2D>();
        spawnPos = rb.position;
        HandleSpawn();
    }

    #region Spawn/Death
    // Handles the character's spawning
    public virtual void HandleSpawn()
    {
        _isDead = false;
        rb.position = spawnPos;
        rb.velocity = Vector2.zero;
        EnableCharacter();
        ResetHealthToMax();
        OnSpawn?.Invoke();
    }

    // Handles the character's death
    protected virtual void HandleDeath()
    {
        _isDead = true;
        OnDeath?.Invoke();
        if(!DoesRespawn)
        {
            DestroySelf();
        }
        else
            respawnCoroutine = StartCoroutine(HandleRespawnTimer());
    }

    // Destroys this gameobject
    protected virtual void DestroySelf()
    {
        Destroy(this.gameObject);
    }

    // Enables the character and its components
    protected virtual void EnableCharacter()
    {
        if(spriteRend != null)
            spriteRend.enabled = true;
        if(rb != null)
            rb.isKinematic = false;
    }

    // Disables the character and its components
    protected virtual void DisableCharacter()
    {
        if(spriteRend != null)
            spriteRend.enabled = false;
        if (rb != null)
            rb.isKinematic = true;
    }

    // Respawns the character
    protected IEnumerator HandleRespawnTimer()
    {
        DisableCharacter();
        float timeRemaining = respawnTime;
        while(timeRemaining > 0.0f)
        {
            timeRemaining -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        HandleSpawn();
    }

    // Sets the spawn position of the character
    public void SetSpawnPos(Vector2 _pos) { spawnPos = _pos; }
    #endregion

    #region Health
    // Takes damage for the character
    public virtual void TakeDamage(DamageInfo info)
    {
        curHealth = Mathf.Clamp(curHealth - info.damage, 0, maxHealth);
        OnHealthChange?.Invoke(HealthPercent);
        if (curHealth <= 0 && !IsDead)
            HandleDeath();
    }

    // Heals the character
    public virtual void Heal(int amount)
    {
        curHealth = Mathf.Clamp(curHealth + amount, 0, maxHealth);
        OnHealthChange?.Invoke(HealthPercent);
    }

    // Resets the characters health to max
    public virtual void ResetHealthToMax()
    {
        curHealth = maxHealth;
        OnHealthChange?.Invoke(HealthPercent);
    }
    #endregion
}
