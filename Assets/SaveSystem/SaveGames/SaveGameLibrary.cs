using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
If you implement your own ConcreteSaveGame classes, you can modify this file to accomodate.
*/

namespace Wabubby {
    public static class SaveGameLibrary
    {
        public static AbstractSaveGame CreateSaveGame(string path, SaveMethod saveMethod, bool doEncrypt) {
            switch(saveMethod) {
                case SaveMethod.Single:
                    return new SingleSaveGame(path, doEncrypt);
                case SaveMethod.Backup:
                    return new BackupSaveGame(path, doEncrypt);
                case SaveMethod.Slots:
                    return new SingleSaveGame(path, doEncrypt);
                default:
                    return new SingleSaveGame(path, doEncrypt);
            }
        }

    }
    
    public enum SaveMethod {
        Single,
        Backup,
        Slots
    }
}
