namespace Scripts.SaveSystem
{
    public interface ISaveable<in TSaveData> : ISaveableBase where TSaveData : SaveData
    {
        string ISaveableBase.SaveKey => GetType().Name;
        
        void ISaveableBase.Load(SaveData data)
        {
            if (data is not TSaveData concreteData) return;
            
            Load(concreteData);
        }
        
        public void Load(TSaveData data);
    }

    public interface ISaveableBase
    {
        string SaveKey { get; }
        public SaveData Save();

        void Load(SaveData data);
    }
}