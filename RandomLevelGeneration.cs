using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomLevelGeneration : MonoBehaviour
{
    public GameObject[] structures;
    public GameObject[] closingStructures;
    public Dictionary<string, bool> mapDict = new Dictionary<string, bool>();
    float xOffset = 52;
    float yOffset = 15;
    Vector2 lastOffset;
    public List<Vector2> roomOffsets;
    public float xMax;
    public float xMin;
    public float yMin;
    public float yMax;
    int structuresGenerated = 0;
    // int structuresGeneratedV = 0;
    public int maxRoomCount, minRoomCount;
    int roomCount;
    int leftCount;
    int rightCount;
    string roomID;

    private void Awake()
    {
        lastOffset = new Vector3(0, 0);
        roomID = $"{lastOffset.x} {lastOffset.y}";
    }

    // Start is called before the first frame update
    void Start()
    {
        leftCount = Random.Range(2, 8);
        rightCount = Random.Range(2, 8);
        roomCount = Random.Range(minRoomCount, maxRoomCount);
       // CheckMapDictionary();
        AddStructure(new Vector2(0, 0));
        GenerateLevel();
    }

    // Update is called once per frame
    void GenerateLevel()
    {
        print("rooms generated: " + roomCount);
        while(structuresGenerated < roomCount)
        {
            int randomDir = Random.Range(0, 4);
            ConsoleOutput();
            if(randomDir == 0)
            {
                print("adding structure to the right");
                Vector2 offset = new Vector2(lastOffset.x + xOffset, lastOffset.y);
                lastOffset = offset;
                roomID = $"{lastOffset.x} {lastOffset.y}";
                print("lastOffset" + offset);

                if(CheckMapDictionary())
                {
                    AddStructure(offset);
                }

            } 
            else if(randomDir == 1)
            {
                print("adding structure to the left");
                Vector2 offset = new Vector2(lastOffset.x - xOffset, lastOffset.y);
                lastOffset = offset;
                roomID = $"{lastOffset.x} {lastOffset.y}";
                print("lastOffset" + offset);

                if (CheckMapDictionary())
                {
                    AddStructure(offset);
                }

            } 
            else if (randomDir == 2)
            {
                print("adding structure upwards");
                Vector2 offset = new Vector2(lastOffset.x, lastOffset.y + yOffset);
                lastOffset = offset;
                roomID = $"{lastOffset.x} {lastOffset.y}";
                print("lastOffset" + offset);

                if (CheckMapDictionary())
                {
                    AddStructure(offset);
                }
            }
            else
            {
                print("adding structure downwards");
                Vector2 offset = new Vector2(lastOffset.x, lastOffset.y - yOffset);
                lastOffset = offset;
                roomID = $"{lastOffset.x} {lastOffset.y}";
                print("lastOffset" + offset);

                if (CheckMapDictionary())
                {
                    AddStructure(offset);
                }
            }
        }
        AddClosingStructure();
    }


    bool CheckMapDictionary()
    {
        if (mapDict.ContainsKey(roomID)) //check if collectable is in database
        {
            //print("Key: " + collectableID);
            if (mapDict[roomID]) //if the collectable in the database's key results in a value of true, delet it (it's been collected) get value from dictionary with key
            {
                return false;
                //print("collectable already exists in database and has been collected, destroying it now");
            }
            return true;
        }
        else
        {
           // mapDict.Add(roomID, false); //if collectable not in database, then add it into the database
            return true;

            //print($"{collectableID}-key was added to database, didnt exist befroe");
        }
    }

    void AddStructure(Vector2 offset)
    {
        if(mapDict.ContainsKey(roomID))
        {
            if(mapDict[roomID])
            {
                print("key already in map, not adding this...");
            }
            //Instantiate(structures[Random.Range(0, structures.Length)], offset, Quaternion.identity);
            mapDict.Add(roomID, true);

        } else
        {
            Instantiate(structures[Random.Range(0, structures.Length)], offset, Quaternion.identity);
            roomOffsets.Add(offset);
            mapDict.Add(roomID, true);
            structuresGenerated++;
        }

    }

    void FindMax()
    {
        xMax = lastOffset.x;
        xMin = lastOffset.x;
        yMax = lastOffset.y;
        yMin = lastOffset.y;
        foreach(Vector2 offset in roomOffsets)
        {
            if(offset.x > xMax)
            {
                xMax = offset.x;
            }
            if(offset.x < xMin)
            {
                xMin = offset.x;
            }
            if(offset.y > yMax)
            {
                yMax = offset.y;
            }
            if(offset.y < yMin)
            {
                yMin = offset.y;
            }
        }
    }

    void AddClosingStructure()
    {
        foreach(Vector2 offset in roomOffsets)
        {
            string xMax = $"{offset.x + xOffset} {offset.y}";
            string xMin = $"{offset.x - xOffset} {offset.y}";
            string yMax = $"{offset.x} {offset.y + yOffset}";
            string yMin = $"{offset.x} {offset.y - yOffset}";

            if (!mapDict.ContainsKey(xMax))
            {
                Instantiate(closingStructures[0], offset, Quaternion.identity);
            } 
            if (!mapDict.ContainsKey(xMin))
            {
                Instantiate(closingStructures[1], offset, Quaternion.identity);
            } 
            if (!mapDict.ContainsKey(yMax))
            {
                Instantiate(closingStructures[2], offset, Quaternion.identity);
            } 
            if (!mapDict.ContainsKey(yMin))
            {
                Instantiate(closingStructures[3], offset, Quaternion.identity);
            }
        }
    }

    void ConsoleOutput()
    {
        foreach (KeyValuePair<string, bool> kvp in mapDict)
            Debug.Log("Key = " + kvp.Key + " Value = " + kvp.Value);
    }
}
