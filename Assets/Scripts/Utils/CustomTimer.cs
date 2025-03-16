using System;
using System.Threading;
using System.Threading.Tasks;

namespace BombDrop.Utils
{
    public class CustomTimer
    {
        private CancellationTokenSource _cts;

        public CustomTimer() { _cts = new CancellationTokenSource(); }
        ~CustomTimer() { _cts.Cancel(); }

        // <summary> Can only wait time in intervals of 0.1s </summary>
        public async Task WaitForSeconds(float timeInSec)
        {
            // float startTime = UnityEngine.Time.deltaTime;
            float currentTime = 0f;

            // int tempCount = 0;
            while (currentTime < timeInSec)
            {
                if (_cts.Token.IsCancellationRequested) break;

                // UnityEngine.Debug.Log($"WaitForSeconds CAlled | currentTime : {currentTime} | timeInSec : {timeInSec} | " +
                // $"elapsed : {currentTime - startTime}");
                currentTime += UnityEngine.Time.deltaTime;
                // await Task.Delay(100);                          //Add this if less precise than 0.1 is required
                await Task.Yield();                             //Add this if more precise than 0.1 is required

                // if (tempCount % 100 == 0)
                // {
                //     tempCount = 0;
                //     UnityEngine.Debug.Log($"Count reached | currentTime : {currentTime} | TimeScale : {UnityEngine.Time.timeScale}");
                // }
                // tempCount++;
            }
        }
    }
}