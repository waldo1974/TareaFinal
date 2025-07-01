using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoritePresenter : MonoBehaviour
{
    private MeteoriteModel _metModel;
    private MeteoriteView _metView;

    static public event Action<int> MakingDamage;

    private void Awake()
    {
        _metModel = GetComponent<MeteoriteModel>();
        _metView = GetComponent<MeteoriteView>();
        // Initialize() es llamado desde OnEnable pero tambien es útil tenerlo aca.
        // si el objeto esta inactivo Awake, OnEnable manejan la inicializacion 
        Initialize(); 
    }

    private void OnEnable()
    {
        // nos aseguramos de que meteorito se inicialice cuando spawmea/respawmea desde el pool
        Initialize();
    }

    public void Initialize()
    {
        if (_metModel == null) _metModel = GetComponent<MeteoriteModel>();
        if (_metView == null) _metView = GetComponent<MeteoriteView>();
    }

    private void OnTriggerEnter(Collider other) //Ya no destruimos meteoritos, los devolvemos al pool
    {
        if (other.CompareTag("Player"))
        {
            MakeDamage();
            if(_metView != null) _metView.PlayDmgSound();
            ObjectPoolManager.Instance.ReturnObjectToPool(this.gameObject, "Meteorite");
        }
    }

    private void MakeDamage()
    {
        if (_metModel != null)
        {
            MakingDamage?.Invoke(_metModel.Damage); 
        }
        
    }
}
