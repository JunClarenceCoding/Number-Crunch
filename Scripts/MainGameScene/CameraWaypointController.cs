//Put This as component on your main Camera gameobject,
//With this we display the waypoints images and message,
// this happens live when you walk around.

public class CameraWaypointController : CameraWaypointBaseController
{
    private void FixedUpdate() {
        if(data.waypoints != null && data.waypoints.Count > 0)
        {
            UpdateUI();
        }
    }

    private void UpdateUI()
    {
        foreach(WayPointController waypoint in data.waypoints)
        {
            waypoint.waypointBaseController.data.item.image.transform.position = UIImagePosition(waypoint.waypointBaseController.data.item);
            waypoint.waypointBaseController.data.item.message.text = WaypointDistance(waypoint.waypointBaseController.data.item) + "M";
        }
    }
}
