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
            
            // look for savegame paths
            string[] saveFilePaths = Directory.GetFiles(EncodingConstants.SavePath);
            System.Array.Sort(saveFilePaths);
            foreach (string savePath in saveFilePaths) { saveGames.Add(SaveGameLibrary.CreateSaveGame(savePath, SaveMethod, DoEncrypt)); }
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
                Debug.Log($"test savegame path(after save): {saveGame.Path}");
            }
        }

        /// <summary>
        /// Add savegame into collection (no persistent path until explicitly saved.)
        /// </summary>
        [ContextMenu("Add SaveGame")]
        public void AddSaveGame() {
            SaveGames.Add(SaveGameLibrary.CreateSaveGame($"{EncodingConstants.SavePath}/new-save-{saveGames.Count}", SaveMethod, DoEncrypt));
        }

        private void Awake() {
            LoadSaveGames();
        }
        
    }

}
