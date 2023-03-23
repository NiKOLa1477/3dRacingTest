using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GarageManager : MonoBehaviour
{
    [SerializeField] private List<CarItem> cars;
    [SerializeField] private int Money;
    [SerializeField] private Button Buy, Upd, Go;
    [SerializeField] private GameObject LevelsWind;
    [SerializeField] private TMP_Text moneyText, Cost;
    private int currCar;
    private int maxCarLevel = 5;
    private void Awake()
    {       
        if(PlayerPrefs.HasKey("Money"))
        {           
            Money = PlayerPrefs.GetInt("Money");
        }
        if(PlayerPrefs.HasKey("CarIndex"))
        {
            currCar = PlayerPrefs.GetInt("CarIndex");
        }
        if (currCar >= cars.Count) currCar = cars.Count - 1;
        showCar(true);
        checkBtns();
        displayInfo();
    }

    private void Update()
    {
        #if UNITY_STANDALONE
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
        #endif
    }
    private void displayInfo()
    {
        moneyText.text = "$: " + Money.ToString();
        if(cars[currCar].isBought)
        {
            Cost.text = cars[currCar].getUpdValue().ToString() + "$";
        }
        else
        {
            Cost.text = cars[currCar].Price.ToString() + "$";
        }        
    }
    public void nextCar()
    {
        showCar(false);
        currCar = (currCar + 1 >= cars.Count) ? 0 : currCar + 1;
        showCar(true);
        checkBtns();
        displayInfo();
    }
    public void prevCar()
    {
        showCar(false);
        currCar = (currCar - 1 < 0) ? cars.Count - 1 : currCar - 1;
        showCar(true);
        checkBtns();
        displayInfo();
    }
    private void showCar(bool value)
    {
        cars[currCar].gameObject.SetActive(value);
    }
    private void checkBtns()
    {
        DeactivateAll();
        if (cars[currCar].isBought && cars[currCar].Level < maxCarLevel)
        {
            Go.gameObject.SetActive(true);
            Upd.gameObject.SetActive(true);
            if(Money < cars[currCar].getUpdValue())
            {
                Upd.interactable = false;
            }
        }
        else if (!cars[currCar].isBought)
        {
            Buy.gameObject.SetActive(true);
            if(Money < cars[currCar].Price)
            {
                Buy.interactable = false;
            }
        }
        else
        {
            Go.gameObject.SetActive(true);
        }
    }
    private void DeactivateAll()
    {
        Buy.gameObject.SetActive(false);
        Buy.interactable = true;
        Upd.gameObject.SetActive(false);
        Upd.interactable = true;
        Go.gameObject.SetActive(false);
    }
    public void buyCar()
    {
        Money -= cars[currCar].Price;
        cars[currCar].Buy();
        checkBtns();
        displayInfo();
    }
    public void updCar()
    {
        Money -= (int)cars[currCar].getUpdValue();
        cars[currCar].UpdCar();
        checkBtns();
        displayInfo();
    }
    public void onGo()
    {
        saveData();
        LevelsWind.SetActive(true);
    }
    private void saveData()
    {
        PlayerPrefs.SetInt("CarIndex", currCar);
        PlayerPrefs.SetInt("CarLevel", cars[currCar].Level);
    }
    public void loadLevel(int index)
    {
        SceneManager.LoadScene(index);
    }
}
