using System.Collections;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;


public class ARModel : MonoBehaviour
{
    SkinnedMeshRenderer rendrer;

    [SerializeField]
    private float speed = .3f;

    [SerializeField]
    Transform rotateParent;

    Animator anim;

    float lastMultiTouchDistance;

    [SerializeField]
    private InputManager inputManager;

    enum ModelStatus
    {
        Rotating,
        Zooming,
        Idle
    }

    Coroutine blinkCoroutine;

    private ModelStatus currentStatus;

    ModelStatus CurrentStatus
    {
        get
        {
            return currentStatus;
        }
        set
        {
            if (value == ModelStatus.Idle)
            {
                blinkCoroutine = StartCoroutine(BlinkObject(0.3f));
                PlayAnimation(1);
            }
            else
            {
                StopCoroutine(blinkCoroutine);
                PlayAnimation(0);
                rendrer.enabled = true;
            }

            currentStatus = value;

        }
    }

    private void Awake()
    {
        //Inisialisasi
        inputManager = InputManager.Instance;
    }

    void Start()
    {
        //Inisialisasi
        anim = GetComponentInChildren<Animator>();

        rendrer = GetComponentInChildren<SkinnedMeshRenderer>();

        CurrentStatus = ModelStatus.Idle;
    }

    private void OnEnable()
    {
        //Subscribe event dari input manager
        inputManager.OnOneFingerTouch += Rotate;
        inputManager.OnTwoFingerTouch += Zoom;
        inputManager.OnTouchEnded += Idle;
    }

    private void OnDisable()
    {
        //Unsubscribe event dari input manager
        inputManager.OnOneFingerTouch -= Rotate;
        inputManager.OnTwoFingerTouch -= Zoom;
        inputManager.OnTouchEnded -= Idle;
    }

    private void PlayAnimation(int speed)
    {
        anim.speed = speed;
    }

    IEnumerator BlinkObject(float time)
    {
        while (true)
        {
            //Object dibuat transparant dengan menonaktifkan mesh rendrer
            rendrer.enabled = false;
            yield return new WaitForSeconds(time / 2f);

            //Object dibuat ditambpilkan dengan mengaktifkan mesh rendrer
            rendrer.enabled = true;
            yield return new WaitForSeconds(time);
        }
    }

    private void Rotate(Touch touch)
    {
        CurrentStatus = ModelStatus.Rotating;

        //Ambil nilai perubahan ketika ada satu jari menyentuh layar
        Vector2 direction = touch.delta;

        //Rotasi object sebesar nilai delta dikali kecepatan rotasi
        Quaternion rotation = Quaternion.Euler(direction.y * speed, -direction.x * speed, 0f);
        rotateParent.rotation = rotation * rotateParent.rotation;
    }

    //Player melakukan zoom
    private void Zoom(Touch firstTouch, Touch secondTouch)
    {
        CurrentStatus = ModelStatus.Zooming;

        Vector3 scale = transform.localScale;

        //Ambil jarak 2 jari ketika pertama kali menyentuh layar
        if (firstTouch.phase == TouchPhase.Began || secondTouch.phase == TouchPhase.Began)
            lastMultiTouchDistance = Vector2.Distance(firstTouch.screenPosition, secondTouch.screenPosition);

        //Jika ada salah satu jari atau keduanya yang tidak menyentuh layar maka zoom selesai
        if (firstTouch.phase != TouchPhase.Moved || secondTouch.phase != TouchPhase.Moved)
            return;

        //Ambil jarak 2 jari terbaru yaitu ketika jari bergerak di layar
        float newMultiTouchDistance = Vector2.Distance(firstTouch.screenPosition, secondTouch.screenPosition);

        float factor = newMultiTouchDistance / lastMultiTouchDistance;

        //Ubah ukuran object sebesar scale awal di kali faktor
        transform.localScale = scale * factor;

        lastMultiTouchDistance = newMultiTouchDistance;
    }

    //Player tidak melakukan interaksi
    private void Idle()
    {
        if(CurrentStatus != ModelStatus.Idle)
            CurrentStatus = ModelStatus.Idle;
    }
}
