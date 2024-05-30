using UnityEngine;


namespace Scipts
{
    [CreateAssetMenu(fileName = "Building", menuName = "New Building/BasicBuilding")]
    public class BasicBuilding : ScriptableObject
    {
        public enum buildingType
        {
            BotanicalGarden,
            BioLaboratory,
            AdvancedResearchCenter,
            CypressShelter,
            TempleOfNature,
            RadiationWaste,
            Idol,
            AllMetalFortres,
            MilitaryCamp,
            MetallurgicalFabric,
            PiercingTrap,
            NanoMetallurgyWorkshop,
            ObservationPoint,
            PurificationSubstation,
            IndustrialWaste
        }

        [Space(15)]
        [Header("Building Settings")]

        public buildingType type;
        public new string name;
        public GameObject buildingPrefab;
        public float buildTime;

        [Space(15)]
        [Header("Building Base Stats")]
        [Space(40)]

        public BuildingStatTypes.Base baseStats;

    }
}
