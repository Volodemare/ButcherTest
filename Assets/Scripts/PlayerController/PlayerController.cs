using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float forwardSpeed = 10f;
    [SerializeField] private float lateralSpeed = 1.5f;
    [SerializeField] private float xClamp = 4f;
    [SerializeField] private float inputDeadZone = 50f;
    [SerializeField] private GameObject StarterUI;

    private CharacterController _controller;
    private Vector2 _touchStartPos;
    private float _lateralPosition;
    private bool _isDragging;
    private bool _gameStarted;

    public void StopGame()
    {
        _gameStarted = false;
    }

    private void Awake()
    {
        _controller = GetComponent<CharacterController>();
        _lateralPosition = transform.position.x;
    }

    private void Update()
    {
        HandleInput();
        MoveCharacter();
    }

    private void HandleInput()
    {
        if (!_gameStarted && IsInputStarted())
        {
            StartGame();
            return;
        }

        if (_gameStarted) 
        {
            // Постоянная проверка нового ввода
            if (IsInputStarted() && !_isDragging)
            {
                StartDragging();
            }
            ProcessDragInput();
        }
    }

    private bool IsInputStarted()
    {
        return Input.GetMouseButtonDown(0) || 
            (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began);
    }

    private void StartGame()
    {
        _gameStarted = true;
        StarterUI.GetComponent<Animator>().SetTrigger("Drop");
        transform.Find("Girl").GetComponent<Animator>().SetBool("Started", true);
        StartDragging();
    }

    private void StartDragging()
    {
        _isDragging = true;
        _touchStartPos = GetInputPosition();
    }

    private void ProcessDragInput()
    {
        if (IsInputEnded())
        {
            _isDragging = false;
            return;
        }

        if (_isDragging)
        {
            Vector2 currentPos = GetInputPosition();
            
            // Обновление стартовой позиции при новом касании
            if (Input.touchCount > 1 || 
               (Input.GetMouseButtonDown(0) && !WasPreviousTouch()))
            {
                _touchStartPos = currentPos;
            }

            float delta = (currentPos.x - _touchStartPos.x) / Screen.width;
            
            _lateralPosition += delta * lateralSpeed;
            _lateralPosition = Mathf.Clamp(_lateralPosition, -xClamp, xClamp);
            
            _touchStartPos = currentPos;
        }
    }

    private bool IsInputEnded()
    {
        return Input.GetMouseButtonUp(0) || 
            (Input.touchCount > 0 && 
             (Input.GetTouch(0).phase == TouchPhase.Ended || 
              Input.GetTouch(0).phase == TouchPhase.Canceled));
    }

    private bool WasPreviousTouch()
    {
        return Input.touchCount > 0 && 
            Input.GetTouch(0).phase != TouchPhase.Began;
    }

    private Vector2 GetInputPosition()
    {
        return Input.touchCount > 0 ? 
            Input.GetTouch(0).position : 
            (Vector2)Input.mousePosition;
    }

    private void MoveCharacter()
    {
        if (!_gameStarted)
        {
            transform.Find("Girl").GetComponent<Animator>().SetBool("Started", false);
            return;
        }

        Vector3 targetPosition = new Vector3(
            _lateralPosition,
            transform.position.y,
            transform.position.z + forwardSpeed * Time.deltaTime
        );

        _controller.Move(targetPosition  - transform.position);
    }
}