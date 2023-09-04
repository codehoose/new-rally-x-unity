using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 _offset;

    [SerializeField]
    private Transform _followTarget;
    
    void Start()
    {
        _offset = new Vector3(24, 0, -10);
    }

    void LateUpdate()
    {
        transform.position = _followTarget.position + _offset;
    }
}
