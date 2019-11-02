using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D PlayerRb;

    //Para programar as animaçoes precisamos do componente Animator
    private Animator PlayerAnimator;

    public float Speed;
    public float JumpForce;
    public float MovHorizontal;

    //Utilizamos para verificar se o personagem esta olhando para a esquerda
    //para mudar a sua animação
    public bool IsLookLeft;

    //Verifica onde o personagem esta pisando. A sua atribuição ocorre no unity.
    public Transform GroundCheck;
    public bool IsGrounded;

    //Verifica se o personagem está atacando
    private bool IsAttack;
    public GameObject HitBoxPrefab;
    public Transform Mao;

    //Controle do jogo: musica, camera e etc...
    private GameController GameController;

    // Start is called before the first frame update
    void Start()
    {
        //Recebe o componente Rigidbody2d.
        PlayerRb = this.GetComponent<Rigidbody2D>();

        //Recebe o componente Animator do objeto Player
        PlayerAnimator = GetComponent<Animator>();

        //Recebe o objeto gamecontroler
        GameController = FindObjectOfType(typeof(GameController)) as GameController;

        GameController.PlayerTransform = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        float SpeedY;

        //Receve a movimentação na horizontal
        MovHorizontal = Input.GetAxisRaw("Horizontal");

        if (IsAttack && IsGrounded) 
        {
            MovHorizontal = 0;
        }

        //MovHorizontal > 0 -> Personagem esta andando para a direita e olhando para a direita
        if (this.MovHorizontal > 0 && this.IsLookLeft)
        {
            Flip();
        }
        else if (this.MovHorizontal < 0 && !this.IsLookLeft)
        {
            Flip();
        }

        //Adquiri a velocidade no plano Y
        SpeedY = PlayerRb.velocity.y;

        //Se pressionar o botao de pulo (Espaço)
        if (Input.GetButtonDown("Jump") && IsGrounded)
        {
            //Reproduz o som do pulo
            GameController.PlaySFX(GameController.SfxJump, 0.5f); 

            //Realiza o pulo do personagem
            PlayerRb.AddForce(new Vector2(0, JumpForce));
        }
        if (Input.GetButtonDown("Fire1") && !this.IsAttack) 
        {
            //Reproduz o som do ataque
            GameController.PlaySFX(GameController.SfxAttack, 0.5f);

            //Simboliza que o personagem esta atacando
            this.IsAttack = true;

            //Realiza o trigger para poder acontecer a animação do ataque
            PlayerAnimator.SetTrigger(ParametersAnimator.ATTACK);
        }

        //Realiza a movimentação do personagem
        PlayerRb.velocity = new Vector2(this.MovHorizontal * Speed, SpeedY);

        /*************ATUALIZANDO O ANIMATOR****************/
        //Altera o valor das variaveis criadas no animation, para realizar a troca de animação
        PlayerAnimator.SetInteger(ParametersAnimator.MOV_H, (int)MovHorizontal);

        PlayerAnimator.SetBool(ParametersAnimator.IS_GROUNDED, IsGrounded);

        PlayerAnimator.SetFloat(ParametersAnimator.SPEED_Y, SpeedY);

        PlayerAnimator.SetBool(ParametersAnimator.IS_ATTACK, IsAttack);
    }

    /// <summary>
    /// Atualiza em 0.02 segundos
    /// </summary>
    public void FixedUpdate()
    {
        IsGrounded = Physics2D.OverlapCircle(GroundCheck.position, 0.02f);
    }
    /// <summary>
    /// Muda o lado que o personagem esta olhando
    /// </summary>
    public void Flip()
    {
        float PositionX;

        //Recebe o valor contrario da posição que o personagem esta olhando
        IsLookLeft = !IsLookLeft;

        //Adquiri a posição que o personagem esta olhando
        //Esse calculo inverte o o lado que o personagem esta olhando. Todo numero multiplicado por -1 o sinal muda
        //Esquerda: é composta pelos numeros negativos e Direita: Positivos
        PositionX = transform.localScale.x * -1;

        //Realiza a inversao 
        transform.localScale = new Vector3(PositionX, transform.localScale.y, transform.localScale.z);
    }

    /// <summary>
    /// Chamada dentro da animação quando o personagem
    /// </summary>
    public void OnEndAttack() 
    {
        //Finaliza o ataque
        IsAttack = false;
    }

    /// <summary>
    /// Cria o colisor do hit box
    /// </summary>
    public void HitBoxAttack() 
    {
        GameObject HitBoxTemp;

        //Instancia a caixa de colisão
        HitBoxTemp = Instantiate(HitBoxPrefab, Mao.position, transform.localRotation);

        //Destroi a caixa de colisão
        Destroy(HitBoxTemp, 0.2f);
    }

    /// <summary>
    /// Realiza a reprodução dos sons dos passos
    /// </summary>
    public void FootStep() 
    {
        //Reproduz os sons dos passos
        GameController.PlaySFX(GameController.SfxStep[Random.Range(0, 1)], 0.5f);
    }
}
