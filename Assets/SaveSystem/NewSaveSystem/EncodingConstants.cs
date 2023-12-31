using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Wabubby {
    public static class EncodingConstants {
        public static string AppPath => Application.persistentDataPath;
        public static string SavePath => AppPath + "/saves";
        public static string TrashPath => AppPath + "/old-saves";

    }
}
