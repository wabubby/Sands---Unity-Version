using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using System.IO;

namespace Wabubby {
    public abstract class AbstractSaveEncoder {
        
        protected AbstractSaveGame SaveGame;

        public static SaveData DefaultSaveData => new SaveData("new-save");

        public AbstractSaveEncoder(AbstractSaveGame saveGame) {
            SaveGame = saveGame;
        }

        // loads from filepath
        public SaveData Load() {
            if (File.Exists(SaveGame.Path) || Directory.Exists(SaveGame.Path)) {
                return JsonEncoder.Load(SaveGame.Path).SaveData;
            } else {
                return DefaultSaveData;
            }
        }

        // saves to filepath
        public void Save() {
            JsonEncoder.Save(new SaveDataContainer(SaveGame.SaveData), SaveGame.Path);
        }

        // deletes savegame path
        public void Delete() {
            Directory.Move(SaveGame.Path, $"{EncodingConstants.TrashPath}/{System.IO.Path.GetFileName(SaveGame.Path)}");
        }

    }
}

