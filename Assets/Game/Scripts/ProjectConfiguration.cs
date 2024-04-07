//Это всё можно сделать ассетом, для того что бы управлять значениями в редактаре, но на данный момент так быстрее
using UnityEngine;

using static ProjectConfiguration.RecurcesPath;

namespace ProjectConfiguration
{
    public static class ProjectConfiguration
    {
        public const string NAME_ROOT = "2D Game/";
        public const string NAME_ROOT_CHARACTER = NAME_ROOT + "Character/";
        public const string NAME_ROOT_PLAYER = NAME_ROOT + "Player/";
        public const string NAME_ROOT_ITEM = NAME_ROOT + "Item/";
        public const string NAME_ROOT_ITEM_COMBAT = NAME_ROOT_ITEM + "Combat/";
    }

    public static class RecurcesPath
    {
        private const string PATH_IMAGES = "Images/";

        public const string PATH_IMAGE_ITEM_STOCK = PATH_IMAGES + "Item stock.png";
    }

    public static class RecurcesUtility
    {
        public static Sprite GetSpriteItemStock()
        {
            return Resources.Load(PATH_IMAGE_ITEM_STOCK, typeof(Sprite)) as Sprite;
        }
    }
}