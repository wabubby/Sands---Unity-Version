using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wabubby {
    public class SaveDataOperator : MonoBehaviour {
        
        public AbstractSaveGame OperatingSaveGame;

        public SaveData SaveData => OperatingSaveGame.SaveData;

        public void Save() {
            OperatingSaveGame.Save();
        }

        public void Load() {
            OperatingSaveGame.Load();
        }

        public void Delete() {
            OperatingSaveGame.Delete();
        }

        public void SetOperatingSaveGame(SaveGameCollection saveGameCollection, int index) {
            OperatingSaveGame = saveGameCollection.SaveGames[index];
        }
        

        [ContextMenu("Test")]
        public void Test() {
            Debug.Log(SaveData.UserName);
        }

    }
}

