using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLogic : MonoBehaviour
{
    [Header("Armory")]
    [SerializeField]
    private TurretWeapons gun;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform fireStartPoint;
    [SerializeField]
    private Transform fireTowards;

    [Header("Stats")]
    [SerializeField]
    private float bulletSpeed;
    [SerializeField]
    private float maxWarmupTime = 0.3f;
    private float curWarmupTime = 0.0f;
    [SerializeField]
    private float maxFireCooldown = 0.2f;
    private float curFireCooldown = 0.0f;
    private bool isPlayerInRange;

    [SerializeField]
    private Sprite[] pics;

    private SpriteRenderer spriteRenderer;
    private Character character;

    private void OnEnable()
    {
        gun.StartFire += Wake;
        gun.StopFire += Sleep;
    }

    private void OnDisable()
    {
        gun.StartFire -= Wake;
        gun.StopFire -= Sleep;
    }

    private void Start()
    {
        spriteRenderer = this.GetComponent<SpriteRenderer>();
        character = this.GetComponent<Character>();
    }

    private void Update()
    {
        if (!isPlayerInRange || !GameManager.Instance.IsGameActive)
            return;

        // Warmup the gun before firing
        if (curWarmupTime > 0.0f)
        {
            curWarmupTime -= Time.deltaTime;
            return;
        }

        // Counts down fire cooldown
        if(curFireCooldown > 0.0f)
        {
            curFireCooldown -= Time.deltaTime;
        }
        // Fires a projectile when cooldown is over, resetting cooldown
        else
        {
            spriteRenderer.sprite = pics[1];
            FireProjectile();
            curFireCooldown = maxFireCooldown;
        }
    }

    // Wakes up the turret when the player is in range
    private void Wake()
    {
        isPlayerInRange = true;
        curWarmupTime = maxWarmupTime;
        curFireCooldown = maxFireCooldown;
    }

    // Puts the turret to sleep when player is out of range
    private void Sleep()
    {
        spriteRenderer.sprite = pics[0];
        isPlayerInRange = false;
    }

    // Fires the projectile
    private void FireProjectile()
    {
        Vector2 direction = (fireTowards.transform.position - fireStartPoint.transform.position);
        GameObject spawned = Instantiate(bullet, fireStartPoint.position, Quaternion.identity, PrefabManager.Instance.projectileHolder);
        spawned.transform.localScale = new Vector3(spawned.transform.localScale.x * ((direction.x > 0.0f) ? -1.0f : 1.0f), spawned.transform.localScale.y, 1.0f);
        Projectile tempProj = spawned.GetComponent<Projectile>();
        if (tempProj != null)
            tempProj.InitProjectile(bulletSpeed * direction, character, true);
    }
}
