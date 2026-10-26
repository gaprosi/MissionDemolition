
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class SlingshotBand : MonoBehaviour
{
    [Header("Slingshot References")]
    public Transform leftTip;
    public Transform rightTip;
    public Transform launchPoint;
    public Slingshot slingshot;

    private LineRenderer band;
    private Vector3[] points = new Vector3[3];

    void Awake()
    {
        band = GetComponent<LineRenderer>();

        band.positionCount = 3;
        band.useWorldSpace = true;
        band.loop = false;

        band.widthMultiplier = 0.08f;
        band.widthCurve = AnimationCurve.Linear(0, 1, 1, 1);
    }

    void LateUpdate()
    {
        if (leftTip == null || rightTip == null ||
            launchPoint == null || slingshot == null)
        {
            return;
        }

        // Normal resting position
        Vector3 middle = launchPoint.position;

        // Stretch toward projectile while aiming
        if (slingshot.aimingMode &&
            slingshot.projectile != null)
        {
            middle = slingshot.projectile.transform.position;
        }

        // Left arm -> Projectile -> Right arm
        points[0] = leftTip.position;
        points[1] = middle;
        points[2] = rightTip.position;

        band.SetPositions(points);
    }
}
