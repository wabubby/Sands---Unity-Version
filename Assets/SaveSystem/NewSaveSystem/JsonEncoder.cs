using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Wabubby {
    public static class JsonEncoder {
        private static string AESKey = "mewhenicantfindthekeyomoriDcolon";

        public static void Save(SaveDataContainer serializableData, string filePath) {
            string json = JsonUtility.ToJson(serializableData);

            Debug.Log(filePath);
            Debug.Log(json);

            File.WriteAllText(filePath, json);
        }

        public static SaveDataContainer Load(string filePath) {
            if (File.Exists(filePath)) {
                try {
                    string json = File.ReadAllText(filePath);
                    return JsonUtility.FromJson<SaveDataContainer>(json);

                } catch {
                    Debug.LogError($"Unable to load {filePath} please move or delete this file.");
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
                    Debug.LogError($"Unable to load {filePath} please move or delete this file.");
                    return null;
                }            
            } else {
                Debug.LogError($"Save file not found: {filePath}");
                return null;
            }
        }

    }
}
