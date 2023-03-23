using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarItem : MonoBehaviour
{
    [field: SerializeField]
    public int Price { get; private set; }    
    [field: SerializeField] 
    public bool isBought { get; private set; }
    [field: SerializeField, Range(0f, 5f)]
    public int Level { get; private set; }
    [SerializeField] private List<int> updValue;
    private int maxCarLevel = 5;

    private void Awake()
    {
        LoadData();
    }
    private void SaveData()
    {
        PlayerPrefs.SetInt($"{name}Level", Level);
        int bought = (isBought) ? 1 : 0;
        PlayerPrefs.SetInt($"{name}IsBought", bought);
    }
    private void LoadData()
    {
        if(PlayerPrefs.HasKey($"{name}Level"))
        {
            Level = PlayerPrefs.GetInt($"{name}Level");
            if(Level > maxCarLevel) Level = maxCarLevel;
        }
        if (PlayerPrefs.HasKey($"{name}IsBought"))
        {
            int bouth = PlayerPrefs.GetInt($"{name}IsBought");
            isBought = (bouth == 0) ? false : true;           
        }
    }
    public void Buy() 
    { 
        isBought = true; 
        SaveData();
    }
    public void UpdCar()
    {
        Level = (Level + 1 > maxCarLevel) ? maxCarLevel : Level + 1;
        SaveData();
    }
    public float getUpdValue()
    {
        return (Level < maxCarLevel) ? updValue[Level] : Mathf.Infinity;
    }
}
