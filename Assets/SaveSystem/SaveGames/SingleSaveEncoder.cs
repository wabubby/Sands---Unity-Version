using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Wabubby {
    public class SingleSaveEncoder : AbstractSaveEncoder {
        private string mainPath => $"{SaveGame.Path}/{Path.GetDirectoryName(SaveGame.Path)}.json";

        public SingleSaveEncoder(AbstractSaveGame saveGame) : base(saveGame) {
            
        }

        public override SaveData Load() {
            if (File.Exists(mainPath)) {
                return JsonEncoder.Load(mainPath, SaveGame.DoEncrypt).SaveData;
            } else {
                return new SaveData();
            }
        }

        public override void Save() {
            Debug.Log($"dir name of {SaveGame.Path} is {Path.GetDirectoryName(SaveGame.Path)}");
            JsonEncoder.Save(new SaveDataContainer(SaveGame.SaveData), mainPath, SaveGame.DoEncrypt);
        }

    }
}
