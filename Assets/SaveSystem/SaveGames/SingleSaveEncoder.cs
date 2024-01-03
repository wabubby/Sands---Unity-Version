using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Wabubby {
    public class SingleSaveEncoder : AbstractSaveEncoder
    {
        public SingleSaveEncoder(AbstractSaveGame saveGame) : base(saveGame) {
        }

        public override SaveData Load() {
            if (File.Exists(SaveGame.Path) || Directory.Exists(SaveGame.Path)) {
                return JsonEncoder.Load(SaveGame.Path, SaveGame.DoEncrypt).SaveData;
            } else {
                return new SaveData();
            }
        }

        public override void Save() {
            JsonEncoder.Save(new SaveDataContainer(SaveGame.SaveData), SaveGame.Path, SaveGame.DoEncrypt);
        }

        public override void Delete() {
            Directory.Move(SaveGame.Path, $"{EncodingConstants.TrashPath}/{System.IO.Path.GetFileName(SaveGame.Path)}");
        }

    }
}
