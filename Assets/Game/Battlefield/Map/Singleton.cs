using UnityEngine;

namespace Game.Battlefield.Map {

	public sealed class Singleton {

		// https://csharpindepth.com/articles/singleton (Fifth version)
		private Singleton() {
		}

		public static Singleton Instance => Nested.instance;

		public void printTest() {
			Debug.Log("singleton test");
		}

		private class Nested {

			// Explicit static constructor to tell C# compiler
			// not to mark type as beforefieldinit
			static Nested() {
			}

			internal static readonly Singleton instance = new Singleton();

		}

	}

}