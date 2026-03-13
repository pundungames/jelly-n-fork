using UnityEngine;
using System.Collections;

public class AnimationTestController : MonoBehaviour
{
    private Animator animator;

    [Header("Animasyon Ayarlarý")]
    // Animator panelindeki State isimleri buradakilerle ayný olmalý!
    public string[] animNames = { "Anim1", "Anim2", "Anim3", "Anim4", "Anim5", "Anim6" };

    [Header("Otomatik Mod Ayarlarý")]
    public float transitionDelay = 2.0f; // G tuþuna basýnca kaç saniyede bir deðiþsin?

    private bool isAutoMode = false;
    private int currentAnimIndex = 0;
    private Coroutine autoCoroutine;

    void Start()
    {
        animator = GetComponent<Animator>();

        if (animator == null)
            Debug.LogError("Hacý Animator'ý bulamadým, objeye eklediðinden emin ol!");
    }

    void Update()
    {
        // 1'den 6'ya kadar tuþ kontrolleri (Alpha tuþlarý klavyenin üstündekilerdir)
        for (int i = 0; i < animNames.Length; i++)
        {
            if (Input.GetKeyDown(KeyCode.Alpha1 + i))
            {
                StopAutoMode(); // Manuel tuþa basýnca otomatiði durdurur
                PlaySelectedAnimation(i);
            }
        }

        // 'G' Tuþu - Otomatik Sýralý Mod
        if (Input.GetKeyDown(KeyCode.G))
        {
            isAutoMode = !isAutoMode;

            if (isAutoMode)
            {
                Debug.Log("OTOMATÝK GEÇÝÞ AÇILDI");
                autoCoroutine = StartCoroutine(AutoCycleRoutine());
            }
            else
            {
                StopAutoMode();
                Debug.Log("OTOMATÝK GEÇÝÞ KAPATILDI");
            }
        }
    }

    void PlaySelectedAnimation(int index)
    {
        if (animator != null && index < animNames.Length)
        {
            currentAnimIndex = index;
            // 0.2f süresiyle animasyonlar arasý yumuþak geçiþ yapar
            animator.CrossFade(animNames[index], 0.2f);
            Debug.Log("Þu an oynuyor: " + animNames[index]);
        }
    }

    void StopAutoMode()
    {
        isAutoMode = false;
        if (autoCoroutine != null)
        {
            StopCoroutine(autoCoroutine);
            autoCoroutine = null;
        }
    }

    IEnumerator AutoCycleRoutine()
    {
        while (isAutoMode)
        {
            PlaySelectedAnimation(currentAnimIndex);

            yield return new WaitForSeconds(transitionDelay);

            // Endekse 1 ekle, listenin sonuna gelince 0'a (baþa) dön
            currentAnimIndex = (currentAnimIndex + 1) % animNames.Length;
        }
    }
}