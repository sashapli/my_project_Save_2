using UnityEngine;
using UnityEngine.UI;
using TMPro;
using YG;
using System.Collections;

public class TimerAdDuringGame : MonoBehaviour
{
    [Header("Настройки таймера")]
    public float adInterval = 120f; // Интервал между рекламой (2 минуты)
    public float countdownDuration = 3f; // Обратный отсчет: 3, 2, 1
    
    [Header("UI элементы")]
    public GameObject adCountdownPanel;
    public TMP_Text countdownText;
    public Image countdownFill;
    
    private float timer = 0f;
    private bool isPaused = false;
    private bool isShowingAd = false;

    private void Start()
    {
        // Скрываем панель при запуске
        if (adCountdownPanel != null)
            adCountdownPanel.SetActive(false);
        
        // Сбрасываем таймер
        timer = 0f;
    }

    private void Update()
    {
        // Не обновляем таймер если игра на паузе или показывается реклама
        if (isPaused || isShowingAd || YandexGame.nowAdsShow)
            return;
        
        // Обновляем таймер
        timer += Time.deltaTime;
        
        // Проверяем, пора ли показывать рекламу
        if (timer >= adInterval)
        {
            StartCoroutine(ShowAdWithCountdown());
        }
    }

    // Вызывайте этот метод когда игра ставится на паузу
    public void SetPaused(bool paused)
    {
        isPaused = paused;
        
        if (paused)
        {
            // Останавливаем корутину если она запущена
            StopAllCoroutines();
            
            // Скрываем панель
            if (adCountdownPanel != null)
                adCountdownPanel.SetActive(false);
        }
    }

    private IEnumerator ShowAdWithCountdown()
    {
        isShowingAd = true;
        
        // Показываем обратный отсчет
        if (adCountdownPanel != null)
            adCountdownPanel.SetActive(true);
        
        // Обратный отсчет: 3, 2, 1
        float countdownTimer = countdownDuration;
        
        while (countdownTimer > 0)
        {
            if (isPaused) // Если во время отсчета поставили на паузу
            {
                if (adCountdownPanel != null)
                    adCountdownPanel.SetActive(false);
                isShowingAd = false;
                yield break;
            }
            
            // Обновляем UI
            if (countdownText != null)
                countdownText.text = Mathf.CeilToInt(countdownTimer).ToString();
            
            if (countdownFill != null)
                countdownFill.fillAmount = countdownTimer / countdownDuration;
            
            countdownTimer -= Time.deltaTime;
            yield return null;
        }
        
        // Скрываем панель
        if (adCountdownPanel != null)
            adCountdownPanel.SetActive(false);
        
        // Показываем рекламу
        if (!YandexGame.nowAdsShow && !isPaused)
        {
            YandexGame.FullscreenShow();
            
            // Ждем завершения рекламы
            yield return new WaitForSeconds(0.1f);
            yield return new WaitWhile(() => YandexGame.nowFullAd);
        }
        
        // Сбрасываем таймер
        timer = 0f;
        isShowingAd = false;
    }

    // Метод для принудительного сброса таймера (например, после показа рекламы другим способом)
    public void ResetTimer()
    {
        timer = 0f;
        StopAllCoroutines();
        
        if (adCountdownPanel != null)
            adCountdownPanel.SetActive(false);
        
        isShowingAd = false;
    }

    // Для отладки - принудительно запустить таймер
    public void ForceStartCountdown()
    {
        if (!isShowingAd && !isPaused)
        {
            StartCoroutine(ShowAdWithCountdown());
        }
    }
}