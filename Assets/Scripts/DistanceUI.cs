using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DistanceUI : MonoBehaviour
{
    public Slider distanceSlider;


    public void UpdateCoinUI()
    {
        distanceSlider.value = CarController.Instance.DistanceTraveled;
    }
}
