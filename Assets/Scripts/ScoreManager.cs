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

    public PlayerData currentPlayer; // 현재 세션 플레이어
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
        //SaveData는 단지 PlayerData의 리스트임
        SaveData data = new SaveData();
        data.allPlayers = allPlayers;

        //SaveData 클래스 데이터를 json으로 변환
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);

        Debug.Log("저장 완료! 파일 경로: " + Application.persistentDataPath);
    }

    // bestPlayerName과 bestScore를 갱신하는 매우 중요한 함수
    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            allPlayers = data.allPlayers;
        }

        // 최고 점수 계산
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
        var players = ScoreManager.Instance.allPlayers; //모든 플레이어 객체

        players.Sort((a, b) => b.playerScore.CompareTo(a.playerScore));

        //        리스트 안의 요소들을 점수 기준으로 내림차순으로 정렬함.        
        //        (a, b) => b.playerScore.CompareTo(a.playerScore) 는 람다식:
        //a와 b는 리스트 안의 PlayerData 객체.
        //CompareTo는 숫자 비교 함수:
        //b.playerScore가 크면 양수 → b가 a보다 앞에 위치
        //0이면 그대로
        //음수면 b가 뒤로
        //결과: 점수가 높은 플레이어가 앞쪽에 오게 됨.

        
        string text = "High Scores\n";

        // 둘 중 더 작은거
        int maxDisplay = Mathf.Min(10, players.Count);

        for (int i = 0; i < maxDisplay; i++)
        {
            text += $"{i + 1}. {players[i].playerName} : {players[i].playerScore}\n";
        }

        highScoreText.text = text;
    }
}
