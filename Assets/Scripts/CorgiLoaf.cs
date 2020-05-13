using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CorgiLoaf : MonoBehaviour
{
    public float speed = 10.0f;
    public bool isGrounded = false;
    public float jumpForce = 3200f;

    Rigidbody2D rigidbody2d;
    public Text score_text;

    [SerializeField] public GameObject[] breads;
    [SerializeField] public float breadDrop = 0.05f;
    private GameObject spawnedBread;
    private bool breadSpawned = false;
    private int score = 0;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        score_text = GameObject.Find("/Canvas/Text").GetComponent<Text>();
        score_text.text = "Score: 0";
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        isGrounded = true;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.name == breads[0].name + "(Clone)" ||
            collision.gameObject.name == breads[1].name + "(Clone)" ||
            collision.gameObject.name == breads[2].name + "(Clone)" ||
            collision.gameObject.name == breads[3].name + "(Clone)") && collision != null
            && rigidbody2d.position.y > -3.54f)
        {
            Destroy(collision.gameObject);
            breadSpawned = false;
            score++;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");

        Vector2 move = new Vector2(horizontal, 0);

        Vector2 position = rigidbody2d.position;
        position = position + move * speed * Time.deltaTime;

        rigidbody2d.MovePosition(position);

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigidbody2d.AddForce(new Vector2(0.0f, jumpForce), ForceMode2D.Force);
            isGrounded = false;
        }

        if (!breadSpawned)
        {
            Vector2 pos = new Vector2(Random.Range(-8f, 8f), 5f);
            spawnedBread = Instantiate(breads[Random.Range(0, breads.Length)], pos, Quaternion.identity);
            breadSpawned = true;
        }
        bread();
        score_text.text = "Score: " + score;
    }

    void bread()
    {
        if (spawnedBread != null)
        {
            spawnedBread.transform.position = new Vector2(spawnedBread.transform.position.x, spawnedBread.transform.position.y - breadDrop);
            if (spawnedBread.transform.position.y < -5f)
            {
                SceneManager.LoadScene("GameOver");
            }
        }
    }
}
