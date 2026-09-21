using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
public class HPSlider : MonoBehaviour
{
     public Slider slider;
     public int SliderHP;
     public int MaxHP;

     public void Start()
     {
         SliderHP = MaxHP;
         slider.value = 1;
     }

     




}
