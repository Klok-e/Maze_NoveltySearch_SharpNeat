using Scripts.ScriptableObjects.Containers;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private FloatContainer _move_speed;
    [SerializeField] private FloatContainer _zoom_speed;

    private void LateUpdate()
    {
        float move_x = Input.GetAxis("Horizontal");
        float move_y = Input.GetAxis("Vertical");
        float cameraZoom = Input.GetAxis("Mouse ScrollWheel");

        var pos = transform.position;

        var movePlane = new Vector3(move_x, move_y, 0) * _move_speed.Value * pos.y;
        var moveDepth = new Vector3(0, 0, cameraZoom) * _zoom_speed.Value * pos.y;

        transform.Translate(movePlane + moveDepth);
    }
}
