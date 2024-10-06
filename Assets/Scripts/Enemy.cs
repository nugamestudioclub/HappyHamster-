using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    Vector3 _run_to;

    const int MAP_SIZE = 57/2;
    const int PLAYER_SAFE_DISTANCE = 10;
    public float maxVelocity = 0.2f; // in units per second
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

    void Push(Vector2 direction) {
        // Vector3 newForce = new Vector3(direction.x, direction.y, 0) * speed * Time.deltaTime;
        // gameObject.GetComponent<Rigidbody2D>().AddForce(newForce);
        // if (gameObject.GetComponent<Rigidbody2D>().totalForce.magnitude >= MAXFORCE) 
        // {
        //     gameObject.GetComponent<Rigidbody2D>().AddForce(-1 * newForce);
        // }
        
        //move the enemy without force
        transform.position += new Vector3(direction.x, direction.y, 0) * maxVelocity;

        //if (enemyBody.velocity.magnitude < maxVelocity) {
        //    enemyBody.AddForce(new Vector3(direction.x, direction.y, 0) * acceleration);
        //}


        spriteRenderer.flipX = (direction.x >= 0);
    }

    // Update is called once per frame

    GameObject GetDistressSignal(float distance) {
        // bool foundSignal = false;
        // foreach (GameObject distressSignal in GameObject.FindGameObjectsWithTag("DistressSignal")) {
        //     Debug.Log(distressSignal.transform.position);
        //     if (Vector3.Distance(distressSignal.transform.position, transform.position) < distance) {
        //         foundSignal = true;
        //     }
        // }
        // return foundSignal;
        GameObject[] activeSignalsArray = GameObject.FindGameObjectsWithTag("DistressSignal");
        List<GameObject> activeSignals = new List<GameObject>(activeSignalsArray);
        
        if (activeSignals == null) { return null; }

        List<GameObject> sortedSignal = SortObjectsByDistance(activeSignals);
        if ((sortedSignal.Count == 0) || (sortedSignal[0] == null)) { return null; }
        GameObject closestSignal = sortedSignal[0];

        if ((closestSignal.transform.position - transform.position).magnitude > distance) {
            return null;
        }
        return closestSignal;
    }

    List<GameObject> SortObjectsByDistance(List<GameObject> signals)
    {
        // Sort by furthest to closest // Can change to OrderBy for the reverse order
        return signals.OrderBy(signal => Vector3.Distance(transform.position, signal.transform.position)).ToList();
    }

    List<GameObject> SortObjectsByDistance2(List<GameObject> spawners)
    {
        // Sort by furthest to closest // Can change to OrderBy for the reverse order
        return spawners.OrderByDescending(spawner => Vector3.Distance(player.position, spawner.transform.position)).ToList();
    }

    void CheckForDistressSignals() {
        GameObject distressSignal = GetDistressSignal(20.0f);
        if (distressSignal == null) {
            _state = EnemyState.Roam;
        //} else if ((distressSignal.transform.position - transform.position).magnitude < 6.0f) {
        //    _run_to = (transform.position - distressSignal.transform.position);
        //    _state = EnemyState.Run;
        //    acceleration = 3f;
        //} else if ((distressSignal.transform.position - player.position).magnitude < 3.0f) {
        //    _run_to = (transform.position - player.position);
        //    acceleration = 1.5f;
        //    _state = EnemyState.Run;
        } else {
            GameObject[] _spawners = GameObject.FindGameObjectsWithTag("SpawnPoint");
           
            GameObject spawner = SortObjectsByDistance2(_spawners.ToList())[0];
            
            Vector3 deltaSpawnerSignal = (spawner.transform.position - distressSignal.transform.position);

            //Scale the signal direction by distance to the signal
            Vector3 position = transform.position;
            Vector3 distressPosition = distressSignal.transform.position;

            Vector3 deltaSignal = position - distressPosition;
            float distanceToSignal = deltaSignal.magnitude;

            float signalWeight = 50f / (distanceToSignal * distanceToSignal + 1e-5f);
            Vector3 weightedDeltaSignal = deltaSignal * signalWeight;

            Vector3 playerPosition = player.position;

            Vector3 deltaPlayer = position - playerPosition;
            float distanceToPlayer = deltaPlayer.magnitude;

            float playerWeight = 80f / (distanceToPlayer * distanceToPlayer + 1e-5f);
            Vector3 weightedDeltaPlayer = deltaPlayer * playerWeight;

            // Calculate the run direction
            _run_to = (weightedDeltaSignal + weightedDeltaPlayer + deltaSpawnerSignal);

            //_run_to =  deltaSpawnerSignal; // (((transform.position - distressSignal.transform.position) * 1/(distressSignal.transform.position - transform.position).magnitude * 50f) + (spawner.transform.position - transform.position))/2;
            _state = EnemyState.Run;
            maxVelocity = 0.2f;
        }
    }

    // Update is called once per frame
    void Update()
    {

        if (player == null || distressSignals == null) {
            Debug.Log("Player or distressSignals not set");
            return;
        }
        CheckForDistressSignals();
        Vector2 direction = player.position - transform.position;
        switch (_state) {
            case EnemyState.Run:
                direction = _run_to; //((transform.position - _run_from.transform.position) + (transform.position - player.position) + new Vector3(Random.Range(-MAP_SIZE, MAP_SIZE), Random.Range(-MAP_SIZE, MAP_SIZE), 0))/3; //-direction;
                direction.Normalize();
                break;
            case EnemyState.Idle:
                direction = Vector2.zero;
                if (Random.Range(0, 100000) < 10) _state = EnemyState.Roam;
                break;
            case EnemyState.Roam:
                // Move towards random point
                maxVelocity = 0.1f;
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
                if (Random.Range(0, 100000) < 10) _state = EnemyState.Idle;
                break;
            case EnemyState.HugPlayer:
                maxVelocity = 0.05f;
                direction.Normalize();
                break;    
            default:
                break; 
        }
        Push(direction);
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
        if (!GetDistressSignal(10.0f)) {
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
