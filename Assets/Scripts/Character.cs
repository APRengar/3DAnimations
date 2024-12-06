using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] Animator animatorController;
    [SerializeField] GameObject buttonsPrefab;

    private GameObject buttonsReady;
    
    public GameObject CreateButtons()
    {
        return buttonsReady;
    }

    private void Start()
    {
        SetupPrefab();
    }

    private void SetupPrefab()
    {
        buttonsReady = Instantiate(buttonsPrefab);
        ActionButtons actionButtons = buttonsReady.GetComponent<ActionButtons>();
        if (actionButtons != null)
        {
            actionButtons.MyName.text = gameObject.name;
            actionButtons.StopAllActions.onClick.AddListener(StopActions);
            actionButtons.Jump.onClick.AddListener(JumpAction);
            actionButtons.Dance.onClick.AddListener(DanceAction);
            actionButtons.Movement.onValueChanged.AddListener(MovementAction);
            actionButtons.InCombat.onValueChanged.AddListener(CombatAction);
        }
        else
        {
            Debug.LogError("ActionButtons component is missing on the prefab!");
        }
    }

    private void StopActions()
    {
        animatorController.SetTrigger("StopAllActions");
    }
    private void JumpAction()
    {
        animatorController.SetTrigger("Jump");
    }
    private void DanceAction()
    {
        animatorController.SetTrigger("Dance");
    }
    private void MovementAction(float value)
    {
        animatorController.SetFloat("ForwardSpeed", value);
    }
    private void CombatAction(bool value)
    {
        animatorController.SetBool("InCombat", value);
    }
}
