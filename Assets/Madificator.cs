using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Madificator : MonoBehaviour
{
  /*  [Range(0,2)]
    public int bakegears;
    [Range(0, 2)]
    public int Forwardgears;
    public bool ISArm;
    public bool ISWheel;
    */
    public GameObject bakegearsImg;
    public GameObject ForwardgearsImg;
    public GameObject chain;
    public SliderJoint2D jointForwar;
    public HingeJoint2D hinge;
    public SpriteRenderer bakegearsImge;

    public GameObject[] jointBake;
    public GameObject palca;
   // public GameObject rearWheel;
 //   public GameObject frontWheel;

  //  public Vector3[] gears;
    public Vector2[] forwardJointPos;
    public Vector2 palcaSdi;
    public Sprite[] gearsSplite;
    public Sprite WheelNor;
    public Sprite WheelDraiv;
    public SpriteRenderer WheelL;
    public SpriteRenderer WheelR;
    public CycleControl bakeMadic;

    private void Awake()
    {
        // Проверяем, что chain не null
        if (chain != null)
        {
            bakegearsImge = chain.GetComponent<SpriteRenderer>();
        }
        else
        {
            Debug.LogError("Chain reference is not set in the inspector");
            return; // Прерываем выполнение, если chain отсутствует
        }

        // Проверяем, что CoinManager.Instance существует
        if (CoinManager.Instance == null)
        {
            Debug.LogError("CoinManager instance is null");
            return;
        }

        int i = (int)CoinManager.Instance.GetForwardgears();
        i = i >= 2 ? 2 : i;

        // Проверяем, что gearsSplite не null и имеет достаточную длину
        if (gearsSplite != null && i < gearsSplite.Length)
        {
            bakegearsImge.sprite = gearsSplite[i];
        }
        else
        {
            Debug.LogError("GearsSplite array is not properly initialized or index out of range");
        }

        // Проверяем, что jointForwar не null
        if (jointForwar != null)
        {
            if (CoinManager.Instance.GetISArm())
            {
                // Проверяем, что forwardJointPos не null и имеет элементы
                if (forwardJointPos != null && forwardJointPos.Length > 0)
                {
                    jointForwar.connectedAnchor = forwardJointPos[1];
                }
            }
            else
            {
                // Проверяем, что forwardJointPos имеет хотя бы 2 элемента
                if (forwardJointPos != null && forwardJointPos.Length > 1)
                {
                    jointForwar.connectedAnchor = forwardJointPos[0];
                }
            }
        }

        // Проверяем, что WheelL и WheelR не null
        if (WheelL != null && WheelR != null)
        {
            if (CoinManager.Instance.GetISWheel())
            {
                // Проверяем, что WheelDraiv не null
                if (WheelDraiv != null)
                {
                    WheelL.sprite = WheelDraiv;
                    WheelR.sprite = WheelDraiv;
                }

                // Проверяем, что bakeMadic не null
                if (bakeMadic != null)
                {
                    bakeMadic.Spees += 25;
                }
            }
            else
            {
                // Проверяем, что WheelNor не null
                if (WheelNor != null)
                {
                    WheelL.sprite = WheelNor;
                    WheelR.sprite = WheelNor;
                }
            }
        }

        // Проверяем, что bakeMadic не null перед изменением скорости
        if (bakeMadic != null)
        {
            bakeMadic.Spees += 50 * CoinManager.Instance.Getbakegears();
            bakeMadic.Spees += 50 * CoinManager.Instance.GetForwardgears();
        }
    }
    void Start()
    {
        bakegearsImg.transform.localScale = new Vector3(1 - 0.15f * CoinManager.Instance.Getbakegears(), 1 - 0.15f * CoinManager.Instance.Getbakegears(), 1);
    }
    private void Update()
    {
        bakegearsImg.transform.localScale = new Vector3(1 - 0.15f * CoinManager.Instance.Getbakegears(), 1 - 0.15f * CoinManager.Instance.Getbakegears(), 1);

        ForwardgearsImg.transform.localScale = new Vector3(1 + 0.3f * CoinManager.Instance.GetForwardgears(), 1 + 0.3f * CoinManager.Instance.GetForwardgears(), 1);
        if (CoinManager.Instance.GetISArm())
        {
            jointBake[0].SetActive(true);
            jointBake[1].SetActive(true);
            jointBake[2].SetActive(true);
           hinge.enabled = false;
            palca.transform.localScale = Vector3.one;
        }
        else
        {
            jointBake[0].SetActive(false);
            jointBake[1].SetActive(false);
            jointBake[2].SetActive(false);
          hinge.enabled = true;
            palca.transform.localScale = new Vector3(palcaSdi.x, palcaSdi.y, 0);

        }
    }
}
