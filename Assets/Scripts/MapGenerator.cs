using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.U2D;
using System.Threading;

[ExecuteAlways]
public class MapGenerator : MonoBehaviour
{
	public SpriteShapeController GroundSpriteShapeController;
	private Spline spline;


	public float VerticalOffset = 0;
	public float VerticalMultiplier = 1;
	public float HorizontalMultiplier = 1;
	public float HorizontalOffset = 0;
	public float scaleMultiplier = 2;

	// параметры для генератора
	public float step = 1;
	public float x_Min = -5;
	public float x_Max = 50;
	public float y_Min = -5;
	private float stepOfPattern = 40f;

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.R)) Refresh();
	}
	private void Start()
	{
		spline = GroundSpriteShapeController.spline;
		Refresh();
	}

	[UnityEngine.ContextMenu("Refresh")]
	public void Refresh()
	{
		ResetSpline();
		GenerateMap();
	}

	private void ResetSpline()
	{
		spline.Clear();
		spline.InsertPointAt(0, Vector2.left * 10);
		spline.InsertPointAt(1, Vector2.left * 8);
		spline.InsertPointAt(2, Vector2.left * 6);
		spline.InsertPointAt(3, Vector2.left * 4);

	}

	private float GetSinusoidResult(float x)
	{
		return scaleMultiplier * (VerticalOffset + VerticalMultiplier * Mathf.Sin(HorizontalMultiplier * x + HorizontalOffset));
	}

	public void GenerateMap()
	{
		//счетчик для точек
		int k = 0;
		// счетчик для координат
		double x = 0;

		for (float i = x_Min; i < x_Max; i += step, k++, x += step)
		{
			Debug.Log("k = " + k + "; i = " + i + "; f(x) = " + GetSinusoidResult(i));

			if (x >= stepOfPattern)
			{
				ChangeParameters();
				x = 0;
			}

			if (k < 4)
				spline.SetPosition(k, new Vector3(i, GetSinusoidResult(i)));

			else
				spline.InsertPointAt(k, new Vector3(i, GetSinusoidResult(i)));

			spline.SetTangentMode(k, ShapeTangentMode.Continuous);
		}
		spline.InsertPointAt(k++, new Vector3(x_Max, y_Min));
		spline.InsertPointAt(k++, new Vector3(x_Min, y_Min));
	}
	private void ChangeParameters()
	{
		VerticalMultiplier = Random.Range(0.1f, 1.5f);
		HorizontalMultiplier = Random.Range(0.5f, 1);
	}
}

//#if UNITY_EDITOR
//[CustomEditor(typeof(MapGenerator))]
//public class CustomInspector : Editor
//{
//	public override void OnInspectorGUI()
//	{
//		base.OnInspectorGUI();
//		var generator = (MapGenerator) target;
//		EditorGUILayout.FloatField(generator.VerticalOffset);
//	}
//}
//#endif
