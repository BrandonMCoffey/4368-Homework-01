using UnityEngine;

namespace Assets.Scripts.Utility {
    public static class DebugHelper {
        public static void Log(string message)
        {
            Debug.Log(message);
        }

        public static void Warn(string message)
        {
            Log("[Warning] " + message);
        }

        public static void Warn(GameObject obj, string message)
        {
            Warn(obj.name + ": " + message);
        }

        public static void Error(string message)
        {
            Log("[Error] " + message);
        }

        public static void Error(GameObject obj, string message)
        {
            Error(obj.name + ": " + message);
        }
    }
}