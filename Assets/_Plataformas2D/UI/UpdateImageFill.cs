using UnityEngine;
using UnityEngine.UI;

public class UpdateImageFill : MonoBehaviour
{
    Image img;
    void Awake()
    {
        img = GetComponent<Image>();
    }

    public void UpdateFill(float f)
    {
        if(img)img.fillAmount = f;
    }
}
