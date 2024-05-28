using Scipts;
using UnityEngine;

public class UnitAbilities : MonoBehaviour
{
    public Ability[] Abilities;
    public bool ShouldActivate;
    public Unit Unit => unit;

    [SerializeField] private Unit unit;

    private void Start()
    {
        for (var i = 0; i < Abilities.Length; i++)
        {
            // Чтобы использовались копии абилок, а не их оригиналы, иначе ведет к синхронизации их между юнитами
            Abilities[i] = Abilities[i].InstantiateAndInitialize();
        }
    }

    public void Update()
    {
        if (!ShouldActivate) return;

        foreach (var ability in Abilities)
        {
            if (!Input.GetKeyDown(ability.hotkey)) continue;
            
            var ray = Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out var hit, 100_000,
                LayerMask.GetMask("Unit"));

            ability.Use(unit.gameObject, hit.transform?.gameObject);
        }
    }
}