using UnityEngine;

namespace Game.Util {

	public static class Logger {

		public static void Log(string msg, object obj) {
			var output = JsonUtility.ToJson(obj, true);
			Debug.Log(msg + ": " + output);
		}

	}

}