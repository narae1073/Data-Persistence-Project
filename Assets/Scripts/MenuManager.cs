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
        // �ְ����� UI ǥ��
        bestScoreText.text = $"Best Score : {ScoreManager.Instance.bestPlayerName} : {ScoreManager.Instance.bestScore}";
    }

    public void OnStartButtonClicked()
    {
        string playerName = nameInputField.text;
        ScoreManager.Instance.AddPlayer(playerName);

        SceneManager.LoadScene(1); // Main ��
    }

    public void OnQuitButtonClicked()
    {
        // �����Ϳ��� ���� ���� �� (�÷��� ���)
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ����� ���� ���� ���� ��
        Application.Quit();
#endif
    }
}
