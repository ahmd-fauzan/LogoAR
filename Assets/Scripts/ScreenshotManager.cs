using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public class ScreenshotManager : MonoBehaviour
{
    [SerializeField]
    Image screnshotImage;

    [SerializeField]
    GameObject canvas;

    [SerializeField]
    TextMeshProUGUI textMessage;

    public void CaptureScreen()
    {
        //Menonaktifkan canvas agar hasil screenshot tidak ada tampilan ui
        canvas.SetActive(false);

        StartCoroutine(Capture((isSuccess) =>
        {
            //Mengaktifkan canvas setelah screenshot selesai
            canvas.SetActive(isSuccess);
        }));
    }

    IEnumerator Capture(Action<bool> isSuccess)
    {
        yield return new WaitForEndOfFrame();
        //Mengambil screenshot dalam bentuk Texture2D
        Texture2D texture = ScreenCapture.CaptureScreenshotAsTexture();
        //Assign screenshot pada screenshotImage sebagai preview hasil screenshot bagi player
        screnshotImage.gameObject.SetActive(true);
        screnshotImage.sprite = Utils.Texture2DToSprite(texture);

        //Membuat nama file screenshot dengan ditambahkan tanggal screenshot agar nama menjadi unique
        string fileName = "Screenshot_" + DateTime.Now.ToString("dd-MM-yyyy-HH-mm-ss") + ".png";

        //Menyimpan hasil screenshot pada gallery dengan menggunakan plugin NativeGallery
        NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(texture, "Gallery", fileName, (success, path) =>
        {
            if (success)
                StartCoroutine(ShowMessage("Screenshot berhasil disimpan di gallery"));
            else
                StartCoroutine(ShowMessage("Screnshot gagal disimpan"));
            isSuccess(success);
        });
    }

    //Mengampilkan message dan akan hilang setelah beberapa detik
    private IEnumerator ShowMessage(string message, float time = 1f)
    {
        textMessage.gameObject.SetActive(true);
        textMessage.text = message;
        yield return new WaitForSeconds(time);
        textMessage.gameObject.SetActive(false);
    }

    //Close tampilan screenshot
    public void Cancel()
    {
        screnshotImage.gameObject.SetActive(false);
    }
}
