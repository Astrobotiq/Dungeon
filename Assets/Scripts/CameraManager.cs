using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : Singleton<CameraManager>
{
    public Transform cameraPivot;// this is pivot point for camera rotation
    
    [SerializeField] private float MousePositionX;
    
    [SerializeField, Range(1,10)] float RotationSpeed = 5f;

    public bool canRotate = false;
    
    public bool isFirst = true;
    void Start()
    {
        cameraPivot.position = CalculatePivot(GridManager.Instance.GetCenter());
    }

    void Update()
    {
        if (canRotate)
        {
            if (isFirst)
            {
                MousePositionX = Mouse.current.position.ReadValue().x;
                isFirst = false;
                return;
            }
            var currentPos = Mouse.current.position.ReadValue().x;
            var diff = currentPos - MousePositionX;
            cameraPivot.transform.Rotate(Vector3.up, diff * RotationSpeed * Time.deltaTime);
            MousePositionX = currentPos;
        }
    }

    public void setRotation(bool _canRotate)
    {
        canRotate = _canRotate;
        
        if (!_canRotate)
        {
            isFirst = true;
        }
    }

    Vector3 CalculatePivot(float pos)
    {
        return new Vector3(pos,cameraPivot.position.y,pos);
    }
}
