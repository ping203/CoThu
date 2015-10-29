using UnityEngine;
using System.Collections;

public class TextureSlider : MonoBehaviour
{
    [SerializeField]
    private float value = 1.0f;
    [SerializeField]
    private float startValue = 1.0f;
    private float oldValue = 0.0f;
    [SerializeField]
    private UISprite spriteTime;

    void Start()
    {
        value = startValue;
        UpdateTexture(value);
    }

    void Update()
    {
        if (oldValue != value)
        {
            oldValue = value;
            UpdateTexture(value);
        }
    }
    void UpdateTexture(float value)
    {
        //value = 1 - value;
        spriteTime.fillAmount = value;
    }
    public void Resset()
    {
        value = startValue;
    }
    public void SetValue(float value)
    {
        this.value = value;
    }
}
