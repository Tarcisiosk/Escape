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
        public int posX;
        public Type type;
        public PlayerManager.Lane lane;

        public GameObject gameObject;
        public SpriteRenderer spriteRenderer;
        public LaneTile(int position, PlayerManager.Lane lane, Type type = Type.Empty, int size = 1, int? posX = null) {
            this.position = position;
            this.lane = lane;
            this.type = type;
            this.size = size;
            this.posX = posX ?? (int) lane - 1;
        }

        public bool CreateGO(string name, Transform parent, Sprite sprite){
            if(gameObject != null) return false;

            gameObject = new GameObject(name);
            gameObject.transform.SetParent(parent);
            spriteRenderer = gameObject.AddComponent<SpriteRenderer>();
            spriteRenderer.sprite = sprite;
            spriteRenderer.drawMode = SpriteDrawMode.Tiled;
            spriteRenderer.tileMode = SpriteTileMode.Adaptive;
            spriteRenderer.size = new Vector2(tileSize.x*2f, tileSize.y);
            gameObject.transform.position = Center;

            return true;
        }

        public Vector3 Bottom {
            get {
                return new Vector3((tileSize.x) * ((((float) posX) * 2f)), offset + tileSize.y * (position - 0.5f), 0f);
            }
        }

        public Vector3 Center {
            get {
                return new Vector3((tileSize.x) * ((((float) posX) * 2f)), offset + tileSize.y * (position + size / 2f - 0.5f), 0f);
            }
        }

        public Vector3 Top {
            get {
                return new Vector3((tileSize.x) * ((((float) posX) * 2f)), offset + tileSize.y * (position + size - 0.5f), 0f);
            }
        }
    }

    public int tilesBufferSize = 10;
    private int _lastTileIdx;
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
    public WeightedLaneType laneTypeWeight = new WeightedLaneType();
    public WeightedInt sizeWeight = new WeightedInt();
    public bool isAligned {
        get {
            return (Left[_lastTileIdx].size + Left[_lastTileIdx].position) == (Middle[_lastTileIdx].size + Middle[_lastTileIdx].position) &&
                (Right[_lastTileIdx].size + Right[_lastTileIdx].position) == (Middle[_lastTileIdx].size + Middle[_lastTileIdx].position);
        }
    }

    public bool keepAligned = false;

    public GameObject[] lanesGO = new GameObject[3];

    public Sprite brickSprite = null;

    void Start() {
        if (tiles == null) {
            InitialSetup();
        }
    }

    void Update() {
        offset -= Time.deltaTime * speed;

        var maxH = Mathf.Max(Left[_lastTileIdx].position + Left[_lastTileIdx].size, Middle[_lastTileIdx].position + Middle[_lastTileIdx].size, Right[_lastTileIdx].position + Right[_lastTileIdx].size);

        for (int lane = 0; lane < 3; lane++) {
            if (tiles[lane][0].Top.y < -tileSize.y * 1.5f) {
                var firstTile = tiles[lane][0];
                firstTile.gameObject.name = (int.Parse(tiles[lane][_lastTileIdx].gameObject.name)+1).ToString();
                firstTile.position = tiles[lane][_lastTileIdx].position + tiles[lane][_lastTileIdx].size;
                firstTile.type = laneTypeWeight.GetRandomItem();
                if (keepAligned) {
                    if (firstTile.position == maxH) {
                        maxH += sizeWeight.GetRandomItem();
                    }
                    firstTile.size = maxH - firstTile.position;
                } else {
                    firstTile.size = sizeWeight.GetRandomItem();
                }
                firstTile.spriteRenderer.size = new Vector2(tileSize.x*2f, tileSize.y*firstTile.size);
                //if(offset < -5f) firstTile.posX -= 1;
                for (int i = 0; i < _lastTileIdx; i++) {
                    tiles[lane][i] = tiles[lane][i + 1];
                }
                tiles[lane][_lastTileIdx] = firstTile;
            }
            for(int tile = 0; tile < tilesBufferSize; tile++){
                tiles[lane][tile].gameObject.transform.position = tiles[lane][tile].Center;
            }
        }
    }

    void OnDrawGizmosSelected() {
        if (tiles != null) {
            Vector3 size;
            size.z = 1f;
            size.x = tileSize.x * 2f;
            for (int lane = 0; lane < 3; lane++) {
                for (int i = 0; i < tilesBufferSize; i++) {
                    Gizmos.color = tileColors[(int) tiles[lane][i].type];
                    size.y = tiles[lane][i].size * tileSize.y;
                    Gizmos.DrawWireCube(tiles[lane][i].Center, size);
                    Gizmos.DrawLine(tiles[lane][i].Bottom, tiles[lane][i].Top);
                }
            }
        }
    }

    public void InitialSetup() {
        offset = 0f;

        tileSize = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width / 3, Screen.height / 6, Camera.main.nearClipPlane));
        tileSize.y *= -1f;
        tileSize.x *= -1f;

        _lastTileIdx = tilesBufferSize - 1;

        tiles = null;
        tiles = new LaneTile[3][];
        for (int lane = 0; lane < 3; lane++) {
            if (lanesGO[lane] != null) DestroyImmediate(lanesGO[lane]);
            lanesGO[lane] = new GameObject(((PlayerManager.Lane) lane).ToString());
            lanesGO[lane].transform.SetParent(this.transform);

            tiles[lane] = new LaneTile[tilesBufferSize];
            for (int i = 0; i < tilesBufferSize; i++) {
                tiles[lane][i] = new LaneTile(i - 1, (PlayerManager.Lane) lane);
                tiles[lane][i].CreateGO(i.ToString(), lanesGO[lane].transform, brickSprite);
            }
        }
    }
}
