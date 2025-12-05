using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KillCounter : MonoBehaviour
{
    [SerializeField] private Text killTextPink; 
    [SerializeField] private Text killTextBlue; 
    [SerializeField] private Text killTextYellow;

    private static int _killScoresPink = 0;
    private static int _killScoresBlue = 0;
    private static int _killScoresYellow = 0;


    public static int KillScoresPink
    {
        get => _killScoresPink;
        set
        {
            _killScoresPink = value;
            Instance?.UpdateText(); // 分数变化时更新
        }
    }

    public static int KillScoresBlue
    {
        get => _killScoresBlue;
        set
        {
            _killScoresBlue = value;
            Instance?.UpdateText();
        }
    }

    public static int KillScoresYellow
    {
        get => _killScoresYellow;
        set
        {
            _killScoresYellow = value;
            Instance?.UpdateText();
        }
    }

    // 单例模式，便于访问
    public static KillCounter Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        // 检查 Text 组件是否分配
        if (killTextPink == null) Debug.LogError("killTextPink 未分配！");
        if (killTextBlue == null) Debug.LogError("killTextBlue 未分配！");
        if (killTextYellow == null) Debug.LogError("killTextGreen 未分配！");
        LoadKills();
        UpdateText();

    }


    private void UpdateText()
    {
        if (killTextPink != null)
            killTextPink.text = $"PINK: {KillScoresPink}";
        if (killTextBlue != null)
            killTextBlue.text = $"BLUE: {KillScoresBlue}";
        if (killTextYellow != null)
            killTextYellow.text = $"YELLOW: {KillScoresYellow}";
    }

    //加载继承击杀数
    public void LoadKills()
    {
        KillScoresPink = PlayerPrefs.GetInt("KillsPink", 0);
        KillScoresBlue = PlayerPrefs.GetInt("KillsBlue", 0);
        KillScoresYellow = PlayerPrefs.GetInt("KillsYellow", 0);
    }

    //保存击杀数
    public void SaveKills()
    {
        PlayerPrefs.SetInt("KillsPink", KillScoresPink);
        PlayerPrefs.SetInt("KillsBlue", KillScoresBlue);
        PlayerPrefs.SetInt("KillsYellow", KillScoresYellow);
        PlayerPrefs.Save();
    }

    //重置击杀计数器
    public void ResetKills()
    {
        _killScoresPink = 0;
        _killScoresBlue = 0;
        _killScoresYellow = 0;
        PlayerPrefs.SetInt("KillsPink", 0);
        PlayerPrefs.SetInt("KillsBlue", 0);
        PlayerPrefs.SetInt("KillsYellow", 0);
        PlayerPrefs.Save();
        UpdateText();
        Debug.Log("击杀计数器已重置！");
    }

}
