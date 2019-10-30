using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    public Transform Background;
    //Velocidade do parallax
    public float Speed;

    private Transform Cam;
    private Vector3 PreviewCamPosition;

    // Start is called before the first frame update
    void Start()
    {
        //Recebe o transform da camera principal
        Cam = Camera.main.transform;

        //Armazena a posição da camera
        PreviewCamPosition = Cam.position;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// Ocorre após o processamento de todos os update
    /// </summary>
    void LateUpdate()
    {
        //Subtrai a posição de onde o personagem estava com a sua posição atual
        float ParallaX = PreviewCamPosition.x - Cam.position.x;

        //Armazena a posição para onde a posição ira ir
        float BgTargetX = Background.position.x + ParallaX;

        //Cria um novo vector3 com as posições de movimento do background
        Vector3 bgPosition = new Vector3(BgTargetX, Background.position.y, Background.position.z);

        //Gera o efeito do parallax
        Background.position = Vector3.Lerp(Background.position, bgPosition, Speed * Time.deltaTime);

        //Salva a posição atual da camera
        PreviewCamPosition = Cam.position;
    }
}
