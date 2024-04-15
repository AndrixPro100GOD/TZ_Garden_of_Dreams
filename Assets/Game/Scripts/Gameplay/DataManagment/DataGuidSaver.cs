using Unity.Plastic.Newtonsoft.Json;

namespace Game2D.DataManagment
{
    [System.Serializable]
    public class DataGuidSaver : IGuidData
    {
        [JsonConstructor]
        public DataGuidSaver(string dataGuid, string dataName)
        {
            DataGuid = dataGuid;
            DataName = dataName;
        }

        public DataGuidSaver(IGuidData item)
        {
            DataGuid = item.DataGuid;
            DataName = item.DataName;
        }

        [JsonProperty]
        public string DataGuid { get; set; }

        [JsonProperty]
        public string DataName { get; set; }
    }
}