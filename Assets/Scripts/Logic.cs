using TMPro;
using UnityEngine;

public class Logic : MonoBehaviour
{
    #region KillCount
    public TMP_Text killCountText;

    public void UpdateKillCount()
    {
        // Update kill count game state dan UI
        GameState.Instance.KillCount++;
        killCountText.text = GameState.Instance.KillCount.ToString();
    }
    #endregion

}
