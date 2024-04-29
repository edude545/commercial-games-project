using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Slider slider;

    public void SetMaxFear(float fear)
    {
        slider.maxValue = fear;
        slider.value = fear;
    }


    public void SetFear(float fear)
    {
        slider.value = fear;
    }

}
