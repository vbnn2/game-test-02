using System.Collections.Generic;
using UnityEngine;

namespace Game
{
	public class HexGizmoTest : MonoBehaviour
	{
		[SerializeField]
		private bool isPointy;

		[SerializeField]
		private int radius;

		[SerializeField]
		private float size;

		[SerializeField]
		private Vector2 origin;

		private void OnDrawGizmos()
		{
			var layout = new HexLayout(
				isPointy ? HexOrientation.kPointy : HexOrientation.kFlat,
				new Vector2(size, size),
				origin
			);

			var hexGrid = new HexGrid();
			hexGrid.InitHexagon(radius);

			// foreach (var pos in hexGrid.Keys)
			// {
			// 	Gizmos.DrawWireSphere(layout.ToWorldPos(pos), layout.size.x * Mathf.Sqrt(3) / 2f);
			// }

			// var listPos = new List<HexPos>
			// {
			// 	HexPos.kZero,
			// 	HexPos.kZero.Neighbor(0),
			// 	HexPos.kZero.Neighbor(1),
			// 	HexPos.kZero.Neighbor(2),
			// 	HexPos.kZero.Neighbor(3),
			// 	HexPos.kZero.Neighbor(4),
			// 	HexPos.kZero.Neighbor(5)
			// };


			// foreach (var pos in listPos)
			// {
			// 	Gizmos.DrawWireSphere(layout.ToWorldPos(pos), layout.size.x * Mathf.Sqrt(3) / 2f);
			// }
		}
	}
}