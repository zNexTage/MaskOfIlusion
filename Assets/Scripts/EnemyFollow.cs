using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFollow : MonoBehaviour
{
    public float Speed;
    private Transform Target;
    public Animator Enemy;
    public bool IsLookLeft;
    public Player player;    

    // Start is called before the first frame update
    void Start()
    {
        Target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();

        Enemy = GetComponent<Animator>();

        player = FindObjectOfType(typeof(Player)) as Player;
    }

    // Update is called once per frame
    void Update()
    {
        if (player.transform.position.x < this.transform.position.x && this.IsLookLeft)
        {
            Flip();
        }
        else if (player.transform.position.x > this.transform.position.x && !this.IsLookLeft)
        {
            Flip();
        }

        if (Vector2.Distance(transform.position, Target.position) > 2)
        {
            transform.position = new Vector3(transform.position.x, -0.31f, transform.position.z);

            transform.position = Vector2.MoveTowards(transform.position, Target.position, Speed * Time.deltaTime);

            Enemy.SetBool("IsWalk", true);
        }
        else 
        {
            Enemy.SetBool("IsWalk", false);
        }        
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
}
