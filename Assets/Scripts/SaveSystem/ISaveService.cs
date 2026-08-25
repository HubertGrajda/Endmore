using System.Collections.Generic;

namespace Scripts.SaveSystem
{
    public interface ISaveService
    {
        void DeleteSave();
        void Load();
        void Save();

        void CaptureState(string id, Dictionary<string, SaveData> stateInfo);
        void RegisterHandler(SaveHandler saveHandler);
        void UnRegisterHandler(SaveHandler saveHandler);
    }
}