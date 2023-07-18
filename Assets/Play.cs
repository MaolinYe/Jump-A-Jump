using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    public Rigidbody playerRigidbody;
    public GameObject body;
    public GameObject head;
    public GameObject cubePrefab;
    public Vector3 cubePlace;
    private Queue<GameObject> cubes = new Queue<GameObject>();
    public int speed = 10;
    public int direction = 0;
    public int maxCubeNumber = 10;
    public Transform massCenter;
    public GameObject currentCube;
    public Text scoreText;
    public int score = 1;
    private bool jump = false;
    private bool inTheAir = false;
    private float start = 0;

    // Start is called before the first frame update
    void Start()
    {
        var cube = Instantiate(cubePrefab, cubePlace, Quaternion.identity);
        cubes.Enqueue(currentCube);
        cubes.Enqueue(cube);
        GetComponent<Rigidbody>().centerOfMass = massCenter.localPosition;
    }

    private void FixedUpdate()
    {
        if (Input.touchCount > 0 && inTheAir == false)
        {
            jump = true;
            if (body.transform.localScale.y > 0.5)
            {
                body.transform.localScale += new Vector3(1, -1, 1) * (0.45f * Time.deltaTime);
                head.transform.localPosition += new Vector3(0, -1, 0) * (0.45f * Time.deltaTime);
            }

            var touch = Input.touches[0];
            if (touch.phase == TouchPhase.Began)
            {
                start = Time.time;
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                var time = Time.time - start;
                if (time > 3)
                {
                    time = 3;
                }else if (time < 0.5f)
                {
                    time = 0.5f;
                }

                if (direction == 0)
                {
                    playerRigidbody.AddForce(new Vector3(1, 1, 0) * (time * speed), ForceMode.Impulse);
                }
                else
                {
                    playerRigidbody.AddForce(new Vector3(0, 1, 1) * (time * speed), ForceMode.Impulse);
                }

                body.transform.localScale = new Vector3(1, 1.5f, 1);
                head.transform.localPosition = new Vector3(0, 3.7f, 0);
                inTheAir = true;
            }
        }

        scoreText.text = score.ToString();
    }

    private void NewCube()
    {
        var random = new System.Random();
        direction = random.Next(0, 2);
        if (direction == 0)
        {
            var cube = Instantiate(cubePrefab, new Vector3(Random.Range(10, 13), 0, 0) + cubePlace,
                Quaternion.identity);
            cubePlace = cube.transform.position;
            cubes.Enqueue(cube);
        }
        else
        {
            var cube = Instantiate(cubePrefab, new Vector3(0, 0, Random.Range(10, 13)) + cubePlace,
                Quaternion.identity);
            cubePlace = cube.transform.position;
            cubes.Enqueue(cube);
        }

        if (cubes.Count > maxCubeNumber)
        {
            var cube = cubes.Dequeue();
            Destroy(cube);
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Ground") || other.gameObject == currentCube && jump)
        {
            // FindObjectOfType<GameManager>().EndGame();
            SceneManager.LoadScene("MainScene");
        }
        else if (other.gameObject != currentCube)
        {
            score += 1;
            inTheAir = false;
            NewCube();
            currentCube = other.gameObject;
        }
    }
}