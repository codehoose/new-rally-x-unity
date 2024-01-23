using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    private Vector3 _offset;
    private Vector3 _initialPosition;

    [SerializeField]
    private Transform _followTarget;
    
    void Start()
    {
        _initialPosition = transform.position;
        _offset = new Vector3(24, 0, -10);
    }

    public void Restart()
    {
        transform.position = _initialPosition;
    }

    void LateUpdate()
    {
        transform.position = _followTarget.position + _offset;
    }
}
