using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public Text globalTimerText;
    public Text levelTimerText;
    public Text coinCountText;

    private float globalTime;
    private float levelTime;
    private int coinCount;

    void Start()
    {
        globalTime = 0f;
        levelTime = 0f;
        coinCount = 0;

        UpdateGlobalTimerText();
        UpdateLevelTimerText();
        UpdateCoinCountText();
    }

    void Update()
    {
        globalTime += Time.deltaTime;
        levelTime += Time.deltaTime;

        UpdateGlobalTimerText();
        UpdateLevelTimerText();
    }

    public void IncreaseCoinCount()
    {
        coinCount++;
        UpdateCoinCountText();
    }

    public void ResetLevelTimer()
    {
        levelTime = 0f;
    }

    private void UpdateGlobalTimerText()
    {
        globalTimerText.text = "Global Time: " + globalTime.ToString("F2");
    }

    private void UpdateLevelTimerText()
    {
        levelTimerText.text = "Level Time: " + levelTime.ToString("F2");
    }

    private void UpdateCoinCountText()
    {
        coinCountText.text = "Coins: " + coinCount;
        coinCountText.color = Color.yellow;
    }
}
