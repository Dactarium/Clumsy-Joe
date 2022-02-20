using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField] private GameObject[] _items;
    [SerializeField] private float _spawnRadius;
    [SerializeField] private int _spawnCount;
    private GameObject[] _spawnedItems = new GameObject[0];
    public int spawn(Transform spawnObject){
        for(int i = 0; i < _spawnedItems.Length; i++){
            Destroy(_spawnedItems[i]);
        }
        _spawnedItems = new GameObject[_spawnCount];

        for(int i = 0; i < _spawnCount; i++){
            GameObject item = Instantiate(_items[Random.Range(0, _items.Length)]);
            
            Vector3 spawnPos = spawnObject.transform.position;

            spawnPos.x += Random.Range(-_spawnRadius, _spawnRadius);
            spawnPos.z += Random.Range(-_spawnRadius, _spawnRadius);

            spawnPos.y += .1f; 

            item.transform.position = spawnPos;

            _spawnedItems[i] = item;
        }

        return _spawnCount;
    }
}
