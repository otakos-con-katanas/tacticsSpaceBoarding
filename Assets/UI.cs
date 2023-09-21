using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class UI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI actionsCount;
    void Update()
    {
        // PJ character = Turn.currentChar;
        // actionsCount.text = Mathf.Round(character.actions)+"/"+character.maxActions;
    }
}
