using UnityEngine;

namespace Game.Battlefield.Map.Factories {

	public static class FieldFactory {
		
		public static Field[,] GenerateFields(int rows, int columns, float tileSize) {
			float halfSize = tileSize / 2;
			Field[,] fields = new Field[rows, columns];
			for (int x = 0; x < rows; x++) {
				for (int y = 0; y < columns; y++) {
					float xPos = x * tileSize + halfSize;
					float zPos = y * tileSize + halfSize;
					Field field = new Field {
						WayCost = 1,
						IsFree = true,
						Position = new Position(x, y),
						RealPosition = new Vector3(xPos, 0.1f, zPos)
					};
					fields[x, y] = field;
				}
			}
			return fields;
		}

	}
	
}