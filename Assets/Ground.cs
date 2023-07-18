using UnityEngine;

public class Ground : MonoBehaviour
{
    public Transform player;

    // Update is called once per frame
    void Update()
    {
        var position = player.position;
        transform.position = new Vector3(position.x, 0, position.z);
    }
}