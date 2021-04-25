namespace Game.Battlefield.Map {

	public readonly struct Position {

		public readonly int x;
		public readonly int z;

		public Position(int x, int z) {
			this.x = x;
			this.z = z;

		}

		public override string ToString() {
			return x + " | " + z;
		}

	}

}