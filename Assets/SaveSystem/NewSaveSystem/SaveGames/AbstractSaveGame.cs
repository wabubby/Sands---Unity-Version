using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wabubby {
    public abstract class AbstractSaveGame {
        public string Path;
        public SaveData SaveData;

        protected AbstractSaveEncoder Encoder;

        public AbstractSaveGame(string path) {
            Path = path;
            // Encoder = new ConcreteSaveEncoder(this);
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
