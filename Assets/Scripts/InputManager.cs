using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class InputManager : MonoBehaviour
{
    public Camera Camera;
    //Todo bütün bu managerler için bir tane Singleton parent class yazılacak yazılacak
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }
    

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnLeftButton(InputValue value)
    {
        if (value.isPressed)
        {
            var mousePosition = Mouse.current.position.ReadValue();
            RaycastHit hit = getRaycastHit(mousePosition);

            IClickable clickable = hit.collider.gameObject.GetComponent<IClickable>();

            if (clickable != null)
            {
                clickable.onLeftClick();
            }
        }
    }
    
    public void OnRightButton(InputValue value)
    {
        if (value.isPressed)
        {
            //Maybe this function can use to deavtivate current action
            
            var mousePosition = Mouse.current.position.ReadValue();
            RaycastHit hit = getRaycastHit(mousePosition);

            IClickable clickable = hit.collider.gameObject.GetComponent<IClickable>();

            if (clickable != null)
            {
                clickable.onRightClick();
            }

        }
    }
    
    public void OnMiddleClick(InputValue value)
    {
        if (value.isPressed)
        {
            //In this area we will rotate our scene 
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