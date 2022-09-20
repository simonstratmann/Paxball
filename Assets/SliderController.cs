using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using DefaultNamespace;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{
    public Slider slider;
    public string textPrefix;
    public TMP_Text text;
    public FloatValue floatValue;
    public UnityEvent<float> onUpdate;

    // Start is called before the first frame update
    void Start()
    {
        var sliderValue = floatValue.Invoke();
        Debug.Log(sliderValue);
        slider.value = sliderValue;
        UpdateText(sliderValue);
        slider.onValueChanged.AddListener(x =>
        {
            UpdateText(x);
            onUpdate.Invoke(x);
        });
    }

    private void UpdateText(float sliderValue)
    {
        text.SetText(textPrefix + " (" + sliderValue + ")");
    }

    // Update is called once per frame
    void Update()
    {
    }
    
    [Serializable]
    public class FloatValue : SerializableCallback<float> {}
}