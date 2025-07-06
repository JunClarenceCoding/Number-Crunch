using UnityEngine;

public class WayPointController : MonoBehaviour
{
    public WaypointBaseController waypointBaseController;
    private void Awake()
    {
        waypointBaseController.SetTarget(GameObject.FindWithTag("Player"));
        waypointBaseController.SetTransform(transform);
        waypointBaseController.EffectExsist(false);
        if(transform.childCount > 0)
        {
            waypointBaseController.EffectExsist(true);
            waypointBaseController.SetWayPointEffect(transform.GetChild(0).gameObject);
        }
    }
    private void FixedUpdate()
    {
        if (waypointBaseController != null && waypointBaseController.data.item.target != null)
        {
            if (waypointBaseController.GetDistance(transform.position, waypointBaseController.data.item.target.transform.position) < waypointBaseController.data.interactDistance)
            {
                waypointBaseController.EnableWaypoint(false);
                waypointBaseController.EnableEffect(true);
            }
            else
            {
                waypointBaseController.EnableWaypoint(true);
                waypointBaseController.EnableEffect(false);
            }
        }
    }   
}