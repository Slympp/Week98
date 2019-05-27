using System.Collections.Generic;
using Entities.Player;
using Game.Level;
using Settings;
using UnityEngine;

namespace Game {
    public class LevelManager : MonoBehaviour {

        [SerializeField] private float Offset;
        [SerializeField] private float RoomSize = 15f;

        private readonly string RoomPath = "Prefabs/Level/Room";
        private readonly string BridgePath = "Prefabs/Level/Bridges/Bridge_";
        private readonly string WallsPath = "Prefabs/Level/Walls/Wall_";
        private readonly string DoorsPath = "Prefabs/Level/Doors/Door_";
        
        private GameManager _gameManager;
        private LevelSettings _settings;
        private Room[,] _rooms;

        private GameObject _level;
        
        void Awake() {

            _gameManager = GameManager.Get();
            
            if (_level != null)
                DestroyImmediate(_level);
            else {
                _level = new GameObject("Level");
            }
        }

        void Start() {
            if ((_settings = _gameManager.GetCurrentLevelSettings()) != null) {
                if (_settings.Procedural) {
                    Room currentRoom = GenerateLevel();
                    BuildLevel(currentRoom);
//                    Print();
                }
            }
        }
        
        private Room GenerateLevel() {
            int gridSize = _settings.NumberOfRooms * 2 + 1;
 
            _rooms = new Room[gridSize, gridSize];
 
            Vector2Int initialRoomCoordinate = new Vector2Int (gridSize / 2 - 1, gridSize / 2 - 1);
 
            Queue<Room> roomsToCreate = new Queue<Room> ();
            roomsToCreate.Enqueue (new Room(initialRoomCoordinate.x, initialRoomCoordinate.y));
            List<Room> createdRooms = new List<Room> ();
            while (roomsToCreate.Count > 0 && createdRooms.Count < _settings.NumberOfRooms) {
                Room currentRoom = roomsToCreate.Dequeue ();
                _rooms [currentRoom.roomCoordinate.x, currentRoom.roomCoordinate.y] = currentRoom;
                createdRooms.Add (currentRoom);
                AddNeighbors (currentRoom, roomsToCreate);
            }
		
            foreach (Room room in createdRooms) {
                List<Vector2Int> neighborCoordinates = room.NeighborCoordinates ();
                foreach (Vector2Int coordinate in neighborCoordinates) {
                    Room neighbor = this._rooms [coordinate.x, coordinate.y];
                    if (neighbor != null) {
                        room.Connect (neighbor);
                    }
                }
            }
 
            return _rooms [initialRoomCoordinate.x, initialRoomCoordinate.y];
        }
        
        private void AddNeighbors(Room currentRoom, Queue<Room> roomsToCreate) {
            List<Vector2Int> neighborCoordinates = currentRoom.NeighborCoordinates ();
            List<Vector2Int> availableNeighbors = new List<Vector2Int> ();
            foreach (Vector2Int coordinate in neighborCoordinates) {
                if (_rooms[coordinate.x, coordinate.y] == null) {
                    availableNeighbors.Add (coordinate);
                }
            }
		
            int numberOfNeighbors = (int)Random.Range (1, availableNeighbors.Count);
 
            for (int neighborIndex = 0; neighborIndex < numberOfNeighbors; neighborIndex++) {
                
                float randomNumber = Random.value;
                float roomFrac = 1f / (float)availableNeighbors.Count;
                Vector2Int chosenNeighbor = new Vector2Int(0, 0);
                
                foreach (Vector2Int coordinate in availableNeighbors) {
                    if (randomNumber < roomFrac) {
                        chosenNeighbor = coordinate;
                        break;
                    } 
                    
                    roomFrac += 1f / (float)availableNeighbors.Count;
                }
                roomsToCreate.Enqueue (new Room(chosenNeighbor));
                availableNeighbors.Remove (chosenNeighbor);
            }
        }

        private void BuildLevel(Room currentRoom) {

            foreach (Room room in _rooms) {
                if (room != null) {
                    GameObject roomObject = (GameObject)Instantiate(Resources.Load(RoomPath), _level.transform);
                    roomObject.transform.position = new Vector3(
                        room.roomCoordinate.x * RoomSize + room.roomCoordinate.x * Offset, 
                        0, 
                        room.roomCoordinate.y * RoomSize + room.roomCoordinate.y * Offset
                    );

                    RoomController controller = roomObject.GetComponent<RoomController>();
                    room.InitController(controller);

                    Transform roomTransform = roomObject.transform;
                    BuildWalls(controller, roomTransform);
                    BuildBridges(controller, roomTransform);
                    
                    controller.InitDoors();
                    
                    if (room != currentRoom) {
                        
                        // TODO: Spawn monsters / Boss
                        // TODO: Load template
                        //                        roomObject.SetActive(false);
                    } else {
                        controller.Fog.SetActive(false);
                        PlayerEntityController.Get().transform.position = roomObject.transform.position;
                    }
                }
            }
        }

        private void BuildWalls(RoomController roomController, Transform parent) {
            Instantiate(Resources.Load($"{(roomController.North ? DoorsPath : WallsPath)}N"), parent);
            Instantiate(Resources.Load($"{(roomController.East ? DoorsPath : WallsPath)}E"), parent);
            Instantiate(Resources.Load($"{(roomController.South ? DoorsPath : WallsPath)}S"), parent);
            Instantiate(Resources.Load($"{(roomController.West ? DoorsPath : WallsPath)}W"), parent);
        }

        private void BuildBridges(RoomController roomController, Transform parent) {

            if (roomController.North) {
                Instantiate(Resources.Load(BridgePath + "NS"), parent);
            }
            
            if (roomController.East) {
                Instantiate(Resources.Load(BridgePath + "EW"), parent);
            }
        }

        private void Print() {
            string grid = "";
            for (int y = 0; y < _rooms.GetLength(1); y++) {
                string column = "";
                for (int x = 0; x < _rooms.GetLength(0); x++) {
                    column += _rooms[x, y] != null ? "R" : "X";
                }
                grid += column + "\n";
            }
            
            Debug.Log(grid);
        }
    }
}
