using System.Collections.Generic;
using UnityEngine;

namespace Game.Level {
    public class Room {
        public Vector2Int               RoomCoordinate { get; }
        public readonly Dictionary<string, Room> Neighbors;
        public bool ExitRoom;

        public Room(int xCoordinate, int yCoordinate) {
            RoomCoordinate = new Vector2Int(xCoordinate, yCoordinate);
            Neighbors = new Dictionary<string, Room>();
        }

        public Room(Vector2Int _roomCoordinate) {
            RoomCoordinate = _roomCoordinate;
            Neighbors = new Dictionary<string, Room>();
        }
        
        public List<Vector2Int> NeighborCoordinates() {
            List<Vector2Int> neighborCoordinates = new List<Vector2Int> {
                new Vector2Int(RoomCoordinate.x, RoomCoordinate.y - 1),
                new Vector2Int(RoomCoordinate.x + 1, RoomCoordinate.y),
                new Vector2Int(RoomCoordinate.x, RoomCoordinate.y + 1),
                new Vector2Int(RoomCoordinate.x - 1, RoomCoordinate.y)
            };

            return neighborCoordinates;
        }
        
        public void Connect(Room neighbor) {
            string direction = "";
            if (neighbor.RoomCoordinate.y < RoomCoordinate.y) {
                direction = "N";
            }
            if (neighbor.RoomCoordinate.x > RoomCoordinate.x) {
                direction = "E";
            }   
            if (neighbor.RoomCoordinate.y > RoomCoordinate.y) {
                direction = "S";
            }
            if (neighbor.RoomCoordinate.x < RoomCoordinate.x) {
                direction = "W";
            }
            Neighbors.Add(direction, neighbor);
        }
        
        public void InitController(RoomController controller) {
            controller.ExitRoom = ExitRoom;
            foreach (KeyValuePair<string, Room> neighborPair in Neighbors) {
                switch (neighborPair.Key) {
                    case "N":
                        controller.North = true;
                        break;
                    case "E":
                        controller.East = true;
                        break;
                    case "S":
                        controller.South = true;
                        break;
                    case "W":
                        controller.West = true;
                        break;
                }
            }
        }

        public int GetDistanceFromOrigin(Vector2Int origin) {
            return Mathf.Abs(RoomCoordinate.x - origin.x) + Mathf.Abs(RoomCoordinate.y - origin.y);
        }
    }
}