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

        // player preferences:
        public string DateTime;
        
    }

    [System.Serializable] // allows this class to be converted to json
    public class SaveDataContainer {
        public SaveData SaveData;
        public SaveDataContainer(SaveData saveData) {SaveData = saveData;}
    }
}

