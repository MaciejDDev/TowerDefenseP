using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GoldManager : MonoBehaviour
{

    [SerializeField] float _initialGoldAmount;
    [SerializeField] float _goldPerSecond;


    [SerializeField] TMP_Text _goldText;

    float _goldAmount;

    public static GoldManager Instance { get; private set; }

    public void Init()
    {
        Instance = this;
        SetInitialGoldAmount();
    }

    public void SetInitialGoldAmount()
    {
        _goldAmount = _initialGoldAmount;
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.Instance.IsGamePaused)
            return;
        _goldAmount += _goldPerSecond * Time.deltaTime;
        int amount = (int) _goldAmount;
        _goldText.text = amount.ToString();
    }


    public int GetCurrentGold()
    {
        return (int) _goldAmount;
    }

    public void SpendGold(int amount)
    {
        _goldAmount -= amount;
    }

    public void AddReward(int amount)
    {
        _goldAmount += amount;
    }
}
