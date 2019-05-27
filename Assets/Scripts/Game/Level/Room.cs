using System.Collections.Generic;
using UnityEngine;

namespace Game.Level {
    public class Room {
        public Vector2Int               roomCoordinate;
        public Dictionary<string, Room> neighbors;

        public Room(int xCoordinate, int yCoordinate) {
            roomCoordinate = new Vector2Int(xCoordinate, yCoordinate);
            neighbors = new Dictionary<string, Room>();
        }

        public Room(Vector2Int _roomCoordinate) {
            roomCoordinate = _roomCoordinate;
            neighbors = new Dictionary<string, Room>();
        }
        
        public List<Vector2Int> NeighborCoordinates() {
            List<Vector2Int> neighborCoordinates = new List<Vector2Int> {
                new Vector2Int(roomCoordinate.x, roomCoordinate.y - 1),
                new Vector2Int(roomCoordinate.x + 1, roomCoordinate.y),
                new Vector2Int(roomCoordinate.x, roomCoordinate.y + 1),
                new Vector2Int(roomCoordinate.x - 1, roomCoordinate.y)
            };

            return neighborCoordinates;
        }
        
        public void Connect(Room neighbor) {
            string direction = "";
            if (neighbor.roomCoordinate.y < roomCoordinate.y) {
                direction = "N";
            }
            if (neighbor.roomCoordinate.x > roomCoordinate.x) {
                direction = "E";
            }   
            if (neighbor.roomCoordinate.y > roomCoordinate.y) {
                direction = "S";
            }
            if (neighbor.roomCoordinate.x < roomCoordinate.x) {
                direction = "W";
            }
            neighbors.Add(direction, neighbor);
        }
        
        public void InitController(RoomController controller) {
            foreach (KeyValuePair<string, Room> neighborPair in neighbors) {
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
    }
}