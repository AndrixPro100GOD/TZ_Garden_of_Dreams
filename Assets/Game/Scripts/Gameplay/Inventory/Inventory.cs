using Game2D.Gameplay.Items.Scriptable;

using System.Collections.Generic;

using UnityEngine;

namespace Game2D.Assets.Game.Scripts.Gameplay.Inventory
{
    public class Inventory : MonoBehaviour
    {
        [Min(1)]
        [SerializeField]
        private int m_slotSizeCount = 10;

        [SerializeField]
        private List<Slot> m_intventorySlots = new();

        public bool HasEmplySlots => m_intventorySlots.Count < m_slotSizeCount;

        public bool AddItem<TItem>(TItem item, int count) where TItem : ItemBase
        {
            Debug.Assert(count > 0, "Нельзя добавить предмет в количестве <= 0");
            Debug.Assert(item != null, "Добовляемый предмет пуст");

            if (!HasEmplySlots)
            {
                return false;//Если нету места
            }

            ////m_intventorySlots.Add(new Slot(item, count));
            return true;
        }

        /// <summary>
        /// Удаление из инвенторя
        /// </summary>
        /// <typeparam name="TItem"></typeparam>
        /// <param name="item">Предмет, который нужно найти</param>
        /// <param name="count"></param>
        /// <returns></returns>
        public bool RemoveItem<TItem>(TItem item, int count) where TItem : ItemBase
        {
            Debug.Assert(count > 0, "Нельзя убрать предмет в количестве <= 0");
            Debug.Assert(item != null, "Убираемый предмет пуст");

            return false;
        }

        [System.Serializable]
        public class Slot
        {
            //public Slot( , int count)
            //{
            //}
        }
    }
}