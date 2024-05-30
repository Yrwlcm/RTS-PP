using Scipts;
using UnityEngine;

public class UnitAbilities : MonoBehaviour
{
    public Ability[] Abilities;
    public bool ShouldActivate;
    public Unit Unit => unit;

    [SerializeField] private Unit unit;
    public Texture2D cursorTexture;

    private Ability selectedAbility;

    private void Start()
    {
        for (var i = 0; i < Abilities.Length; i++)
        {
            // Чтобы использовались копии абилок, а не их оригиналы, иначе ведет к синхронизации их между юнитами
            Abilities[i].hotkey = (KeyCode)(49 + i); // 49 соответствует клавише '1'
            Abilities[i] = Abilities[i].InstantiateAndInitialize();
        }
    }

    private void SelectAbility(Ability ability)
    {
        selectedAbility = ability;
        Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width / 2, cursorTexture.height / 2),
            CursorMode.Auto);
    }

    private void Update()
    {
        if (!ShouldActivate) return;

        foreach (var ability in Abilities)
        {
            if (!Input.GetKeyDown(ability.hotkey) || ability.OnCooldown) continue;

            if (!ability.requireTarget)
            {
                ability.Use(unit.gameObject);
                return;
            }

            SelectAbility(ability);
            break;
        }

        if (selectedAbility == null || !Input.GetMouseButtonDown(0)) return;

        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        
        if (Physics.Raycast(ray, out var hit, 100_000, LayerMask.GetMask("Unit")))
        {
            selectedAbility.Use(unit.gameObject, hit.collider.gameObject);
        }

        selectedAbility = null;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}