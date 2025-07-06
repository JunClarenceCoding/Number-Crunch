using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Waypoint Item Data is the data that is added on the waypoint target game object.
//The Target gameobject is the gameobject that needs a waypoint, but also need data to use it.

[System.Serializable]

public class WayPointItem
{
    public Sprite icone;
    public float heightOffset;
    [HideInInspector] public Image image;
    [HideInInspector] public TMP_Text message;
    [HideInInspector] public GameObject effect;
    [HideInInspector] public GameObject waypointUI;
    [HideInInspector] public GameObject target;
    [HideInInspector] public Transform transform;
}