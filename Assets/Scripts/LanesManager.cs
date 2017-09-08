using UnityEngine;

public class LanesManager : MonoBehaviour {

    public class LaneTile
    {
		public enum Type{
			Empty,
			Monster,
			Treasure,
			Obstacle,
			Trap
		}
		public int position;
		public int size;
		public Type type;
        public LaneTile(int position, Type type = Type.Empty, int size = 1)
        {
			this.position = position;
			this.type = type;
			this.size = size;
        }
    }

	public int tilesBufferSize = 10;
	public float offset = 0f;
    public float speed = 0.5f;
	private Vector3 _tileSize;
	private LaneTile[][] tiles = null;
	public LaneTile[] Left {get{
		return tiles[(int)PlayerManager.Lane.Left];
	}}
	public LaneTile[] Middle {get{
		return tiles[(int)PlayerManager.Lane.Middle];
	}}
	public LaneTile[] Right {get{
		return tiles[(int)PlayerManager.Lane.Right];
	}}
	public Color[] tileColors = new Color[5];
	

    void Awake () {
		if(tiles == null){
			Debug.LogError("INICIALIZE OS TILES PRIMEIRO!", this);
		}
	}

	void Update()
	{
		offset -= Time.deltaTime * speed;

		for(int lane = 0; lane < 3; lane++){
			var tileTopPos = offset + _tileSize.y*(tiles[lane][0].position+tiles[lane][0].size);
			if(tileTopPos < -_tileSize.y){
				var firstTile = tiles[lane][0];
				firstTile.position = tiles[lane][tilesBufferSize-1].position+tiles[lane][tilesBufferSize-1].size;
				firstTile.type = (LaneTile.Type)Random.Range(0, 5);
				for(int i = 0; i < tilesBufferSize-1; i++){
					tiles[lane][i] = tiles[lane][i+1];
				}
				tiles[lane][tilesBufferSize-1] = firstTile;
			}
		}
	}

	void OnValidate()
	{
		if(tiles == null){
			InitialSetup();
		}
	}
	
	void OnDrawGizmosSelected()
	{
		if(tiles != null){
			Vector3 size, pos;
			size.z = 1f;
			size.x = _tileSize.x*2f;
			pos.z = _tileSize.z;
			for(int lane = 0; lane < 3; lane++){
				//Gizmos.color = lane == 0 ? Color.red : lane == 1 ? Color.green : Color.blue;
				pos.x = (_tileSize.x)*((lane-1f)*2f);
				for(int i = 0; i < tilesBufferSize; i++){
					Gizmos.color = tileColors[(int)tiles[lane][i].type];
					pos.y = offset + _tileSize.y*tiles[lane][i].position;
					size.y = tiles[lane][i].size*_tileSize.y;
					Gizmos.DrawCube(pos, size);
					Gizmos.DrawWireCube(pos, size);
				}
			}
		}
	}

	public void InitialSetup(){
		_tileSize = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/3, Screen.height/6, Camera.main.nearClipPlane) );
		_tileSize.y *= -1f;
		_tileSize.x *= -1f;
		tiles = null;
		tiles = new LaneTile[3][];
		for(int lane = 0; lane < 3; lane++){
			tiles[lane] = new LaneTile[tilesBufferSize];
			int size;
			int pos = -1;
			for(int i = 0; i < tilesBufferSize; i++){
				size = 1; //Random.Range(0, 3)+1;
				tiles[lane][i] = new LaneTile(pos, LaneTile.Type.Empty, size);
				pos += size;
			}
		}
	}
}
