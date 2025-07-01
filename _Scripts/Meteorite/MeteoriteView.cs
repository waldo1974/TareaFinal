using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoriteView : MonoBehaviour
{
    private Renderer _renderer;
    private float _speed = 5f; // Default speed
    private AudioSource damageSound;
    private MeteoriteModel _model;

    private void Awake()
    {
        _model = GetComponent<MeteoriteModel>();
        _renderer = GetComponent<Renderer>();
        damageSound = GameObject.FindGameObjectWithTag("Buffer").GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        StartCoroutine(ApplyStatsRoutine());
    }

    IEnumerator ApplyStatsRoutine()
    {
        yield return null; // Esperar un frame


        if (_model == null)
        {
            yield break;
        }
        SetSize(_model.Size);
        SetColor(_model.Color);
        SetSpeed(_model.Speed);
    }

    public void SetSize(float size)
    {
        transform.localScale = Vector3.one * size;
    }

    public void SetColor(Color color)
    {
        if (_renderer != null && _renderer.material != null)
        {
            _renderer.material.color = color;
        }  
    }

    public void SetSpeed(float speed)
    {
        this._speed = speed;
    }

    void Update()
    {
        transform.Translate(Vector3.down * _speed * Time.deltaTime);
    }

    public void PlayDmgSound()
    {
        if (damageSound != null)
        {
            damageSound.Play();
        }
        
    }
}
