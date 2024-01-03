using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wabubby {
    public abstract class AbstractSaveGame {
        public string Path;
        public SaveData SaveData;
        public bool DoEncrypt;
        public bool isCorrupted => SaveData==null;

        protected AbstractSaveEncoder Encoder;

        public AbstractSaveGame(string path, bool doEncrypt) {
            Path = path;
            DoEncrypt = doEncrypt;
            // Encoder = new ConcreteSaveEncoder(this);
            // Load();
        }

        public virtual void Save() {
            Encoder.Save();
        }

        public virtual void Load() {
            SaveData = Encoder.Load();
        }

        public virtual void Delete() {
            Encoder.Delete();
        }
        
    }
}
