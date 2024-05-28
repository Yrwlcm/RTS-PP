using System.Linq;
using Scipts;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenu : MonoBehaviour
{
    public GameObject abilityButtonPrefab;
    public Transform abilityPanel;

    private UnitAbilities selectedUnitAbilities;

    private void Start()
    {
        HideMenu();
    }

    private void Update()
    {
        if (SelectionManager.Instance.SelectedUnit.Count != 1)
        {
            HideMenu();
            return;
        }

        var unit = SelectionManager.Instance.SelectedUnit.First() as Unit;
        var unitAbilities = unit?.GetComponent<UnitAbilities>();
        if (unitAbilities == null) return;
        ShowMenu(unitAbilities);
    }

    public void ShowMenu(UnitAbilities unit)
    {
        selectedUnitAbilities = unit;
        selectedUnitAbilities.ShouldActivate = true;

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
            // TODO: Сделать абилки кликабельными
            // button.GetComponent<Button>().onClick.AddListener(() => ability.Use());
        }

        abilityPanel.gameObject.SetActive(true);
    }

    private void HideMenu()
    {
        abilityPanel.gameObject.SetActive(false);
        if (selectedUnitAbilities != null)
            selectedUnitAbilities.ShouldActivate = false;
        selectedUnitAbilities = null;
    }
}