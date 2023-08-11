using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Tool
{
    [Serializable]
    public class ManagerLog
    {
        private static int LogNumber { get; set; }
        public static bool IsDebug { get; set; } = true;

        public static void Log(object obj)
        {
            if (IsDebug)
            {
                Debug.Log($"{LogNumber}:{obj}");
                LogNumber++;
            }
        }
        
        public static void LogWarning(object obj)
        {
            if (IsDebug)
            {
                Debug.LogWarning($"{LogNumber}:{obj}");
                LogNumber++;
            }
        }
        
        public static void LogError(object obj)
        {
            if (IsDebug)
            {
                Debug.LogError($"{LogNumber}:{obj}");
                LogNumber++;
            }
        }
    }
}
