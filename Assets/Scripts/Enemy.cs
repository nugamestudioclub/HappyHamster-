using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static int enemyCount;
    public GameObject player;

    public Rigidbody2D enemyBody;

    public SpriteRenderer spriteRenderer;

    Vector2 _target = Vector2.zero;

    const int MAP_SIZE = 57/2;
    const int PLAYER_SAFE_DISTANCE = 10;
    public float speed = 20000f; // in units per second
    public int MAXFORCE = 10;

    void SetRandomTarget() {
        _target = new Vector2(Random.Range(-MAP_SIZE, MAP_SIZE), Random.Range(-MAP_SIZE, MAP_SIZE));
    }
    
    // Start is called before the first frame update
    void Start()
    {
        enemyCount += 1;
        enemyBody = gameObject.GetComponent<Rigidbody2D>();
        SetRandomTarget();
    }

    void Push(Vector2 direction) {
        Vector3 newForce = new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
        gameObject.GetComponent<Rigidbody2D>().AddForce(newForce);
        if (gameObject.GetComponent<Rigidbody2D>().totalForce.magnitude >= MAXFORCE) 
        {
            gameObject.GetComponent<Rigidbody2D>().AddForce(-1 * newForce);
        }


        spriteRenderer.flipX = (direction.x >= 0);
    }

    // Update is called once per frame
    void Update()
    {

        // Vector2 direction = player.transform.position - transform.position;

        // direction.Normalize();
        //transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;



        // // Move away from player
        Vector2 direction = transform.position - player.transform.position;
        if (direction.magnitude < PLAYER_SAFE_DISTANCE) {
            direction.Normalize();
            Push(direction * 2);
        } else {
            // Move towards random point
            Vector2 directionToTarget = _target - new Vector2(transform.position.x, transform.position.y);
            if (directionToTarget.magnitude < 1) { // Find a new random target on the map to go to
                SetRandomTarget();
            } else {
                directionToTarget.Normalize();
                Push(directionToTarget);
            }
        }
    }

    public void Kill()
    {
        // TODO: Death vfx and animations and score
        Destroy(gameObject);
    }



    private void OnDestroy()
    {
        enemyCount--;
    }
}
