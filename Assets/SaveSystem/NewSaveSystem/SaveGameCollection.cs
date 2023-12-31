using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

namespace Wabubby {
    public enum SaveMethod {
        Single,
        Backup,
        Slots
    }

    public class SaveGameCollection : MonoBehaviour {

        public AbstractSaveGame CreateSaveGame(string path, SaveMethod saveMethod) {
            switch(saveMethod) {
                case SaveMethod.Single:
                    return new SingleSaveGame(path);
                default:
                    return new SingleSaveGame(path);
            }
        }

        public SaveMethod SaveMethod = SaveMethod.Single;

        private List<AbstractSaveGame> saveGames;
        public List<AbstractSaveGame> SaveGames { get {  if (saveGames == null) { LoadSaveGames();} return saveGames;} }
        
        /// <summary>
        /// Load all save games from the persistent SavePath directory into collection.
        /// </summary>
        [ContextMenu("Load SaveGames")]
        public void LoadSaveGames() {
            ResolveEncodingDirectories();

            saveGames = new List<AbstractSaveGame>();

            string[] saveDirPaths = Directory.GetDirectories(EncodingConstants.SavePath);
            string[] saveFilePaths = Directory.GetFiles(EncodingConstants.SavePath);
            System.Array.Sort(saveDirPaths);
            System.Array.Sort(saveFilePaths);

            foreach (string savePath in saveDirPaths) {
                saveGames.Add(CreateSaveGame(savePath, SaveMethod));
            }

            foreach (string savePath in saveFilePaths) {
                saveGames.Add(CreateSaveGame(savePath, SaveMethod));
            }
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
            SaveGames.Add(CreateSaveGame($"{EncodingConstants.SavePath}/new-save-{saveGames.Count}", SaveMethod));
        }

        private void Awake() {
            LoadSaveGames();
        }
        
    }

}
