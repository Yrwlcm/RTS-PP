using Scipts;
using UnityEngine;

public class UnitAbilities : MonoBehaviour
{
    public Ability[] Abilities;
    public bool ShouldActivate;
    public Unit Unit => unit;

    [SerializeField] private Unit unit;

    private Ability selectedAbility;
    public Texture2D cursorTexture;

    private void Start()
    {
        for (var i = 0; i < Abilities.Length; i++)
        {
            // Чтобы использовались копии абилок, а не их оригиналы, иначе ведет к синхронизации их между юнитами
            Abilities[i].hotkey = (KeyCode)(49 + i);
            Abilities[i] = Abilities[i].InstantiateAndInitialize();
        }
    }

    private void SelectAbility(Ability ability)
    {
        selectedAbility = ability;
        Cursor.SetCursor(cursorTexture, new Vector2(cursorTexture.width / 2, cursorTexture.height / 2),
            CursorMode.Auto);
    }

    public void Update()
    {
        if (!ShouldActivate) return;

        foreach (var ability in Abilities)
        {
            if (!Input.GetKeyDown(ability.hotkey) || ability.OnColdown) continue;

            if (ability.requireTarget == false)
            {
                ability.Use(unit.gameObject);
                return;
            }

            SelectAbility(ability);
            break;
        }

        if (selectedAbility == null || !Input.GetMouseButtonDown(0))
            return;


        var rayHit = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100_000,
            LayerMask.GetMask("Unit"));

        if (rayHit)
        {
            selectedAbility.Use(unit.gameObject, hit.collider.gameObject);
        }

        selectedAbility = null;
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}