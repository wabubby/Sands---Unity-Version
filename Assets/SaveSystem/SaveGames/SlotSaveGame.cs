using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wabubby {
    public class SlotSaveGame : AbstractSaveGame
    {

        public List<SaveData> SaveDatas;

        public SlotSaveGame(string path, bool doEncrypt) : base(path, doEncrypt){
            Encoder = new SlotSaveEncoder(this);
            LoadAll();
            Load();
        }

        public void LoadAll() {
            SaveDatas = (Encoder as SlotSaveEncoder).LoadAll();
        }
        
    }
}
