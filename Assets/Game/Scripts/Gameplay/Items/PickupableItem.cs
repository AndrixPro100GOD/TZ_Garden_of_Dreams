using Game2D.Gameplay.Items.Scriptable;

using UnityEngine;

namespace Game2D.Gameplay.Items
{
    [RequireComponent(typeof(Collider2D), typeof(SpriteRenderer))]
    public class PickupableItem : MonoBehaviour
    {
        [SerializeField]
        private ItemDataBase itemData;

        private void Awake()
        {
            var collider = GetComponent<Collider2D>();
            collider.isTrigger = true;

            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = itemData.GetSprite;
        }
    }
}