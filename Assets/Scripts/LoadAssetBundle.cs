using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

public class LoadAssetBundle : MonoBehaviour
{
    #region SERIALIZEFIELDS

    [SerializeField] GameObject loadingText;

    #endregion

    #region PRIVATE FIELDS

    AssetBundle bundle;

    #region ASSETS GAME OBEJCT NAME

    public const string assetName = "character";

    const string jump = "Jump";

    const string dance = "Dance";

    const string attack = "Attack";

    const string dodging= "Dodging";

    const string uppercut = "Uppercut";

#if UNITY_ANDROID

    const string characterUrl = "https://drive.google.com/uc?export=download&id=15U-uNJmNAtPDKCSSmuuFpvu7H2CxGFPR";

    const string jumpUrl= "https://drive.google.com/uc?export=download&id=1UXGdUW9lzSznf7DRc3QiLWoe4aBQ1Ubd";

    const string danceUrl= "https://drive.google.com/uc?export=download&id=1E3VfXze54UzdgFqrP6iGSbjvbCXvdgr1";

    const string attackUrl = "https://drive.google.com/uc?export=download&id=1CUttdp7-Qj8Zlz4RVYRWMaIGzEyef3k2";

    const string dodgingUrl = "https://drive.google.com/uc?export=download&id=1gqoIj7gzNrtEdn_B6JRvgIx9hrRkdWH8";

    const string uppercutUrl= "https://drive.google.com/uc?export=download&id=1p5F3wa2X9QXyxxKWMkE-5u42uayn1l-S";

#elif UNITY_IOS

    const string characterUrl = "https://drive.google.com/uc?export=download&id=1ic32334HoyFI8H4qqkiMljKKoGPLBo61";

    const string jumpUrl= "https://drive.google.com/uc?export=download&id=1sUDsHl7uae2VKyKht3-0tefVKFXV5mqJ";

    const string danceUrl= "https://drive.google.com/uc?export=download&id=17-0vEs_FcRyyqEf-8z386IyN0-6cJoH3";

    const string attackUrl = "https://drive.google.com/uc?export=download&id=1dVXZSxRe89wCHtxdXkfXYkeble6rH0rO";

    const string dodgingUrl = "https://drive.google.com/uc?export=download&id=13Y0gMo2LBHHD6HaTRjqAYRqyU2O3Pa79";

    const string uppercutUrl= "https://drive.google.com/uc?export=download&id=1ku_puRakltdcb28dAmMW9QSdqI4Fv6JD";

#endif

    #endregion

    GameObject go_Character;

    GameObject go_Cache;

    Animation animation;

    AnimationClip clip;

    Vector3 playerPosition;

    Quaternion playerQuaternion;

    Vector3 mouseReference;

    Vector3 offeset;

    Vector3 rotation = Vector3.zero;

    float sensitivity = 0.4f;

    bool isRotating = false;

#endregion

    void Start()
    {
        StartCoroutine(DownloadAsset(characterUrl, assetName));
    }

    private void Update()
    {
        if (go_Character != null)
        {
            RotatePlayer();
        }
    }
    public void LoadPlayer(Vector3 position,Quaternion quaternion)
    {
        playerPosition = position;

        playerQuaternion = quaternion;

        StartCoroutine(DownloadAsset(characterUrl, assetName));
    }

#region PRIVATE METHODS

    void RotatePlayer()
    {

        //if(Input.touchCount > 0)
        //{
        //    if(Input.GetTouch(0).phase == TouchPhase.Began)
        //    {
        //        startTouchPosition = Input.mousePosition;
        //    }

        //    else if(Input.GetTouch(0).phase == TouchPhase.Moved)
        //    {
        //        moveTouchPosition = Input.mousePosition;
        //    }
        //}

        if(isRotating)
        {
            offeset = Input.mousePosition - mouseReference;

            rotation.y = -(offeset.x + offeset.y) * sensitivity;

            go_Character.transform.Rotate(rotation);

            mouseReference = Input.mousePosition;

        }
    }
    public void OnPointerDown()
    {
        isRotating = true;

        Debug.Log("Mouse down");

        mouseReference = Input.mousePosition;
    }
    public void OnPointerUp()
    {
        isRotating = false;
    }
    IEnumerator DownloadAsset(string url, string gameObjectName)
    {
        loadingText.SetActive(true);
        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(url))
        {
            yield return uwr.SendWebRequest();

            if (uwr.isNetworkError || uwr.isHttpError)
            {
                Debug.Log(uwr.error);
            }
            else
            {
                loadingText.SetActive(false);
                // Get downloaded asset bundle
                bundle = DownloadHandlerAssetBundle.GetContent(uwr);

                if (url.Contains(characterUrl))
                {
                    LoadIntoScene(gameObjectName);
                }
                else
                {
                    LoadRespectiveFBX(gameObjectName);
                }

                bundle.Unload(false);

            }
        }
    }

    void LoadRespectiveFBX(string name)
    {
        GameObject obj = bundle.LoadAsset(name) as GameObject;

        go_Cache = Instantiate(obj, transform);

        go_Cache.SetActive(false);

        clip = go_Cache.GetComponent<Animation>().clip;

        animation = go_Character.GetComponent<Animation>();

        if (clip != null)
        {
            animation.AddClip(clip, clip.name);

            animation.clip = clip;

            PlayAnimation();

            UnloadGameObject();
        }

    }
    void LoadIntoScene(string name)
    {
        GameObject obj = bundle.LoadAsset(name) as GameObject;

        go_Character = Instantiate(obj,playerPosition,playerQuaternion);

    }
    void PlayAnimation()
    {
        animation.Play();
    }

#endregion

#region BUTTON ACTIONS

    public void OnClickJump()
    {
        StartCoroutine(DownloadAsset (jumpUrl, jump));  
    }

    public void OnClickDance()
    {
        StartCoroutine(DownloadAsset(danceUrl, dance));
    }

    public void OnClickAttack()
    {
        StartCoroutine(DownloadAsset(attackUrl, attack));
    }
    public void OnClickDodging()
    {
        StartCoroutine(DownloadAsset(dodgingUrl, dodging));
    }
    public void OnClickUpperCut()
    {
        StartCoroutine(DownloadAsset(uppercutUrl, uppercut));
    }

    void UnloadGameObject()
    {
        if (go_Cache != null)
            Destroy(go_Cache);
    }

#endregion
}
