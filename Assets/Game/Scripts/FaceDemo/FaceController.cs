using UnityEngine;
using UnityEngine.UI;

public class FaceController : MonoBehaviour
{
    [Header("Material Referanslarý")]
    public Material eyeL;
    public Material eyeR;
    public Material eyeLightL;
    public Material eyeLightR;
    public Material mouth;

    [Header("UI Slider Kontrolleri")]
    public Slider eyeMainSlider;
    public Slider eyeL_Slider;
    public Slider eyeR_Slider;
    public Slider mouthSlider;

    // --- OFFSET VERÝLERÝ ---
    private float[] eyeOffsets = { 0f, 0.14f, 0.26f, 0.38f, 0.5f, 0.63f, 0.75f, 0.87f };
    private float[] mouthOffsets = { 0f, 0.11f, 0.23f, 0.33f, 0.44f, 0.57f, 0.66f, 0.78f, 0.89f };
    private float[] lightOffsets = { 0f, 0.19f, 0.4f, 0.59f, 0.94f };

    // Eye -> EyeLight Eþleþme Tablosu
    // Senin isteðinle güncellendi: 8. eye (index 7) -> 5. light (index 4)
    private int[] eyeToLightMap = { 4, 0, 4, 1, 4, 2, 2, 4 };

    // Unity 2022.3 URP için varsayýlan isim _BaseMap'tir. 
    // Eðer Standart Shader kullanýyorsan bunu "_MainTex" yapmalýsýn.
    private string propertyName = "_MainTex";

    void Start()
    {
        SetupSliders();

        // Eventleri Baðla
        eyeMainSlider.onValueChanged.AddListener(OnMainEyeSliderChanged);
        eyeL_Slider.onValueChanged.AddListener((val) => UpdateEye(eyeL, eyeLightL, (int)val));
        eyeR_Slider.onValueChanged.AddListener((val) => UpdateEye(eyeR, eyeLightR, (int)val));
        mouthSlider.onValueChanged.AddListener((val) => UpdateMouth((int)val));
    }

    void SetupSliders()
    {
        // Slider ayarlarýný otomatik yapalým
        ConfigureSlider(eyeMainSlider, eyeOffsets.Length);
        ConfigureSlider(eyeL_Slider, eyeOffsets.Length);
        ConfigureSlider(eyeR_Slider, eyeOffsets.Length);
        ConfigureSlider(mouthSlider, mouthOffsets.Length);
    }

    void ConfigureSlider(Slider slider, int length)
    {
        if (slider == null) return;
        slider.minValue = 0;
        slider.maxValue = length - 1;
        slider.wholeNumbers = true;
    }

    void OnMainEyeSliderChanged(float val)
    {
        int index = (int)val;
        // Alt slider'larý görsel olarak eþitle
        eyeL_Slider.value = index;
        eyeR_Slider.value = index;

        // Her iki gözü ve ýþýðý güncelle
        UpdateEye(eyeL, eyeLightL, index);
        UpdateEye(eyeR, eyeLightR, index);
    }

    void UpdateEye(Material eyeMat, Material lightMat, int index)
    {
        if (eyeMat != null)
        {
            float x = eyeOffsets[index];
            eyeMat.SetTextureOffset(propertyName, new Vector2(x, 0));
        }

        if (lightMat != null && index < eyeToLightMap.Length)
        {
            int lightIndex = eyeToLightMap[index];
            float lightX = lightOffsets[lightIndex];
            lightMat.SetTextureOffset(propertyName, new Vector2(lightX, 0));
        }
    }

    void UpdateMouth(int index)
    {
        if (mouth != null)
        {
            float x = mouthOffsets[index];
            mouth.SetTextureOffset(propertyName, new Vector2(x, 0));
        }
    }
}