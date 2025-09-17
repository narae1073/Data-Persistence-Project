using System;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    [Serializable]
    public class PlayerData
    {
        public string playerName;
        public int playerScore;
    }

    [Serializable]
    class SaveData
    {
        public List<PlayerData> allPlayers = new List<PlayerData>();
    }

    public List<PlayerData> allPlayers = new List<PlayerData>();
    public string bestPlayerName;
    public int bestScore;

    public PlayerData currentPlayer; // ���� ���� �÷��̾�
    public TextMeshProUGUI highScoreText;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadData();
    }
    void Start()
    {
        DisplayHighScores();
    }

    public PlayerData AddPlayer(string name)
    {
        if (string.IsNullOrEmpty(name))
            name = "Player";

        PlayerData newPlayer = new PlayerData
        {
            playerName = name,
            playerScore = 0
        };

        currentPlayer = newPlayer;
        allPlayers.Add(newPlayer);
        return newPlayer;
    }

    public void SaveDataFunction()
    {
        //SaveData�� ���� PlayerData�� ����Ʈ��
        SaveData data = new SaveData();
        data.allPlayers = allPlayers;

        //SaveData Ŭ���� �����͸� json���� ��ȯ
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

        Debug.Log("���� �Ϸ�! ���� ���: " + Application.persistentDataPath);
    }

    // bestPlayerName�� bestScore�� �����ϴ� �ſ� �߿��� �Լ�
    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            allPlayers = data.allPlayers;
        }

        // �ְ� ���� ���
        if (allPlayers.Count > 0)
        {
            PlayerData best = allPlayers[0];
            foreach (var p in allPlayers)
            {
                if (p.playerScore > best.playerScore)
                    best = p;
            }
            bestPlayerName = best.playerName;
            bestScore = best.playerScore;
        }
    }

    public void UpdateCurrentPlayerScore(int score)
    {
        if (currentPlayer != null)
        {
            currentPlayer.playerScore = score;

            if (score > bestScore)
            {
                bestScore = score;
                bestPlayerName = currentPlayer.playerName;
            }
        }
    }

    void DisplayHighScores()
    {
        var players = ScoreManager.Instance.allPlayers; //��� �÷��̾� ��ü

        players.Sort((a, b) => b.playerScore.CompareTo(a.playerScore));

        //        ����Ʈ ���� ��ҵ��� ���� �������� ������������ ������.        
        //        (a, b) => b.playerScore.CompareTo(a.playerScore) �� ���ٽ�:
        //a�� b�� ����Ʈ ���� PlayerData ��ü.
        //CompareTo�� ���� �� �Լ�:
        //b.playerScore�� ũ�� ��� �� b�� a���� �տ� ��ġ
        //0�̸� �״��
        //������ b�� �ڷ�
        //���: ������ ���� �÷��̾ ���ʿ� ���� ��.

        
        string text = "High Scores\n";

        // �� �� �� ������
        int maxDisplay = Mathf.Min(10, players.Count);

        for (int i = 0; i < maxDisplay; i++)
        {
            text += $"{i + 1}. {players[i].playerName} : {players[i].playerScore}\n";
        }

        highScoreText.text = text;
    }
}
