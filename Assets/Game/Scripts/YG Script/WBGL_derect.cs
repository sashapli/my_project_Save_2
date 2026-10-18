using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;
using YG;

public class WBGL_derect : MonoBehaviour
{
    public bool testInIspector = false;
    public GameStateManager gameState;

    public bool IsMobileDevice;
    private string firstLaunchKey = "FirstLaunch";
    private bool isFirstLaunch = false;

    private void Start()
    {
        CheckFirstLaunch();
    }

    private void CheckFirstLaunch()
    {
        // Проверяем, первый ли это запуск
        if (PlayerPrefs.GetInt(firstLaunchKey, 0) == 0)
        {
            // Первый запуск - останавливаем время и показываем тутор
            Time.timeScale = 0f; // Останавливаем игру
            isFirstLaunch = true;
            DetermineDeviceType();
            PlayerPrefs.SetInt(firstLaunchKey, 1);
            PlayerPrefs.Save();
        }
        else
        {
            // Не первый запуск - время нормальное, показываем только контроллер
            Time.timeScale = 1f; // Нормальная скорость игры
            isFirstLaunch = false;
            ShowControllerOnly();
        }
    }

    private void DetermineDeviceType()
    {
        // Определяем тип устройства
#if UNITY_EDITOR
        IsMobileDevice = SystemInfo.deviceType == DeviceType.Handheld;
#else
        IsMobileDevice = YandexGame.EnvironmentData.isMobile || YandexGame.EnvironmentData.isTablet;
#endif
        if (gameState != null)
            gameState.StartWeb(IsMobileDevice);
    }

    private void ShowControllerOnly()
    {
        // Не первый запуск: показываем только контроллер для мобилок
#if UNITY_EDITOR
        IsMobileDevice = SystemInfo.deviceType == DeviceType.Handheld;
#else
        IsMobileDevice = YandexGame.EnvironmentData.isMobile || YandexGame.EnvironmentData.isTablet;
#endif
        if(gameState != null)
        gameState.StartWeb(IsMobileDevice);

        Debug.Log("Не первый запуск - показан только контроллер для мобилок");
    }

    // Публичный метод для кнопки - размораживает игру
    public void ResumeGame()
    {
        if (isFirstLaunch)
        {
            Time.timeScale = 1f; // Возобновляем игру
            isFirstLaunch = false;

            Debug.Log("Игра разморожена, туториал скрыт");
        }
    }

    // Метод для сброса первого запуска
    public void ResetFirstLaunch()
    {
        PlayerPrefs.DeleteKey(firstLaunchKey);
        PlayerPrefs.Save();
        Time.timeScale = 1f; // Восстанавливаем время
        isFirstLaunch = false;
        Debug.Log("Первый запуск сброшен, время восстановлено");
    }
}