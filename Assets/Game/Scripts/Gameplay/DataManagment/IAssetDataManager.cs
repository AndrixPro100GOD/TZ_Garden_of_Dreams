using System.Collections.Generic;

namespace Game2D.Gameplay.DataManagment
{
    public interface IAssetDataManager<TSavingData, TAssetData> : IDataManager<TSavingData>
    {
        /// <summary>
        /// Обновляет текущею базу данных, добавляя в проект найденные <typeparamref name="TAssetData"/>, которые отсутствовали в нём
        /// </summary>
        void RestoreDatabase();

        /// <summary>
        /// Ковертурует Asset в <typeparamref name="TSavingData"/>
        /// </summary>
        /// <param name="guidData">Ассет для конвертации</param>
        /// <returns>Данные для сохранения</returns>
        TSavingData Convert(TAssetData guidData);

        /// <summary>
        /// <c>EDITOR</c> Получает все ненайденные <typeparamref name="TAssetData"/> при найденных ссылающих на них <typeparamref name="TSavingData"/> из базы данных
        /// </summary>
        /// <returns>Все ненайденные <typeparamref name="TAssetData"/> данные</returns>
        List<(TAssetData assetData, string assetPath)> GetUnfoundAssetsData();

        /// <summary>
        /// <c>EDITOR</c> Ищет в проекте все <typeparamref name="TAssetData"/> на которые ссылается <typeparamref name="TSavingData"/>
        /// </summary>
        /// <returns>Все найденые пути с <typeparamref name="TAssetData"/></returns>
        IEnumerable<(TSavingData data, string assetPath)> GetDataWithAssetPaths();

        /// <summary>
        /// <c>EDITOR</c> Получает все ненайденные <typeparamref name="TSavingData"/> при найденных ссылающих на них <typeparamref name="TAssetData"/> из Assets проекта
        /// </summary>
        /// <returns>Все ненайденные <typeparamref name="TSavingData"/> данные</returns>
        public List<TSavingData> GetUnfoundData();
    }
}