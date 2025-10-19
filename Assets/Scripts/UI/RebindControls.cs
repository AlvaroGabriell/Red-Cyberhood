using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class RebindingUI : MonoBehaviour
{
    [Serializable]
    public class ActionRebind
    {
        public string actionName; // nome exato da Action no InputActionAsset
        public int bindingIndex;
        public TextMeshProUGUI bindingText; // referência ao Text que mostra a binding
        public Button rebindButton; // botão pra iniciar rebind
    }

    public GameObject pressAnyKeyPanel; // painel que aparece quando está esperando input
    public PlayerInput playerInput; // seu PlayerInput
    public ActionRebind[] rebinds; // lista de bindings que podem ser rebindados

    private void OnEnable()
    {
        playerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInput>();

        pressAnyKeyPanel.SetActive(false);
        // Carrega overrides
        var json = PlayerPrefs.GetString("rebinds", "");
        if (!string.IsNullOrEmpty(json)) playerInput.actions.LoadBindingOverridesFromJson(json);

        foreach (var r in rebinds)
        {
            UpdateBindingDisplay(r);

            var tmp = r;
            tmp.rebindButton.onClick.AddListener(() => StartRebind(tmp));
        }
    }

    private void OnDisable()
    {
        foreach (var r in rebinds)
            r.rebindButton.onClick.RemoveAllListeners();
    }

    private void UpdateBindingDisplay(ActionRebind r)
    {
        var action = playerInput.actions[r.actionName];
        r.bindingText.text = action.GetBindingDisplayString(r.bindingIndex);
    }

    private void StartRebind(ActionRebind r)
    {
        var action = playerInput.actions[r.actionName];
        int index = r.bindingIndex;
        if (index < 0 || index >= action.bindings.Count) return;

        r.bindingText.text = "...";
        pressAnyKeyPanel.SetActive(true);
        r.rebindButton.interactable = false;

        action.PerformInteractiveRebinding(index)
            .WithCancelingThrough("<Keyboard>/escape")
            .WithCancelingThrough("<Gamepad>/start")
            .OnComplete(operation =>
            {
                operation.Dispose();
                UpdateBindingDisplay(r);
                r.rebindButton.interactable = true;
                pressAnyKeyPanel.SetActive(false);

                // salva todas as overrides
                var json = playerInput.actions.SaveBindingOverridesAsJson();
                PlayerPrefs.SetString("rebinds", json);
                PlayerPrefs.Save();
            })
            .OnCancel(operation =>
            {
                operation.Dispose();
                UpdateBindingDisplay(r);
                r.rebindButton.interactable = true;
                pressAnyKeyPanel.SetActive(false);
            })
            .Start();
    }
}