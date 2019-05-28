using System.Collections.Generic;
using Cinemachine;
using Entities.Interactable;
using Settings;
using UnityEngine;

namespace Game.Level {
    public class LevelManager : MonoBehaviour {

        [SerializeField] private GameObject PlayerPrefab;
        [SerializeField] private float Offset;
        [SerializeField] private float RoomSize = 15f;

        private readonly string RoomPath = "Prefabs/Level/Room";
        private readonly string BridgePath = "Prefabs/Level/Bridges/Bridge_";
        private readonly string WallsPath = "Prefabs/Level/Walls/Wall_";
        private readonly string DoorsPath = "Prefabs/Level/Doors/Door_";
        
        private GameManager _gameManager;
        private LevelSettings _settings;
        private Room[,] _rooms;
        private CinemachineVirtualCamera _vcam;

        private GameObject _level;
        
        void Awake() {

            _gameManager = GameManager.Get();
            _vcam = GameObject.FindWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
            
            if (_level != null)
                DestroyImmediate(_level);
            else {
                _level = new GameObject("Level");
            }
        }

        void Start() {
            if ((_settings = _gameManager.GetCurrentLevelSettings()) != null) {
                Room currentRoom = GenerateLevel();
                BuildLevel(currentRoom);
                BuildPlayer(currentRoom.RoomCoordinate);
            } else {
                BuildPlayer(new Vector2Int(0, 0));
            }
        }
        
        private Room GenerateLevel() {
            int gridSize = _settings.NumberOfRooms * 3 + 1;
 
            _rooms = new Room[gridSize, gridSize];
 
            Vector2Int initialRoomCoordinate = new Vector2Int (gridSize / 2 - 1, gridSize / 2 - 1);
 
            Queue<Room> roomsToCreate = new Queue<Room> ();
            roomsToCreate.Enqueue (new Room(initialRoomCoordinate.x, initialRoomCoordinate.y));
            List<Room> createdRooms = new List<Room> ();
            while (roomsToCreate.Count > 0 && createdRooms.Count < _settings.NumberOfRooms) {
                Room currentRoom = roomsToCreate.Dequeue ();
                _rooms [currentRoom.RoomCoordinate.x, currentRoom.RoomCoordinate.y] = currentRoom;
                createdRooms.Add (currentRoom);
                AddNeighbors (currentRoom, roomsToCreate);
            }

            Room finalRoom = _rooms[initialRoomCoordinate.x, initialRoomCoordinate.y];
            int maxDistance = 0;
            foreach (Room room in createdRooms) {
                List<Vector2Int> neighborCoordinates = room.NeighborCoordinates ();
                foreach (Vector2Int coordinate in neighborCoordinates) {
                    Room neighbor = _rooms [coordinate.x, coordinate.y];
                    if (neighbor != null) {
                        room.Connect (neighbor);
                    }
                }

                int distance = room.GetDistanceFromOrigin(initialRoomCoordinate);
                if (distance > maxDistance) {
                    finalRoom = room;
                }
            }
            finalRoom.ExitRoom = true;

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
                    roomObject.transform.position = CoordinatesToVector3(room.RoomCoordinate);

                    RoomController controller = roomObject.GetComponent<RoomController>();
                    room.InitController(controller);

                    Transform roomTransform = roomObject.transform;
                    BuildWalls(controller, roomTransform);
                    BuildBridges(controller, roomTransform);
                    
                    controller.Init();
                    
                    if (room != currentRoom) {

                        SpawnMonsters(controller);

                        if (!room.ExitRoom) {
                            BuildTemplate(roomTransform);
                        } else {
                            // TODO: Spawn boss
                        }
                    } else {
                        controller.Fog.SetActive(false);
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

        private void BuildTemplate(Transform parent) {
            List<GameObject> templates = _gameManager.GetCurrentLevelSettings().Templates;
            if (templates != null && templates.Count > 0) {
                GameObject template = Instantiate(templates[(int)Random.Range(0, templates.Count)], parent);
                float randomRotation = Random.Range(0, 4) * 90;
                template.transform.rotation = new Quaternion(0, randomRotation, 0, 0);
            }
        }

        private void SpawnMonsters(RoomController roomController) {
            LevelSettings settings = _gameManager.GetCurrentLevelSettings();

            if (settings.Monsters.Count == 0 || settings.MonstersPerRoom == Vector2Int.zero)
                return;
            
            Vector2Int range = settings.MonstersPerRoom;
            int toSpawn = Random.Range(range.x, range.y + 1);
            for (int i = 0; i < toSpawn; i++) {
                int monsterIndex = Random.Range(0, settings.Monsters.Count);

                GameObject monster = Instantiate(settings.Monsters[monsterIndex], GetRandomPositionInRoom(roomController.transform.position), Quaternion.identity);
                if (monster != null) {
                    roomController.Monsters.Add(monster.GetComponent<InteractableEntityController>());
                    monster.SetActive(false);
                }
            }
        }

        private void BuildPlayer(Vector2Int coordinates) {
            GameObject player = Instantiate(PlayerPrefab, CoordinatesToVector3(coordinates), Quaternion.identity);
            _vcam.Follow = player.transform;
        }

        private Vector3 CoordinatesToVector3(Vector2Int roomCoordinates) {
            return new Vector3(
                roomCoordinates.x * RoomSize + roomCoordinates.x * Offset, 
                0, 
                roomCoordinates.y * RoomSize + roomCoordinates.y * Offset
            );
        }

        private Vector3 GetRandomPositionInRoom(Vector3 roomPosition) {

            // TODO: Check if position is empty !
            float radius = RoomSize / 2;
            float x = Random.Range(roomPosition.x - radius + 1, roomPosition.x + radius - 1);
            float z = Random.Range(roomPosition.z - radius + 1, roomPosition.z + radius - 1);
            return new Vector3 (x, 1, z);
        }
    }
}
