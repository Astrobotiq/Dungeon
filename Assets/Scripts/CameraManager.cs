using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using Sequence = DG.Tweening.Sequence;

public class CameraManager : Singleton<CameraManager>
{
    [SerializeField]
    private Transform cameraPivot;// this is pivot point for camera rotation
    
    [SerializeField]
    private Transform cameraMainMenuPos;
    
    [SerializeField]
    private Transform cameraInGamePos;

    [SerializeField] 
    private Button changeCameraPos;
    
    [SerializeField] private float MousePositionX;
    
    [SerializeField, Range(1,10)] float RotationSpeed = 5f;

    [SerializeField] float walkDuration = 5;
    
    [SerializeField] float walkJumpPower = 0.8f;
    
    [SerializeField] int walkJumpNumber = 8;

    [SerializeField] GameObject statue;

    public bool canRotate = false;
    
    public bool isFirst = true;
    
    public Camera cam;
    public float transitionDuration = 1.5f; // Geçiş süresi

    public bool isOrthographic = false;
    void Start()
    {
        cameraPivot.position = CalculatePivot(GridManager.Instance.GetCenter());

        transform.position = cameraMainMenuPos.position;
    }

    public void StartGame()
    {
        
        
        // Aradaki yön vektörünü hesapla
        Vector3 direction = (cameraInGamePos.position - cameraMainMenuPos.position).normalized;

        // Eğer sadece yatay düzlemde döndürmek istiyorsan:
        direction.y = 0;

        // Hedef rotasyonu hesapla
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // DOTween ile döndür
        Sequence sequence = DOTween.Sequence();
        sequence.Append(transform.DORotateQuaternion(targetRotation, 2)
            .SetEase(Ease.OutQuad).OnComplete((() => FeelManager.Instance.CameraLookAt(transform,statue.transform,walkDuration))));
        sequence.Append(transform.DOJump(cameraInGamePos.position, walkJumpPower,walkJumpNumber,walkDuration).SetEase(Ease.Linear));
        sequence.Append(transform.DOLocalRotate(new Vector3(45,45,0),2).OnComplete((() => StartCoroutine(SmoothTransition()))));
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
    
    IEnumerator SmoothTransition()
    {
        float elapsedTime = 0f;
        float startFOV = cam.fieldOfView;
        float targetFOV = isOrthographic ? 70f : 5f; // Perspective FOV → 60, Ortho Size → 5
        float startOrthoSize = cam.orthographicSize;
        float targetOrthoSize = isOrthographic ? 5f : 70f; // Ters geçiş için
        bool startOrtho = cam.orthographic;

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            if (!isOrthographic)
            {
                cam.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            }
            else
            {
                cam.orthographicSize = Mathf.Lerp(startOrthoSize, targetOrthoSize, t);
            }

            yield return null;
        }

        cam.orthographic = !isOrthographic;
        isOrthographic = !isOrthographic;
        
        Timed.Run(() => LevelManager.Instance.BuildLevel(),2f);
    }
}
