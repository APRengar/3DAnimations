using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionButtons : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI myName;
    [SerializeField] Button stopAllActions;
    [SerializeField] Button jump;
    [SerializeField] Button dance;
    [SerializeField] Slider movement;
    [SerializeField] Toggle inCombat;

    public TextMeshProUGUI MyName => myName;
    public Button StopAllActions => stopAllActions;
    public Button Jump => jump;
    public Button Dance => dance;
    public Slider Movement => movement;
    public Toggle InCombat => inCombat;
}
