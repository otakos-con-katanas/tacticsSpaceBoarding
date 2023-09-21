using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;


[RequireComponent(typeof(Rigidbody))]
public class PJ : MonoBehaviour
{
    public Vector3 lookAxis;
    public float life = 5;
    PlayerInput playerInput;
    public GameObject mira;

    float maxDistance = 20f;
    public float maxActions = 12f;
    public float actions = 12f;
    float stickDeadZone = 0.2f;
    public float speed = 12f;
    public float magnitude = 0f;
    public delegate void EndTurn(PJ pj);
    public event EndTurn endTurn;
    public bool canMove = false;
    public bool isMyTurn = false;
    public int actionIndex = 0;
    public List<CharacterAction> charActions = new List<CharacterAction>();
    public delegate void GetDamage(PJ pj);
    // public event GetDamage getDamage;
    public int team = 1;
    public Gamepad gamepad;
    public bool active = true;

    public bool isPlayerControlledCharacter = true;

    public Transform iaGoal;
    void Start()
    {
        playerInput = new PlayerInput();
        playerInput.Aim.Enable();
        playerInput.Aim.Shoot.performed += shoot;
    }

    void Update()
    {
        
        var renderer = GetComponent<Renderer>();
        /* if (team == 1)
        {
            renderer.material.SetColor("_Color", Color.blue);
        }
        else
        {
            renderer.material.SetColor("_Color", Color.red);

        } */
        if (life < 1)
        {
            active = false;
            renderer.material.SetColor("_Color", Color.gray);
            actions = 0;
        }
        if (!canMove)
        {
            return;
        }
        
        aim();
        move();
        if (actions <= 0)
        {
            endTurn.Invoke(this);
        }
        if (!isPlayerControlledCharacter)
        {
            iaController();
            return;
        }
    }
    void iaController()
    {
        
        // las áreas expuestas deben tener un costo más alto para moverse
        // entones el agent evitará tomar esas rutas
        // investigar generación del navmesh
        
        /* NavMeshAgent agent = GetComponent<NavMeshAgent>();
        // sobre IA a distancia
        // https://forum.unity.com/threads/finding-a-location-where-archer-can-shoot-player.776693/
        agent.destination = transform.position;
        if (actions > 0 && isMyTurn && canMove) {
            actions -= 5f;
            agent.destination = iaGoal.position;
        } */
        // IA que se esconde
        // https://www.youtube.com/watch?v=t9e2XBQY4Og&ab_channel=LlamAcademy
        // pesos para buscar escondites ofensivos o defensivos
        
    }

    private void aim()
    {
        lookAxis = playerInput.Aim.LookAt.ReadValue<Vector2>();
        /* if (Gamepad.current == null) {
            lookAxis =  Input.mousePosition;
        } */
        lookAxis = new Vector3(lookAxis.x, transform.position.y, lookAxis.y);
        Ray ray = new Ray(transform.position, lookAxis.normalized);
        var pos = ray.GetPoint(5);
        pos.y = transform.position.y;
        mira.transform.position = pos;

        RaycastHit hit;
        Vector3 far = ray.GetPoint(maxDistance) - ray.origin;
        far.y = 0;
        bool isHit = Physics.Raycast(transform.position, far, out hit, maxDistance);
        Debug.DrawRay(transform.position, far, Color.red);
        if (isHit)
        {
            mira.transform.position = hit.point;
        }
    }

    private void move()
    {
        Vector3 v = playerInput.Aim.Move.ReadValue<Vector2>();
        magnitude = v.magnitude;
        if (v.magnitude < stickDeadZone) return;
        v = new Vector3(v.x, 0, v.y);
        if (actions > 0)
        {
            transform.position += (v * speed) * Time.deltaTime;
            actions -= 0.3f;
        }
    }
    void shoot(InputAction.CallbackContext context)
    {

        charActions[actionIndex].perform(this);
    }
}
