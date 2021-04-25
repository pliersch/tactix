using System.Collections.Generic;

namespace Game.Units {

	public static class UnitFactory {
		
		// TODO: arrays or lists for armies and units?

		public static Army[] InitArmies(int count, int length) {
			Army[] armies = new Army[count];
			for (int i = 0; i < count; i++) {
				armies[i] = InitUnits(length);
			}

			return armies;
		}

		private static Army InitUnits(int length) {
			List<Unit> units = new List<Unit>(length);
			Army army = new Army(units);
			for (int i = 0; i < length; i++) {
				Unit unit = new Tank(army);
				units.Add(unit);
			}

			return army;
		}

	}

}