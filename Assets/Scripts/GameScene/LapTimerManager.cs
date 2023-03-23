using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LapTimerManager : MonoBehaviour
{
    private float totalMiliseconds;
    private int minutes, seconds;
    private float miliseconds;
    private bool gameStarted;

    public string getTimeString()
    {
        string time = "";
        time += (minutes < 10) ? "0" + minutes : minutes;
        time += ":";
        time += (seconds < 10) ? "0" + seconds : seconds;
        time += "." + miliseconds.ToString("F0");        
        return time;
    }
    public float getTime()
    {
        return totalMiliseconds;
    }    
    public void StartGame(bool value = true) { gameStarted = value; }
    private void Update()
    {
        if(gameStarted)
        {
            miliseconds += Time.deltaTime * 10;
            if(miliseconds >= 10)
            {
                seconds += 1;
                totalMiliseconds += 10;
                miliseconds = 0;
            }
            if(seconds >= 60)
            {
                minutes += 1;
                totalMiliseconds += 600;
                seconds = 0;
            }
        }
    }

}
