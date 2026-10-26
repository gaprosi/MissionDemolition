
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Inscribed")]
    public GameObject projectilePrefab;
    
    public GameObject heavyProjectilePrefab;
    public bool useHeavyProjectile = false;

    public float velocityMult = 10f;
    
public GameObject projLinePrefab;

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
    // select the current ammunition
    GameObject selectedPrefab = useHeavyProjectile
        ? heavyProjectilePrefab
        : projectilePrefab;

    if (selectedPrefab == null)
    {
        Debug.LogError("Projectile prefab is missing!");
        return;
    }

    aimingMode = true;

    // create selected projectile
    projectile = Instantiate(selectedPrefab);
    projectile.transform.position = launchPos;

    // hold projectile while aiming
    projectile.GetComponent<Rigidbody>().isKinematic = true;
}


    void Update()
    {

        
    // allow ammunition switching only when not aiming
    if (!aimingMode)
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            useHeavyProjectile = false;
            Debug.Log("Selected Normal Projectile");
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            useHeavyProjectile = true;
            Debug.Log("Selected Heavy Projectile");
        }
    }

        if (!aimingMode) return;

        // get mouse position in world coordinates
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        // calculate distance from launch point
        Vector3 mouseDelta = mousePos3D - launchPos;

        // limit how far the projectile can be pulled
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



FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot);

FollowCam.POI = projectile;

Instantiate<GameObject>(
    projLinePrefab,
    projectile.transform
);

projectile = null;

MissionDemolition.SHOT_FIRED();


AudioSource shotAudio = GetComponent<AudioSource>();

if (shotAudio != null)
{
    shotAudio.Play();
}


        }
    }
}
