using UnityEngine;

public class ContentPanelsManager : MonoBehaviour
{
	public ContentPanel defaultContentPanel;

	public ContentPanel pausePanel;

	public AudioClip buttonSound;

	private AudioSource uiAudio;

	public AudioSource newLevelAudio;

	private ContentPanel[] contentPanels;

	private ContentPanel currentCP;

	private static ContentPanelsManager _instance;

	public static ContentPanelsManager instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = Object.FindObjectOfType<ContentPanelsManager>();
			}
			return _instance;
		}
	}

	private void Start()
	{
		PersistentData.notStart = true;
		uiAudio = GetComponent<AudioSource>();
		uiAudio.clip = buttonSound;
		contentPanels = GetComponentsInChildren<ContentPanel>();
		if (ScoreManager.instance == null || ScoreManager.instance.runCount != 0)
		{
			ChangeContentPanel(defaultContentPanel);
		}
	}

	public void ChangeContentPanel(ContentPanel _targetCP)
	{
		if (currentCP != null)
		{
			currentCP.Exit();
		}
		_targetCP.Enter();
		currentCP = _targetCP;
	}

	public void CloseCurrentContentPanel()
	{
		if (currentCP != null)
		{
			currentCP.Exit();
		}
	}

	public void OpenContentPanel(ContentPanel _cp)
	{
		if (currentCP != null)
		{
			currentCP.Exit();
		}
		currentCP = _cp;
		currentCP.Enter();
	}

	public void PlayerButtonSound()
	{
		uiAudio.clip = buttonSound;
		uiAudio.Play();
	}

	public void PlayNewLevelSound()
	{
		newLevelAudio.clip = ScoreManager.instance.newLevelAudioClip;
		newLevelAudio.Play();
	}

	public void OpenURL(string link)
	{
		Application.OpenURL(link);
	}
}
