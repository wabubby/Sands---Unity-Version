using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wabubby {
    public class SingleSaveGame : AbstractSaveGame
    {
        public SingleSaveGame(string path, bool doEncrypt) : base(path+".json", doEncrypt) {
            Encoder = new SingleSaveEncoder(this);
            Load();
        }
        
    }
}
