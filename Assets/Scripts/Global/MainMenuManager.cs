using UnityEngine;
using UnityEngine.UI;

namespace FrontLineDefense.Global
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField] private Button _startBt, _quitBt;
        [SerializeField] private Button _quitYesBt, _quitNoBt;
        [SerializeField] private Image _quitPanel;

        // Start is called before the first frame update
        void Start()
        {
            //Buttons Callback
            _startBt.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneToLoad.MAIN_GAMEPLAY));
            _quitBt.onClick.AddListener(() => _quitPanel.gameObject.SetActive(true));

            _quitYesBt.onClick.AddListener(() =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            });
            _quitNoBt.onClick.AddListener(() => _quitPanel.gameObject.SetActive(false));
        }
    }
}
