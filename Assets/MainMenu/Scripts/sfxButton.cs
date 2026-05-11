using System.Collections;
using UnityEngine;

public class sfxButton : MonoBehaviour
{
    [SerializeField] AudioSource src;
    [SerializeField] AudioClip click;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Click(){
        if (!src.isPlaying){
            src.Stop();
            src.clip = click;
            src.Play();
        }
    }

}
