using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public TMP_InputField nameInputField;
    public TMP_Text bestScoreText;

    void Start()
    {
        Debug.Log("Application.persistentDataPath: " + Application.persistentDataPath);
        // 최고점수 UI 표시
        bestScoreText.text = $"Best Score : {ScoreManager.Instance.bestPlayerName} : {ScoreManager.Instance.bestScore}";
    }

    public void OnStartButtonClicked()
    {
        string playerName = nameInputField.text;
        ScoreManager.Instance.AddPlayer(playerName);

        SceneManager.LoadScene(1); // Main 씬
    }

    public void OnQuitButtonClicked()
    {
        // 에디터에서 실행 중일 때 (플레이 모드)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // 빌드된 게임 실행 중일 때
        Application.Quit();
#endif
    }
}
