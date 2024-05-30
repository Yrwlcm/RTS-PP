using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Scipts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Transform = UnityEngine.Transform;

public class AbilityMenu : MonoBehaviour
{
    public GameObject abilityButtonPrefab;
    public Transform abilityPanel;
    public readonly int TeamId;

    private UnitAbilities selectedUnitAbilities;
    private readonly List<AbilityButton> abilityButtons = new();

    private Coroutine cooldownCoroutine;

    private void Start()
    {
        HideMenu();
    }

    private void Update()
    {
        if (SelectionManager.Instances[TeamId].SelectedUnit.Count != 1)
        {
            HideMenu();
            return;
        }

        var unit = SelectionManager.Instances[TeamId].SelectedUnit.First() as Unit;
        var unitAbilities = unit?.GetComponent<UnitAbilities>();
        if (unitAbilities == null) return;
        ShowMenu(unitAbilities);
    }

    public void ShowMenu(UnitAbilities unit)
    {
        if (selectedUnitAbilities == unit) return;

        selectedUnitAbilities = unit;
        selectedUnitAbilities.ShouldActivate = true;
        abilityButtons.Clear();

        if (cooldownCoroutine != null)
        {
            StopCoroutine(cooldownCoroutine);
        }

        // Очистить старые кнопки способностей
        foreach (Transform child in abilityPanel)
        {
            Destroy(child.gameObject);
        }

        // Создать новые кнопки для способностей
        foreach (var ability in selectedUnitAbilities.Abilities)
        {
            var button = Instantiate(abilityButtonPrefab, abilityPanel);
            var icon = button.GetComponent<Image>();
            var hotkeyText = button.transform.Find("HotkeyText").GetComponent<TextMeshProUGUI>();

            icon.sprite = ability.icon;
            hotkeyText.text = ability.hotkey.ToString();
            abilityButtons.Add(new AbilityButton(ability, button));
            // TODO: Сделать абилки кликабельными
            // button.GetComponent<Button>().onClick.AddListener(() => ability.Use());
        }

        abilityPanel.gameObject.SetActive(true);

        cooldownCoroutine = StartCoroutine(UpdateCooldowns());
    }


    private IEnumerator UpdateCooldowns()
    {
        while (true)
        {
            foreach (var abilityButton in abilityButtons)
            {
                abilityButton.UpdateCooldown();
            }

            yield return new WaitForSeconds(0.2f);
        }
    }

    private void HideMenu()
    {
        abilityPanel.gameObject.SetActive(false);
        if (selectedUnitAbilities != null)
            selectedUnitAbilities.ShouldActivate = false;
        selectedUnitAbilities = null;
    }

    private class AbilityButton
    {
        public Ability ability;
        public GameObject button;
        private Image buttonImage;
        private TextMeshProUGUI cooldownText;

        public AbilityButton(Ability ability, GameObject button)
        {
            this.ability = ability;
            this.button = button;
            cooldownText = button.transform.Find("CooldownText").GetComponent<TextMeshProUGUI>();
            buttonImage = button.GetComponent<Image>();
        }

        public void UpdateCooldown()
        {
            if (ability.RemainingCooldown <= 0)
            {
                cooldownText.text = "Ready";
                buttonImage.color = Color.green;
                return;
            }

            buttonImage.color = Color.red;
            cooldownText.text = ability.RemainingCooldown.ToString(CultureInfo.InvariantCulture).Substring(0, 3);
        }
    }
}