using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class madificatorVid : MonoBehaviour
{
    [Range(0, 2)]
    public int bakegears;
    [Range(0, 2)]
    public int Forwardgears;
    public bool ISArm;
    public bool ISWheel;


    public GameObject bakegearsImg;
    public GameObject ForwardgearsImg;
    public GameObject chain;
    public SpriteRenderer bakegearsImge;

    public GameObject[] jointBake;
    public GameObject palca;
    public GameObject rearWheel;
    public GameObject frontWheel;

    public Vector3[] gears;
    public Vector2[] forwardJointPos;
    public Vector2 palcaSdi;
    public Sprite[] gearsSplite;
    public Sprite WheelNor;
    public Sprite WheelDraiv;
    public SpriteRenderer WheelL;
    public SpriteRenderer WheelR;
    public int Id;
    public Scrollbar SpeedScroll;
    public Scrollbar aerScroll;
    public Scrollbar AucScroll;
    public Scrollbar SpeedScrollN;
    public Scrollbar aerScrollN;
    public Scrollbar AucScrollN;
    public TMP_Text coin;
    public Animator animator;

    private void Start()
    {
        bakegears = CoinManager.Instance.Getbakegears();
        Forwardgears = CoinManager.Instance.GetForwardgears();
        ISWheel = CoinManager.Instance.GetISWheel();
        ISArm = CoinManager.Instance.GetISArm();
        Debug.Log(CoinManager.Instance.GetForwardgears());
        Forwardgears = Forwardgears > 2 ? 2 : Forwardgears;
        bakegears = bakegears > 2 ? 2 : bakegears;
        if (ISArm)
        {
            jointBake[0].SetActive(true);
            jointBake[1].SetActive(true);
            jointBake[2].SetActive(true);
            aerScrollN.size = aerScrollN.size + 0.5f;
            palca.transform.localScale = new Vector3(1, 1, 1);
        }
        if (ISWheel)
        {
            WheelL.sprite = WheelDraiv;
            WheelR.sprite = WheelDraiv;
            SpeedScrollN.size = SpeedScrollN.size + 0.05f;
            AucScrollN.size = SpeedScrollN.size + 0.2f;
        }
        SpeedScrollN.size = SpeedScrollN.size + (0.1f * bakegears);
        SpeedScrollN.size = SpeedScrollN.size + (0.1f *Forwardgears);
    }
    private void Update()
    {
        bakegearsImg.transform.localScale = new Vector3(1 - 0.15f * bakegears, 1 - 0.15f * bakegears, 1);
        ForwardgearsImg.transform.localScale = new Vector3(1 + 0.3f * Forwardgears, 1 + 0.3f * Forwardgears, 1);
        bakegearsImge = chain.GetComponent<SpriteRenderer>();
        bakegearsImge.sprite = gearsSplite[Forwardgears];
        switch (Id)
        {
            case 3: if (bakegears != 2) { SpeedScroll.size = SpeedScrollN.size + 0.1f; } break;
            case 4: if (Forwardgears != 2) { SpeedScroll.size = SpeedScrollN.size + 0.1f; } break;
        }
    }

    public void colosa()
    {
        if (!ISWheel)
        {
            Id = 1;
            aerScroll.size = aerScrollN.size;
            SpeedScroll.size = aerScrollN.size;
            AucScroll.size = AucScrollN.size;
            SpeedScroll.size = SpeedScrollN.size + 0.05f; AucScroll.size = AucScroll.size + 0.2f;
            coin.text = "2000";
        }
        else
        {
            coin.text = "Max";
            aerScroll.size = aerScrollN.size;
            SpeedScroll.size = SpeedScrollN.size;
            AucScroll.size = AucScrollN.size;
        }

    }
    public void Arm()
    {
        if (!ISArm)
        {
            Id = 2;
            aerScroll.size = aerScrollN.size;
            SpeedScroll.size = SpeedScrollN.size;
            AucScroll.size = AucScrollN.size;
            aerScroll.size = aerScrollN.size + 0.5f;
            coin.text = "3000";
        }
        else
        {
            coin.text = "Max";
            aerScroll.size = aerScrollN.size;
            SpeedScroll.size = SpeedScrollN.size;
            AucScroll.size = AucScrollN.size;
        }


    }
    public void Bake()
    {
        if (bakegears < 2)
        {
            Id = 3;
            aerScroll.size = aerScrollN.size;
            SpeedScroll.size = SpeedScrollN.size;
            AucScroll.size = AucScrollN.size;
            SpeedScroll.size = SpeedScrollN.size + 0.1f;
            coin.text = "500";
        }
        else
        {
            coin.text = "Max";
            aerScroll.size = aerScrollN.size;
            SpeedScroll.size = SpeedScrollN.size;
            AucScroll.size = AucScrollN.size;
        }
    }
    public void Forward()
    {
        if (Forwardgears < 2)
        {
            Id = 4;
            aerScroll.size = aerScrollN.size;
            SpeedScroll.size = SpeedScrollN.size;
            AucScroll.size = AucScrollN.size;
            SpeedScroll.size = SpeedScrollN.size + 0.1f;
            coin.text = "500";
        }
        else
        {
            coin.text = "Max";
            aerScroll.size = aerScrollN.size;
            SpeedScroll.size = SpeedScrollN.size;
            AucScroll.size = AucScrollN.size;
        }
    }
    public void Cupit()
    {
        switch (Id)
        {
            case 1:if (CoinManager.Instance.GetCoins() >= 2000 && !ISWheel)
                {
                    WheelL.sprite = WheelDraiv;
                    WheelR.sprite = WheelDraiv;
                    SpeedScrollN.size = SpeedScroll.size;
                    AucScrollN.size = AucScroll.size;
                    CoinManager.Instance.AddISWheel(true);
                    ISWheel = true;
                    CoinManager.Instance.AddCoins(-2000);
                }
                else { animator.SetTrigger("No"); Debug.Log("aaaaaa"); }
                break;

            case 2:
              if  (CoinManager.Instance.GetCoins() >= 3000 && !ISArm)
                {
                    jointBake[0].SetActive(true);
                    jointBake[1].SetActive(true);
                    jointBake[2].SetActive(true);
                    aerScrollN.size = aerScroll.size;
                    palca.transform.localScale = new Vector3(1, 1, 1);
                    CoinManager.Instance.AddISArm(true);
                    ISArm = true; CoinManager.Instance.AddCoins(-3000);
                }
                else { animator.SetTrigger("No"); Debug.Log("aaaaaa"); ; }
                break;
            case 3:if (bakegears != 2 && CoinManager.Instance.GetCoins() >= 500) { CoinManager.Instance.AddCoins(-500); bakegears++; SpeedScrollN.size = SpeedScroll.size; CoinManager.Instance.Addbakegears(1); }
                else { animator.SetTrigger("No"); Debug.Log("aaaaaa");  }
                break;
            case 4: if (Forwardgears != 2 && CoinManager.Instance.GetCoins() >= 500) { CoinManager.Instance.AddCoins(-500); Forwardgears++; SpeedScrollN.size = SpeedScroll.size; CoinManager.Instance.AddForwardgears(1); }
                else { animator.SetTrigger("No"); Debug.Log("aaaaaa"); } break;
        }
   }        
    public void BakeScen()
    {
        SceneManager.LoadScene("Minu");
    }
}
