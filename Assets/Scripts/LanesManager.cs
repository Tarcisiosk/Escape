using UnityEngine;

public class LanesManager : MonoBehaviour {

	public class LaneTile {
		public enum Type {
			Empty,
			Monster,
			Treasure,
			Obstacle,
			Trap
		}
		public int position;
		public int size;
		public Type type;
		public PlayerManager.Lane lane;
		public LaneTile (int position, PlayerManager.Lane lane, Type type = Type.Empty, int size = 1) {
			this.position = position;
			this.lane = lane;
			this.type = type;
			this.size = size;
		}

		public Vector3 Bottom {
			get {
				return new Vector3 ((tileSize.x) * ((((float) lane - 1f) * 2f)), offset + tileSize.y * (position - 0.5f), 0f);
			}
		}

		public Vector3 Center {
			get {
				return new Vector3 ((tileSize.x) * ((((float) lane - 1f) * 2f)), offset + tileSize.y * (position + size / 2f - 0.5f), 0f);
			}
		}

		public Vector3 Top {
			get {
				return new Vector3 ((tileSize.x) * ((((float) lane - 1f) * 2f)), offset + tileSize.y * (position + size - 0.5f), 0f);
			}
		}
	}

	public int tilesBufferSize = 10;
	public static float offset = 0f;
	public float speed = 0.5f;
	public static Vector3 tileSize;
	private LaneTile[][] tiles = null;
	public LaneTile[] Left {
		get {
			return tiles[(int) PlayerManager.Lane.Left];
		}
	}
	public LaneTile[] Middle {
		get {
			return tiles[(int) PlayerManager.Lane.Middle];
		}
	}
	public LaneTile[] Right {
		get {
			return tiles[(int) PlayerManager.Lane.Right];
		}
	}
	public Color[] tileColors = new Color[5];

	void Awake () {
		if (tiles == null) {
			Debug.LogError ("INICIALIZE OS TILES PRIMEIRO!", this);
		}
	}

	void Update () {
		offset -= Time.deltaTime * speed;

		for (int lane = 0; lane < 3; lane++) {
			if (tiles[lane][0].Top.y < -tileSize.y * 1.5f) {
				var firstTile = tiles[lane][0];
				firstTile.position = tiles[lane][tilesBufferSize - 1].position + tiles[lane][tilesBufferSize - 1].size;
				firstTile.type = (LaneTile.Type) Random.Range (0, 5);
				firstTile.size = Random.Range (0, Random.Range (0, 3)) + 1;
				for (int i = 0; i < tilesBufferSize - 1; i++) {
					tiles[lane][i] = tiles[lane][i + 1];
				}
				tiles[lane][tilesBufferSize - 1] = firstTile;
			}
		}
	}

	void OnValidate () {
		if (tiles == null) {
			InitialSetup ();
		}
	}

	void OnDrawGizmosSelected () {
		if (tiles != null) {
			Vector3 size;
			size.z = 1f;
			size.x = tileSize.x * 2f;
			for (int lane = 0; lane < 3; lane++) {
				for (int i = 0; i < tilesBufferSize; i++) {
					Gizmos.color = tileColors[(int) tiles[lane][i].type];
					size.y = tiles[lane][i].size * tileSize.y;
					Gizmos.DrawWireCube (tiles[lane][i].Center, size);
					Gizmos.DrawLine (tiles[lane][i].Bottom, tiles[lane][i].Top);
				}
			}
		}
	}

	public void InitialSetup () {
		tileSize = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width / 3, Screen.height / 6, Camera.main.nearClipPlane));
		tileSize.y *= -1f;
		tileSize.x *= -1f;

		tiles = null;
		tiles = new LaneTile[3][];
		for (int lane = 0; lane < 3; lane++) {
			tiles[lane] = new LaneTile[tilesBufferSize];
			for (int i = 0; i < tilesBufferSize; i++) {
				tiles[lane][i] = new LaneTile (i - 1, (PlayerManager.Lane) lane);
			}
		}
	}
}