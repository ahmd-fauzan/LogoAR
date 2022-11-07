using UnityEngine;
using UnityEngine.UI;

public class ShareManager : MonoBehaviour
{

    [SerializeField]
    Image screenshotImage;

    public void Share()
    {
        //Mengambil texture pada component sprite Image yang sedang ditampilkan sebagai preview screenshot
        //Convert Sprite menjadi Texture2D
        Texture2D texture = Utils.SpriteToTexture2D(screenshotImage.sprite);
        //Share texture dengan menggunakan plugin NativeShare
        new NativeShare().AddFile(texture).Share();
        Destroy(texture);
    }
}
