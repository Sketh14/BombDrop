using BombDrop.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace BombDrop.Global
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private Button _startBt, _quitBt;
        [SerializeField] private Button _quitYesBt, _quitNoBt;
        [SerializeField] private Image _quitPanel;

        //Level Creation
        [SerializeField] private RectTransform _LevelInfoPanel;
        [SerializeField] private Button _levelInfoCloseBt, _levelInfoUseCodeStartBt, _levelInfoGenerateRandomStartBt;
        [SerializeField] private TMPro.TMP_InputField _levelInfoIF;
        [SerializeField] private LevelInfo _levelInfo;

        // Start is called before the first frame update
        void Start()
        {
            //Buttons Callback
            _startBt.onClick.AddListener(() =>
            {
                _LevelInfoPanel.gameObject.SetActive(true);

                Debug.Log($"Size : {_levelInfoIF.text} | Empty : {_levelInfoIF.text == ""}  | Null : {_levelInfoIF.text == null}");
                // if (_levelInfoIF)
            });
            _quitBt.onClick.AddListener(() => _quitPanel.gameObject.SetActive(true));

            _quitYesBt.onClick.AddListener(() =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            });
            _quitNoBt.onClick.AddListener(() => _quitPanel.gameObject.SetActive(false));

            _levelInfoCloseBt.onClick.AddListener(() => _LevelInfoPanel.gameObject.SetActive(false));
            _levelInfoUseCodeStartBt.onClick.AddListener(() =>
            {
                _levelInfo.LevelHash = HelperFunctions.GenerateRandom6CharacterHash();
                // UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneToLoad.MAIN_GAMEPLAY);
            });
            _levelInfoGenerateRandomStartBt.onClick.AddListener(() =>
            {
                _levelInfo.LevelHash = "";
                // UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneToLoad.MAIN_GAMEPLAY);
            });
        }
    }
}
