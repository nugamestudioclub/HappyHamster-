using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum EnemyState {
    Run,
    Idle,
    Roam,
    HugPlayer // :)
}

public class Enemy : MonoBehaviour
{
    public static int enemyCount;
    public Transform player;
    public GameObject distressSignals;
    public GameObject enemyKilledSignalPrefab;
    public Rigidbody2D enemyBody;

    public SpriteRenderer spriteRenderer;

    Vector2 _target = Vector2.zero;

    const int MAP_SIZE = 57/2;
    const int PLAYER_SAFE_DISTANCE = 10;
    public float acceleration = 2f; // in units per second
    private EnemyState _state = EnemyState.Roam;

    void SetRandomTarget() {
        _target = new Vector2(Random.Range(-MAP_SIZE, MAP_SIZE), Random.Range(-MAP_SIZE, MAP_SIZE));
    }
    
    // Start is called before the first frame update
    void Start()
    {
        enemyCount += 1;
        SetRandomTarget();
    }

    void Push(Vector2 direction, float maxVelocity) {
        // Vector3 newForce = new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
        // gameObject.GetComponent<Rigidbody2D>().AddForce(newForce);
        // if (gameObject.GetComponent<Rigidbody2D>().totalForce.magnitude >= MAXFORCE) 
        // {
        //     gameObject.GetComponent<Rigidbody2D>().AddForce(-1 * newForce);
        // }
        //move the enemy without force
        // transform.position += new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
        if (enemyBody.velocity.magnitude < maxVelocity) {
            enemyBody.AddForce(new Vector3(direction.x, direction.y, 0) * acceleration);
        }


        spriteRenderer.flipX = (direction.x >= 0);
    }

    // Update is called once per frame

    bool HasDistressSignal(float distance) {
        bool foundSignal = false;
        foreach (GameObject distressSignal in GameObject.FindGameObjectsWithTag("DistressSignal")) {
            if (Vector3.Distance(distressSignal.transform.position, transform.position) < distance) {
                foundSignal = true;
            }
        }
        return foundSignal;
    }

    void CheckForDistressSignals() {
        if (HasDistressSignal(15.0f)) {
            _state = EnemyState.Run;
        } else {
            _state = EnemyState.Roam;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (player == null || distressSignals == null) {
            Debug.Log("Player or distressSignals not set");
            return;
        }
        Vector2 direction = player.position - transform.position;
        switch (_state) {
            case EnemyState.Run:
                direction = -direction;
                direction.Normalize();
                acceleration = 5f;
                break;
            case EnemyState.Idle:
                direction = Vector2.zero;
                // if (Random.Range(0, 100000) < 10) _state = EnemyState.Roam;
                break;
            case EnemyState.Roam:
                // Move towards random point
                acceleration = 1.5f;
                if (direction.magnitude < PLAYER_SAFE_DISTANCE) {
                    _state = EnemyState.HugPlayer;
                    direction.Normalize();
                    break;
                }
                direction = _target - new Vector2(transform.position.x, transform.position.y);
                if (direction.magnitude < 2) { // Find a new random target on the map to go to
                    SetRandomTarget();
                    direction = Vector2.zero;
                }
                direction.Normalize();
                // if (Random.Range(0, 100000) < 10) _state = EnemyState.Idle;
                break;
            case EnemyState.HugPlayer:
                direction.Normalize();
                break;    
            default:
                break; 
        }
        Push(direction, 4);
        CheckForDistressSignals();
        // if (direction.magnitude < PLAYER_SAFE_DISTANCE) {
        //     // Move towards from player
        //     direction.Normalize();
        //     Push(direction * 2);
        // } else {
        //     // Move towards random point
        //     Vector2 directionToTarget = _target - new Vector2(transform.position.x, transform.position.y);
        //     if (directionToTarget.magnitude < 1) { // Find a new random target on the map to go to
        //         SetRandomTarget();
        //     } else {
        //         directionToTarget.Normalize();
        //         Push(directionToTarget);
        //     }
        // }
    }


    public void Kill()
    {
        // TODO: Death vfx and animations and score
        if (!HasDistressSignal(10.0f)) {
            GameObject signal = Instantiate(enemyKilledSignalPrefab, transform.position, Quaternion.identity);
            signal.transform.parent = distressSignals.transform;
        }
        Destroy(gameObject);
    }



    private void OnDestroy()
    {
        enemyCount--;
    }
}
