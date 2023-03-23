using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class WaypointManager : EditorWindow
{
    [MenuItem("Waypoint/Waypoints Editor Tools")]
    public static void ShowWindow()
    {
        GetWindow<WaypointManager>("Waypoints Editor Tools");
    }
    [SerializeField] private Transform waypointsParent;
    private void OnGUI()
    {
        SerializedObject obj = new SerializedObject(this);
        EditorGUILayout.PropertyField(obj.FindProperty("waypointsParent"));
        if(waypointsParent == null)
        {
            EditorGUILayout.HelpBox("Assign waypoints parent", MessageType.Warning);
        }
        else
        {
            EditorGUILayout.BeginVertical("Box");
            CreateButtons();
            EditorGUILayout.EndVertical();
        }
        obj.ApplyModifiedProperties();
    }

    private void CreateButtons()
    {
        if(GUILayout.Button("Create Waypoint"))
        {
            CreateWaypoint();
        }
    }

    private void CreateWaypoint()
    {
        GameObject wpObj = new GameObject("Waypoint" + waypointsParent.childCount, typeof(Waipoint));
        wpObj.transform.SetParent(waypointsParent, false);

        Waipoint wp = wpObj.GetComponent<Waipoint>();
        if(waypointsParent.childCount > 1)
        {
            wp.prev = waypointsParent.GetChild(waypointsParent.childCount - 2).GetComponent<Waipoint>();
            wp.prev.next = wp;
            wp.transform.position = wp.prev.transform.position;
            wp.transform.forward = wp.prev.transform.forward;
        }
        Selection.activeGameObject = wp.gameObject;
    }
}
