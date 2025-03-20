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
        [SerializeField] private Button _levelInfoCloseBt, _levelInfoStartBt, _openSettingsBt;           //, _levelInfoGenerateRandomStartBt;
        [SerializeField] private TMPro.TMP_InputField _levelInfoIF;
        [SerializeField] private LevelInfo _levelInfo;

        //Volume Control
        [SerializeField] private Button _settingsCloseBt;
        [SerializeField] private RectTransform _settingsPanel;
        [SerializeField] private Slider _bgmSlider, _sfxSlider, _engineSlider;

        //Audio
        // [SerializeField] private AudioManager AudioManager.Instance;

        // Start is called before the first frame update
        void Start()
        {
            //Buttons Callback
            _startBt.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySFXClip(AudioTypes.CLICK_BUTTON);
                _LevelInfoPanel.gameObject.SetActive(true);
                _levelInfoIF.text = HelperFunctions.GenerateRandom6CharacterHash();

                // Debug.Log($"Size : {_levelInfoIF.text} | Empty : {_levelInfoIF.text == ""}  | Null : {_levelInfoIF.text == null}");
                // if (_levelInfoIF)
            });
            _quitBt.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySFXClip(AudioTypes.CLICK_BUTTON);
                _quitPanel.gameObject.SetActive(true);
            });

            _quitYesBt.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySFXClip(AudioTypes.CLICK_BUTTON);
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            });
            _quitNoBt.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySFXClip(AudioTypes.CLICK_BUTTON);
                _quitPanel.gameObject.SetActive(false);
            });

            _levelInfoCloseBt.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySFXClip(AudioTypes.CLICK_BUTTON);
                _LevelInfoPanel.gameObject.SetActive(false);
            });
            _levelInfoStartBt.onClick.AddListener(() =>
            {
                AudioManager.Instance.PlaySFXClip(AudioTypes.CLICK_BUTTON);
                _levelInfo.LevelHash = _levelInfoIF.text;
                UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneToLoad.MAIN_GAMEPLAY);
                AudioManager.Instance.EngineAudioSource.gameObject.SetActive(true);
            });

            /*_levelInfoGenerateRandomStartBt.onClick.AddListener(() =>{ _levelInfo.LevelHash = "";
                // UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneToLoad.MAIN_GAMEPLAY);});*/

            //Slider CAllbacks
            // _levelInfo.LevelLoaded = false;
            _openSettingsBt.onClick.AddListener(() => { _settingsPanel.gameObject.SetActive(true); });
            _settingsCloseBt.onClick.AddListener(() => { _settingsPanel.gameObject.SetActive(false); });

            _sfxSlider.onValueChanged.AddListener((value) => { AudioManager.Instance.SetAudioSourcesLevels(AudioMixers.SFX, value); });
            _bgmSlider.onValueChanged.AddListener((value) => { AudioManager.Instance.SetAudioSourcesLevels(AudioMixers.BGM, value); });
            _engineSlider.onValueChanged.AddListener((value) => { AudioManager.Instance.SetAudioSourcesLevels(AudioMixers.ENGINE, value); });
        }
    }
}
