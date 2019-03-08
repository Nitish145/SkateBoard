using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public Rigidbody rb;
    public Text ScoreText;
    public Text txt;
    private float timer = 0;
    private static int score;
    private float v;
    private int i = 0;
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        i++;
        v += rb.velocity.magnitude;
        timer += Time.deltaTime;
        score = ((int)(v*timer)/i);
        ScoreText.text = score.ToString();
        txt.text = "Your Score : " + score.ToString();
    }
    
}
