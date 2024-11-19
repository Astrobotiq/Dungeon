using System;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : Singleton<InputManager>
{
    //event manager yazmadığım için şimdilik böyle koyucam
    public event Action onRightClicked;
    public bool canTakeInput = true;
    //Todo bütün bu managerler için bir tane Singleton parent class yazılacak yazılacak

    public void OnLeftButton(InputAction.CallbackContext value)
    {
        if (value.performed && canTakeInput)
        {
            var mousePosition = Mouse.current.position.ReadValue();
            RaycastHit hit = getRaycastHit(mousePosition);

            if (hit.collider == null)
            {
                return;
            }
            
            IClickable clickable = hit.collider.gameObject.GetComponent<IClickable>();

            if (clickable != null)
            {
                clickable.onLeftClick();
            }
        }
    }
    
    public void OnRightButton(InputValue value)
    {
        if (value.isPressed && canTakeInput)
        {
            onRightClicked?.Invoke();
        }
    }
    
    public void OnMiddleClick(InputAction.CallbackContext value)
    {
        if (value.performed)
        {
            CameraManager.Instance.setRotation(true);
        }
        else if (value.canceled)
        {
            CameraManager.Instance.setRotation(false);

        }
        
    }
    
    public void OnScrollWheel(InputValue value)
    {
        if (value.isPressed)
        {
            

        }
    }

    private RaycastHit getRaycastHit(Vector2 position)
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(position.x, position.y, Camera.main.nearClipPlane));

        Physics.Raycast(ray, out RaycastHit hit);

        return hit;
    }
}