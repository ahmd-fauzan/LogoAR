using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

[DefaultExecutionOrder(-1)]
public class InputManager : Singleton<InputManager>
{
    public delegate void OneFingerTouchDelegate(Touch touch);
    public delegate void TwoFingerTouchDelegate(Touch firstTouch, Touch secondTouch);
    public delegate void TouchEndDelegate();

    public event OneFingerTouchDelegate OnOneFingerTouch;
    public event TwoFingerTouchDelegate OnTwoFingerTouch;
    public event TouchEndDelegate OnTouchEnded;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        //Cek tidak ada jari yang menyentuh layar
        if (Touch.activeFingers.Count == 0)
            OnTouchEnded?.Invoke();
        //Cek ada satu jari yang menyentuh layar
        else if (Touch.activeFingers.Count == 1)
            OnOneFingerTouch?.Invoke(Touch.activeTouches[0]);
        //Cek ada dua jari yang menyentuh layar
        else if (Touch.activeFingers.Count == 2)
            OnTwoFingerTouch?.Invoke(Touch.activeTouches[0], Touch.activeTouches[1]);


    }
}
