using Unity.Plastic.Newtonsoft.Json;

namespace Game2D.DataManagment
{
    [System.Serializable]
    public class DataGuidSaver : IGuidData
    {
        [JsonConstructor]
        public DataGuidSaver(string itemGUID, string itemName)
        {
            DataGuid = itemGUID;
            DataName = itemName;
        }

        public DataGuidSaver(IGuidData item)
        {
            DataGuid = item.DataGuid;
            DataName = item.DataName;
        }

        public DataGuidSaver()
        { }

        [JsonProperty]
        public string DataGuid { get; set; }

        [JsonProperty]
        public string DataName { get; set; }
    }
}