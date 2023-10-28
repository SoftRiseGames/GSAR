using UnityEngine;
using TMPro;

public class Geser_StateSystem : MonoBehaviour
{
    public enum AnimState
    {
        SwordReady,
        SwordWalk,
        SwordAttack,
        BowReady,
        
    }
    public AnimState currentState = AnimState.SwordReady;

    public TextMeshProUGUI stateText;

    private void Update()
    {
        StateControl();
    }
    private void StateControl()
    {
        switch (currentState)
        {
            case AnimState.SwordReady:
                stateText.text = "Sword Ready";
                break;

            case AnimState.BowReady:
                stateText.text = "Bow Ready";
                break;
            case AnimState.SwordWalk:
                stateText.text = "Sword Walk";
                break;

            case AnimState.SwordAttack:
                stateText.text = "SwordAttack";
                break;
        }
    }
}
