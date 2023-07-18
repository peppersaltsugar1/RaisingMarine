using UnityEngine;

public class SelectableCharacter : MonoBehaviour {

    public SpriteRenderer selectImage;
    private void Awake() {
        selectImage.enabled = true;
    }

    //Turns off the sprite renderer
    public void TurnOffSelector()
    {
        //selectImage.enabled = false;
    }

    //Turns on the sprite renderer
    public void TurnOnSelector()    
    {
        selectImage.enabled = true;
        UIManager.instance.PlayerUISet();
    }

}
