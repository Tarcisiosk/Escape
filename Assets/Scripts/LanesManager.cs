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
		public Vector2 size;
		public Type type;
        public LaneTile(Type type = Type.Empty, Vector2? size = null)
        {
			this.type = type;
			this.size = size ?? Vector2.one;
        }
    }

	public int tilesBufferSize = 10;
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

    void Awake () {
		if(tiles == null){
			Debug.LogError("INICIALIZE OS TILES PRIMEIRO!", this);
		}
	}
	
	void OnDrawGizmos()
	{
		var size = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/3, Screen.height/6, Camera.main.nearClipPlane) );
		size.y *= -1f;
		if(tiles != null){
			for(int lane = 0; lane < 3; lane++){
				float lastY = -size.y;
				Gizmos.color = lane == 0 ? Color.red : lane == 1 ? Color.green : Color.blue;
				for(int i = 0; i < tilesBufferSize; i++){
					var pos = Camera.main.ScreenToWorldPoint( new Vector3(Screen.width/6+(Screen.width/6*lane*2), Screen.height/6, Camera.main.nearClipPlane) );
					pos.y = lastY;
					Gizmos.DrawWireCube(pos, new Vector3(tiles[lane][i].size.x*size.x*2f, tiles[lane][i].size.y*size.y, 1f));
					lastY += size.y;
				}
			}
		}
	}

	public void InitialSetup(){
		tiles = null;
		tiles = new LaneTile[3][];
		for(int lane = 0; lane < 3; lane++){
			tiles[lane] = new LaneTile[tilesBufferSize];
			for(int i = 0; i < tilesBufferSize; i++){
				tiles[lane][i] = new LaneTile();
			}
		}
	}
}
