using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("关卡设置")]
    public int currentLevel = 1; // 当前关卡（1-3）
    public float levelDuration = 60f; // 每关持续时间（秒）
    public int maxLevels = 3; // 最大关卡数

    [Header("UI引用")]
    public GameObject upgradePanel; // 升级面板
    public GameObject gameOverPanel; // 死亡/通关面板
    public Text gameOverText; // 死亡/通关文本
    public Text totalKillsText; // 显示总击杀数

    [Header("怪物调整（每关递增）")]
    public float[] spawnRates = { 3f, 2f, 1f }; // 生成间隔（秒）：关1=3s, 关2=2s, 关3=1s
    public int[] enemyHealthMultipliers = { 1, 2, 3 }; // 血量倍数
    public int[] enemyAttackMultipliers = { 1, 2, 3 }; // 攻击倍数
    public float[] enemySpeedMultipliers = { 1f, 1.5f, 1.8f };

    private MyPlayerHealth playerHealth;
    private KillCounter killCounter;
    private MyEnemyManager enemyManager;
    private float levelTimer;
    private bool isLevelActive = true;
    private int totalKills; // 总击杀数（继承）


    private void Awake()
    {
        // 初始化引用
        playerHealth = FindObjectOfType<MyPlayerHealth>();
        killCounter = FindObjectOfType<KillCounter>();
        enemyManager = FindObjectOfType<MyEnemyManager>();
        if (playerHealth == null) Debug.LogError("MyPlayerHealth 未找到！");
        if (killCounter == null) Debug.LogError("KillCounter 未找到！");
        if (enemyManager == null) Debug.LogError("MyEnemyManager 未找到！");
        if (upgradePanel == null) Debug.LogError("UpgradePanel 未分配！");
        if (gameOverPanel == null) Debug.LogError("GameOverPanel 未分配！");
        if (gameOverText == null) Debug.LogError("GameOverText 未分配！");
        if (totalKillsText == null) Debug.LogError("TotalKillsText 未分配！");

        // 隐藏UI面板
        upgradePanel.SetActive(false);
        gameOverPanel.SetActive(false);

        // 加载继承数据（击杀数）
        totalKills = PlayerPrefs.GetInt("TotalKills", 0);
        killCounter.LoadKills(); // 从PlayerPrefs加载击杀数

        // 订阅事件
        playerHealth.OnPlayerDeath += OnPlayerDeath;

        // 初始化第一关
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

        // 调整怪物数值
        AdjustEnemyStats(level);

        Debug.Log($"开始关卡 {level}");
    }

    private void AdjustEnemyStats(int level)
    {
        if (enemyManager != null)
        {
            float spawnRate = spawnRates[Mathf.Clamp(level - 1, 0, spawnRates.Length - 1)];
            enemyManager.SetSpawnRate(spawnRate); // 调用修改后的方法
        }
    }

    public int GetEnemyHealthMultiplier() => enemyHealthMultipliers[Mathf.Clamp(currentLevel - 1, 0, enemyHealthMultipliers.Length - 1)];
    public int GetEnemyAttackMultiplier() => enemyAttackMultipliers[Mathf.Clamp(currentLevel - 1, 0, enemyAttackMultipliers.Length - 1)];
    public float GetEnemySpeedMultiplier() => enemySpeedMultipliers[Mathf.Clamp(currentLevel - 1, 0, enemySpeedMultipliers.Length - 1)];

    private void LevelComplete()
    {
        isLevelActive = false;
        Time.timeScale = 0f; // 暂停

        if (currentLevel < maxLevels)
        {
            //显示升级面板
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
            Debug.LogError("MyPlayerShooting 未找到！");
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
            Debug.LogError("MyPlayerHealth 未找到！");
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

        //保存击杀数
        totalKills = KillCounter.KillScoresPink + KillCounter.KillScoresBlue + KillCounter.KillScoresYellow;
        PlayerPrefs.SetInt("TotalKills", totalKills);
        PlayerPrefs.Save();

        //重置敌人生成
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
            gameOverText.text = $"通关游戏！击败了 {totalKills} 个敌人，其中 PINK {KillCounter.KillScoresPink}, BLUE {KillCounter.KillScoresBlue}, YELLOW {KillCounter.KillScoresYellow}";
        }
        else
        {
            float survivedTime = levelDuration - levelTimer; // 坚持时间（秒）
            int minutes = Mathf.FloorToInt(survivedTime / 60);
            int seconds = Mathf.FloorToInt(survivedTime % 60);
            gameOverText.text = $"共坚持了 {minutes}m {seconds}s，击败了 {totalKills} 个敌人";
        }
        totalKillsText.text = $"总击杀: PINK {KillCounter.KillScoresPink}, BLUE {KillCounter.KillScoresBlue}, YELLOW {KillCounter.KillScoresYellow}";
    }

    public void RestartGame()
    {
        killCounter.ResetKills();
        Time.timeScale = 1f;
        PlayerPrefs.DeleteAll(); // 清空保存
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnDestroy()
    {
        playerHealth.OnPlayerDeath -= OnPlayerDeath;
    }


}
