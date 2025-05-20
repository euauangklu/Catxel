using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Events/Cat Trash Event")]
public class CatTrashEvent : GameEvents
{
    public float minDistanceBetweenTrash = 1.0f;

    private GameObject TopLeftSpawnArea;
    
    private GameObject BottomRightSpawnArea;

    public List<GameObject> Trash;
    
    private List<Vector3> usedPositions = new List<Vector3>();

    private Vector3 CurrentSpawnPos;
    
    private RandomEventManager manager;
    public override void TriggerEvent(RandomEventManager mgr)
    {
        GameObject TopLeftSpawnArea = GameObject.FindGameObjectWithTag("TopLeftSpawnArea");
        GameObject BottomRightSpawnArea = GameObject.FindGameObjectWithTag("BottomRightSpawnArea");
        Vector3 topLeft = TopLeftSpawnArea.transform.position;
        Vector3 bottomRight = BottomRightSpawnArea.transform.position;
        foreach (var trashPrefab in Trash)
        {
            Vector3 spawnPos = GetRandomPosition(topLeft, bottomRight);
            GameObject obj = Instantiate(trashPrefab, spawnPos, Quaternion.identity);
            usedPositions.Add(spawnPos);
            CatTrash catTrash = obj.GetComponent<CatTrash>();
            
            if (catTrash != null)
            {
                catTrash.eventSource = this;
            }
        }
        manager = mgr;
    }

    public void OnEventDone()
    {
        if (manager != null)
        {
            manager.DoneEvent = true;
            manager.ResetRandomEvent();
        }
    }
    private Vector3 GetRandomPosition(Vector3 topLeft, Vector3 bottomRight)
    {
        int maxAttempts = 30;

        for (int i = 0; i < maxAttempts; i++)
        {
            float x = Random.Range(bottomRight.x, topLeft.x);
            float y = Random.Range(bottomRight.y, topLeft.y);
            CurrentSpawnPos = new Vector3(x, y, 0);

            bool valid = true;
            foreach (var pos in usedPositions)
            {
                if (Vector3.Distance(CurrentSpawnPos, pos) < minDistanceBetweenTrash)
                {
                    valid = false;
                    break;
                }
            }

            if (valid)
            {
                return CurrentSpawnPos;
            }
        }
        return new Vector3(Random.Range(bottomRight.x, topLeft.x),Random.Range(bottomRight.y, topLeft.y),0);
    }
}
