using UnityEngine;

public class ObjectColor : MonoBehaviour
{
	private Renderer rend;

	private MaterialPropertyBlock propBlock;

	private int _Color;

	private Color[] resultColors = new Color[2];

	private void Start()
	{
		propBlock = new MaterialPropertyBlock();
		rend = GetComponent<Renderer>();
		rend.GetPropertyBlock(propBlock);
		InvokeRepeating("DefineColor", 0f, Random.Range(0.05f, 0.1f));
		_Color = Shader.PropertyToID("_Color");
	}

    private void DefineColor()
    {
        if (DayTimeManager.instance == null ||
            DayTimeManager.instance.interpolatedDayState == null ||
            DayTimeManager.instance.interpolatedDayState.intState == null ||
            DayTimeManager.instance.dtms == null)
        {
            return;
        }

        float z = base.transform.position.z;
        float time = z / 3200f;

        var dayState = DayTimeManager.instance.interpolatedDayState.intState;
        Color mainColor = dayState.mainColor;
        Color secondColor = dayState.secondColor;

        float H, S, V;
        Color.RGBToHSV(mainColor, out H, out S, out V);

        float t = DayTimeManager.instance.dtms.distanceValueCurve.Evaluate(time);
        V = Mathf.Lerp(0.2f, 0.7f, t);

        if (z == 5f) V = 0.19f;
        if (z < 3f) V = 0.15f;

        if (z > 7f && z < 50f)
        {
            V = dayState.closeLayerValue;
            S /= dayState.closeLayerSaturation;
        }

        float H2, S2, V2;
        Color.RGBToHSV(secondColor, out H2, out S2, out V2);
        V2 = Mathf.Lerp(0.2f, 1f, t);

        mainColor = Color.HSVToRGB(H, S, V);
        secondColor = Color.HSVToRGB(H2, S2, V2);

        time = DayTimeManager.instance.dtms.distanceColorCurve.Evaluate(time);
        resultColors[0] = Color.Lerp(mainColor, secondColor, time);
        resultColors[1] = Color.Lerp(dayState.fogColor, secondColor, time);

        propBlock.SetColor(_Color, resultColors[0]);
        rend.SetPropertyBlock(propBlock);
    }
}
