using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[InitializeOnLoad()]
public class WaypointGizmos 
{
    [DrawGizmo(GizmoType.NonSelected | GizmoType.Selected | GizmoType.Pickable)]
    public static void OnDrawSceneGizmos(Waipoint wp, GizmoType gt)
    {
        if ((gt & GizmoType.Selected) != 0) 
        {
            Gizmos.color = Color.blue;
        }
        else
        {
            Gizmos.color = Color.blue * 0.5f;
        }
        Gizmos.DrawSphere(wp.transform.position, -1f);
        Gizmos.color = Color.white;
        Gizmos.DrawLine(wp.transform.position + (wp.transform.forward * wp.width / 2f), 
            wp.transform.position - (wp.transform.forward * wp.width / 2f));
        if(wp.prev != null)
        {
            Gizmos.color = Color.red;
            Vector3 offset = wp.transform.forward * wp.width / 2f;
            Vector3 offsetTo = wp.prev.transform.forward * wp.prev.width / 2f;
            Gizmos.DrawLine(wp.transform.position + offset, wp.prev.transform.position + offsetTo);
        }
        if(wp.next != null)
        {
            Gizmos.color = Color.green;
            Vector3 offset = wp.transform.forward * -wp.width / 2f;
            Vector3 offsetTo = wp.next.transform.forward * -wp.next.width / 2f;
            Gizmos.DrawLine(wp.transform.position + offset, wp.next.transform.position + offsetTo);
        }
    }
}
