using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharactersController : MonoBehaviour
{
    public List<Character> Characters;

    [SerializeField] private Canvas parentUI;

    void Start()
    {
        Invoke("CreateButtons", 0.5f);
    }

    private void CreateButtons()
    {
        foreach (Character character in Characters)
        {
            character.CreateButtons().transform.SetParent(parentUI.transform);
        }

        FaderController.Instance.OnFadeOut.Invoke();
    }
}
