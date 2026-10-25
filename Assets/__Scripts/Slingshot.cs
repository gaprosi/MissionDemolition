
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject projectilePrefab;
    public float velocityMult = 10f;

    [Header("Dynamic")]
    public GameObject launchPoint;
    public Vector3 launchPos;
    public GameObject projectile;
    public bool aimingMode;

    void Awake()
    {
        Transform launchPointTrans = transform.Find("LaunchPoint");
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false);
        launchPos = launchPointTrans.position;
    }

    void OnMouseEnter()
    {
        launchPoint.SetActive(true);
    }

    void OnMouseExit()
    {
        launchPoint.SetActive(false);
    }

    void OnMouseDown()
    {
        aimingMode = true;

        projectile = Instantiate(projectilePrefab) as GameObject;
        projectile.transform.position = launchPos;

        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }

    void Update()
    {
        if (!aimingMode) return;

        // Get mouse position in world coordinates
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        // Calculate distance from launch point
        Vector3 mouseDelta = mousePos3D - launchPos;

        // Limit how far the projectile can be pulled
        float maxMagnitude = GetComponent<SphereCollider>().radius;

        if (mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;
        }

        // Move projectile with the mouse
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;

        // Release projectile
        if (Input.GetMouseButtonUp(0))
        {
            aimingMode = false;

            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;
            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;

#if UNITY_6000_0_OR_NEWER
            projRB.linearVelocity = -mouseDelta * velocityMult;
#else
            projRB.velocity = -mouseDelta * velocityMult;
#endif

FollowCam.POI = projectile;

            projectile = null;
        }
    }
}
