using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wabubby {
    [System.Serializable]
    public class SaveData {

        public SaveData(string name) {
            UserName = name;
        }

        public string UserName;
        public int LevelIndex;
        public int Coins;

    }

    [System.Serializable] // allows this class to be converted to json
    public class SaveDataContainer {

        public SaveData SaveData;
        
        public SaveDataContainer(SaveData saveData) {
            SaveData = saveData;
        }

    }
}

