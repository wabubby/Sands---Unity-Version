using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wabubby;
using System.IO;

namespace Wabubby {
    public class BackupSaveEncoder : AbstractSaveEncoder {
        private static string currentSaveIdentifier = "current-save";
        private static string backupSaveIdentifier = "backup-save";

        private string currentSavePath => $"SaveGame.Path/{currentSaveIdentifier}";
        private string backupSavePath => $"SaveGame.Path/{backupSaveIdentifier}";

        public BackupSaveEncoder(AbstractSaveGame saveGame) : base(saveGame) {  }

        public override SaveData Load() {
            // if the load throws an error
            if (Directory.Exists(SaveGame.Path)) {
                SaveData temp = JsonEncoder.Load(currentSavePath, SaveGame.DoEncrypt).SaveData;
                if (temp != null) {
                    return temp;
                }
                // load the backup save, log a message
                Debug.Log("current save failed. loding backup instead...");
                return JsonEncoder.Load(backupSavePath, SaveGame.DoEncrypt).SaveData;
            } else {
                return new SaveData();
            }
        }

        public override void Save() {
            WabubbyIO.ResolveDirectory(SaveGame.Path);
            // move current to backup
            if (File.Exists(currentSavePath)) {
                File.Move(currentSavePath, backupSavePath);
            }
            // save SaveData to current
            JsonEncoder.Save(new SaveDataContainer(SaveGame.SaveData), currentSavePath, SaveGame.DoEncrypt);
        }
    }
}