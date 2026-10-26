
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Renderer))]
public class Goal : MonoBehaviour
{
    // A static field accessible by code anywhere
    static public bool goalMet = false;

    void OnTriggerEnter(Collider other)
    {
        // Check whether the object is a Projectile
        Projectile proj = other.GetComponent<Projectile>();

        if (proj != null)
        {
            // Mark the goal as achieved
            Goal.goalMet = true;

            // Make the goal more opaque
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 0.75f;
            mat.color = c;
        }
    }
}
