using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static UnityEngine.EventSystems.EventTrigger;

[RequireComponent(typeof(LapTimerManager))]
public class GameController : MonoBehaviour
{
    [SerializeField] private TMP_Text timer, info, leaderBoard, rewardInfo;
    [SerializeField] private GameObject finishWind, mobileInput;     
    private CarController Player;
    [SerializeField] private List<EnemyController> Enemies;
    [SerializeField] private List<int> rewards;
    private List<int> makedLaps;
    private Dictionary<string, float> floatFinishTime;
    private Dictionary<string, string> strFinishTime;
    private bool gameStarted = false;
    private LapTimerManager TM;
    [SerializeField] private int lapsCount = 2;
    private int playerPosition = 0;
    private CarLoader carLoader;

    private int findEnemyInd(EnemyController enemy)
    {
        for (int i = 0; i < Enemies.Count; i++)
        {
            if (Enemies[i] == enemy) return i;
        }
        return -1;
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent<CarController>(out var pl))
        {          
            makedLaps[Enemies.Count] += 1;
            if (lapsCount == makedLaps[Enemies.Count])
            {
                info.text = $"Lap: {makedLaps[Enemies.Count]}/{lapsCount}\n{TM.getTimeString()}";
                AllowMovement(false);
                TM.StartGame(false);
                for (int i = 0; i < Enemies.Count; i++)
                {
                    Enemies[i].gameObject.SetActive(false);
                }
                gameStarted = false;               
                floatFinishTime["Player"] = TM.getTime();
                strFinishTime["Player"] = TM.getTimeString();              
                leaderBoard.text = getLeaderBoard();
                getReward(playerPosition);
                finishWind.SetActive(true);

            }           
        }
        else if(other.TryGetComponent<EnemyController>(out var enemy))
        {
            makedLaps[findEnemyInd(enemy)] += 1;
            if (lapsCount == makedLaps[findEnemyInd(enemy)])
            {
                enemy.allowMovement(false);
                Enemies[findEnemyInd(enemy)].gameObject.SetActive(false);
                floatFinishTime[enemy.Name] = TM.getTime();
                strFinishTime[enemy.Name] = TM.getTimeString();
            }           
        }
    }

    private void getReward(int pos)
    {
        pos = (pos > rewards.Count) ? pos = rewards.Count - 1 : pos - 1;
        rewardInfo.text = rewards[pos].ToString();
        if(PlayerPrefs.HasKey("Money"))
        {
            PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") + rewards[pos]);
        }
        else
        {
            PlayerPrefs.SetInt("Money", rewards[pos]);
        }
    }

    private string getLeaderBoard()
    {
        string leaders = "";
        int position = 1;
        while(floatFinishTime.Count > 0)
        {
            float minTime = Mathf.Infinity;
            string name = "";
            foreach (var time in floatFinishTime)
            {
                if(time.Value < minTime && time.Value != 0)
                {
                    minTime = time.Value;
                    name = time.Key;
                    if (name == "Player") playerPosition = position;
                }                
            }
            floatFinishTime.Remove(name);
            leaders += $"{position}. {name}\t{strFinishTime[name]}\n";
            position++;
            if (name == "Player") break;
        }
        foreach (var time in floatFinishTime)
        {
            leaders += $"{position}. {time.Key}\t{strFinishTime[time.Key]}\n";
            position++;
        }
        return leaders;
    }
    private void Awake()
    {
        #if UNITY_IOS || UNITY_ANDROID
        mobileInput.gameObject.SetActive(true);
        #endif
        carLoader = GetComponent<CarLoader>();
        TM = GetComponent<LapTimerManager>();
        makedLaps = new List<int>();
        floatFinishTime = new Dictionary<string, float>();
        strFinishTime = new Dictionary<string, string>();
        foreach (var en in Enemies)
        {
            makedLaps.Add(0);
            floatFinishTime.Add(en.Name, 0);
            strFinishTime.Add(en.Name, "N/F");
        }
        makedLaps.Add(0);
        floatFinishTime.Add("Player", 0);
        strFinishTime.Add("Player", "N/F");
    }
    private void Start()
    {        
        Player = carLoader.getActiveCar();      
        StartCoroutine(InitCountdown());
    }  
    private void Update()
    {
        if(gameStarted)
        {
            info.text = $"Lap: {makedLaps[Enemies.Count]}/{lapsCount}\n{TM.getTimeString()}";
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene(0);
    }
    private void AllowMovement(bool value)
    {
        Player.allowMovement(value);
        foreach (var en in Enemies)
        {
            if(en.gameObject.activeInHierarchy)
                en.allowMovement(value);
        }
    }

    private IEnumerator InitCountdown()
    {
        yield return new WaitForSeconds(1);
        timer.text = "3";
        yield return new WaitForSeconds(1);
        timer.text = "2";
        yield return new WaitForSeconds(1);
        timer.text = "1";
        yield return new WaitForSeconds(1);
        timer.text = "Go";        
        AllowMovement(true);
        TM.StartGame();
        gameStarted = true;
        yield return new WaitForSeconds(1);
        timer.text = "";
    }
}
