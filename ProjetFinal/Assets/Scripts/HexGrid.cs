using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

	public int width = 6;
	public int height = 6;

	public Color defaultColor = Color.white;

	public HexCell cellPrefab;
	public Text cellLabelPrefab;

	HexCell[] cells;

	Canvas gridCanvas;
	HexMesh hexMesh;

	public void Generate () {

		gridCanvas = GetComponentInChildren<Canvas>();
		//hexMesh = GetComponentInChildren<HexMesh>();


		if (gridCanvas.transform.childCount > 0)
		{
			Debug.LogError("Supprimez les enfants actuels pour générer une nouvelle map");
			return;
		}

		//hexMesh.Generate();

		cells = new HexCell[height * width];

		for (int z = 0, i = 0; z < width; z++) {
			for (int x = 0; x < height; x++) {
				CreateCell(x, z, i++);
			}
		}

		//hexMesh.Triangulate(cells);
	}

	public void ColorCell (Vector3 position, Color color) {
		position = transform.InverseTransformPoint(position);
		HexCoordinates coordinates = HexCoordinates.FromPosition(position);
		int index = coordinates.X + coordinates.Z * width + coordinates.Z / 2;
		HexCell cell = cells[index];
		cell.color = color;
		hexMesh.Triangulate(cells);
	}

	void CreateCell (int x, int z, int i) {
		Vector3 position;
		position.x = z * (HexMetrics.outerRadius * 1.36f);
		position.y = 0f;
		position.z = (x + z * 0.5f - z / 2) * (HexMetrics.innerRadius * 1.81f);

		HexCell cell = cells[i] = Instantiate<HexCell>(cellPrefab);
		cell.transform.SetParent(transform, false);
		cell.transform.localPosition = position;
		cell.coordinates = HexCoordinates.FromOffsetCoordinates(x, z);
		cell.color = defaultColor;

		Text label = Instantiate<Text>(cellLabelPrefab);
		label.rectTransform.SetParent(gridCanvas.transform, false);
		label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
		label.text = cell.coordinates.ToStringOnSeparateLines();
	}
}