using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class VolumeManager : MonoBehaviour
{
    Volume volume;
    ColorAdjustments colorAdjustments;

    [SerializeField, Range(0f,3f)] float effectAnimationTime = 0.2f;
    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);

    }

    // Update is called once per frame
    public void EnableBlackWhiteEffect()
    {
        colorAdjustments.active = true;

        //colorAdjustments.saturation.Override(-100f);
        StartCoroutine(ChangeValue(colorAdjustments.saturation, colorAdjustments.saturation.value, -100, effectAnimationTime));
    }

    public void DisableBlackWhiteEffect()
    {
        colorAdjustments.active = false;
        colorAdjustments.saturation.Override(0f);
    }

    IEnumerator ChangeValue(FloatParameter value,float initValue, float targetValue, float changeTime)
    {
        float t = 0f;

        while(t < changeTime)
        {
            t += Time.deltaTime;
            float tmp = Mathf.Lerp(initValue, targetValue, t / changeTime);
            value.Override(tmp);
            yield return new WaitForEndOfFrame();
        }

        value.Override(targetValue);
    }
}
