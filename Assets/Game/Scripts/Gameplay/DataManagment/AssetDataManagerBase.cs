using Game2D.DataManagment;
using Game2D.DataManagment.Scriptable;

using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace Game2D.Gameplay.DataManagment
{
    /// <summary>
    /// DATA MANAGER EXPLANE:
    /// <list>
    /// <item>Хранит данные в JSON файле</item>
    /// <item>Проверяет все GUID <typeparamref name="TAssetData"/> в проекте сравнивая их в JSON файле</item>
    /// <item>Если будет отсутствовать в проекте <typeparamref name="TAssetData"/>, то через в JSON файл можно будет об этом узнать</item>
    /// <item>Если в проекте будет присутствовать <typeparamref name="TAssetData"/>, а в JSON файле нет, то об этом можно будет узнать</item>
    /// </list>
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    /// <typeparam name="TAssetData"></typeparam>
    public abstract class AssetDataManagerBase<TData, TAssetData> : DataManagerBase<TData>, IAssetDataManager<TData, TAssetData> where TData : DataGuidSaver where TAssetData : DataGuidScriptable
    {
        public abstract TData Convert(TAssetData guidData);

        /// <summary>
        /// Удаляет <typeparamref name="TData"/> из JSON файла и удаляет все Asset <see cref="DataGuidScriptable"/> из проекта, на которые ссылался <paramref name="dataToDelete"/>
        /// </summary>
        /// <param name="dataToDelete"></param>
        /// <returns><see langword="true"/> если данные были удалены, иначе <see langword="false"/></returns>
        public override bool DeleteData(TData dataToDelete)
        {
            bool isRemovedJSON = RemoveData(dataToDelete);

            bool isDeletedAsset = RemoveDataAssets(dataToDelete);

            if (isRemovedJSON && isDeletedAsset)
            {
                UnityEngine.Debug.Log($" \"{dataToDelete.DataName}\" удалён из проекта (все Assets) и JSON файла");
            }

            if (!isRemovedJSON)
            {
                UnityEngine.Debug.Log($"Не удалось удалить \"{dataToDelete.DataName}\" из JSON списка");
            }
#if UNITY_EDITOR
            if (!isDeletedAsset)
            {
                UnityEngine.Debug.Log($"Не удалось удалить \"{dataToDelete.DataName}\" из проекта (возможно его не сущуствует)");
            }
#endif

            return isRemovedJSON || isDeletedAsset;
        }

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