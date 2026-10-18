using UnityEngine;

public class RainManager : MonoBehaviour
{
	public float rainTreshold;

	public float maxEmissionRate;

	public AudioSource audioSource;

	public AnimationCurve volumeCurve;

	public ParticleSystem rainCaster;

	private ParticleSystem.EmissionModule rainCasterEmission;

	private float effectsVolume;

    private void Start()
    {
        effectsVolume = PlayerPrefs.GetFloat("effectsVolume");

        if (rainCaster == null)
        {
            Debug.LogError("RainManager: rainCaster не назначен в инспекторе!");
            enabled = false;
            return;
        }

        rainCasterEmission = rainCaster.emission;
        audioSource.Play();
    }

    public void Update()
    {
        if (WeatherManager.instance == null || rainCaster == null)
            return;

        float tWeather = WeatherManager.instance.tWeather;
        float rainLevel = WeatherManager.instance.interpolatedWeatherType.rainLevel;

        if (rainLevel > rainTreshold)
        {
            rainCasterEmission.enabled = true;
        }
        else
        {
            rainCasterEmission.enabled = false;
        }

        rainCasterEmission.rateOverTime = Mathf.Lerp(0f, maxEmissionRate, (rainLevel - rainTreshold) / (1f - rainTreshold));

        float num = volumeCurve.Evaluate(rainLevel);
        audioSource.volume = num * effectsVolume;
    }
}
