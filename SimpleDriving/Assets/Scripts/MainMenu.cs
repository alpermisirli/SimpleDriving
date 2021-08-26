using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private TMP_Text highScoreText;
    [SerializeField] private TMP_Text energyText;
    [SerializeField] private AndroidNotificationHandler _androidNotificationHandler;
    [SerializeField] private IOSNotificationHandler _iosNotificationHandler;
    [SerializeField] private int maxEnergy;
    [SerializeField] private int energyRechargeMinutes;

    private int energy;

    private const string EnergyKey = "Energy";
    private const string EnergyReadyKey = "EnergyReady";


    private void Start()
    {
        int highScore = PlayerPrefs.GetInt(ScoreSystem.HighScoreKey, 0);
        highScoreText.text = "High Score : " + highScore.ToString(); //or $"High Score {highScore}";
        energy = PlayerPrefs.GetInt(EnergyKey, maxEnergy);

        if (energy == 0)
        {
            string energyReadyString = PlayerPrefs.GetString(EnergyReadyKey, string.Empty);
            if (energyReadyString == String.Empty)
            {
                return;
            }

            DateTime energyReady = DateTime.Parse(energyReadyString);
            if (DateTime.Now > energyReady)
            {
                energy = maxEnergy;
                PlayerPrefs.SetInt(EnergyKey, energy);
            }
        }

        energyText.text = "Play :" + energy;
    }

    public void PlayButton()
    {
        if (energy < 1)
        {
            return;
        }

        energy--;

        PlayerPrefs.SetInt(EnergyKey, energy);

        if (energy == 0)
        {
            DateTime energyReady = DateTime.Now.AddMinutes(energyRechargeMinutes);
            PlayerPrefs.SetString(EnergyReadyKey, energyReady.ToString());
#if UNITY_ANDROID
            _androidNotificationHandler.ScheduleNotification(energyReady);
#elif UNITY_IOS
            _iosNotificationHandler.ScheduleNotification(energyRechargeMinutes);
#endif
        }

        SceneManager.LoadScene(1);
    }
}