using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SliderColorHandle : MonoBehaviour, ISelectHandler, IDeselectHandler
{
    Slider _slider;
    [SerializeField] Image backgroundImage;
    [SerializeField] TextMeshProUGUI text;


    [SerializeField] Color selectedBackgroundColor = Color.white;


    Color defaultColorSlider, defaultColorText;

    void Start()
    {
        _slider = GetComponent<Slider>();
        defaultColorSlider = backgroundImage.color;
        defaultColorText = text.color;
    }

    public void OnSelect(BaseEventData eventData)
    {
        backgroundImage.color = selectedBackgroundColor;//_slider.colors.selectedColor;
        text.color = selectedBackgroundColor;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        backgroundImage.color = defaultColorSlider;
        text.color=defaultColorText;
    }
}
