using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : SingletonMonoBehaviour<InputManager>
{
    [SerializeField] private PlayerInput playerInput;

    private Input inputs;
    public Input Inputs
    {
        get
        {
            if (inputs == null) inputs = new Input();
            return inputs;
        }
    }

    protected override void PostAwake()
    {
        Inputs.DefaultMap.Enable();
    }
}
