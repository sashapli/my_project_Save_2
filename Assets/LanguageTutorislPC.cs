using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using YG;
using TMPro;

public class LanguageTutorislPC : MonoBehaviour
{
	public TMP_Text textObj;
	private void OnEnable() => SwitchLanguage(YandexGame.savesData.language);

	public void SwitchLanguage(string lang)
	{
		switch (lang)
		{
			case "ru":
				textObj.text = CoinManager.Instance.Getleft()+" / " +CoinManager.Instance.Getright()+" вращение, газ "+CoinManager.Instance.Getgas()+", тормоз "+CoinManager.Instance.Getbrak()+",  настройки TAB";
				break;
			default:
				textObj.text = CoinManager.Instance.Getleft() + " / " + CoinManager.Instance.Getright() + " rotation, gas " + CoinManager.Instance.Getgas() + ", brake " + CoinManager.Instance.Getbrak() + ",  settings TAB"; 
				break;
		}
	}
}
