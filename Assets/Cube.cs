using UnityEngine;

public class Cube : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Renderer>().material.color = new Color(Random.value, Random.value, Random.value);
        var radius = Random.Range(4, 7);
        transform.localScale = new Vector3(radius,Random.Range(4,7),radius);
    }
}
