using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

namespace Wabubby {
    public static class WabubbyIO {
    public static bool IsFileNameValid(string filename) { return filename.IndexOfAny(Path.GetInvalidFileNameChars()) == -1; }

    public static bool  ResolveDirectory(string path) {
        // returns true if path existed, false otherwise
            if (!Directory.Exists(path)) {
                Directory.CreateDirectory(path);
                return false;
            }
            return true;
        }
    }
}
