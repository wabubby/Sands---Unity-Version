using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Wabubby {
    public class SlotSaveEncoder : AbstractSaveEncoder
    {
        public SlotSaveEncoder(AbstractSaveGame saveGame) : base(saveGame) { }
        private string savePathPrefix => $"{SaveGame.Path}/{Path.GetFileName(SaveGame.Path)}"+"{0}"+".json";

        public List<SaveData> LoadAll() {
            List<SaveData> saveDatas = new List<SaveData>();
            string[] saveDataPaths = Directory.GetFiles(SaveGame.Path);
            foreach (string saveDataPath in saveDataPaths) { saveDatas.Add(JsonEncoder.Load(saveDataPath, SaveGame.DoEncrypt).SaveData); }

            return saveDatas;
        }

        /// <summary>
        /// loads the first save alphabetially.
        /// don't really know why you'd want to use this, like at all. use LoadAll() then display those to the player.
        /// store time data in saveData if you want to sort my recency.
        /// </summary>
        /// <returns></returns>
        public override SaveData Load() {
            string[] saveDataPaths = Directory.GetFiles(SaveGame.Path);
            System.Array.Sort(saveDataPaths);

            if (saveDataPaths.Length == 0) {
                Debug.LogError("no savedata available in this slot save game");
                return null;
            }

            return JsonEncoder.Load(saveDataPaths[0], SaveGame.DoEncrypt).SaveData;
        }

        /// <summary>
        /// saves current save with the username attached to the savedata. depending on the savedata, players can overwrite.
        /// </summary>
        public override void Save() {
            JsonEncoder.Save(new SaveDataContainer(SaveGame.SaveData), System.String.Format(savePathPrefix, SaveGame.SaveData.UserName), SaveGame.DoEncrypt);
        }

    }
}

