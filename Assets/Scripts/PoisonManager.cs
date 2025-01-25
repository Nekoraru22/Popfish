using UnityEngine;

public class PoisonManager : MonoBehaviour
{
    public GameObject poisonPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreatePoison(new Vector3((float)-5.854, (float)2.688, (float)0));
        CreatePoison(new Vector3((float)6.082, (float)4.666, (float)-10));
    }

    void CreatePoison(Vector3 position)
    {
        Instantiate(poisonPrefab, position, Quaternion.identity);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
