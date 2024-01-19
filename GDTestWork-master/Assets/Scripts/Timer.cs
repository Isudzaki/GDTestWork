using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Timer : MonoBehaviour
{
    public static Timer instance;

    [HideInInspector] public float maTime;
    private float timeLeft;

    private Image timerBar;


    private void Awake()
    {
        instance = this;
        gameObject.SetActive(false);
    }
 
    void Start()
    {
        timerBar = GetComponent<Image>();
        timeLeft = maTime;
    }
    void Update()
    {
        if(timeLeft > 0)
        {
            timeLeft -= Time.deltaTime;
            timerBar.fillAmount = timeLeft / maTime;
        } else
        {
            Player.instance.timerEnd = true;
            gameObject.SetActive(false);
            timeLeft = maTime;
        }
    }
}
