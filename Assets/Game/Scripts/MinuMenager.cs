using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.EventSystems;
using YG;
using System;

public class MinuMenager : MonoBehaviour
{
    public GameObject pausePanel;
    public GameObject blackScreenOverlay; // Черный экран для имитации предыдущей сцены
    public TMP_Text coinText;
    public WBGL_derect wBGL;
    public bool isinfaOff;
    public GameObject istutorpc;
    public GameObject istutorMobail;
    public GameObject istutor;

    private bool isPaused = false;
    private int targetSceneName = 0;
    private bool isTransitioning = false;
    private bool isAdShowingOnStart = false;
    private void Start()
    {
        Time.timeScale = 1;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }

        // Инициализируем черный экран
        InitializeBlackScreen();

        // Подписываемся на события закрытия рекламы
        YandexGame.CloseFullAdEvent += OnFullscreenAdClosed;
        YandexGame.CloseVideoEvent += OnRewardedAdClosed;

        UpdateCoinText();

        // Проверяем, если реклама уже показывается при загрузке сцены
        CheckForActiveAdsOnStart();
    }

    private void OnDestroy()
    {
        // Отписываемся от событий
        YandexGame.CloseFullAdEvent -= OnFullscreenAdClosed;
        YandexGame.CloseVideoEvent -= OnRewardedAdClosed;
    }

    // Проверка активной рекламы при старте сцены
    private void CheckForActiveAdsOnStart()
    {
        if (YandexGame.nowAdsShow || YandexGame.nowVideoAd)
        {
            isAdShowingOnStart = true;
            ShowBlackScreen();

            // Запускаем проверку закрытия рекламы
            StartCoroutine(CheckAdCompletionOnStart());
        }
    }

    // Постоянная проверка закрытия рекламы при старте
    private IEnumerator CheckAdCompletionOnStart()
    {
        while (YandexGame.nowAdsShow || YandexGame.nowVideoAd)
        {
            yield return new WaitForSeconds(0.1f);
        }

        // Реклама закрылась - убираем черный экран
        HideBlackScreen();
        isAdShowingOnStart = false;
    }

    // Инициализация черного экрана
    private void InitializeBlackScreen()
    {
        if (blackScreenOverlay != null)
        {
            // Делаем черный экран поверх всего
            blackScreenOverlay.transform.SetAsLastSibling();
            blackScreenOverlay.SetActive(false);
        }
    }

    // Показать черный экран
    private void ShowBlackScreen()
    {
        if (blackScreenOverlay != null)
        {
            blackScreenOverlay.SetActive(true);
            blackScreenOverlay.transform.SetAsLastSibling(); // Всегда поверх всего
        }
    }

    // Скрыть черный экран
    private void HideBlackScreen()
    {
        if (blackScreenOverlay != null)
        {
            blackScreenOverlay.SetActive(false);
        }
    }

    // ОСНОВНОЙ МЕТОД ДЛЯ ПЕРЕХОДА МЕЖДУ СЦЕНАМИ
    public void LoadSceneWithAd(int sceneName)
    {
        if (isTransitioning || isAdShowingOnStart) return;

        isTransitioning = true;
        targetSceneName = sceneName;

        // Показываем черный экран перед рекламой
        ShowBlackScreen();

        // Показываем рекламу перед переходом
        ShowFullscreenAd();
    }

    // Метод для показа полноэкранной рекламы
    private void ShowFullscreenAd()
    {
        if (YandexGame.nowAdsShow || YandexGame.nowVideoAd)
        {
            // Если уже показывается реклама, ждем её завершения
            StartCoroutine(WaitForAdToComplete());
            return;
        }

        // Показываем полноэкранную рекламу
        YandexGame.FullscreenShow();

        // Запускаем таймаут на случай, если реклама не покажется
        StartCoroutine(AdTimeoutCheck());
    }

    // Корутина для проверки таймаута рекламы
    private IEnumerator AdTimeoutCheck()
    {
        yield return new WaitForSeconds(2f);

        // Если через 2 секунды реклама так и не началась, переходим без рекламы
        if (!YandexGame.nowAdsShow && !YandexGame.nowVideoAd)
        {
            CompleteSceneTransition();
        }
    }

    // Ожидание завершения уже идущей рекламы
    private IEnumerator WaitForAdToComplete()
    {
        while (YandexGame.nowAdsShow || YandexGame.nowVideoAd)
        {
            yield return new WaitForSeconds(0.1f);
        }

        // После завершения рекламы показываем новую
        ShowFullscreenAd();
    }

    // Обработчик закрытия полноэкранной рекламы
    private void OnFullscreenAdClosed()
    {
        CompleteSceneTransition();
    }

    // Обработчик закрытия rewarded рекламы
    private void OnRewardedAdClosed()
    {
        // Для rewarded рекламы тоже убираем черный экран
        if (isAdShowingOnStart)
        {
            HideBlackScreen();
            isAdShowingOnStart = false;
        }
    }

    // Завершение перехода на сцену
    private void CompleteSceneTransition()
    {
        // Сначала убираем черный экран
        HideBlackScreen();
        SceneManager.LoadScene(targetSceneName);
        
        isTransitioning = false;
        targetSceneName = 1;
    }
    // ОБНОВЛЕННЫЕ МЕТОДЫ ДЛЯ КНОПОК:
    public void PlayButtonClicked()
    {
        LoadSceneWithAd(2);
    }

    public void HomeButtonClicked()
    {
        LoadSceneWithAd(0);
    }

    public void ShopButtonClicked()
    {
        LoadSceneWithAd(1);
    }

    public void RestartButtonClicked()
    {
        LoadSceneWithAd(2);
    }

    public void Pause()
    {
        isPaused = !isPaused;
        Time.timeScale = isPaused ? 0 : 1;

        if (pausePanel != null)
        {
            pausePanel.SetActive(isPaused);
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab) && SceneManager.GetActiveScene().name == "SampleScene")
        {
            Pause();
        }

        UpdateCoinText();

        // Дополнительная проверка рекламы в реальном времени
        if (isAdShowingOnStart && !YandexGame.nowAdsShow && !YandexGame.nowVideoAd)
        {
            HideBlackScreen();
            isAdShowingOnStart = false;
        }
    }

    private void UpdateCoinText()
    {
        if (coinText != null && CoinManager.Instance != null)
        {
            coinText.text = $"{CoinManager.Instance.GetCoins()}";
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ContinueGame()
    {
        isPaused = false;
        Time.timeScale = 1;

        if (pausePanel != null)
        {
            pausePanel.SetActive(false);
        }
    }

    // Старые методы для совместимости
    public void ClikScen()
    {
        PlayButtonClicked();
    }

    public void Home()
    {
        SceneManager.LoadScene(0);
    }

    public void magas()
    {
        ShopButtonClicked();
    }

    public void infa()
    {
        if (isinfaOff)
        {
            if (wBGL.IsMobileDevice)
            {
                istutorMobail.SetActive(false);
            }
            else
            {
                istutorpc.SetActive(false);
            }
            istutor.SetActive(false);
        }
        else
        {
            if (wBGL.IsMobileDevice)
            {
                istutorMobail.SetActive(true);
            }
            else
            {
                istutorpc.SetActive(true);
            }
            istutor.SetActive(true);
        }
        isinfaOff = !isinfaOff;
    }
}