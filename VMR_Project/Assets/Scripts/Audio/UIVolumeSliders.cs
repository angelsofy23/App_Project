using UnityEngine;
using UnityEngine.UI;

public class UIVolumeSliders : MonoBehaviour
{
   public Slider musicSlider, SFXSlider;
   public Sprite muteOnImage;
   public Sprite muteOffImage;
   public Button muteMusicButton;
   public Button muteSFXButton;
   private bool isOnMusic;
   private bool isOnSFX;
   void Start(){
      isOnMusic = !AudioManager.Instance.musicSource.mute;
      isOnSFX = !AudioManager.Instance.SFXSource.mute;

      muteMusicButton.image.sprite = isOnMusic ? muteOffImage : muteOnImage;
      muteSFXButton.image.sprite = isOnSFX ? muteOffImage : muteOnImage;
   }
   //Método para mutar/desmutar a música
   public void MuteMusic()
   {
      AudioManager.Instance.MuteMusic();

      // Atualiza o sprite do botão de mutar a música com base no estado atual
      if (isOnMusic)
      {
         muteMusicButton.image.sprite = muteOnImage;
         isOnMusic = false;
      }
      else
      {
         muteMusicButton.image.sprite = muteOffImage;
         isOnMusic = true;
      }
   }

   //Método para mutar/desmutar os efeitos sonoros
   public void MuteSFX()
   {
      AudioManager.Instance.MuteSFX();

      // Atualiza o sprite do botão de mutar os efeitos sonoros com base no estado atual
      if (isOnSFX)
      {
         muteSFXButton.image.sprite = muteOnImage;
         isOnSFX = false;
      }
      else
      {
         muteSFXButton.image.sprite = muteOffImage;
         isOnSFX = true;
      }

   }

   //Método para ajustar o volume da música com base no valor do slider
   public void MusicVolume()
   {
      AudioManager.Instance.MusicVolume(musicSlider.value);
   }

   //Método para ajustar o volume da música com base no valor do slider
   public void SFXVolume()
   {
      AudioManager.Instance.SFXVolume(SFXSlider.value);
   }
}

