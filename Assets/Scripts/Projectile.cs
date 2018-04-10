using UnityEngine;

public class Projectile : MonoBehaviour {

    private void OnCollisionEnter2D(Collision2D collision2d)
    {
        Destroy(gameObject);
    }
}