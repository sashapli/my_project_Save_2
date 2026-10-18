using UnityEngine;
using YG;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Подписываемся на событие загрузки данных
        YandexGame.GetDataEvent += OnDataLoaded;
    }

 private void Update()
    {
        if (Input.GetKey(KeyCode.P) && Input.GetKey(KeyCode.L))
            AddCoins(200);
        if (Input.GetKey(KeyCode.R))
        {
            ResetAll();
        }
    }

    private void OnDestroy()
    {
        // Отписываемся от события при уничтожении объекта
        YandexGame.GetDataEvent -= OnDataLoaded;
    }

    public void AddSesion()
    {
        YandexGame.savesData.isFirstSessions = false;
        SaveData();
    }

    public void AddDistan(int amount)
    {
        YandexGame.savesData.Distan = amount;
        SaveData();
        Debug.Log($"Добавлено {amount} монет. Всего: {YandexGame.savesData.coins}");
    }
    public void AddCoins(int amount)
    {
            YandexGame.savesData.coins += amount;
            SaveData();
            Debug.Log($"Добавлено {amount} монет. Всего: {YandexGame.savesData.coins}");
    }
    public void Addbakegears(int amount)
    {
        YandexGame.savesData.bakegears += amount;
        SaveData();
        Debug.Log($"Добавлено {amount} монет. Всего: {YandexGame.savesData.coins}");
    }

    public void AddForwardgears(int amount)
    {
        YandexGame.savesData.Forwardgears += amount;
        SaveData();
        Debug.Log($"Добавлено {amount} монет. Всего: {YandexGame.savesData.coins}");
    }

    public void AddISArm(bool amount)
    {
        YandexGame.savesData.ISArm = amount;
        SaveData();

    }

    public void AddISWheel(bool amount)
    {
        YandexGame.savesData.ISWheel = amount;
        SaveData();

    }

    public void AddBike(int[] amount)
    {
        YandexGame.savesData.BikeMe = amount;
        SaveData();

    }

    public void AddISBikeEsc(int amount)
    {
        YandexGame.savesData.BikeEcp = amount;
        SaveData();

    }

    public void AddCharGas(char amount)
    {
        YandexGame.savesData.gas = amount;
        SaveData();
    }

    public void AddCharbrak(char amount)
    {
        YandexGame.savesData.bake = amount;
        SaveData();
    }

    public void AddCharRight(char amount)
    {
        YandexGame.savesData.right = amount;
        SaveData();
    }

    public void AddCharLeft(char amount)
    {
        YandexGame.savesData.left = amount;
        SaveData();
    }

    // Метод для получения текущего количества монет

    public bool GetSesions()
    {
        if (YandexGame.SDKEnabled)
        {
            return YandexGame.savesData.isFirstSessions;
        }
        return false;
    }

    public int GetDistan()
    {
        if (YandexGame.SDKEnabled)
        {
            return YandexGame.savesData.Distan;
        }
        return 0;
    }
    public int GetCoins()
    {
        if (YandexGame.SDKEnabled)
        {
            return YandexGame.savesData.coins;
        }
        return 0;
    }

    public int Getbakegears()
    {
        if (YandexGame.SDKEnabled)
        {
            return YandexGame.savesData.bakegears;
        }
        return 0;
    }

    public int GetForwardgears()
    {
        if (YandexGame.SDKEnabled)
        {
            return YandexGame.savesData.Forwardgears;
        }
        return 0;
    }

    public bool GetISArm()
    {
        if (YandexGame.SDKEnabled)
        {
            return YandexGame.savesData.ISArm;
        }
        return false;
    }

    public bool GetISWheel()
    {
        if (YandexGame.SDKEnabled)
        {
            return YandexGame.savesData.ISWheel;
        }
        return false;
    }

    public int[] GetBike()
    {
        if (YandexGame.SDKEnabled)
        {
            return YandexGame.savesData.BikeMe;
        }
        return new int[] { 1,0,0,0,0};
    }

    public int GetBikeEsc()
    {
        if (YandexGame.SDKEnabled)
        {
            return YandexGame.savesData.BikeEcp;
        }
        return 0;
    }

    public char Getgas()
    {
        if (YandexGame.SDKEnabled)
        {
            return YandexGame.savesData.gas;
        }
        return 'd';
    }
    public char Getbrak()
    {
        if (YandexGame.SDKEnabled)
        {
            return YandexGame.savesData.bake;
        }
        return 'a';
    }
    public char Getright()
    {
        if (YandexGame.SDKEnabled)
        {
            return YandexGame.savesData.right;
        }
        return 'e';
    }
    public char Getleft()
    {
        if (YandexGame.SDKEnabled)
        {
            return YandexGame.savesData.left;
        }
        return 'a';
    }

    // Метод для сохранения данных
    public void SaveData()
    {
        YandexGame.SaveProgress();
    }

    // Обработчик загрузки данных
    private void OnDataLoaded()
    {
        Debug.Log($"Данные загружены. Монеты: {YandexGame.savesData.coins}"); 
    }

    // Метод для сброса монет (опционально)
    public void ResetCoins()
    {
        if (YandexGame.SDKEnabled)
        {
            YandexGame.savesData.coins = 0;
            SaveData();
        }
    }
    public void ResetAll()
    {
        if (YandexGame.SDKEnabled)
        {
            YandexGame.savesData.isFirstSessions = true;
            YandexGame.savesData.coins = 0;
            YandexGame.savesData.bakegears = 0;
            YandexGame.savesData.Forwardgears = 0;
            YandexGame.savesData.ISArm = false;
            YandexGame.savesData.ISWheel = false;
            YandexGame.savesData.BikeMe = new int[]{1,0,0,0,0};
            YandexGame.savesData.BikeEcp = 0;
            SaveData();
        }
    }
}