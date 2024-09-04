using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.SmartFormat.Extensions;
using UnityEngine.Localization.SmartFormat.GlobalVariables;

public class HistoryUI : MonoBehaviour
{
    public string[] names = { };

    public TextMeshProUGUI playTimeText;

    private PersistentVariablesSource source; 

    void Start()
    {
        source = LocalizationSettings.StringDatabase.SmartFormatter.GetSourceExtension<PersistentVariablesSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChangeText()
    {
        //source = LocalizationSettings.StringDatabase.SmartFormatter.GetSourceExtension<PersistentVariablesSource>();
        var myFloat = source["global"]["my-float"] as UnityEngine.Localization.SmartFormat.PersistentVariables.FloatVariable;
        myFloat.Value = 123;
    }

}
