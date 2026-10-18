using UnityEngine;
using UnityEngine.UI;
using TMPro;



namespace YG.Example
{
	public class LanguageExample : MonoBehaviour
	{
		[SerializeField] string ru;
		[SerializeField] string en;

		TMP_Text textObj;
	    Text textObj2;
		bool tipCommponent;

		private void Awake()
		{
			tipCommponent = GetComponent<TMP_Text>() != null;
			if (tipCommponent)
				textObj = GetComponent<TMP_Text>();
			else
				textObj2 = GetComponent<Text>();
			SwitchLanguage(YandexGame.savesData.language);
		}

		private void OnEnable() => YandexGame.SwitchLangEvent += SwitchLanguage;
		private void OnDisable() => YandexGame.SwitchLangEvent -= SwitchLanguage;

		public void SwitchLanguage(string lang)
		{
			switch (lang)
			{
				case "ru":
					if (tipCommponent)
						textObj.text = ru;
					else
						textObj2.text = ru;
					break;
				default:
					if (tipCommponent)
						textObj.text = en;
					else
						textObj2.text = en;
					break;
			}
		}
	}
}