using UnityEngine;

[DefaultExecutionOrder(-999)]
public class DifficultyManager : MonoBehaviour
{
    public GameObject[] AdditionalEnemies;

    public void Awake()
    {
        SetDifficulty(OptionsManager.Difficulty);
    }
    public void SetDifficulty(int difficulty)
    {
        foreach (GameObject enemy in AdditionalEnemies)
        {
            if (difficulty > 0) enemy.SetActive(true);
            else enemy.SetActive(false);
            difficulty--;
        }
    }
}
