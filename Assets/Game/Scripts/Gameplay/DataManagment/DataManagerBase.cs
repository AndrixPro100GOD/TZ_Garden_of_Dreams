using System.Collections.Generic;
using System.Linq;

using Unity.Plastic.Newtonsoft.Json;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Game2D.DataManagment
{
    public interface IDataManager<TSavingData, TAssetData>
    {
        /// <summary>
        /// Ковертурует Asset в <typeparamref name="TSavingData"/>
        /// </summary>
        /// <param name="guidData">Ассет для конвертации</param>
        /// <returns>Данные для сохранения</returns>
        TSavingData Convert(TAssetData guidData);

        /// <summary>
        /// Обновляет текущею базу данных, добавляя в проект найденные <typeparamref name="TAssetData"/>, которые отсутствовали в нём
        /// </summary>
        void RestoreDatabase();

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

        /// <summary>
        /// <c>EDITOR</c> Получает все ненайденные <typeparamref name="TAssetData"/> при найденных ссылающих на них <typeparamref name="TSavingData"/> из базы данных
        /// </summary>
        /// <returns>Все ненайденные <typeparamref name="TAssetData"/> данные</returns>
        List<(TAssetData assetData, string assetPath)> GetUnfoundAssetsData();
    }

    /// <summary>
    /// DATA MANAGER EXPLANE:
    /// <list>
    /// <item>Хранит данные в JSON файле</item>
    /// <item>Проверяет все GUID <typeparamref name="TAssetData"/> в проекте сравнивая их в JSON файле</item>
    /// <item>Если будет отсутствовать в проекте <typeparamref name="TAssetData"/>, то через в JSON файл можно будет об этом узнать</item>
    /// <item>Если в проекте будет привустовать <typeparamref name="TAssetData"/>, а в JSON файле нет, то об этом можно будет узнать</item>
    /// </list>
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TAssetData"></typeparam>
    public abstract class DataManagerBase<TData, TAssetData> : IDataManager<TData, TAssetData> where TData : DataGuidSaver, new() where TAssetData : DataGuidScriptable
    {
        protected abstract void SaveJson(in string json);

        protected abstract string LoadJson();

        public virtual TData Convert(TAssetData guidData)
        {
            TData newData = new()
            {
                DataGuid = guidData.DataGuid,
                DataName = guidData.DataName
            };

            return newData;
        }

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
        /// Удаляет <typeparamref name="TData"/> из JSON файла и удаляет все Asset <see cref="DataGuidScriptable"/> из проекта, на которые ссылался <paramref name="dataToDelete"/>
        /// </summary>
        /// <param name="dataToDelete"></param>
        /// <returns></returns>
        public bool DeleteData(TData dataToDelete)
        {
            bool isRemovedJSON = RemoveData(dataToDelete);

            bool isDeletedAsset = RemoveDataAssets(dataToDelete);

            if (isRemovedJSON && isDeletedAsset)
            {
                UnityEngine.Debug.Log($" \"{dataToDelete.DataName}\" удалён из проекта (все Assets) и JSON файла");
            }
            else if (!isRemovedJSON)
            {
                UnityEngine.Debug.Log($"Не удалось удалить \"{dataToDelete.DataName}\" из JSON списка");
            }
#if UNITY_EDITOR
            else if (!isDeletedAsset)
            {
                UnityEngine.Debug.Log($"Не удалось удалить \"{dataToDelete.DataName}\" из проекта (возможно его не сущуствует)");
            }
#endif

            return isRemovedJSON || isDeletedAsset;
        }

        #endregion Delete Data

        #region AssetsDataBase

        /// <summary>
        /// <c>EDITOR</c> ищет GUIDs в проекте по имени <typeparamref name="TAssetData"/>
        /// </summary>
        /// <returns>GUIDs</returns>
        private string[] GetAssetPathsGUID()
        {
#if UNITY_EDITOR
            return AssetDatabase.FindAssets($"t:{typeof(TAssetData)}", new string[] { "Assets" });

#else
            return new string[0];
#endif
        }

        /// <summary>
        /// <c>EDITOR</c> загружает все <typeparamref name="TAssetData"/> из проекта
        /// </summary>
        /// <returns><typeparamref name="TAssetData"/> найденый в проекте с file path к нему</returns>
        private IEnumerable<(TAssetData assetData, string assetPath)> GetAssetsData()
        {
#if UNITY_EDITOR

            string[] assetPathsGUID = GetAssetPathsGUID();

            foreach (string assetPathGUID in assetPathsGUID)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(assetPathGUID);
                yield return (AssetDatabase.LoadAssetAtPath<TAssetData>(assetPath), assetPath);
            }

#else
            yield break;
#endif
        }

        /// <summary>
        /// <c>EDITOR</c> Удаляет Scriptable Assets из проекта
        /// </summary>
        /// <param name="dataToDelete">Данные, по которым будет удаляться Assets из проекта</param>
        /// <returns>Если произошло удаление <see langword="ture"/> иначе <see langword="false"/></returns>
        private bool RemoveDataAssets(in TData dataToDelete)
        {
            bool result = false;
#if UNITY_EDITOR

            foreach ((TAssetData asset, string path) in GetAssetsData())
            {
                // Проверяем, совпадает ли GUID предмета с GUID ассета
                if (asset.DataGuid == dataToDelete.DataGuid)
                {
                    if (!AssetDatabase.DeleteAsset(path))
                    {
                        UnityEngine.Debug.LogError($"Не удалось удалить {asset.name} по пути {path}");
                    }

                    AssetDatabase.Refresh();

                    result = true;
                }
            }
#endif
            return result;
        }

        /// <summary>
        /// <c>EDITOR</c> Получает все найденные <typeparamref name="TData"/>, на которых нету ссылающих на них <typeparamref name="TAssetData"/> из Assets проекта
        /// </summary>
        /// <returns>Все ненайденные <typeparamref name="TData"/> данные</returns>
        public List<TData> GetUnfoundData()
        {
#if UNITY_EDITOR
            List<TData> saveData = LoadDatabase();
            List<(TAssetData assetData, string assetPath)> foundAssetData = GetAssetsData().ToList();

            return saveData.Where(data => !foundAssetData.Exists(asset => asset.assetData.DataGuid == data.DataGuid)).ToList();
#else
            return new();
#endif
        }

        /// <summary>
        /// <c>EDITOR</c> Получает все найденные <typeparamref name="TAssetData"/>, на которых нету ссылающих на них <typeparamref name="TData"/> из базы данных
        /// </summary>
        /// <returns>Все ненайденные <typeparamref name="TAssetData"/> данные, которые должны быть в базе данных</returns>
        public List<(TAssetData assetData, string assetPath)> GetUnfoundAssetsData()
        {
#if UNITY_EDITOR
            List<TData> savedData = LoadDatabase();

            return GetAssetsData().Where(asset => !savedData.Exists(data => data.DataGuid == asset.assetData.DataGuid)).ToList();
#else
            return new();
#endif
        }

        /// <summary>
        /// <c>EDITOR</c> Ищет в проекте <typeparamref name="TAssetData"/> с таким же GUID
        /// </summary>
        /// <param name="assetData">Данные для поиска</param>
        /// <returns>File path если <typeparamref name="TAssetData"/> был найден, иначе пустая строка</returns>
        public string GetAssetDataPath(TAssetData assetData)
        {
#if UNITY_EDITOR
            foreach ((TAssetData asset, string path) in GetAssetsData())
            {
                if (asset.DataGuid == assetData.DataGuid)
                {
                    return path;
                }
            }
#endif
            return string.Empty;
        }

        /// <summary>
        /// <c>EDITOR</c> Ищет в проекте <typeparamref name="TAssetData"/> с таким же GUID как и у <typeparamref name="TData"/>
        /// </summary>
        /// <param name="data"></param>
        /// <param name="assetPath"></param>
        /// <returns></returns>
        public IEnumerable<(TAssetData asset, string assetPath)> GetAssetsData(TData data)
        {
#if UNITY_EDITOR
            foreach ((TAssetData asset, string path) in GetAssetsData())
            {
                // Проверяем, совпадает ли GUID предмета с GUID ассета
                if (asset.DataGuid == data.DataGuid)
                {
                    yield return (asset, path);
                }
            }
#else
            yield break;
#endif
        }

        /// <summary>
        /// <c>Editor</c> ищет все ассеты в проекте по GUID связаных с <typeparamref name="TData"/>
        /// </summary>
        /// <returns>Все найденые пути с <typeparamref name="TAssetData"/></returns>
        public IEnumerable<(TData data, string assetPath)> GetDataWithAssetPaths()
        {
#if UNITY_EDITOR
            List<TData> database = LoadDatabase();

            foreach (TData dataDB in database)
            {
                foreach ((TAssetData asset, string assetPath) in GetAssetsData(dataDB))
                {
                    yield return (dataDB, assetPath);
                }
            }
#else
            yield break;
#endif
        }

        /// <summary>
        /// Обновляет текущий JSON файл, добавляя в проект найденные <typeparamref name="TAssetData"/>, которые отсутствовали в нём
        /// </summary>
        public void RestoreDatabase()
        {
            List<TData> savedData = LoadDatabase();

            if (savedData.Count < 1)//Если вообще сохранённых данных нету то сканирует весь проект и добавляет каждый найденный asset в JSON файл
            {
                SaveData(GetAssetsData().Select(asset => Convert(asset.assetData)).ToList());
                return;
            }

            foreach ((TAssetData assetData, _) in GetUnfoundAssetsData())
            {
                SaveData(Convert(assetData));
            }
        }

        #endregion AssetsDataBase
    }
}