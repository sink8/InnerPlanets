using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class UIScripts : MonoBehaviour
{

    public TMP_Text findStuffText;


    void Start()
    {
        //findStuffText = GetComponent<TextMeshProUGUI>();
        findStuffText.text = "Take it ??";
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EnableText()
    {
        findStuffText.enabled = true;
    }

    public void DisableText()
    {
        findStuffText.enabled = false;
    }
}
