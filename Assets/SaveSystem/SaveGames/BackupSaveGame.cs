using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Wabubby;

namespace Wabubby {
    public class BackupSaveGame : AbstractSaveGame {
        public BackupSaveGame(string path, bool doEncrypt=false) : base(path, doEncrypt) {
            Encoder = new BackupSaveEncoder(this);
            Load();
        }
        
    }
}
