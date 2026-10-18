using UnityEngine;
using TMPro;
using System.Collections;
using YG;
public class WheelieTrick : MonoBehaviour
{
    [Header("Настройки трюков на колесах")]
    public TMP_Text rearWheelText; // Текст для заднего колеса
    public TMP_Text frontWheelText; // Текст для переднего колеса

    public Color rearWheelColor = Color.green;
    public Color frontWheelColor = Color.blue;

    public string rearWheelMessage = "ON THE REAR WHEEL";
    public string frontWheelMessage = "ON THE FRONT WHEEL";

    public string rearWheelMessageRu = "НА ЗАДНЕМ КОЛЕСЕ";
    public string frontWheelMessageRu = "НА ПЕРЕДНЕМ КОЛЕСЕ";

    public float fadeDuration = 0.5f;
    public float coinsPerSecond = 50f; // Монет в секунду
    public float activationThreshold = 1f; // Порог активации в секундах

    [Header("Ссылки")]
    public CycleControl cycleControl; // Ссылка на скрипт управления велосипедом
    public JointObject jointObject;

    [Header("Только для просмотра")]
    [SerializeField] private float rearWheelTime = 0f;
    [SerializeField] private float frontWheelTime = 0f;
    [SerializeField] private bool isRearWheelie = false;
    [SerializeField] private bool isFrontWheelie = false;
    [SerializeField] private bool isRearWheelieActive = false;
    [SerializeField] private bool isFrontWheelieActive = false;
    [SerializeField] private float totalCoinsEarned = 0f;

    private Coroutine rearWheelCoroutine;
    private Coroutine frontWheelCoroutine;

    void Start()
    {
        // Инициализируем тексты как прозрачные
        InitializeText(rearWheelText, rearWheelColor);
        InitializeText(frontWheelText, frontWheelColor);

        // Устанавливаем текст
        if (rearWheelText != null) rearWheelText.text = rearWheelMessage;
        if (frontWheelText != null) frontWheelText.text = frontWheelMessage;
    }

    void Update()
    {
        CheckWheelieTricks();
    }

    void CheckWheelieTricks()
    {
        if (cycleControl == null)
        {
            return;
        }

        // Проверка трюка на заднем колесе
        bool rearWheelOnGround = cycleControl.isRearWheelOnGround;
        bool frontWheelOnGround = cycleControl.isFrontWheelOnGround;

        // Заднее колесо: переднее в воздухе, заднее на земле
        if (!frontWheelOnGround && rearWheelOnGround && !isFrontWheelieActive)
        {
            if (!isRearWheelie)
            {
                StartRearWheelie();
            }
            rearWheelTime += Time.deltaTime;

            // Проверяем достижение порога
            if (rearWheelTime >= activationThreshold && !isRearWheelieActive)
            {
                ActivateRearWheelie();
            }
        }
        else if (isRearWheelie)
        {
            StopRearWheelie();
        }

        // Переднее колесо: заднее в воздухе, переднее на земле
        if (!rearWheelOnGround && frontWheelOnGround && !isRearWheelieActive)
        {
            if (!isFrontWheelie)
            {
                StartFrontWheelie();
            }
            frontWheelTime += Time.deltaTime;

            // Проверяем достижение порога
            if (frontWheelTime >= activationThreshold && !isFrontWheelieActive)
            {
                ActivateFrontWheelie();
            }
        }
        else if (isFrontWheelie)
        {
            StopFrontWheelie();
        }
    }

    void StartRearWheelie()
    {
        isRearWheelie = true;
        rearWheelTime = 0f;
        isRearWheelieActive = false;
    }

    void ActivateRearWheelie()
    {
        isRearWheelieActive = true;

        if (rearWheelCoroutine != null) StopCoroutine(rearWheelCoroutine);
        rearWheelCoroutine = StartCoroutine(RearWheelieEffect());
    }

    void StartFrontWheelie()
    {
        isFrontWheelie = true;
        frontWheelTime = 0f;
        isFrontWheelieActive = false;

   
    }

