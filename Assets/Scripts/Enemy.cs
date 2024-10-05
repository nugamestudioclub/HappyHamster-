using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static int enemyCount;
    public GameObject player;

    public Rigidbody2D enemyBody;

    Vector2 _target = Vector2.zero;

    const int MAP_SIZE = 57 * 2;
    const int PLAYER_SAFE_DISTANCE = 10;
    public float speed = 20000f; // in units per second

    // Start is called before the first frame update
    void Start()
    {
        enemyCount += 1;
        enemyBody = gameObject.GetComponent<Rigidbody2D>();
    }

    void SetRandomTarget() {
        _target = new Vector2(Random.Range(-MAP_SIZE, MAP_SIZE), Random.Range(-MAP_SIZE, MAP_SIZE));
    }

    // Update is called once per frame
    void Update()
    {
        // Get random point within map
        SetRandomTarget();

        // Vector2 directionToTarget = _target - new Vector2(transform.position.x, transform.position.y);

        //Debug.Log(Time.deltaTime, speed);
        //transform.position += new Vector3(directionToTarget.x, directionToTarget.y, 0).Normalize() * speed * Time.deltaTime;

        Vector2 direction = player.transform.position - transform.position;

        direction.Normalize();
        //transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime);



        // // Move away from player
        // Vector2 direction = transform.position - player.transform.position;
        // if (direction.magnitude < PLAYER_SAFE_DISTANCE) {
        //     direction.Normalize();
        //     transform.position += new Vector3(direction.x, direction.y, 0) * Time.deltaTime;
        // } else {
        //     // Move towards random point
        //     Vector2 directionToTarget = _target - new Vector2(transform.position.x, transform.position.y);
        //     if (directionToTarget.magnitude < 1) { // Find a new random target on the map to go to
        //         SetRandomTarget();
        //         while (Vector2.Distance(_target, player.transform.position) < PLAYER_SAFE_DISTANCE) {
        //             SetRandomTarget();
        //         }
        //     } else {
        //         directionToTarget.Normalize();
        //         transform.position += new Vector3(directionToTarget.x, directionToTarget.y, 0) * speed * Time.deltaTime;
        //     }
        // }
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
