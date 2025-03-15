using System.Threading;
using Task = System.Threading.Tasks.Task;

using UnityEngine;
using UnityEngine.UI;

namespace FrontLineDefense.Global
{
    public class GameUIManager : MonoBehaviour
    {
        [SerializeField] private Image _playerHealthBar, _bombCooldownBar;
        [SerializeField] private Image _gameOverPanel;
        [SerializeField] private Button _restartBt, _mainMenuBt, _pauseBt;
        [SerializeField] private TMPro.TMP_Text _coinCounterTxt, _pauseTxt;

        private CancellationTokenSource _cts;

        private byte _blinkEffectStatus;
        // private Task _blinkWaitTask;
        private const int _blinkWaitTime = 1500;

        private void OnDestroy()
        {
            _cts.Cancel();
            GameManager.Instance.OnPlayerAction -= UpdateUIHelper;
        }

        private void Start()
        {
            _cts = new CancellationTokenSource();
            // _blinkWaitTask = Task.Delay(_blinkWaitTime);
            _blinkEffectStatus = 0;
            GameManager.Instance.OnPlayerAction += UpdateUIHelper;

            //Button Callbacks
            // _restartBt.onClick.AddListener(() => GameManager.Instance.OnButtonClicked?.Invoke((int)ButtonClicked.RESTART));
            _restartBt.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneToLoad.MAIN_GAMEPLAY));
            _mainMenuBt.onClick.AddListener(() => UnityEngine.SceneManagement.SceneManager.LoadScene((int)SceneToLoad.MAIN_MENU));
            _pauseBt.onClick.AddListener(PauseGame);
        }

        private void PauseGame()
        {
            if (Time.timeScale > 0.9f)
                Time.timeScale = 0f;
            else
                Time.timeScale = 1f;

            PauseBlinkEffect();
        }

        private async void PauseBlinkEffect()
        {
            Color currentColor = _pauseTxt.color;
            if (_blinkEffectStatus == 1)
            {
                _blinkEffectStatus = 0;
                _pauseTxt.gameObject.SetActive(false);
                currentColor.a = 1f;
                _pauseTxt.color = currentColor;
                return;
            }
            else
                _blinkEffectStatus = 1;

            _pauseTxt.gameObject.SetActive(true);
            bool active = true;

            // int debugTotal = 0;

            while (_blinkEffectStatus != 0)
            {
                currentColor.a = active ? 1f : 0f;
                _pauseTxt.color = currentColor;
                active = !active;
                // await _blinkWaitTask;
                await Task.Delay(_blinkWaitTime);

                if (_cts.IsCancellationRequested) return;

                /*debugTotal++;
                if (debugTotal > 1000)
                {
                    Debug.Log($"DEbug Total : {debugTotal}");
                    return;
                }*/
            }
        }

        #region UpdateUI
        private void UpdateUIHelper(float updateVal, int actionFlag)
        {
            switch (actionFlag)
            {
                case (int)PlayerAction.BOMB_DROP:
                    UpdateBombCooldownBar(updateVal);
                    break;

                case (int)PlayerAction.PLAYER_HIT:
                    UpdateHealthBar(updateVal);
                    break;

                case (int)PlayerAction.PLAYER_DEAD:
                    UpdateHealthBar(0f);
                    _gameOverPanel.gameObject.SetActive(true);
                    break;

                case (int)PlayerAction.COIN_COLLECTED:
                    UpdateCoinCounter((int)updateVal);
                    break;
            }
        }

        private void UpdateHealthBar(float _healthPercent)
        {
            _playerHealthBar.fillAmount = _healthPercent;
        }

        private async void UpdateBombCooldownBar(float waitTime)
        {
            float timePassed = 0f;
            while (timePassed <= waitTime)
            {
                if (_cts.Token.IsCancellationRequested) return;
                timePassed += Time.deltaTime;
                _bombCooldownBar.fillAmount = (timePassed / waitTime);

                await Task.Yield();
            }
        }

        private async void UpdateCoinCounter(int coinAmount)
        {
            // await Task.Delay(UniversalConstants.WaitTimeBeforeCoinCollection);
            const float cLerpMultiplier = 2f;           //, cScaleMultiplier = 3f;
            float tempTimeElapsed = 0f;                 //, textScaleMult = 0f;
            Vector3 ogTextScale = _coinCounterTxt.transform.localScale;
            float ogAmount = GameManager.Instance.PlayerCoins - coinAmount;
            while (true)
            {
                _coinCounterTxt.text = ((int)Mathf.Lerp(ogAmount, GameManager.Instance.PlayerCoins, tempTimeElapsed)).ToString();
                _coinCounterTxt.transform.localScale = ogTextScale * (Mathf.Sin(tempTimeElapsed * 180.0f) + 1.0f);
                // textScaleMult += Time.deltaTime * cScaleMultiplier;
                tempTimeElapsed += Time.deltaTime * cLerpMultiplier;
                if (tempTimeElapsed >= 1) break;
                await Task.Yield();
            }
            _coinCounterTxt.transform.localScale = ogTextScale;

            if (_cts.Token.IsCancellationRequested) return;
            _coinCounterTxt.text = GameManager.Instance.PlayerCoins.ToString();
        }
        #endregion UpdateUI
    }
}
