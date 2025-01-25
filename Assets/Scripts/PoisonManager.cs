using UnityEngine;

public class PoisonManager : MonoBehaviour
{
    public GameObject poisonPrefab;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CreatePoison(new Vector3((float)4.1, (float)5.281, (float)-5.05));
        CreatePoison(new Vector3((float)-6.78, (float)3.251, (float)-5.05));
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
