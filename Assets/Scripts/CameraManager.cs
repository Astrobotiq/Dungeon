using System;
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


    [SerializeField] GameObject mask;

    [SerializeField] List<GameObject> lights;

    public bool canRotate = false;
    
    public bool isFirst = true;
    
    public Camera cam;
    public float transitionDuration = 5f; // Geçiş süresi

    public bool isOrthographic = false;
    
    private SkyController _skyController;

    [SerializeField] private Renderer SkyRenderer;

    private SoundManager _soundManager;

    [SerializeField] private float gameStartWalkSound = 1f;
    void Start()
    {
        cameraPivot.position = CalculatePivot(GridManager.Instance.GetCenter());

        transform.position = cameraMainMenuPos.position;

        if (SkyRenderer)
            _skyController = new SkyController(SkyRenderer);

        _soundManager = GameObject.FindWithTag("SoundManager").GetComponent<SoundManager>();
        
        _soundManager.StartMainTheme();
    }

    public void StartGame()
    {
        _soundManager.PlaySound(SoundType.GameStartWalkSound, gameStartWalkSound);

        // DOTween ile döndür
        Sequence sequence = DOTween.Sequence();
        
        // Hedef rotasyonu hesapla
        Quaternion targetRotation = GetRotation(statue.transform, cameraMainMenuPos.transform);
        
        sequence.Append(transform.DORotateQuaternion(targetRotation, 2)
            .SetEase(Ease.OutQuad));
        
        sequence.Append(transform.DOJump(cameraInGamePos.position, walkJumpPower,walkJumpNumber,walkDuration).SetEase(Ease.Linear)
            .OnUpdate((() => transform.LookAt(statue.transform)))
            .OnComplete((() =>
            {
                LightManager.Instance.GameLightShine(6.6f,2f);
            })));
        
        targetRotation = GetRotation(mask.gameObject.transform, cameraInGamePos);
        
        sequence.Append(transform.DORotateQuaternion(targetRotation, 3f)
                .OnComplete((() =>
                {
                    Timed.Run((() => LightManager.Instance.StatueLightShine(1.5f, walkDuration)),1.5f);
                })));
        
        var pos = new Vector3(mask.gameObject.transform.position.x,transform.position.y,mask.gameObject.transform.position.z);
        
        sequence.Append(mask.gameObject.transform.DOMove(pos, 3f)
            .OnUpdate((() => transform.LookAt(mask.gameObject.transform))));
        
        sequence.Append(mask.gameObject.transform.DOLocalRotate(new Vector3(0f,mask.transform.eulerAngles.y,mask.transform.eulerAngles.z),2f)
            .OnUpdate((() => transform.LookAt(mask.gameObject.transform))));
        
        sequence.Append(mask.gameObject.transform.DOLocalRotate(new Vector3(0f,80f,mask.transform.eulerAngles.z),2f)
            .OnUpdate((() => transform.LookAt(mask.gameObject.transform))));
        
        sequence.Append(mask.gameObject.transform.DOMove(new Vector3(-0.25999999f,5.88999987f,-0.449999988f), 2f)
            .OnUpdate((() => Debug.DrawLine(transform.position,mask.transform.position, Color.red)))
            .OnComplete(() => mask.gameObject.transform.parent = this.transform));
        
        sequence.Append(transform.DOLocalRotate(new Vector3(45,45,0),2)
            .OnComplete((() => StartCoroutine(TransitionToOrthographic((() => mask.gameObject.SetActive(false))))
                )));
        
        _soundManager.StopMainThemeTimed();

        
    }

    public void OnLevelCompleted()
    {
        StartCoroutine(TransitionToPerspective(ChangeCameraForLevelSelection,null));


        void ChangeCameraForLevelSelection()
        {
            Sequence sequence = DOTween.Sequence();

            sequence.Append(transform.DOLocalRotate(new Vector3(6,45,0),2));
            sequence.Append(transform.DOLocalRotate(new Vector3(6,-45,0),4).SetEase(Ease.InOutCubic).OnComplete((() => LevelManager.Instance.DestroyLevel())));
            sequence.Append(transform.DOJump(new Vector3(-5, transform.position.y, 5),  walkJumpPower, 2, 4f)
                .OnComplete((() => StartCoroutine(_skyController.ChangeSky(0.5f,1f,0.65f,1f,4f)))));
            sequence.Append(transform.DOLocalRotate(new Vector3(-19,-35,0),2).SetEase(Ease.InOutQuad));
            sequence.Append(transform.DOLocalRotate(new Vector3(-54, -35, 0), 2).SetEase(Ease.InOutQuad).OnComplete((() => InGameUITextMesh.Instance.OpenLevelSelection())));

        }
    }
    
    public void ChangeCameraForLevel()
    {
        StartCoroutine(_skyController.ChangeSky(1f,0.5f,1f,0.65f, 4f));
        transform.DOLocalRotate(new Vector3(6,-35,0),6).OnComplete((() =>
        {
            var rotation = GetRotation(statue.transform, transform);
            transform.DORotateQuaternion(rotation, 4f).OnComplete((() =>
            {
                transform.DOJump(cameraInGamePos.position, walkJumpPower, 2, 4f)
                    .OnUpdate((() => transform.LookAt(statue.transform)))
                    .OnComplete((() =>
                    {
                        transform.DOLocalRotate(new Vector3(45, 45, 0), 4).OnComplete((() => StartCoroutine(TransitionToOrthographic(null))));
                    }));
            }));
        }));
    }

    private Quaternion GetRotation(Transform target, Transform current)
    {
        // Aradaki yön vektörünü hesapla
        Vector3 direction = (target.position - current.position).normalized;

        // Eğer sadece yatay düzlemde döndürmek istiyorsan:
        

        // Hedef rotasyonu hesapla
        return Quaternion.LookRotation(direction);
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
    
    IEnumerator SmoothTransitionToOrtographic(Action action)
    {
        bool startOrtho = cam.orthographic;
        float elapsedTime = 0f;
        float startFOV = cam.fieldOfView;
        float targetFOV = startOrtho ? 5f : 70f; // Perspective FOV → 60, Ortho Size → 5
        float startOrthoSize = cam.orthographicSize;
        float targetOrthoSize = startOrtho ? 70f : 5f; // Ters geçiş için
        

        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / transitionDuration;

            if (!startOrtho)
            {
                cam.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            }
            else
            {
                cam.orthographicSize = Mathf.Lerp(startOrthoSize, targetOrthoSize, t);
            }

            yield return null;
        }

        cam.orthographic = !cam.orthographic;
        startOrtho = cam.orthographic;
        
        action.Invoke();
        
        Timed.Run(() => LevelManager.Instance.BuildLevel(),2f);
    }
    
    IEnumerator TransitionToPerspective(Action onComplete, Action onMidComplete)
    {
        float elapsedTime = 0f;
        float startOrthoSize = 0.65f;
        float targetFOV = 70f;
        float startFOV = 10;

        // İlk olarak OrthoSize küçülüyor
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = EaseInOutQuad(elapsedTime / transitionDuration);

            cam.orthographicSize = Mathf.Lerp(5, startOrthoSize, t);
            yield return null;
        }
        
        cam.orthographic = false;
        onMidComplete?.Invoke();
        
        // Ardından Perspective FOV genişliyor
        elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = EaseInOutQuad(elapsedTime / transitionDuration);

            cam.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            yield return null;
        }

        isOrthographic = false;
        
        onComplete?.Invoke();
    }

    IEnumerator TransitionToOrthographic(Action onComplete)
    {
        float elapsedTime = 0f;
        float startFOV = cam.fieldOfView;
        float targetOrthoSize = 5f;
        float startOrthoSize = 0.65f;

        // Önce Perspective FOV daralıyor
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = EaseInOutQuad(elapsedTime / transitionDuration);

            cam.fieldOfView = Mathf.Lerp(startFOV, 10f, t);
            yield return null;
        }

        cam.orthographicSize = 0.65f;
        cam.orthographic = true;
        
        onComplete?.Invoke();

        // Sonra Ortho boyutu büyüyor
        elapsedTime = 0f;
        while (elapsedTime < transitionDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = EaseInOutQuad(elapsedTime / transitionDuration);

            cam.orthographicSize = Mathf.Lerp(startOrthoSize, targetOrthoSize, t);
            yield return null;
        }

        isOrthographic = true;

        Timed.Run(() => LevelManager.Instance.StartNewLevel(), 2f);
        
        //OnLevelCompleted();
    }

    
    float EaseInOutQuad(float t)
    {
        return t < 0.5f ? 2f * t * t : -1f + (4f - 2f * t) * t;
    }

}
