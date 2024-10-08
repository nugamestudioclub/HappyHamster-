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
    public float maxVelocity = 2f; // in units per second
    private EnemyState _state = EnemyState.Roam;

    private FMOD.Studio.EventInstance hamsterDieInstance;
    public FMODUnity.EventReference hamsterDieEvent;

    public EnemyObjectPool enemyPool; // For improved performance

    public DistressObjectPool distressPool; // For improved performance

    private float distressCheckInterval = 0.25f; // Check every [value] second
    private float timeSinceLastCheck = 0f;

    void SetRandomTarget() {
        _target = new Vector2(Random.Range(-MAP_SIZE, MAP_SIZE), Random.Range(-MAP_SIZE, MAP_SIZE));
    }
    
    // Start is called before the first frame update
    void Start()
    {
        //enemyCount += 1;
        enemyBody = gameObject.GetComponent<Rigidbody2D>();
        // Get the pools from scene
        enemyPool = FindObjectOfType<EnemyObjectPool>();
        distressPool = FindObjectOfType<DistressObjectPool>();
        SetRandomTarget();

        hamsterDieInstance = FMODUnity.RuntimeManager.CreateInstance(hamsterDieEvent);
    }

    void Push(Vector2 direction) {
        //move the enemy without force
        //transform.position += new Vector3(direction.x, direction.y, 0) * (maxVelocity * 3 * Time.fixedDeltaTime);
        enemyBody.velocity = new Vector3(direction.x, direction.y, 0) * (maxVelocity * 3);

        spriteRenderer.flipX = (direction.x >= 0);
    }

    // Update is called once per frame

    GameObject GetDistressSignal(float distance) {
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

    // void CheckForDistressSignals() {
    //     GameObject distressSignal = GetDistressSignal(20.0f);
    //     if (distressSignal == null) {
    //         _state = EnemyState.Roam;
    //     } else {
    //         GameObject[] _spawners = GameObject.FindGameObjectsWithTag("SpawnPoint");

    //         GameObject spawner = SortObjectsByDistance2(_spawners.ToList())[0];
            
    //         Vector3 deltaSpawnerSignal = (spawner.transform.position - distressSignal.transform.position);

    //         //Scale the signal direction by distance to the signal
    //         Vector3 position = transform.position;
    //         Vector3 distressPosition = distressSignal.transform.position;

    //         Vector3 deltaSignal = position - distressPosition;
    //         float distanceToSignal = deltaSignal.magnitude;

    //         float signalWeight = 40f / (distanceToSignal * distanceToSignal + 1e-5f);
    //         Vector3 weightedDeltaSignal = deltaSignal * signalWeight;

    //         Vector3 playerPosition = player.position;

    //         Vector3 deltaPlayer = position - playerPosition;
    //         float distanceToPlayer = deltaPlayer.magnitude;

    //         float playerWeight = 60f / (distanceToPlayer * distanceToPlayer + 1e-5f);
    //         Vector3 weightedDeltaPlayer = deltaPlayer * playerWeight;

    //         if (playerWeight > signalWeight) {
    //             weightedDeltaSignal = weightedDeltaPlayer;
    //         }

    //         // Calculate the run direction
    //         _run_to = (weightedDeltaSignal + deltaSpawnerSignal); //(weightedDeltaSignal + weightedDeltaPlayer + deltaSpawnerSignal);

    //         //_run_to =  deltaSpawnerSignal; // (((transform.position - distressSignal.transform.position) * 1/(distressSignal.transform.position - transform.position).magnitude * 50f) + (spawner.transform.position - transform.position))/2;
    //         _state = EnemyState.Run;
    //         maxVelocity = 6f;
    //     }
    // }

    // Experimental
    void CheckForDistressSignals() {
        GameObject distressSignal = GetDistressSignal(20.0f);
        
        if (distressSignal == null) {
            _state = EnemyState.Roam;
            return;
        }
        
        GameObject[] _spawners = GameObject.FindGameObjectsWithTag("SpawnPoint");
        GameObject spawner = SortObjectsByDistance2(_spawners.ToList())[0];
        
        Vector3 position = transform.position;
        Vector3 distressPosition = distressSignal.transform.position;

        Vector3 deltaSignal = position - distressPosition;
        float distanceToSignal = deltaSignal.magnitude;

        // Calculate signal weight
        float signalWeight = 40f / (distanceToSignal * distanceToSignal + 1e-5f);
        Vector3 weightedDeltaSignal = deltaSignal * signalWeight;

        Vector3 playerPosition = player.position;
        Vector3 deltaPlayer = position - playerPosition;
        float distanceToPlayer = deltaPlayer.magnitude;

        // Calculate player weight
        float playerWeight = 60f / (distanceToPlayer * distanceToPlayer + 1e-5f);
        Vector3 weightedDeltaPlayer = deltaPlayer * playerWeight;

        // Choose the stronger direction
        Vector3 targetDirection = playerWeight > signalWeight ? weightedDeltaPlayer : weightedDeltaSignal;

        // Incorporate the spawner position
        Vector3 deltaSpawnerSignal = (spawner.transform.position - distressPosition);
        _run_to = Vector3.Lerp(targetDirection, deltaSpawnerSignal, 0.5f); // Blend towards spawner

        // Normalize the direction and apply velocity
        _run_to.Normalize();
        _run_to *= maxVelocity;

        // Update enemy state
        _state = EnemyState.Run;
    }

    // Update is called once per frame
    void Update()
    {
        // No longer checks per frame
        timeSinceLastCheck += Time.deltaTime;
        if (timeSinceLastCheck < distressCheckInterval) {
            return;
        }
        timeSinceLastCheck = 0f; // Reset the timer
        CheckForDistressSignals();

        Vector2 direction = player.position - transform.position;
        switch (_state) {
            case EnemyState.Run:
                direction = _run_to; //((transform.position - _run_from.transform.position) + (transform.position - player.position) + new Vector3(Random.Range(-MAP_SIZE, MAP_SIZE), Random.Range(-MAP_SIZE, MAP_SIZE), 0))/3; //-direction;
                //direction.Normalize();
                break;
            case EnemyState.Idle:
                direction = Vector2.zero;
                if (Random.Range(0, 100000) < 10) _state = EnemyState.Roam;
                break;
            case EnemyState.Roam:
                // Move towards random point
                maxVelocity = 3f;
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
                maxVelocity = 1.5f;
                direction.Normalize();
                break;    
            default:
                break; 
        }
        Push(direction);
    }


    // Kill without Object Pool Optimization
    // public void Kill()
    // {
    //     // TODO: Death vfx and animations and score
    //     if (!GetDistressSignal(5.0f)) {
    //         GameObject signal = Instantiate(enemyKilledSignalPrefab, transform.position, Quaternion.identity);
    //         signal.transform.parent = distressSignals.transform;
    //         signal.transform.localScale = transform.localScale;
    //     }
    //     hamsterDieInstance.start();
    //     gameObject.SetActive(false);
    //     enemyCount--;
    //     Destroy(gameObject, 1f);
    // }

    // Kill with Object Pool Optimization (OLD)
    // public void Kill()
    // {
    //     // Check for distress signal before handling the enemy's death
    //     if (!GetDistressSignal(5.0f))
    //     {
    //         // Get a distress signal from the pool if needed
    //         GameObject signal = distressSignalPool.GetFromPool();
    //         if (signal != null) // Ensure we have a signal
    //         {
    //             signal.transform.position = transform.position;
    //             signal.transform.localScale = transform.localScale;
    //             signal.transform.parent = distressSignals.transform;
    //         }
    //     }

    //     // Play the death sound effect
    //     hamsterDieInstance.start();
        
    //     // Decrement the enemy count
    //     enemyCount--;

    //     // Return the enemy GameObject to the pool instead of destroying it
    //     gameObject.SetActive(false); // Deactivate the enemy
    //     distressSignalPool.ReturnToPool(gameObject); // Return it to the pool
    // }

    // Kill without Object Pool Optimization
    public void Kill()
    {
        // TODO: Death vfx and animations and score

        // Potentially spawn a distress signal 
        if (Random.Range(0, 100) < 10) { //(!GetDistressSignal(5.0f)) {
            // Get a distress signal from the pool if needed
            GameObject signal = distressPool.GetFromPool();
            
            if (signal != null) // Ensure we have a signal
            {
                signal.transform.position = transform.position;
                signal.transform.localScale = transform.localScale;
                signal.transform.parent = distressSignals.transform;
                // No need to returnToPool, signal self deletes
            }
        }

        // Play the death sound effect
        hamsterDieInstance.start();
        
        // Deincrement the enemy count (well it's in pool now)
        //enemyCount--;

        // Return the enemy GameObject to the pool instead of destroying it
        enemyPool.ReturnToPool(gameObject);
    }
}
