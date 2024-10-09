using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleSaw : MonoBehaviour
{
    private TitleController titleController;
    public float rotationSpeed;
    public GameObject Saw;
    private Animator anim;

    //SFX
    private AudioSource audioSource;
    private SaveManager saveManager;
    private float sawSFXMod = .5f;

    void Start()
    {
        anim = GetComponent<Animator>();
        anim.SetBool("Title", true);

        audioSource = GetComponent<AudioSource>();
        saveManager = GameObject.Find("SaveManager").GetComponent<SaveManager>();

        titleController = Camera.main.GetComponent<TitleController>();
    }

    void Update()
    {
        Saw.transform.Rotate(new Vector3(0,0,rotationSpeed * Time.deltaTime), Space.World);
    }

    void OnMouseDown(){
        audioSource.volume = saveManager.state.sfxVolume * sawSFXMod;
        audioSource.Play();
        anim.SetTrigger("Clicked");
        titleController.CodeClick(1);
    }
}
