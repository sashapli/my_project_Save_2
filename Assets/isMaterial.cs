using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class isMaterial : MonoBehaviour
{
    public Image imageMat;
    public Toggle Toggle;
    public Material norm;
    public Material Static;

    // Update is called once per frame
    void Update()
    {
        if (Toggle.isOn)
        {
            imageMat.material = Static;
        }
        else
        {
            imageMat.material = norm;
        }
    }
}
