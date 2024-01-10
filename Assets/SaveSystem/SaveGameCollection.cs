using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

namespace Wabubby {
    public class SaveGameCollection : MonoBehaviour {

        public SaveMethod SaveMethod = SaveMethod.Single;
        public bool DoEncrypt = false;

        private List<AbstractSaveGame> saveGames;
        public List<AbstractSaveGame> SaveGames { get {  if (saveGames == null) { LoadSaveGames();} return saveGames;} }
        
        /// <summary>
        /// Load all save games from the persistent SavePath directory into collection.
        /// </summary>
        [ContextMenu("Load SaveGames")]
        public void LoadSaveGames() {
            ResolveEncodingDirectories();

            saveGames = new List<AbstractSaveGame>();

            // look for savegame directories
            string[] saveDirPaths = Directory.GetDirectories(EncodingConstants.SavePath);
            System.Array.Sort(saveDirPaths);
            foreach (string savePath in saveDirPaths) { saveGames.Add(SaveGameLibrary.CreateSaveGame(savePath, SaveMethod, DoEncrypt)); }
        }

        private void ResolveEncodingDirectories() {
            WabubbyIO.ResolveDirectory(EncodingConstants.SavePath);
            WabubbyIO.ResolveDirectory(EncodingConstants.TrashPath);
        }

        /// <summary>
        /// removes all instances of savegames in this class. does not acccount for instances in other classes.
        /// </summary>
        public void DeLoadSaveGames() {
            saveGames = null;
        }

        /// <summary>
        /// Saves all savegames in the collection.
        /// </summary>
        [ContextMenu("Save SaveGames")]
        public void SaveSaveGames() {
            foreach (AbstractSaveGame saveGame in saveGames) {
                saveGame.Save();
                Debug.Log($"savegame path: {saveGame.Path}");
                Debug.Log($"savegame doEncrypt flag: {saveGame.DoEncrypt}");
            }
        }

        /// <summary>
        /// Add savegame into collection (no persistent path until explicitly saved.)
        /// </summary>
        [ContextMenu("Add SaveGame")]
        public void AddSaveGame() {
            SaveGames.Add(SaveGameLibrary.CreateSaveGame($"{EncodingConstants.SavePath}/new-save-{saveGames.Count}", SaveMethod, DoEncrypt));
            SaveGames[SaveGames.Count-1].SaveData.UserName = $"new-save-{saveGames.Count-1}";
        }

        /// <summary>
        /// Prrint all savedata contents for debugging
        /// </summary>
        [ContextMenu("Log SaveDatas")]
        public void LogSaveDatas() {
            foreach (AbstractSaveGame saveGame in saveGames) {
                Debug.Log(saveGame.SaveData.ToString());
            }
        }


        private void Awake() {
            LoadSaveGames();
        }
        
    }

}
