using UnityEngine;

public class AnimSFXPlayer : MonoBehaviour
{
    public void PlaySFX(string sfxName)
    {
        SFXManager.Play(sfxName);
    }
}
