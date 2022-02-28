using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MapGenerator : MonoBehaviour
{   
    public List<string>RoomNumbers{get; private set;}

    [SerializeField] private GameObject[] _4ways;
    [SerializeField] private GameObject[] _3ways;
    [SerializeField] private GameObject[] _corridors;
    [SerializeField] private GameObject[] _edges;
    [SerializeField] private GameObject[] _ends;
    [SerializeField] private Color[] _wallColors;
    [SerializeField] private Color[] _floorColors;
    [SerializeField] private int _mapMinSize;
    [SerializeField] private int _mapMaxSize;

    [SerializeField] private float _blockLength;
    private int _mapSize;
    private Color _wallColor;
    private Color _floorColor;
    private GameObject[,] _map;
    void Start(){
        _mapSize = (int)(Random.Range(_mapMinSize, _mapMaxSize) * PlayerPrefs.GetFloat("difficulty", 1f));
        _map = new GameObject[_mapSize, _mapSize];
        RoomNumbers = new List<string>();
        
        _floorColor = _floorColors[Random.Range(0, _floorColors.Length)];
        _wallColor = _wallColors[Random.Range(0, _wallColors.Length)];

        generate();

        GameManager.Instance.GameController.Setup();
    }

    void generate(){

        BlockMap blockMap = new BlockMap(_mapSize);
        
        for(int y = 0; y < _mapSize; y++){
            for(int x = 0; x < _mapSize; x++){
                Block block = blockMap.getBlock(x, y);

                if(block == null) continue;
                var blockData = block.getBlockType();
                GameObject tile = null;
                switch(blockData.Item1){
                    case BlockType.FourWay:
                        tile = _4ways[Random.Range(0, _4ways.Length)];
                        break;
                    case BlockType.Threeway:
                        tile = _3ways[Random.Range(0, _3ways.Length)];
                        break;
                    case BlockType.Corridor:
                        tile = _corridors[Random.Range(0, _corridors.Length)];
                        break;
                    case BlockType.Edge:
                        tile = _edges[Random.Range(0, _edges.Length)];
                        break;
                    case BlockType.End:
                        tile = _ends[Random.Range(0, _ends.Length)];
                        break;
                }

                tile = Instantiate(tile);
                tile.transform.position = new Vector3(x * _blockLength, 0, y * _blockLength);
                tile.transform.eulerAngles = new Vector3(0 , 90f * ((int)blockData.Item2 - 1), 0);

                RandomColorPicker[] colorPickers = tile.GetComponentsInChildren<RandomColorPicker>();

                foreach(RandomColorPicker colorPicker in colorPickers){
                     Mesh mesh = colorPicker.GetComponent<MeshFilter>().sharedMesh;
                    if(mesh.name.Equals("Door")) continue;

                    colorPicker.Colors = new Color[1];
                   
                    if(mesh.name.Equals("Floor")){
                        colorPicker.Colors[0] = _floorColor;
                    }else{
                        colorPicker.Colors[0] = _wallColor;
                    }
                }

                RoomNumber roomNumber = tile.GetComponentInChildren<RoomNumber>();
                if(roomNumber != null)
                    for(int i = 0; i < roomNumber.RoomNumbers.Length; i++){
                        roomNumber.RoomNumbers[i].text = getRandomRoomNumber();
                    }

                _map[x, y] = tile;
            }
        }

    }

    string getRandomRoomNumber(){
        string roomNumber = "";
        bool isExist;
        do{
            isExist = false;
            roomNumber = Random.Range(0, 4096).ToString("X");
            for(int i = 0; i < RoomNumbers.Count; i++){
                if(RoomNumbers[i].Equals(roomNumber)) isExist = true;
            }
        }while(isExist);

        RoomNumbers.Add(roomNumber);
        return roomNumber;
    }

    private class BlockMap{
        public Block[,] map {get; private set;}

        public BlockMap(int size){
            map = new Block[size, size];

            map[0, 0] = new Block(0, 0);

            generate(size);
        }

        void generate(int limit){
            Block current = map[0, 0];
            Direction currentDirection = current.getRandomDirection(limit, map);

            do{
                Block child = new Block();

                switch(currentDirection){
                    case Direction.Forward:
                        child = new Block(current.x, current.y + 1);
                        child.SetChild(current, Direction.Back);
                        break;
                    case Direction.Right:
                        child = new Block(current.x + 1, current.y);
                        child.SetChild(current, Direction.Left);
                        break;
                    case Direction.Back:
                        child = new Block(current.x, current.y - 1);
                        child.SetChild(current, Direction.Forward);
                        break;
                    case Direction.Left:
                        child = new Block(current.x - 1, current.y);
                        child.SetChild(current, Direction.Right);
                        break;
                    
                }
                child.SetParent(current);
                current.SetChild(child, currentDirection);

                current = child;
                map[current.x, current.y] = current;

                currentDirection = current.getRandomDirection(limit, map);
                while(currentDirection == Direction.None){
                    current = current.GetParent();
                    if(current == null) break;
                    currentDirection = current.getRandomDirection(limit, map);
                }

                if(current == null) break;

            }while(current.getRandomDirection(limit, map) != Direction.None );
        }

        public Block getBlock(int x, int y){
            return map[x, y];
        }

    }

    private class Block{
        Block parent;
        Block forward;
        Block right;
        Block back;
        Block left;

        public int x {get; private set;}
        public int y {get; private set;}

        public Block(){}
        public Block(int x, int y){
            this.x = x;
            this.y = y;
        }

        public Direction getRandomDirection(int limit, Block[,] map){
            List<Direction> empty = new List<Direction>();

            if(forward == null && y + 1 < limit){
                if(checkAround(x, y + 1, map, limit)) empty.Add(Direction.Forward);
            }

            if(right == null && x + 1 < limit){
                if(checkAround(x + 1, y, map, limit)) empty.Add(Direction.Right);
            }

            if(back == null && y - 1 >= 0){
                if(checkAround(x, y - 1, map, limit)) empty.Add(Direction.Back);
            }
            
            if(left == null && x - 1 >= 0){
                if(checkAround(x - 1, y, map, limit)) empty.Add(Direction.Left);
            }

            if(empty.Count == 0) return Direction.None;

            return empty[Random.Range(0, empty.Count)];
        }

        bool checkAround(int x, int y, Block[,] map, int limit){
            int count = 0;
            if(x + 1 < limit){
                if(map[x + 1, y] != null) count++;
            }

            if(y + 1 < limit){
                if(map[x, y + 1] != null) count++;
            }

            if(x - 1 >= 0){
                if(map[x - 1, y] != null) count++;
            }

            if(y - 1 >= 0){
                if(map[x, y - 1] != null) count++;
            }
            
            return (count<=1) && map[x, y] == null;
        }

        public (BlockType,Direction) getBlockType(){
            BlockType blockType = BlockType.None;
            Direction direction = Direction.None;

            int wallCount = 0;
            Direction threeWayDirection = Direction.None;
            Direction endDirection = Direction.None;

            if(forward == null){
                wallCount++;
                threeWayDirection = Direction.Back;
            }else{
                endDirection = Direction.Forward;
            }

            if(right == null){
                wallCount++;
                threeWayDirection = Direction.Left;
            }else{
                endDirection = Direction.Right;
            }

            if(back == null){
                wallCount++;
                threeWayDirection = Direction.Forward;
            }else{
                endDirection = Direction.Back;
            }
            
            if(left == null){
                wallCount++;
                threeWayDirection = Direction.Right;
            }else{
                endDirection = Direction.Left;
            }

            switch(wallCount){
                case 0:
                    blockType = BlockType.FourWay;
                    direction = Direction.Forward;
                    break;
                case 1:
                    blockType = BlockType.Threeway;
                    direction = threeWayDirection;
                    break;
                case 2:
                    if((forward == null && back == null) || (right == null && left == null)){
                        blockType = BlockType.Corridor;
                        direction = Direction.Forward;
                        if(forward == null) direction = Direction.Right;
                    }else{
                        blockType = BlockType.Edge;
                        direction = Direction.Back;
                        if(back == null){
                            if(right == null){
                                direction = Direction.Left;
                            }

                            if(left == null){
                                direction = Direction.Forward;
                            }
                        }
                        if(forward == null && left == null) direction = Direction.Right;
                    }
                    break;
                case 3:
                    blockType = BlockType.End;
                    direction = endDirection;
                    break;

            }

            return (blockType, direction);
        }

        public Block GetParent(){
            return parent;
        }

        public void SetParent(Block parent){
            this.parent = parent;
        }

        public void SetChild(Block child, Direction direction){
            switch(direction){
                case Direction.Forward:
                    forward = child;
                    break;
                case Direction.Right:
                    right = child;
                    break;
                case Direction.Back:
                    back = child;
                    break;
                case Direction.Left:
                    left = child;
                    break;
            }
        }

    }
    private enum Direction{
        None,
        Forward,
        Right,
        Back,
        Left
    }

    private enum BlockType{
        None,
        FourWay,
        Threeway,
        Corridor,
        Edge,
        End
    }
}
