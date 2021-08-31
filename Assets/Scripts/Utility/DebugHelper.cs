using UnityEngine;

namespace Assets.Scripts.Utility {
    public static class DebugHelper {
        public static void Log(string message)
        {
            Debug.Log(message);
        }

        public static void Warn(GameObject obj, string message)
        {
            Log("[Warning] " + obj.name + ": " + message);
        }

        public static void Error(GameObject obj, string message)
        {
            Log("[Error] " + obj.name + ": " + message);
        }
    }
}