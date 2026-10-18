using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using UnityEngine.UI;
using System.Runtime.InteropServices;
public class kekPad : MonoBehaviour
{
    public List<char> key;
    public List<char> key2;
    public GameObject[] buttonKey;
    public RectTransform pad;
    public CanvasGroup keyDeform;

    public TMP_Text rig, lef, gas, bak;
    public bool flagClik;

    private Vector2 startPosition;
    public int curetTab;
    public bool isTutarial;
    public char[] gamebuton;
    public Color[] colorKey;

    private void Start()
    {
        if (!isTutarial)
        {
            startPosition = pad.transform.localPosition;
            rig.text = "" + CoinManager.Instance.Getright();
            lef.text = "" + CoinManager.Instance.Getleft();
            gas.text = "" + CoinManager.Instance.Getgas();
            bak.text = "" + CoinManager.Instance.Getbrak();
        }
        gamebuton = new char[] { CoinManager.Instance.Getgas(), CoinManager.Instance.Getbrak(), CoinManager.Instance.Getright(), CoinManager.Instance.Getleft() };

        if (isTutarial)
        {
            for(int i = 0; i < gamebuton.Length;i++)
            {
                int index = key.IndexOf(gamebuton[i]);
                buttonKey[index].GetComponent<Image>().color = colorKey[i];
                buttonKey[index].GetComponentInChildren<TMP_Text>().color = colorKey[i];
            }
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown && !isTutarial) { 
            string input = Input.inputString;
            if (!string.IsNullOrEmpty(input))
            {
                char c = input[0]; // берЄм первый символ
                Debug.Log(c);
                if (!flagClik)
                {
                    flagClik = true;
                    int index = key2.Contains(c) ? key2.IndexOf(c) : key.IndexOf(c);
                    if (index < 0) { flagClik = false; return; }   // символа нет Ч выходим
                    if (index >= buttonKey.Length) { flagClik = false; return; }
                    buttonKey[index].GetComponent<RectTransform>().DOScale(0.61f, 0.1f).SetLoops(2, LoopType.Yoyo).OnComplete(() => buttonKey[index].transform.localScale = Vector3.one);
                    if (keyDeform.gameObject.activeSelf)
                    {
                        switch (curetTab)
                        {
                            case 0: CoinManager.Instance.AddCharRight(key[index]); rig.text = "" + CoinManager.Instance.Getright(); break;
                            case 1: CoinManager.Instance.AddCharLeft(key[index]); lef.text = "" + CoinManager.Instance.Getleft(); break;
                            case 2: CoinManager.Instance.AddCharGas(key[index]); gas.text = "" + CoinManager.Instance.Getgas(); break;
                            case 3: CoinManager.Instance.AddCharbrak(key[index]); bak.text = "" + CoinManager.Instance.Getbrak(); break;
                        }
                        StartCoroutine(aim(c));
                    }
                    else
                        flagClik = false;
                }
            }
        }
    }

    IEnumerator aim(char c)
    {
        yield return new WaitForSeconds(0.5f);
        if (keyDeform.gameObject.activeSelf)
        {
            pad.DOAnchorPos(startPosition, 0.5f);
            pad.DOScale(0.72f, 0.5f);
            keyDeform.DOFade(0, 0.5f).OnComplete(() => { keyDeform.gameObject.SetActive(false); flagClik = false; })  ;
        }
    }

    public void OnClik(int o)
    {
        if (flagClik) return;
        pad.DOAnchorPos(Vector2.zero, 0.5f);
        pad.DOScale(1, 0.5f);
        keyDeform.DOFade(1, 0.5f);
        keyDeform.gameObject.SetActive(true);
        curetTab = o;
    }
}
