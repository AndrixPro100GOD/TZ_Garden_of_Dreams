using Game2D.Gameplay.DataManagment;

using System.Collections.Generic;
using System.Linq;

using Unity.Plastic.Newtonsoft.Json;

#if UNITY_EDITOR

#endif

namespace Game2D.DataManagment
{
    /// <summary>
    /// DATA MANAGER EXPLANE:
    /// <list>
    /// <item>Хранит данные в JSON файле</item>
    /// </list>
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public abstract class DataManagerBase<TData> : IDataManager<TData> where TData : DataGuidSaver
    {
        protected abstract void SaveJson(in string json);

        protected abstract string LoadJson();

        #region Saving Data

        /// <summary>
        /// Сохраняет <paramref name="dataToSave"/> добавляя или перезаписывая его в JSON файле, не изменяя список с остальными данными
        /// </summary>
        /// <param name="dataToSave">Новые данные</param>
        public void SaveData(TData dataToSave)
        {
#nullable enable
            if (dataToSave == null)
            {
                UnityEngine.Debug.LogError("Was sent empty data to save");
                return;
            }

            List<TData> databaseList = LoadDatabase();

            TData? existingItem = databaseList.Find(itemData => itemData.DataGuid == dataToSave.DataGuid);

            if (existingItem != null)
            {
                existingItem.DataName = dataToSave.DataName;
            }
            else
            {
                databaseList.Add(dataToSave);
            }

            SaveDatabase(databaseList);

#nullable disable
        }

        /// <summary>
        /// Сохраняет <paramref name="dataToSave"/> добавляя или перезаписывая его в JSON файле, не изменяя список с остальными данными
        /// </summary>
        /// <param name="dataToSave">Новые данные</param>
        public void SaveData(ICollection<TData> dataToSave)
        {
#nullable enable
            List<TData> databaseList = LoadDatabase();

            foreach (TData? data in dataToSave)
            {
                TData? existingItem = databaseList.Find(itemData => itemData.DataGuid == data.DataGuid);

                if (existingItem != null)
                {
                    existingItem.DataName = data.DataName;
                }
                else
                {
                    databaseList.Add(data);
                }
            }

            SaveDatabase(databaseList);

#nullable disable
        }

        /// <summary>
        /// Сохраняет все данные, полностью перезаписывая весь список
        /// </summary>
        /// <param name="dataList">Список данных</param>
        private void SaveDatabase(in List<TData> dataList)
        {
            string json = JsonConvert.SerializeObject(dataList, typeof(List<TData>), null);
            SaveJson(json);
        }

        #endregion Saving Data

        #region Loading Data

        /// <summary>
        /// Считает все данные хранившиеся в JSON файле
        /// </summary>
        /// <returns>Список всех данных, которые когда либо были созданы в проекте</returns>
        public List<TData> LoadDatabase()
        {
            string json = LoadJson();
            return string.IsNullOrEmpty(json) ? new List<TData>() : NullDeleter(JsonConvert.DeserializeObject<List<TData>>(json).ToList());
        }

        private List<TData> NullDeleter(in List<TData> savedData)
        {
            if (savedData.Count > 1 && savedData.Exists(data => data == null))
            {
                UnityEngine.Debug.Log("The \"null\" have been removed from the JSON file");
                savedData.RemoveAll(data => data == null);
                SaveDatabase(savedData);
            }

            return savedData;
        }

        #endregion Loading Data

        #region Removing Data

        /// <summary>
        /// Удаляет из JSON файла данные
        /// </summary>
        /// <param name="dataToRemove">Данные для удаления</param>
        /// <returns></returns>
        public bool RemoveData(TData dataToRemove)
        {
            List<TData> databaseList = LoadDatabase();

            TData itemFound = databaseList.Find(item => item.DataGuid == dataToRemove.DataGuid);
            bool isRemoved = databaseList.Remove(itemFound);

            if (isRemoved)
            {
                SaveDatabase(databaseList);
            }

            return isRemoved;
        }

        #endregion Removing Data

        #region Delete Data

        /// <summary>
        /// Удаляет <typeparamref name="TData"/> из JSON файла
        /// </summary>
        /// <param name="dataToDelete"></param>
        /// <returns><see langword="true"/> если данные были удалены, иначе <see langword="false"/></returns>
        public virtual bool DeleteData(TData dataToDelete)
        {
            bool isRemovedJSON = RemoveData(dataToDelete);

            if (isRemovedJSON)
            {
                UnityEngine.Debug.Log($" \"{dataToDelete.DataName}\" удалён из JSON файла");
            }
            else
            {
                UnityEngine.Debug.Log($"Не удалось удалить \"{dataToDelete.DataName}\" из JSON списка");
            }

            return isRemovedJSON;
        }

        #endregion Delete Data
    }
}