using Game2D.Assets.Game.Scripts.Gameplay.Items;

using UnityEngine;

using static ProjectConfiguration.ProjectNames;

namespace Game2D.Gameplay.Character
{
    [AddComponentMenu(NAME_ROOT_CHARACTER + "Character item controller")]
    public class CharacterItemController : MonoBehaviour
    {
        [SerializeField]
        private ItemController itemController;

        private void Start()
        {
        }

        // Update is called once per frame
        private void Update()
        {
        }
    }
}