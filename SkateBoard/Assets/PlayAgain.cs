using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayAgain : MonoBehaviour
{
    public Button button;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(Restart);
    }

    // Update is called once per frame
    public void Restart()
    {
        SceneManager.LoadScene("Skateboard_Level2");
    }
}
