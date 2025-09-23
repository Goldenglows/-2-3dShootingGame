using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MyPlayerScores : MonoBehaviour
{
    public Text ScoresText;
    public static int Scores = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        ScoresText.text = $"Score:{Scores}";
    }
}
