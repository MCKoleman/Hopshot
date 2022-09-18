using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretLogic : MonoBehaviour
{
    [Header("Armory")]
    [SerializeField]
    private TurretWeapons gun;
    [SerializeField]
    private TargetLight led;
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform fireStartPoint;

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
            FireProjectile();
            curFireCooldown = maxFireCooldown;
        }
    }

    // Wakes up the turret when the player is in range
    private void Wake()
    {
        led.SetColor(Color.red);
        isPlayerInRange = true;
        curWarmupTime = maxWarmupTime;
        curFireCooldown = maxFireCooldown;
    }

    // Puts the turret to sleep when player is out of range
    private void Sleep()
    {
        led.SetColor(Color.green);
        isPlayerInRange = false;
    }

    // Fires the projectile
    private void FireProjectile()
    {
        Vector2 direction = (fireStartPoint.transform.position *Vector2.right);
        Projectile tempProj = Instantiate(bullet, fireStartPoint.position, Quaternion.identity, PrefabManager.Instance.projectileHolder).GetComponent<Projectile>();
        if (tempProj != null)
            tempProj.InitProjectile(bulletSpeed * direction, character, true);
    }
}
