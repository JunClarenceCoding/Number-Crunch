using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WayPointManager : MonoBehaviour
{
    [System.Serializable]
    struct Data
    {
        public int totalWayPoints;
        public GameObject waypointCanvas;
        public GameObject worldWaypoints;
        public GameObject waypointUIPrefab;
        public List<GameObject> waypointPrefab;
    }
    [SerializeField] Data data;
    private void Awake()
    {
        for (int i = 0; i < data.totalWayPoints; i++)
        {
            if(data.waypointPrefab.Count > 0)
            {
                for(int j = 0; j < data.waypointPrefab.Count; j++)
                {
                    GameObject tmpWaypoint = Instantiate(data.waypointPrefab[j]);
                    WayPointController tmpWayPointController = tmpWaypoint.GetComponent<WayPointController>();
                    GameObject tmpWaypointUI = Instantiate(data.waypointUIPrefab);
                    tmpWaypointUI.GetComponent<Image>().sprite = tmpWayPointController.waypointBaseController.data.item.icone;
                    tmpWaypointUI.transform.SetParent(data.waypointCanvas.transform);
                    tmpWayPointController.waypointBaseController.data.item.image = tmpWaypointUI.GetComponent<Image>();
                    tmpWayPointController.waypointBaseController.data.item.message = tmpWaypointUI.transform.GetChild(0).GetComponent<TMP_Text>();
                    tmpWayPointController.waypointBaseController.data.item.waypointUI = tmpWaypointUI;
                    tmpWayPointController.transform.SetParent(data.worldWaypoints.transform);
                }
            }
        }
    }
}