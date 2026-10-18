using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using YG;
using UnityEngine.UI;

public class minuMadicBike : MonoBehaviour
{
    public Animator nextbike;
    public Animator noCoin;
    public bool ISanim;
    public madificatorVid bike1;
    public TMP_Text text;
    public GameObject Slader;
    public GameObject Slader2;
    public GameObject Slader3;
    public GameObject Slader4;
    public TMP_Text Coin;

    public GameObject Buut1;
    public GameObject Buut2;
    public GameObject madific;
    public TMP_Text[] TMP;
    public int[] allMoney;
    public int atap = 1;
    public bool TackaIs;
    public Color Color1;
    public Color color2;
    public Image[] list;

    private void Awake()
    {
        // ѕолучаем состо€ние всех транспортных средств
        int[] isBikePurchased = CoinManager.Instance.GetBike();
        int isBikeEquipped = CoinManager.Instance.GetBikeEsc();

        // ќпредел€ем, применен ли стандартный транспорт (ни один не экипирован)
        bool isDefaultTransportEquipped = isBikeEquipped ==0;

        // Madific активен только когда применен стандартный транспорт
        madific.SetActive(isDefaultTransportEquipped);

        // ”станавливаем тексты дл€ всех транспортных средств
        SetTransportTexts(isBikePurchased, isBikeEquipped);
    }

    // ћетод дл€ установки текстов всех транспортных средств
    private void SetTransportTexts(int[] isBikePurchased, int isBikeEquipped)
    {
        for(var i = 0;i < isBikePurchased.Length; i++)
            if (isBikePurchased[i] == 1)
            {
                if (i == isBikeEquipped)
                    TMP[i].text = GetLocalizedText("экиперован", "equipped");
                else
                    TMP[i].text = GetLocalizedText("экиперовать", "equip");
            }
            else
                TMP[i].text = GetLocalizedText($"купить за {allMoney[i]}", $"buy for {allMoney[i]}");
    }

    // ¬спомогательный метод дл€ локализации
    private string GetLocalizedText(string russianText, string englishText)
    {
        return YandexGame.EnvironmentData.language == "ru" ? russianText : englishText;
    }

    private void Update()
    {
        Coin.text = $"{CoinManager.Instance.GetCoins()}";
    }

    public void Right()
    {
        if (atap == 4)
        {
            nextbike.SetTrigger("right3");
            bike1.enabled = false;
            text.text = "0";
            Slader.SetActive(false);
            Slader2.SetActive(true);
            Slader3.SetActive(false);
            Slader4.SetActive(true);
            atap++;
            list[4].color = color2;
            list[3].color = Color1;
            list[2].color = Color1;
            list[1].color = Color1;
            list[0].color = Color1;
        }
        if (atap == 3)
        {
            nextbike.SetTrigger("right2");
            bike1.enabled = false;
            text.text = "0";
            Slader.SetActive(false);
            Slader2.SetActive(false);
            Slader3.SetActive(true);
            atap++;
            list[4].color = Color1;
            list[3].color = color2;
            list[2].color = Color1;
            list[1].color = Color1;
            list[0].color = Color1;
        }
        if (atap == 2)
        {
            nextbike.SetTrigger("Right");
            bike1.enabled = false;
            text.text = "0";
            Slader.SetActive(false);
            Slader2.SetActive(true);
            atap++;
            list[4].color = Color1;
            list[3].color = Color1;
            list[2].color = color2;
            list[1].color = Color1;
            list[0].color = Color1;
        }
        if (atap ==  1)
        {
            nextbike.SetTrigger("right");
            bike1.enabled = false;
            text.text = "0";
            Slader.SetActive(true);
            atap++;
            list[4].color = Color1;
            list[3].color = Color1;
            list[1].color = color2;
            list[2].color = Color1;
            list[0].color = Color1;
            // ISanim = true;
        }
    }

    public void Left()
    {
        if (atap == 2)
        {
            nextbike.SetTrigger("left");
            bike1.enabled = true;
            Slader.SetActive(false);
            Slader2.SetActive(false);
            Slader3.SetActive(false);
            Slader4.SetActive(false);
            atap--;
            list[4].color = Color1;
            list[0].color = color2;
            list[2].color = Color1;
            list[1].color = Color1;
            list[3].color = Color1;
            //  ISanim = false;
        }

        if(atap == 3)
        {
            nextbike.SetTrigger("Left");
            bike1.enabled = false;
            text.text = "0";
            Slader.SetActive(true);
            Slader2.SetActive(false);
            Slader3.SetActive(false);
            Slader4.SetActive(false);
            atap--;
            list[4].color = Color1;
            list[3].color = Color1;
            list[1].color = color2;
            list[2].color = Color1;
            list[0].color = Color1;
        }

        if (atap == 4)
        {
            nextbike.SetTrigger("left2");
            bike1.enabled = false;
            text.text = "0";
            Slader.SetActive(false);
            Slader2.SetActive(true);
            Slader3.SetActive(false);
            Slader4.SetActive(false);
            atap--;
            list[4].color = Color1;
            list[3].color = Color1;
            list[2].color = color2;
            list[1].color = Color1;
            list[0].color = Color1;
        }
        if (atap == 5)
        {
            nextbike.SetTrigger("left3");
            bike1.enabled = false;
            text.text = "0";
            Slader.SetActive(false);
            Slader2.SetActive(false);
            Slader3.SetActive(true);
            Slader4.SetActive(false);
            atap--;
            list[4].color = Color1;
            list[2].color = Color1;
            list[3].color = color2;
            list[1].color = Color1;
            list[0].color = Color1;
        }
    }
    public void newTacka(int i)
    {
        if (CoinManager.Instance.GetCoins() >= allMoney[i] && (CoinManager.Instance.GetBike()[i] !=1))
        {
            int[] nInt = new int[] { CoinManager.Instance.GetBike()[0], 1, CoinManager.Instance.GetBike()[2], CoinManager.Instance.GetBike()[3], CoinManager.Instance.GetBike()[4] };
            nInt[i] = 1;
            CoinManager.Instance.AddBike(nInt);
            CoinManager.Instance.AddCoins(-allMoney[i]);
            CoinManager.Instance.AddISBikeEsc(i);
            madific.SetActive(false);
            UpdateAllTexts();
        }
        else if (CoinManager.Instance.GetBike()[i] == 1 && CoinManager.Instance.GetBikeEsc() != i)
        {
            CoinManager.Instance.AddISBikeEsc(i);
            madific.SetActive(false);
            UpdateAllTexts();
        }
        else
        {
            noCoin.SetTrigger("noCoin");
        }
    }

    private void UpdateAllTexts()
    {
        int[] isBikePurchased = CoinManager.Instance.GetBike();
        int isBikeEquipped = CoinManager.Instance.GetBikeEsc();

        SetTransportTexts(isBikePurchased, isBikeEquipped);

        // ќбновл€ем состо€ние madific
        bool isDefaultTransportEquipped = isBikeEquipped == 0;
        madific.SetActive(isDefaultTransportEquipped);
    }

    public void oldbike()
    {
        madific.SetActive(true);
        UpdateAllTexts();
    }
}