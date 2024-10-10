using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleMole : MonoBehaviour
{
    private TitleController titleController;
    public SpriteRenderer[] Zs;
    private Animator moleAnim;
    public Animator zAnim;
    public AudioClip moleSFX;
    public float moleSFXMod;

    private bool moleAwake = false;
    private bool zAppearing = false;
    private float ZAppearSpeed = 2f;
    private float zAppearAlpha = 0;
    private float moleSleepLim = 2;
    private float moleSleepCounter = 0;
    private float volume = .5f;

    void Start()
    {
        moleAnim = GetComponent<Animator>();
        volume = GameObject.Find("SaveManager").GetComponent<SaveManager>().state.sfxVolume;
        titleController = Camera.main.GetComponent<TitleController>();
    }

    void Update()
    {
        if (moleAwake){
            moleSleepCounter += Time.deltaTime;
            if (moleSleepCounter >= moleSleepLim){
                moleAwake = false;
                moleAnim.SetTrigger("Sleep");
            }
        }

        if (zAppearing){
            if (zAppearAlpha >= 1){
                zAppearing = false;
                zAnim.SetBool("Awake", false);
                zAppearAlpha = 0;
            } else {
                zAppearAlpha += Time.deltaTime * ZAppearSpeed;
                for(int i=0; i < 3; i++){
                    Zs[i].color = new Vector4(1,1,1,zAppearAlpha);
                }
            }
        }
    }

    public void ZAppear(){
        zAppearing = true;
    }

    void OnMouseDown(){
        moleSleepCounter = 0;
        moleAwake = true;
        moleAnim.SetTrigger("Clicked");
        titleController.CodeClick(2);
        AudioSource.PlayClipAtPoint(moleSFX, Camera.main.transform.position, volume * moleSFXMod);

        zAnim.SetBool("Awake", true);
        for(int i=0; i<3; i++){
            Zs[i].color = new Vector4(1,1,1,0);
        }
    }
}
