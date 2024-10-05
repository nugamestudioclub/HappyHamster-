using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public static int enemyCount;
    public GameObject player;

    Vector2 _target = Vector2.zero;

    const int MAP_SIZE = 57;
    const int PLAYER_SAFE_DISTANCE = 10;
    public float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        enemyCount += 1;
    }

    void SetRandomTarget() {
        _target = new Vector2(Random.Range(-MAP_SIZE, MAP_SIZE), Random.Range(-MAP_SIZE, MAP_SIZE));
    }

    // Update is called once per frame
    void Update()
    {
        // Move away from player
        Vector2 direction = transform.position - player.transform.position;
        if (direction.magnitude < PLAYER_SAFE_DISTANCE) {
            direction.Normalize();
            transform.position += new Vector3(direction.x, direction.y, 0) * Time.deltaTime;
        } else {
            // Move towards random point
            Vector2 directionToTarget = _target - new Vector2(transform.position.x, transform.position.y);
            if (directionToTarget.magnitude < 1) { // Find a new random target on the map to go to
                SetRandomTarget();
                while (Vector2.Distance(_target, player.transform.position) < PLAYER_SAFE_DISTANCE) {
                    SetRandomTarget();
                }
            } else {
                directionToTarget.Normalize();
                transform.position += new Vector3(directionToTarget.x, directionToTarget.y, 0) * speed * Time.deltaTime;
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
