using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UberStrike.Realtime.UnitySdk;
using UnityEngine;

public class CheatDetection : MonoBehaviour {
	private static int _gameTime;
	private static DateTime _dateTime;
	private static List<float> _speedHack_table = new List<float>();

	private int GameTime {
		get { return SystemTime.Running - _gameTime; }
	}

	private int RealTime {
		get { return (int)(DateTime.Now - _dateTime).TotalMilliseconds; }
	}

	public static void SyncSystemTime() {
		_gameTime = SystemTime.Running;
		_dateTime = DateTime.Now;
	}

	private IEnumerator StartCheckSecureMemory() {
		for (;;) {
			try {
				SecureMemoryMonitor.Instance.PerformCheck();
			} catch {
				AutoMonoBehaviour<CommConnectionManager>.Instance.DisableNetworkConnection("You have been disconnected. Please restart UberStrike.");
			}

			yield return new WaitForSeconds(10f);
		}

		yield break;
	}

	private IEnumerator StartNewSpeedhackDetection() {
		yield return new WaitForSeconds(5f);

		SyncSystemTime();
		var timeDifference = new LimitedQueue<float>(5);

		for (;;) {
			yield return new WaitForSeconds(5f);

			if (GameState.Current.HasJoinedGame) {
				timeDifference.Enqueue(GameTime / (float)RealTime);
				SyncSystemTime();

				if (timeDifference.Count == 5) {
					var avg = averageSpeedHackResults(timeDifference);

					if (avg != -1f) {
						if (avg >= 0.75) {
							break;
						}

						timeDifference.Clear();
					}
				}
			}
		}

		AutoMonoBehaviour<CommConnectionManager>.Instance.Client.Lobby.Operations.SendSpeedhackDetectionNew(timeDifference.ToList());
	}

	private float averageSpeedHackResults(IEnumerable<float> list) {
		if (IsSpeedHacking(list)) {
			_speedHack_table.Add(1f);
		} else {
			_speedHack_table.Add(0f);
		}

		if (_speedHack_table.Count == 10) {
			var num = 0f;

			foreach (var num2 in _speedHack_table) {
				var num3 = num2;
				num += num3;
			}

			var num4 = num / _speedHack_table.Count;
			_speedHack_table.Clear();

			return num4;
		}

		return -1f;
	}

	private bool IsSpeedHacking(IEnumerable<float> list) {
		var num = 0;
		var num2 = 0f;

		foreach (var num3 in list) {
			var num4 = num3;
			num2 += num4;
			num++;
		}

		num2 /= num;
		var num5 = 0f;

		foreach (var num6 in list) {
			var num7 = num6;
			num5 += Mathf.Pow(num7 - num2, 2f);
		}

		num5 /= num - 1;

		return num2 > 2f || (num2 > 1.1f && num5 < 0.02f);
	}
}
