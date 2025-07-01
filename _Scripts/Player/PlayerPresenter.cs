using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerPresenter : MonoBehaviour
{
    private Rigidbody _rb;
    private PlayerModel _model;
    private PlayerView _view;
    private PlayerInput _pInput;


    [SerializeField]
    private GameObject _mesh;
    

    public Action<bool> OnPlayerMoving { get; set; }
    public Action<int> OnCoinsCollected { get; set; }
    public Action<int> OnLifeChanged { get; set; }

    public Action OnGameOver;

    void Awake()
    {
        _rb = GetComponent<Rigidbody>();
        _model = GetComponent<PlayerModel>();
        _pInput = GetComponent<PlayerInput>();
        _view = GetComponent<PlayerView>();
    }

    private void OnEnable()
    {
        _model.OnCoinsChanged += PresenterCoinsChanged;
        _model.OnLifeChanged += PresenterLifeChanged;
        _model.OnGameOver += PresenterGameOver;
        MeteoritePresenter.MakingDamage += _model.TakeDamage;
    }


    private void OnDisable()
    {
        _model.OnCoinsChanged -= PresenterCoinsChanged;
        _model.OnLifeChanged -= PresenterLifeChanged;
        _model.OnGameOver -= PresenterGameOver;
        MeteoritePresenter.MakingDamage -= _model.TakeDamage;
    }

    public int GetCurrentLife()
    {
        return _model.CurrentLife;
    }

    private void PresenterGameOver()
    {
        OnGameOver?.Invoke();
    }
    void FixedUpdate()
    {
        Vector3 input = _pInput.Axis;
        ApplyMovement(input);
        UpdateTilt(input.x);
    }

    public void ApplyMovement(Vector3 direction)
    {
        _rb.velocity = _model.CalculateMove(direction);

        bool isMoving = direction.magnitude > 0.1f;
        OnPlayerMoving?.Invoke(isMoving);
    }

    // Metodo para actualizar la inclinación del mesh
    private void UpdateTilt(float inputX)
    {
        Quaternion targetRotation = _model.CalculateTargetRotation(inputX);
        Quaternion currentRotation = _mesh.transform.localRotation;
        _mesh.transform.localRotation = Quaternion.Slerp(currentRotation, targetRotation, _model.TiltSpeed * Time.fixedDeltaTime);
    }
    private void OnTriggerEnter(Collider other)  //Ya no destruimos monedas ahora las mandamos al pool
    {
        if (other.CompareTag("Coin"))
        {
            _model.AddCoin();
            ObjectPoolManager.Instance.ReturnObjectToPool(other.gameObject, "Coin"); 
        }
    }
    private void PresenterCoinsChanged(int newCoinCount)
    {
        OnCoinsCollected?.Invoke(newCoinCount);
    }

    private void PresenterLifeChanged(int newCurrentLife)
    {
        OnLifeChanged.Invoke(newCurrentLife);
    }
}
