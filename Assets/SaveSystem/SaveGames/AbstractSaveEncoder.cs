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
        public abstract SaveData Load();

        public abstract void Save();

        public virtual void Delete() {
            Directory.Move(SaveGame.Path, $"{EncodingConstants.TrashPath}/{System.IO.Path.GetFileName(SaveGame.Path)}");
        }

    }
}

