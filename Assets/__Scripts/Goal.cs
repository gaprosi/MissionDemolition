
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Goal : MonoBehaviour
{
    static public bool goalMet = false;

    [Header("Visual Effects")]
    public GameObject explosionPrefab;

    void OnTriggerEnter(Collider other)
    {
        Projectile proj = other.GetComponent<Projectile>();

        if (proj == null || goalMet)
        {
            return;
        }

        // record that the player hit the Goal
        goalMet = true;

        // Create the green explosion
        if (explosionPrefab != null)
        {
            GameObject explosion = Instantiate(
                explosionPrefab,
                transform.position,
                Quaternion.identity
            );

            // clean up after the particles disappear
            Destroy(explosion, 3f);
        }

        // make the Goal more opaque
        Material mat = GetComponent<Renderer>().material;

        Color c = mat.color;
        c.a = 0.75f;
        mat.color = c;
    }
}
