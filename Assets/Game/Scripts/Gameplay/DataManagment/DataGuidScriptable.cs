using UnityEngine;

namespace Game2D.DataManagment
{
    public abstract class DataGuidScriptable : ScriptableObject, IGuidData
    {
        [SerializeField, HideInInspector]
        private string dataGuid = string.Empty;

        [SerializeField]
        private string dataName = "Item";

        public string DataGuid => dataGuid;
        public string DataName => dataName;
    }
}