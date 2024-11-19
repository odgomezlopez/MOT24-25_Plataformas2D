using System;
using UnityEngine;
using UnityEngine.Events;


[System.Serializable]
public class SmartVariable<T>
{
    //Datos
    [Header("Variable value")]
    public T initialValue;
    [SerializeField] protected T runtimeValue;

    [Header("OnChange events")]
    public UnityEvent<T> OnValueUpdate; //Cambiable por Action<T> sino se requiere asociar información en el inspector

    //Get y Set publico
    public virtual T CurrentValue
    {
        get => runtimeValue;
        set
        {
            T oldValue = runtimeValue;
            runtimeValue = value;

            if (!oldValue.Equals(runtimeValue)) {
                try
                {
                    OnValueUpdate?.Invoke(CurrentValue);
                }
                catch (Exception e) { Debug.LogError("An event attached to the indicator has failed"); Debug.LogException(e); }
            }
        }
    }

    public void Reset()
    {
        CurrentValue = initialValue;
    }

    public virtual void Update(SmartVariable<T> newValue)
    {
        initialValue = newValue.initialValue;
        CurrentValue = newValue.runtimeValue;
    }
}

[System.Serializable]
public class RangedSmartFloat : SmartVariable<float> 
{
    [Header("Ranged value")]

    public float minValue = 0;
    public float maxValue = 100;

    public  override float CurrentValue
    {
        get => runtimeValue;
        set
        {
            float oldValue = runtimeValue;
            runtimeValue = Mathf.Clamp(value, minValue, maxValue);

            if (!oldValue.Equals(runtimeValue))
            {
                try
                {
                    OnValueUpdate?.Invoke(GetPercentage());
                }
                catch (Exception e) { Debug.LogError("An event attached to the indicator has failed"); Debug.LogException(e); }
            }
        }
    }

    public float GetPercentage()
    {
        if ((maxValue - minValue) == 0) return 0;
        return (runtimeValue - minValue) / (maxValue - minValue);
    }

    public void Update(RangedSmartFloat newValue)
    {
        base.Update(newValue);
        maxValue = newValue.maxValue;
        minValue = newValue.minValue;
    }
}