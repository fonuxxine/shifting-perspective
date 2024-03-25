using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.DualShock;
using UnityEngine.InputSystem.XInput;

public class ControlImageSwapper : MonoBehaviour
{
    public Sprite keyboardSprite;
    public Sprite playstationSprite;
    public Sprite xboxSprite;
    public Image imageToChange;

    private enum ControllerType { Keyboard, PlayStation, Xbox }
    private ControllerType _currentControllerType;

    private void Start()
    {
        UpdateControllerType();
        UpdateObjectAccordingToController();
    }

    private void UpdateObjectAccordingToController()
    {
        switch (_currentControllerType)
        {
            case ControllerType.Keyboard:
                imageToChange.sprite = keyboardSprite;
                // Debug.Log("Using Keyboard");
                break;
            case ControllerType.PlayStation:
                imageToChange.sprite = playstationSprite;
                // Debug.Log("Using PlayStation Controller");
                break;
            case ControllerType.Xbox:
                imageToChange.sprite = xboxSprite;
                // Debug.Log("Using Xbox Controller");
                break;
            default:
                // Debug.LogError("Unknown Controller Type");
                break;
        }
    }

    private void UpdateControllerType()
    {
        foreach (InputDevice inputDevice in InputSystem.devices)
        {
            if (inputDevice is Gamepad)
            {
                // Debug.Log("it is a gamepad");
                Gamepad gamepad = (Gamepad) inputDevice;
                if (gamepad is XInputController)
                {
                    // Debug.Log("it is a xbox");
                    _currentControllerType = ControllerType.Xbox;
                    return;
                }
                else if (gamepad is DualShockGamepad)
                {
                    // Debug.Log("it is a ps");
                    _currentControllerType = ControllerType.PlayStation;
                    return;
                }
                else
                {
                    // Debug.Log("it is a other");
                    // Debug.Log(gamepad.device.name);
                    _currentControllerType = ControllerType.PlayStation;
                    return;
                }
            }
        }
        // Debug.Log("it is a default");
        _currentControllerType = ControllerType.Keyboard;
    }
}
