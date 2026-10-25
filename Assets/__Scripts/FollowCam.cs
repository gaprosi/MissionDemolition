
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;

    [Header("Inscribed")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Dynamic")]
    public float camZ;

    void Awake()
    {
        camZ = transform.position.z;
    }

    void FixedUpdate()
    {
        // Default camera target is the origin
        Vector3 destination = Vector3.zero;

        if (POI != null)
        {
            // Check if projectile has stopped moving
            Rigidbody poiRigid = POI.GetComponent<Rigidbody>();

            if ((poiRigid != null) && poiRigid.IsSleeping())
            {
                POI = null;
            }
        }

        // Follow the projectile if it is still active
        if (POI != null)
        {
            destination = POI.transform.position;
        }

        // Prevent camera from moving too far left or down
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        // Smooth camera movement
        destination = Vector3.Lerp(
            transform.position,
            destination,
            easing
        );

        // Maintain camera depth
        destination.z = camZ;

        // Move camera
        transform.position = destination;

        // Adjust camera size to keep ground visible
        Camera.main.orthographicSize = destination.y + 10;
    }
}
