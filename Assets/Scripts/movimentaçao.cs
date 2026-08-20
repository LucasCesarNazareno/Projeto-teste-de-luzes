using UnityEngine;
using UnityEngine.InputSystem;
public class ControleJogadorCompleto : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 5f;
    private Rigidbody rb;
    private Vector2 inputMovimento;

    [Header("Configurações da Câmera")]
    public Transform cameraTransform;
    public float sensibilidadeMouse = 0.1f;
    private float rotationalX = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        Cursor.lockState = CursorLockMode.Locked;

        if (cameraTransform == null)
        {
            cameraTransform = GetComponentInChildren<Camera>().transform;
        }
    }

    void Update()
    {
        if (Keyboard.current != null)
        {
            float frente = Keyboard.current.wKey.isPressed ? 1f : 0f;
            float tras = Keyboard.current.sKey.isPressed ? 1f : 0f;
            float direita = Keyboard.current.dKey.isPressed ? 1f : 0f;
            float esquerda = Keyboard.current.aKey.isPressed ? 1f : 0f;

            inputMovimento.y = frente - tras;
            inputMovimento.x = direita - esquerda;
        }

        if (Mouse.current != null && cameraTransform != null)
        {
            Vector2 deltaMouse = Mouse.current.delta.ReadValue();

            float mouseX = deltaMouse.x * sensibilidadeMouse;
            float mouseY = deltaMouse.y * sensibilidadeMouse;

            rotationalX -= mouseY;
            rotationalX = Mathf.Clamp(rotationalX, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(rotationalX, 0f, 0f);

            transform.Rotate(Vector3.up * mouseX);
        }
    }

    void FixedUpdate()
    {
        Vector3 direcao = (transform.forward * inputMovimento.y + transform.right * inputMovimento.x).normalized;

        Vector3 velocidadeFinal = direcao * velocidade;
        velocidadeFinal.y = rb.linearVelocity.y;

        rb.linearVelocity = velocidadeFinal;
    }
}
