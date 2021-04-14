namespace Game.Battlefield.Map {

	public struct Position {

		public int x;
		public int z;

		public Position(int x, int z) {
			this.x = x;
			this.z = z;

		}

		public override string ToString() {
			return x + " | " + z;
		}

	}

}