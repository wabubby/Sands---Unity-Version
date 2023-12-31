using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wabubby {
    public class SingleSaveGame : AbstractSaveGame
    {
        public SingleSaveGame(string path) : base(path) {
            Encoder = new SingleSaveEncoder(this);
            Load();
        }
        
    }
}
