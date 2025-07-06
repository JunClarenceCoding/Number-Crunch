using UnityEngine;
using TMPro;

public class PlayerHUDItem : MonoBehaviour
{
    public TMP_Text usernameText;  // To display username
    public TMP_Text levelText;     // To display level
    public TMP_Text maxHealthText; // To display max health

    public void SetPlayer(string username, int level, int maxHealth)
    {
        usernameText.text = username;
        levelText.text = $"{level}";
        maxHealthText.text = $"{maxHealth}";
    }
}
