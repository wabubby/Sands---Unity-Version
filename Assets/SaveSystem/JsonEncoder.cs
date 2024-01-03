using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Wabubby {
    public static class JsonEncoder {
        private static string AESKey = "mewhenicantfindthekeyomoriDcolon";

        /// <summary>
        /// Serialize an SDC into given filepath. Will override filepath if it already exists.
        /// </summary>
        /// <param name="serializableData"></param>
        /// <param name="filePath"></param>
        /// <param name="doEncryptFlag"></param>
        public static void Save(SaveDataContainer serializableData, string filePath, bool doEncryptFlag=false) {
            if (doEncryptFlag) {
                SaveEncrypted(serializableData, filePath);
            } else {
                SaveUnencrypted(serializableData, filePath);
            }
        }


        /// <summary>
        /// Load an SDC from given filepath.
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="doEncryptFlag"></param>
        /// <returns></returns>
        public static SaveDataContainer Load(string filePath, bool doEncryptFlag=false) {
            if (doEncryptFlag) {
                return LoadEncrypted(filePath);
            } else {
                return LoadUnencrypted(filePath);
            }
        }

        public static void SaveUnencrypted(SaveDataContainer serializableData, string filePath) {
            string json = JsonUtility.ToJson(serializableData);
            File.WriteAllText(filePath, json);
        }

        public static SaveDataContainer LoadUnencrypted(string filePath) {
            if (File.Exists(filePath)) {
                try {
                    string json = File.ReadAllText(filePath);
                    return JsonUtility.FromJson<SaveDataContainer>(json);

                } catch {
                    Debug.LogError($"Unable to load {filePath} please move or delete this file and restart.");
                    return null;
                }            
            } else {
                Debug.LogError($"Save file not found: {filePath}");
                return null;
            }
        }
        
        public static void SaveEncrypted(SaveDataContainer serializableData, string filePath) {
            string json = JsonUtility.ToJson(serializableData);

            AESEncoder crypto = new AESEncoder();
            byte[] soup = crypto.Encrypt(json, AESKey);

            File.WriteAllBytes(filePath, soup);
        }

        public static SaveDataContainer LoadEncrypted(string filePath) {
            if (File.Exists(filePath)) {
                try {
                    byte[] soup = File.ReadAllBytes(filePath);

                    AESEncoder crypto = new AESEncoder();
                    string json = crypto.Decrypt(soup, AESKey);

                    return JsonUtility.FromJson<SaveDataContainer>(json);

                } catch {
                    Debug.LogError($"Unable to load {filePath} please move or delete this file and restart.");
                    return null;
                }            
            } else {
                Debug.LogError($"Save file not found: {filePath}");
                return null;
            }
        }

    }
}
