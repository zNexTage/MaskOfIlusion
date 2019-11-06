using Assets.Scripts.Util;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float Speed;
    private Transform Target;
    public Animator Enemy;
    public bool IsLookLeft;
    bool Attack;
    public float PositionY;
    public GameObject HitBoxPrefab;
    public Transform Mao;
    GameObject HitBoxTemp;
    public float Life;
    public Player Jogador;
    public bool IsDead;
    public static GameObject EnemyObj;

    // Start is called before the first frame update
    void Start()
    {
        //Procura o objeto Player que sera o target (alvo)
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        //Adquiri o componente Animator do objeto
        Enemy = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Se a vida do personagem for maior que 0
        if (Life > 0 && !IsDead)
        {
            //Troca o lado para qual o personagem inimigo esta olhando
            if (Target.transform.position.x < this.transform.position.x && this.IsLookLeft)
            {
                Flip();
            }
            else if (Target.transform.position.x > this.transform.position.x && !this.IsLookLeft)
            {
                Flip();
            }

            //Verifica a distancia entre o inimigo e o jogador e se o inimigo ja esta atacando
            if (Vector2.Distance(transform.position, Target.position) > 2 && !Attack)
            {
                //Verifica pela tag qual é o inimigo para determinar sua posição eixo Y
                VerifyTag();

                //Recebe as posições para qual o personagem inimigo irá se mover
                transform.position = new Vector3(transform.position.x, PositionY, transform.position.z);

                //Realiza a movimentação
                transform.position = Vector2.MoveTowards(transform.position, Target.position, Speed * Time.deltaTime);

                //Seta a variavel IsWalk como true para o personagem realiza a animação 
                Enemy.SetBool("IsWalk", true);

            }
            else
            {
                //Se o personagem inimigo estiver numa distancia menor do que a condição, ele para de andar
                Enemy.SetBool("IsWalk", false);

                //Realiza o ataque.
                InitAttack();
            }
        }



        //Seta a animação de ataque;
        Enemy.SetBool("IsAttack", Attack);

        Enemy.SetBool("IsDead", IsDead);
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
    /// Seta a variavel de ataque como true
    /// </summary>
    public void InitAttack()
    {
        Attack = true;
    }

    /// <summary>
    /// Seta a variavel de finalizar o ataque como false e destroi o hitbox do ataque
    /// </summary>
    public void EndAttack()
    {
        Attack = false;

        DestroyHitBoxTemp();
    }

    public void DestroyHitBoxTemp() 
    {
        //Destroi a caixa de colisão
        Destroy(HitBoxTemp, 0.2f);
    }

    /// <summary>
    /// Verifica pela tag qual é o personagem inimigo para determinar a sua posição no eixo Y
    /// </summary>
    public void VerifyTag()
    {
        switch (this.tag)
        {
            //Se for o inimigo Loren Knight
            case TagParameters.LOREN_KNIGHT:
                {
                    //Define sua posição no eixo Y
                    PositionY = -0.31f;
                    break;
                }
            //Se for o inimigo Minotauro
            case TagParameters.BOSS_MINOTAURO:
                {
                    //Define sua posição no eixo Y
                    PositionY = -0.88f;
                    break;
                }
            //Se for o inimigo Blue Knight
            case TagParameters.BOSS_BLUE_KNIGHT:
                {
                    //Define sua posição no eixo Y
                    PositionY = -0.78f;
                    break;
                }
            case TagParameters.BOSS_FINAL_BOSS:
                {
                    PositionY = -0.87f;
                    break;
                }
        }
    }

    /// <summary>
    /// HitBox do ataquel
    /// </summary>
    public void HitBoxAttack()
    {
        BoxCollider2D boxPlayer;

        //Instancia a caixa de colisão
        HitBoxTemp = Instantiate(HitBoxPrefab, Mao.position, transform.localRotation);

        boxPlayer = HitBoxTemp.GetComponent<BoxCollider2D>();

        boxPlayer.size = new Vector2(1.118301f, 2.52275f);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagParameters.HIT_BOX)
        {
            Life -= Jogador.Dano;

            if (Life <= 0)
            {
                Attack = false;
                IsDead = true;
            }
        }
    }

    /// <summary>
    /// Ocorre quando o inimigo personagem morre
    /// </summary>
    public void OnDead()
    {
        Destroy(this.gameObject);
    }
}
