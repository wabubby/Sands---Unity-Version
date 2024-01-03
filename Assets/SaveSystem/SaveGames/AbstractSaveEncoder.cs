using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;

namespace Wabubby {
    public abstract class AbstractSaveEncoder {
        
        protected AbstractSaveGame SaveGame;

        public AbstractSaveEncoder(AbstractSaveGame saveGame) {
            SaveGame = saveGame;
        }

        // loads from filepath
        public virtual SaveData Load() {
            if (File.Exists(SaveGame.Path) || Directory.Exists(SaveGame.Path)) {
                return JsonEncoder.Load(SaveGame.Path, SaveGame.DoEncrypt).SaveData;
            } else {
                return new SaveData();
            }
        }

        public virtual void Save() {
            JsonEncoder.Save(new SaveDataContainer(SaveGame.SaveData), SaveGame.Path, SaveGame.DoEncrypt);
        }

        public virtual void Delete() {
            Directory.Move(SaveGame.Path, $"{EncodingConstants.TrashPath}/{System.IO.Path.GetFileName(SaveGame.Path)}");
        }

    }
}

