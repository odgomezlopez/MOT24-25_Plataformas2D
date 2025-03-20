using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PostprocManager : MonoBehaviour
{
    //References
    Volume volume;
    ColorAdjustments colorAdjustments;
    LensDistortion lensDistortion;

    //Control
    [SerializeField, Range(0f,3f)] float effectAnimationTime = 0.2f;
    void Start()
    {
        volume = GetComponent<Volume>();
        volume.profile.TryGet<ColorAdjustments>(out colorAdjustments);
        volume.profile.TryGet<LensDistortion>(out lensDistortion);
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    // Update is called once per frame
    public void EnableBlackWhiteEffect(float targetValue=-100f)
    {
        colorAdjustments.active = true;

        //colorAdjustments.saturation.Override(-100f);
        StartCoroutine(ChangeValue(colorAdjustments.saturation, 0f, targetValue, effectAnimationTime));
    }

    public void DisableBlackWhiteEffect()
    {
        colorAdjustments.active = false;
        colorAdjustments.saturation.Override(0f);
    }

    public void EnableLensDistortion(float targetValue)
    {
        lensDistortion.active = true;

        //colorAdjustments.saturation.Override(-100f);
        StartCoroutine(ChangeValue(lensDistortion.intensity, 0f, targetValue, effectAnimationTime));
    }

    public void DisableLensDistortion()
    {
        lensDistortion.active = false;
        lensDistortion.intensity.Override(0f);
    }


    IEnumerator ChangeValue(FloatParameter value,float initValue, float targetValue, float changeTime)
    {
        float t = 0f;

        while(t < changeTime && volume != null)
        {
            t += Time.deltaTime;
            float tmp = Mathf.Lerp(initValue, targetValue, t / changeTime);
            value.Override(tmp);
            yield return new WaitForEndOfFrame();
        }

        value.Override(targetValue);
    }
}
