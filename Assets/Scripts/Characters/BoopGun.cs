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
        Vector2 direction = fireStartPoint.transform.position - fireTowards.transform.position;
        GameObject spawned = Instantiate(bullet, fireStartPoint.position, Quaternion.identity, PrefabManager.Instance.projectileHolder);
        spawned.transform.localScale = new Vector3(spawned.transform.localScale.x * ((direction.x > 0.0f) ? -1.0f : 1.0f), spawned.transform.localScale.y, 1.0f);
        Projectile tempProj = spawned.GetComponent<Projectile>();
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
