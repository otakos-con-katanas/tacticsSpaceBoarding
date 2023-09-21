using System.Collections.Generic;
using UnityEngine;
using Unity.AI.Navigation;


public class Turn : MonoBehaviour {
    [SerializeField] private List<PJ> characters = new List<PJ>();
    [SerializeField] private int currentTurn = 0;
    [SerializeField] Transform cameraTransform; 
    public PJ currentChar;
    [SerializeField] private NavMeshSurface surface;


    // Para cargar statics cuando play mode está activado
    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    // [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]    
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    private void Start() {
        PJ[] chars = FindObjectsOfType<PJ>();
        foreach (var c in chars) {
            characters.Add(c);
            c.endTurn += nextTurn;
        }
        currentChar = chars[0];
        chars[0].canMove = true;
        chars[0].isMyTurn = true;
    }

    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
    void nextTurn(PJ pj) {
        currentTurn += 1;
        if (currentTurn >= characters.Count) {
            currentTurn = 0;
        }
        // if (!pj.active) pj.endTurn.Invoke;
        pj.actions = pj.maxActions;
        pj.canMove = false;
        pj.isMyTurn = false;
        currentChar = characters[currentTurn];
        currentChar.canMove = true;
        currentChar.isMyTurn = true;
        Debug.Log("turno terminado");
        surface.BuildNavMesh();
        
        cameraTransform.position = currentChar.transform.position;
        
        cameraTransform.parent = currentChar.transform;
    }
}
