using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private List<Transform> focusTargets = new List<Transform>();

    private Vector2 min, max;
    private Vector3 minDir, maxDir;

    private void Start()
    {
        minDir = (Camera.main.ScreenToWorldPoint(new Vector3(0, 0, Camera.main.nearClipPlane)) - Camera.main.transform.position).normalized;
        maxDir = (Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, Camera.main.nearClipPlane)) - Camera.main.transform.position).normalized;
    }

    private void Update()
    {
        foreach (Transform t in focusTargets)
        {
            if (t.position.x < min.x)
                min.x = t.position.x;
            if (t.position.x > max.x)
                max.x = t.position.x;
            if (t.position.z < min.y)
                min.y = t.position.z;
            if (t.position.z > max.y)
                max.y = t.position.z;
        }

        Vector3 interSect = new Vector3();
        LineLineIntersection(out interSect, min, -minDir, max, -maxDir);
        Camera.main.transform.position = interSect;
    }

    public static bool LineLineIntersection(out Vector3 intersection, Vector3 linePoint1,
        Vector3 lineVec1, Vector3 linePoint2, Vector3 lineVec2)
    {

        Vector3 lineVec3 = linePoint2 - linePoint1;
        Vector3 crossVec1and2 = Vector3.Cross(lineVec1, lineVec2);
        Vector3 crossVec3and2 = Vector3.Cross(lineVec3, lineVec2);

        float planarFactor = Vector3.Dot(lineVec3, crossVec1and2);

        //is coplanar, and not parallel
        if (Mathf.Abs(planarFactor) < 0.0001f
                && crossVec1and2.sqrMagnitude > 0.0001f)
        {
            float s = Vector3.Dot(crossVec3and2, crossVec1and2)
                    / crossVec1and2.sqrMagnitude;
            intersection = linePoint1 + (lineVec1 * s);
            return true;
        }
        else
        {
            intersection = Vector3.zero;
            return false;
        }
    }

}
