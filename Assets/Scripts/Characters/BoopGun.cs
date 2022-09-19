using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoopGun : MonoBehaviour
{
    [Header("Armory")]
    [SerializeField]
    private GameObject bullet;
    [SerializeField]
    private Transform fireStartPoint;
    [SerializeField]
    private Transform fireTowards;

    private Character character;
    private SpriteRenderer spriteRenderer;
    [SerializeField]
    private Sprite[] pics;

    [Header("Stats")]
    [SerializeField]
    private float bulletSpeed;

    private void Start()
    {
        character = this.GetComponent<Character>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    // Fires the projectile
    public Vector2 FireProjectile()
    {
        spriteRenderer.sprite = pics[1];
        Vector2 direction = (fireTowards.transform.position - fireStartPoint.transform.position) * -1.0f;
        Projectile tempProj = Instantiate(bullet, fireStartPoint.position, Quaternion.identity, PrefabManager.Instance.projectileHolder).GetComponent<Projectile>();
        if (tempProj != null)
            tempProj.InitProjectile(bulletSpeed * direction, character, true);
        else
            return Vector2.zero;
        return direction;
    }

    public void Reload()
        {
        spriteRenderer.sprite = pics[0];
        }
}
