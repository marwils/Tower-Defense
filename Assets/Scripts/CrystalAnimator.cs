using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public class CrystalAnimator : MonoBehaviour
{
    Animator _animator;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _animator.SetTrigger("Create");
    }

    private void Update()
    {
        // Prüfe auf Mausklick
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue());

            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                Debug.Log("Geklickt auf: " + hit.collider.gameObject.name);

                if (hit.collider is BoxCollider)
                {
                    Debug.Log("BoxCollider getroffen!");
                    _animator.SetTrigger("Destroy");
                }
            }
        }
    }
}
