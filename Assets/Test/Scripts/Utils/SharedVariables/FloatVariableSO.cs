using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Shared Variable/Float")]
public class FloatVariableSO : ScriptableObject
{
    //Propiedades
    [Header("Float Initial Value / Properties")]
    public float InitialValue;
    public float MinValue = 0;
    public float MaxValue = 100;

    
    [Header("Float Runtime Value")]
    [SerializeField] private float RuntimeValue;

    public float CurrentValue
    {
        get => RuntimeValue;
        set
        {
            RuntimeValue = Mathf.Clamp(value, MinValue, MaxValue);
            try
            {
                onValueUpdate?.Invoke();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }
    }

    //Eventos
    [Header("Events for changes in variable")]
    [NonSerialized] public UnityEvent onValueUpdate;

    private void OnEnable()
    {
        RuntimeValue = InitialValue;
        onValueUpdate ??= new();
    }

    //Check para detectar cambios
    private float check;

    private void OnValidate()
    {
        if (check != RuntimeValue) 
        {
            CurrentValue = CurrentValue;
            check = RuntimeValue;
        }
    }
}
