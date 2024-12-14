using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class textScript : MonoBehaviour
{
    // Start is called before the first frame update
    public Text text;
    void Start()
    {
        text = GetComponent<Text>();
    }

    // Update is called once per frame
    float timer;
    void Update()
    {
        timer += Time.deltaTime;
        text.text = timer.ToString("F2");
    }
}
