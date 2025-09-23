using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("�ؿ�����")]
    public int currentLevel = 1; // ��ǰ�ؿ���1-3��
    public float levelDuration = 60f; // ÿ�س���ʱ�䣨�룩
    public int maxLevels = 3; // ���ؿ���

    [Header("UI����")]
    public GameObject upgradePanel; // �������
    public GameObject gameOverPanel; // ����/ͨ�����
    public Text gameOverText; // ����/ͨ���ı�
    public Text totalKillsText; // ��ʾ�ܻ�ɱ��

    [Header("���������ÿ�ص�����")]
    public float[] spawnRates = { 3f, 2f, 1f }; // ���ɼ�����룩����1=3s, ��2=2s, ��3=1s
    public int[] enemyHealthMultipliers = { 1, 2, 3 }; // Ѫ������
    public int[] enemyAttackMultipliers = { 1, 2, 3 }; // ��������
    public float[] enemySpeedMultipliers = { 1f, 1.5f, 1.8f };

    private MyPlayerHealth playerHealth;
    private KillCounter killCounter;
    private MyEnemyManager enemyManager;
    private float levelTimer;
    private bool isLevelActive = true;
    private int totalKills; // �ܻ�ɱ�����̳У�


    private void Awake()
    {
        // ��ʼ������
        playerHealth = FindObjectOfType<MyPlayerHealth>();
        killCounter = FindObjectOfType<KillCounter>();
        enemyManager = FindObjectOfType<MyEnemyManager>();
        if (playerHealth == null) Debug.LogError("MyPlayerHealth δ�ҵ���");
        if (killCounter == null) Debug.LogError("KillCounter δ�ҵ���");
        if (enemyManager == null) Debug.LogError("MyEnemyManager δ�ҵ���");
        if (upgradePanel == null) Debug.LogError("UpgradePanel δ���䣡");
        if (gameOverPanel == null) Debug.LogError("GameOverPanel δ���䣡");
        if (gameOverText == null) Debug.LogError("GameOverText δ���䣡");
        if (totalKillsText == null) Debug.LogError("TotalKillsText δ���䣡");

        // ����UI���
        upgradePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // ���ؼ̳����ݣ���ɱ����
        totalKills = PlayerPrefs.GetInt("TotalKills", 0);
        killCounter.LoadKills(); // ��PlayerPrefs���ػ�ɱ��

        // �����¼�
        playerHealth.OnPlayerDeath += OnPlayerDeath;

        // ��ʼ����һ��
        StartLevel(1);
    }

    private void Update()
    {
        if (!isLevelActive) return;

        levelTimer -= Time.deltaTime;
        if (levelTimer <= 0f)
        {
            LevelComplete();
        }
    }
    public float GetLevelTimer() => levelTimer;


    public void StartLevel(int level)
    {
        currentLevel = level;
        levelTimer = levelDuration;
        isLevelActive = true;

        // ����������ֵ
        AdjustEnemyStats(level);

        Debug.Log($"��ʼ�ؿ� {level}");
    }

    private void AdjustEnemyStats(int level)
    {
        if (enemyManager != null)
        {
            float spawnRate = spawnRates[Mathf.Clamp(level - 1, 0, spawnRates.Length - 1)];
            enemyManager.SetSpawnRate(spawnRate); // �����޸ĺ�ķ���
        }
    }

    public int GetEnemyHealthMultiplier() => enemyHealthMultipliers[Mathf.Clamp(currentLevel - 1, 0, enemyHealthMultipliers.Length - 1)];
    public int GetEnemyAttackMultiplier() => enemyAttackMultipliers[Mathf.Clamp(currentLevel - 1, 0, enemyAttackMultipliers.Length - 1)];
    public float GetEnemySpeedMultiplier() => enemySpeedMultipliers[Mathf.Clamp(currentLevel - 1, 0, enemySpeedMultipliers.Length - 1)];

    private void LevelComplete()
    {
        isLevelActive = false;
        Time.timeScale = 0f; // ��ͣ

        if (currentLevel < maxLevels)
        {
            //��ʾ�������
            upgradePanel.SetActive(true);
        }
        else
        {
            EndGame(true);
        }
    }

    public void UpgradeAttack()
    {
        var playerShooting = FindObjectOfType<MyPlayerShooting>();
        if (playerShooting == null)
        {
            Debug.LogError("MyPlayerShooting δ�ҵ���");
            return;
        }
        playerHealth.AddMaxHealth(0);
        playerShooting.AddDamage(25);
        Debug.Log("Upgraded attack by 25!");
        ProceedToNextLevel();
    }

    public void UpgradeHealth()
    {
        if (playerHealth == null)
        {
            Debug.LogError("MyPlayerHealth δ�ҵ���");
            return;
        }
        playerHealth.AddMaxHealth(50);
        FindObjectOfType<MyPlayerShooting>().AddDamage(0);
        Debug.Log("Upgraded health by 50!");
        ProceedToNextLevel();
    }

    private void ProceedToNextLevel()
    {
        upgradePanel.SetActive(false);
        Time.timeScale = 1f;
        killCounter.SaveKills();

        //�����ɱ��
        totalKills = KillCounter.KillScoresPink + KillCounter.KillScoresBlue + KillCounter.KillScoresYellow;
        PlayerPrefs.SetInt("TotalKills", totalKills);
        PlayerPrefs.Save();

        //���õ�������
        enemyManager.ResetSpawn();

        StartLevel(currentLevel + 1);
    }

    private void OnPlayerDeath()
    {
        isLevelActive = false;
        EndGame(false);
    }

    private void EndGame(bool isVictory)
    {
        Time.timeScale = 0f;
        if (gameOverPanel == null) Debug.LogError("GameOverPanel is null!");
        else gameOverPanel.SetActive(true);

        totalKills = KillCounter.KillScoresPink + KillCounter.KillScoresBlue + KillCounter.KillScoresYellow;
        if (isVictory)
        {
            gameOverText.text = $"ͨ����Ϸ�������� {totalKills} �����ˣ����� PINK {KillCounter.KillScoresPink}, BLUE {KillCounter.KillScoresBlue}, YELLOW {KillCounter.KillScoresYellow}";
        }
        else
        {
            float survivedTime = levelDuration - levelTimer; // ���ʱ�䣨�룩
            int minutes = Mathf.FloorToInt(survivedTime / 60);
            int seconds = Mathf.FloorToInt(survivedTime % 60);
            gameOverText.text = $"������� {minutes}m {seconds}s�������� {totalKills} ������";
        }
        totalKillsText.text = $"�ܻ�ɱ: PINK {KillCounter.KillScoresPink}, BLUE {KillCounter.KillScoresBlue}, YELLOW {KillCounter.KillScoresYellow}";
    }

    public void RestartGame()
    {
        killCounter.ResetKills();
        Time.timeScale = 1f;
        PlayerPrefs.DeleteAll(); // ��ձ���
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        playerHealth.OnPlayerDeath -= OnPlayerDeath;
    }


}
