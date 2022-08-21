using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCCharacterManager.Models
{
	public class Timer
	{
		public Action? OnTimerEnd;

		private float timeRemaining;
		private bool isPlaying;
		
		public float CountDownTime { get; private set; }

		public Timer(float countDownTime)
		{
			CountDownTime = countDownTime;
			timeRemaining = countDownTime;
		}

		public void Start()
		{
			isPlaying = true;
		}

		public void Stop()
		{
			isPlaying = false;
		}

		public void Reset()
		{
			timeRemaining = CountDownTime;
		}

		public void Tick(float deltaTime)
		{
			if (!isPlaying) return;

			timeRemaining -= deltaTime;

			if (timeRemaining <= 0) OnTimerEnd?.Invoke();
		}
	}
}
