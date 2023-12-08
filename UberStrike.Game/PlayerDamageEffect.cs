using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDamageEffect : MonoBehaviour {
	private const int WIDTH = 313;
	private const int HEIGHT = 43;
	private Vector3 _direction;

	[SerializeField]
	private float _duration;

	[SerializeField]
	private float _height;

	private float _offset;

	[SerializeField]
	private MeshRenderer _renderer;

	private bool _show;
	public float _speed;
	private Vector3 _start;
	private float _time;
	private Transform _transform;

	[SerializeField]
	private float _width;

	private Vector2[] FONT_METRICS = {
		new Vector2(0f, 42f),
		new Vector2(42f, 21f),
		new Vector2(63f, 29f),
		new Vector2(92f, 28f),
		new Vector2(120f, 34f),
		new Vector2(154f, 28f),
		new Vector2(182f, 33f),
		new Vector2(215f, 31f),
		new Vector2(246f, 34f),
		new Vector2(280f, 33f)
	};

	private void Awake() {
		_transform = transform;
		_start = _transform.position;
	}

	private void Update() {
		if (_show) {
			var num = _time * _speed - _offset;
			var vector = _direction * _time;
			vector.y = _height - num * num * _width;
			_time += Time.deltaTime;
			_transform.position = _start + vector;
			UpdateTransform();

			if (_time > _duration) {
				var color = _renderer.material.GetColor("_Color");
				color.a = Mathf.Lerp(color.a, 0f, Time.deltaTime * 3f);
				_renderer.material.SetColor("_Color", color);

				if (color.a < 0.2f) {
					Destroy(gameObject);
				}
			}
		}
	}

	public void Show(DamageInfo shot) {
		if (_width == 0f) {
			_width = 1f;
		}

		var meshFilter = gameObject.AddComponent<MeshFilter>();

		if (meshFilter) {
			meshFilter.mesh = CreateCharacterMesh(shot.Damage, FONT_METRICS, 313, 43);
		}

		UpdateTransform();
		_show = true;
		_offset = Mathf.Sqrt(_height / _width);
		_direction = UnityEngine.Random.onUnitSphere;
		_renderer.material = new Material(_renderer.material);
		StartCoroutine(StartEnableRenderer());
	}

	private IEnumerator StartEnableRenderer() {
		yield return new WaitForSeconds(0.1f);

		_renderer.enabled = true;
	}

	private void UpdateTransform() {
		var num = Vector3.Distance(_transform.position, GameState.Current.PlayerData.Position);
		var num2 = 0.003f + 0.0005f * num * LevelCamera.FieldOfView / 60f;
		_transform.localScale = new Vector3(num2, num2, num2);
		_transform.rotation = GameState.Current.PlayerData.HorizontalRotation;
	}

	private Mesh CreateCharacterMesh(int number, Vector2[] metrics, int width, int height) {
		var mesh = new Mesh();
		var text = Mathf.Abs(number).ToString();
		var list = new List<Vector3>();
		var list2 = new List<Vector2>();
		var list3 = new List<int>();
		var array = new Vector3[4];
		var array2 = new Vector2[4];

		int[] array3 = {
			0,
			1,
			2,
			0,
			2,
			3
		};

		var num = 0f;
		var num2 = 0f;

		for (var i = 0; i < text.Length; i++) {
			var num3 = text[i] - '0';

			if (num3 >= 0 && num3 < 10) {
				for (var j = 0; j < 6; j++) {
					list3.Add(array3[j] + list.Count);
				}

				array[0] = new Vector3(metrics[num3].x, 0f, 0f);
				array[1] = new Vector3(metrics[num3].x + metrics[num3].y, 0f, 0f);
				array[2] = new Vector3(metrics[num3].x + metrics[num3].y, height, 0f);
				array[3] = new Vector3(metrics[num3].x, height, 0f);
				array2[0] = new Vector2(array[0].x / width, array[0].y / height);
				array2[1] = new Vector2(array[1].x / width, array[1].y / height);
				array2[2] = new Vector2(array[2].x / width, array[2].y / height);
				array2[3] = new Vector2(array[3].x / width, array[3].y / height);
				list.AddRange(array);
				list2.AddRange(array2);
				num += metrics[num3].y;
			}
		}

		for (var k = 0; k < list.Count / 4; k++) {
			List<Vector3> list5;
			var list4 = (list5 = list);
			int num5;
			var num4 = (num5 = k * 4 + 1);
			var vector = list5[num5];
			list4[num4] = vector - new Vector3(list[k * 4].x + num / 2f - num2, height / 2);
			List<Vector3> list7;
			var list6 = (list7 = list);
			var num6 = (num5 = k * 4 + 2);
			vector = list7[num5];
			list6[num6] = vector - new Vector3(list[k * 4 + 3].x + num / 2f - num2, height / 2);
			List<Vector3> list9;
			var list8 = (list9 = list);
			var num7 = (num5 = k * 4 + 3);
			vector = list9[num5];
			list8[num7] = vector - new Vector3(list[k * 4 + 3].x + num / 2f - num2, height / 2);
			List<Vector3> list11;
			var list10 = (list11 = list);
			var num8 = (num5 = k * 4);
			vector = list11[num5];
			list10[num8] = vector - new Vector3(list[k * 4].x + num / 2f - num2, height / 2);
			num2 += list[k * 4 + 1].x - list[k * 4].x;
		}

		mesh.vertices = list.ToArray();
		mesh.uv = list2.ToArray();
		mesh.triangles = list3.ToArray();

		return mesh;
	}
}
