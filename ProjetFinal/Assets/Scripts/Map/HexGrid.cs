using UnityEngine;
using UnityEngine.UI;

public class HexGrid : MonoBehaviour {

	public int width = 6;
	public int height = 6;

	public HexCell cellPrefab;
	public Text cellLabelPrefab;

	HexCell[] cells;

	Canvas gridCanvas;

	public void Generate () {

        //gridCanvas = GetComponentInChildren<Canvas>();

        //if (gridCanvas.transform.childCount > 0)
        //{
        //    Debug.LogError("Supprimez les enfants actuels pour générer une nouvelle map");
        //    return;
        //}


        cells = new HexCell[height * width];

		for (int z = 0, i = 0; z < width; z++) {
			for (int x = 0; x < height; x++) {
				CreateCell(x, z, i++);
			}
		}

		
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

        //Text label = Instantiate<Text>(cellLabelPrefab);
        //label.rectTransform.SetParent(gridCanvas.transform, false);
        //label.rectTransform.anchoredPosition = new Vector2(position.x, position.z);
        //label.text = cell.coordinates.ToStringOnSeparateLines();
    }
}