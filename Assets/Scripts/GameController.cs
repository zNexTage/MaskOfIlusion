using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private Camera cam;
    public Transform PlayerTransform;

    public Transform limiteCamEsq, limiteCamDir, limiteCamSup, limiteCamInf;

    public float SpeedCam;

    #region Sounds Variables
    [Header("Audio")]
    public AudioSource SfxSource;
    public AudioSource MusicSource;

    public AudioClip SfxJump;
    public AudioClip SfxAttack;
    public AudioClip[] SfxStep;

    #endregion

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Ocorre após o processamento de todos os update
    /// </summary>
    public void LateUpdate()
    {
        float posCamX = 0;
        float posCamY = 0;
        Vector3 posCam;

        //Recebe a posição no eixo X
        posCamX = PlayerTransform.position.x;

        //Recebe a posição no eixo Y
        posCamY = PlayerTransform.position.y;

        //Validações da limitação da camera
        #region Limitação da camera
        #region Limitador Horizontal
        if (cam.transform.position.x < limiteCamEsq.position.x && PlayerTransform.position.x < limiteCamEsq.position.x)
        {
            posCamX = limiteCamEsq.position.x;
        }
        else if (cam.transform.position.x > limiteCamDir.position.x && PlayerTransform.position.x > limiteCamDir.position.x)
        {
            posCamX = limiteCamDir.position.x;
        }
        #endregion
        #region Limitador Vertical
        if (cam.transform.position.y < limiteCamInf.position.y && PlayerTransform.position.y < limiteCamInf.position.y)
        {
            posCamY = limiteCamInf.position.y;
        }
        else if (cam.transform.position.y > limiteCamSup.position.y && PlayerTransform.position.y > limiteCamSup.position.y)
        {
            posCamY = limiteCamSup.position.y;
        }
        #endregion
        #endregion

        //Cria um novo vector com os eixos do personagem 
        posCam = new Vector3(posCamX, posCamY, cam.transform.position.z);

        //Faz a camera seguir o personagem
        cam.transform.position = Vector3.Lerp(cam.transform.position, posCam, SpeedCam * Time.deltaTime);
    }

    #region Sound Play
    /// <summary>
    /// Reproduz os sons de passos, pulo, ataque e etc...
    /// </summary>
    public void PlaySFX(AudioClip SfxClip, float Volume) 
    {
        //Dispara o som uma vez
        SfxSource.PlayOneShot(SfxClip, Volume);
    }
    #endregion
}