    void ActivateFrontWheelie()
    {
        isFrontWheelieActive = true;

        if (frontWheelCoroutine != null) StopCoroutine(frontWheelCoroutine);
        frontWheelCoroutine = StartCoroutine(FrontWheelieEffect());

    
    }

    void StopRearWheelie()
    {
        if (!isRearWheelie) return;

        // Начисляем монеты только если трюк был активирован
        if (isRearWheelieActive)
        {
            float activeTime = rearWheelTime - activationThreshold;
            int coinsEarned = Mathf.RoundToInt(activeTime * coinsPerSecond);
            if (coinsEarned > 0)
            {
                CoinManager.Instance.AddCoins(coinsEarned);
              //  jointObject.AddCoinWheel(coinsEarned);
                totalCoinsEarned += coinsEarned;
            }
        }

        // Скрываем текст если он показывался
        if (isRearWheelieActive && rearWheelCoroutine != null)
        {
            StopCoroutine(rearWheelCoroutine);
            StartCoroutine(FadeOutText(rearWheelText, rearWheelColor));
        }

     

        isRearWheelie = false;
        isRearWheelieActive = false;
        rearWheelTime = 0f;
    }

    void StopFrontWheelie()
    {
        if (!isFrontWheelie) return;

        // Начисляем монеты только если трюк был активирован
        if (isFrontWheelieActive)
        {
            float activeTime = frontWheelTime - activationThreshold;
            int coinsEarned = Mathf.RoundToInt(activeTime * coinsPerSecond);
            if (coinsEarned > 0)
            {
                CoinManager.Instance.AddCoins(coinsEarned);
            //    jointObject.AddCoinWheel(coinsEarned);
                totalCoinsEarned += coinsEarned;
            }
        }

        // Скрываем текст если он показывался
        if (isFrontWheelieActive && frontWheelCoroutine != null)
        {
            StopCoroutine(frontWheelCoroutine);
            StartCoroutine(FadeOutText(frontWheelText, frontWheelColor));
        }

        isFrontWheelie = false;
        isFrontWheelieActive = false;
        frontWheelTime = 0f;
    }

    IEnumerator RearWheelieEffect()
    {
        // Плавное появление
        yield return StartCoroutine(FadeInText(rearWheelText, rearWheelColor));

        // Обновление текста с временем
        while (isRearWheelieActive)
        {
            if (rearWheelText != null)
            {
                float activeTime = rearWheelTime - activationThreshold;
                rearWheelText.text = YandexGame.EnvironmentData.language == "ru" ? $"{rearWheelMessageRu}\n+{activeTime:F1}сек" : $"{rearWheelMessage}\n+{activeTime:F1}сек";
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator FrontWheelieEffect()
    {
        // Плавное появление
        yield return StartCoroutine(FadeInText(frontWheelText, frontWheelColor));

        // Обновление текста с временем
        while (isFrontWheelieActive)
        {
            if (frontWheelText != null)
            {
                float activeTime = frontWheelTime - activationThreshold;
                frontWheelText.text = YandexGame.EnvironmentData.language == "ru" ? $"{frontWheelMessageRu}\n+{activeTime:F1}сек" : $"{frontWheelMessage}\n+{activeTime:F1}сек";
            }
            yield return new WaitForSeconds(0.1f);
        }
    }

    IEnumerator FadeInText(TMP_Text text, Color color)
    {
        if (text == null) yield break;

        float timer = 0f;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.SmoothStep(0f, 1f, timer / fadeDuration);
            Color newColor = color;
            newColor.a = progress;
            text.color = newColor;
            yield return null;
        }

        text.color = color;
    }

    IEnumerator FadeOutText(TMP_Text text, Color color)
    {
        if (text == null) yield break;

        float timer = 0f;
        Color startColor = text.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.SmoothStep(0f, 1f, timer / fadeDuration);
            Color newColor = color;
            newColor.a = Mathf.Lerp(startColor.a, 0f, progress);
            text.color = newColor;
            yield return null;
        }

        text.color = new Color(color.r, color.g, color.b, 0f);
    }

    void InitializeText(TMP_Text text, Color color)
    {
        if (text != null)
        {
            text.color = new Color(color.r, color.g, color.b, 0f);
        }
    }

}