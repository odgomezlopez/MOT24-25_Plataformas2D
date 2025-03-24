using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using static UnityEngine.Rendering.DebugUI;

public class FloatVariableListener : MonoBehaviour
{
    [SerializeField] private FloatVariableSO floatVariableSO;

    public UnityEvent<float> OnValueUpdate;
    public UnityEvent<float> OnPercentageUpdate;

    private void Start()
    {
        OnValue();
    }
    private void OnEnable()
    {
        floatVariableSO.onValueUpdate.AddListener(OnValue);
    }

    private void OnDisable()
    {
        floatVariableSO.onValueUpdate.RemoveListener(OnValue);
    }

    private void OnValue()
    {
        OnValueUpdate.Invoke(floatVariableSO.CurrentValue);
        OnPercentageUpdate.Invoke(floatVariableSO.CurrentValue / floatVariableSO.MaxValue);
    }
}
