using UnityEngine;
using UnityEngine.InputSystem; // Obrigatório na Unity 6

public class ControleJogadorCompleto : MonoBehaviour
{
    [Header("Configurações de Movimento")]
    public float velocidade = 5f;
    private Rigidbody rb;
    private Vector2 inputMovimento;

    [Header("Configurações da Câmera")]
    public Transform cameraTransform; // Arraste a sua Main Camera aqui no Inspector
    public float sensibilidadeMouse = 0.1f;
    private float rotationalX = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();

        // Trava e esconde o mouse na tela
        Cursor.lockState = CursorLockMode.Locked;

        // Se você esquecer de arrastar a câmera, o script tenta achar automaticamente no objeto filho
        if (cameraTransform == null)
        {
            cameraTransform = GetComponentInChildren<Camera>().transform;
        }
    }

    void Update()
    {
        // 1. LEITURA DIRETA DO TECLADO (Solução para o erro do wasdAction)
        if (Keyboard.current != null)
        {
            // Lê se as teclas estão pressionadas (retorna 1 se sim, 0 se não)
            float frente = Keyboard.current.wKey.isPressed ? 1f : 0f;
            float tras = Keyboard.current.sKey.isPressed ? 1f : 0f;
            float direita = Keyboard.current.dKey.isPressed ? 1f : 0f;
            float esquerda = Keyboard.current.aKey.isPressed ? 1f : 0f;

            // Monta o vetor de movimento baseado nas teclas pressionadas
            inputMovimento.y = frente - tras;   // Eixo Vertical (W - S)
            inputMovimento.x = direita - esquerda; // Eixo Horizontal (D - A)
        }

        // 2. LEITURA E CONTROLE DO MOUSE (CÂMERA)
        if (Mouse.current != null && cameraTransform != null)
        {
            Vector2 deltaMouse = Mouse.current.delta.ReadValue(); //

            float mouseX = deltaMouse.x * sensibilidadeMouse;
            float mouseY = deltaMouse.y * sensibilidadeMouse;

            // Rotação Vertical (Olhar para cima e para baixo - afeta apenas a Câmera)
            rotationalX -= mouseY;
            rotationalX = Mathf.Clamp(rotationalX, -90f, 90f);
            cameraTransform.localRotation = Quaternion.Euler(rotationalX, 0f, 0f);

            // Rotação Horizontal (Olhar para os lados - afeta o Corpo inteiro do jogador)
            transform.Rotate(Vector3.up * mouseX);
        }
    }

    void FixedUpdate()
    {
        // 3. APLICAÇÃO DO MOVIMENTO FÍSICO
        // Converte o input do teclado baseado na direção que o jogador está olhando
        Vector3 direcao = (transform.forward * inputMovimento.y + transform.right * inputMovimento.x).normalized;

        // Aplica a velocidade no Rigidbody usando a nova propriedade da Unity 6 (linearVelocity)
        Vector3 velocidadeFinal = direcao * velocidade;
        velocidadeFinal.y = rb.linearVelocity.y; // Mantém a gravidade original do objeto

        rb.linearVelocity = velocidadeFinal;
    }
}
