using System.Collections.Generic;

namespace Game2D.Gameplay.DataManagment
{
    public interface IDataManager<TSavingData>
    {
        /// <summary>
        /// Сохраняет данные в базе данных
        /// </summary>
        /// <param name="dataToSave"></param>
        void SaveData(TSavingData dataToSave);

        /// <summary>
        /// Сохраняет данные в базе данных
        /// </summary>
        /// <param name="dataToSave"></param>
        void SaveData(ICollection<TSavingData> dataToSave);

        /// <summary>
        /// Загружает данные из базы данных
        /// </summary>
        /// <returns>Список данных</returns>
        List<TSavingData> LoadDatabase();

        /// <summary>
        /// Удалет из базы данных данные
        /// </summary>
        /// <param name="dataToRemove"></param>
        /// <returns>если данные удалены <see langword="true"/> иначе <see langword="false"/> </returns>
        bool RemoveData(TSavingData dataToRemove);

        /// <summary>
        /// <c>EDITOR</c> Удаляет из базы данных и из проекта все связанные <typeparamref name="TAssetData"/>
        /// </summary>
        /// <param name="dataToDelete"></param>
        /// <returns>если хоть какие-то данные удалены <see langword="true"/> иначе <see langword="false"/> </returns>
        bool DeleteData(TSavingData dataToDelete);
    }
}