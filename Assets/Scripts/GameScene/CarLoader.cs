using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(GameController))]
public class CarLoader : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private List<CarController> cars;
    [SerializeField] private int carIndex = 0, carLevel = 0, updValue = 20;
    private int maxLevel = 5;

    private void Awake()
    {
        loadCar();
    }

    private void loadCar()
    {
        if (PlayerPrefs.HasKey("CarIndex"))
        {
            carIndex = PlayerPrefs.GetInt("CarIndex");
        }
        if (PlayerPrefs.HasKey("CarLevel"))
        {
            carLevel = PlayerPrefs.GetInt("CarLevel");
        }
        if (carIndex >= cars.Count) carIndex = cars.Count - 1;
        if (carLevel > maxLevel) carLevel = maxLevel;
        cars[carIndex].updSpeed(carLevel * updValue);
        cars[carIndex].gameObject.SetActive(true);
        camera.LookAt = cars[carIndex].transform;
        camera.Follow = cars[carIndex].transform;
    }

    public CarController getActiveCar()
    {
        return cars[carIndex];
    }
}
