using UnityEngine;
using YG;
using System.Collections;

public class AdSceneManager : MonoBehaviour
{
    // Публичный метод для показа полноэкранной рекламы
    public void ShowFullscreenAd()
    {
        StartCoroutine(TryShowAdRepeatedly());
    }

    private IEnumerator TryShowAdRepeatedly()
    {
        int attempts = 0;

        while (attempts < 5 && !YandexGame.nowAdsShow)
        {
            YandexGame.FullscreenShow();
            attempts++;
            yield return new WaitForSeconds(0.2f);
        }

        if (attempts >= 5)
        {
            Debug.Log("Не удалось показать рекламу после нескольких попыток");
        }
    }

    // Публичный метод для рекламы за вознаграждение
    public void ShowRewardedAd()
    {
        if (!YandexGame.nowVideoAd)
        {
            YandexGame.RewVideoShow(0);
        }
    }

    // Обработчик завершения рекламы за вознаграждение
    private void OnRewardedAdCompleted(int id)
    {
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.AddCoins(250);
            Debug.Log("Награда получена: +250 монет!");
        }
    }

    private void Start()
    {
        YandexGame.RewardVideoEvent += OnRewardedAdCompleted;
    }

    private void OnDestroy()
    {
        YandexGame.RewardVideoEvent -= OnRewardedAdCompleted;
    }
}