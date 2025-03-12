using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Timed : Singleton<Timed>
{

		private static List<IEnumerator> _listIEnumerator = new List<IEnumerator>();

		public static readonly WaitForSeconds Wait25MS  = new WaitForSeconds(0.025f);
		public static readonly WaitForSeconds Wait50MS  = new WaitForSeconds(0.05f);
		public static readonly WaitForSeconds Wait100MS = new WaitForSeconds(0.1f);
		public static readonly WaitForSeconds Wait200MS = new WaitForSeconds(0.2f);
		public static readonly WaitForSeconds Wait250MS = new WaitForSeconds(0.25f);
		public static readonly WaitForSeconds Wait300MS = new WaitForSeconds(0.3f);
		public static readonly WaitForSeconds Wait333MS = new WaitForSeconds(0.33f);
		public static readonly WaitForSeconds Wait400MS = new WaitForSeconds(0.4f);
		public static readonly WaitForSeconds Wait500MS = new WaitForSeconds(0.5f);
		public static readonly WaitForSeconds Wait750MS = new WaitForSeconds(0.75f);
		public static readonly WaitForSeconds Wait1S    = new WaitForSeconds(1f);
		public static readonly WaitForSeconds Wait1_5S  = new WaitForSeconds(1.5f);
		public static readonly WaitForSeconds Wait2S    = new WaitForSeconds(2f);
		public static readonly WaitForSeconds Wait2_5S  = new WaitForSeconds(2.5f);
		public static readonly WaitForSeconds Wait3S    = new WaitForSeconds(3f);
		public static readonly WaitForSeconds Wait5S    = new WaitForSeconds(5f);

		protected void OnDisable()
		{
			StopAllCoroutines();
		}


	#region Internal

	#region Queue

		private void _AddToQueue(IEnumerator function)
		{
			_listIEnumerator.Add(function);

			if(_listIEnumerator.Count == 1)
				StartCoroutine(_ListProcess());
		}

		private void _AddToQueue(int index, IEnumerator function)
		{
			_listIEnumerator.Insert(index, function);
		}

		private void _AddToQueue(IEnumerator[] functions)
		{
			List<IEnumerator> list = new List<IEnumerator>();
			foreach(var item in functions)
			{
				list.Add(item);
			}

			_AddSubQueue(list);
		}

		private void _AddSubQueue(List<IEnumerator> functionList)
		{
			functionList.Reverse();

			int index = 0;
			if(_listIEnumerator.Count != 0)
			{
				index = 1;
			}

			foreach(var item in functionList)
			{
				_listIEnumerator.Insert(index, item);
			}

			if(index == 0)
			{
				StartCoroutine(_ListProcess());
			}
		}

		private IEnumerator _ListProcess()
		{
			while(_listIEnumerator.Count > 0)
			{
				yield return StartCoroutine(_listIEnumerator[0]);

				if(_listIEnumerator.Count > 0)
					_listIEnumerator.RemoveAt(0);
			}
		}

		private void _StopAllQueue()
		{
			foreach(var enumerator in _listIEnumerator)
				StopCoroutine(enumerator);

			_listIEnumerator = new List<IEnumerator>();
		}

	#endregion


		private static IEnumerator _Run(Action action, int afterFrame)
		{
			var eof = new WaitForEndOfFrame();
			while(afterFrame > 0)
			{
				afterFrame--;
				yield return eof;
			}

			action?.Invoke();
		}

		private static IEnumerator _RunFixed(Action action, int afterFrame)
		{
			while(afterFrame > 0)
			{
				afterFrame--;
				yield return new WaitForFixedUpdate();
			}

			action?.Invoke();
		}

		private static IEnumerator _Run(Action action, float delay)
		{
			if(delay > 0)
				yield return WaitForSeconds(delay);

			action?.Invoke();
		}

		private static IEnumerator _RunContinuous(Action continuousAction, float duration, Action startAction, Action endAction, float delay)
		{
			if(delay > 0)
				yield return WaitForSeconds(delay);

			startAction?.Invoke();

			do
			{
				duration -= Time.deltaTime;
				yield return null;

				continuousAction?.Invoke();
			} while(duration > 0);

			endAction?.Invoke();
		}

		private static IEnumerator _RunContinuous(Action<float> continuousAction, float duration, Action startAction, Action endAction, float delay)
		{
			if(delay > 0)
				yield return WaitForSeconds(delay);

			startAction?.Invoke();

			float elapsedTime = 0;

			do
			{
				elapsedTime += Time.deltaTime;
				yield return null;

				continuousAction?.Invoke(elapsedTime);
			} while(elapsedTime < duration);

			endAction?.Invoke();
		}

		private static IEnumerator _Stop(Coroutine coroutine, float delay)
		{
			if(delay > 0)
				yield return WaitForSeconds(delay);

			Instance?.StopCoroutine(coroutine);
		}

	#endregion


	#region API

	#region Queue

		//TODO: Bu queue sistemi sahnede tek olacak ve haliyle tek bir sıra var. Belki bunu başka bir üst sisteme çıkarıp öyle birden fazla queue yapabiliriz; ya da burayı düzenleyip yapabiliriz.

		/// <summary>
		/// Adds an Enumerator to the Queue and automatically starts it if this is the first one.
		/// </summary>
		/// <param name="enumerator">Enumerator to be added</param>
		public void AddToQueue(IEnumerator enumerator)
		{
			_AddToQueue(enumerator);
		}

		/// <summary>
		/// Inserts an Enumerator to the Queue in the specific index and automatically starts it if this is the first one.
		/// </summary>
		/// <param name="index">Index for this Enumerator to be inserted in the Queue</param>
		/// <param name="enumerator">Enumerator to be added</param>
		public void AddToQueue(int index, IEnumerator enumerator)
		{
			_AddToQueue(enumerator);
		}

		/// <summary>
		/// Adds multiple Enumerators to the Queue and automatically starts it if these are the first ones.
		/// </summary>
		/// <param name="enumerators">Enumerators to be added</param>
		public void AddToQueue(IEnumerator[] enumerators)
		{
			_AddToQueue(enumerators);
		}

		/// <summary>
		/// Adds a delayed action to the Queue and automatically starts it if this is the first one.
		/// </summary>
		/// <param name="action">Action to be invoked</param>
		/// <param name="delay">Action invoke delay in seconds</param>
		public void AddToQueue(Action action, float delay)
		{
			_AddToQueue(_Run(action, delay));
		}

		/// <summary>
		/// Inserts a delayed action to the Queue in the specific index and automatically starts it if this is the first one.
		/// </summary>
		/// <param name="index">Index for the Action to be inserted in the Queue</param>
		/// <param name="action">Action to be invoked</param>
		/// <param name="delay">Action invoke delay in seconds</param>
		public void AddToQueue(int index, Action action, float delay)
		{
			_AddToQueue(index, _Run(action, delay));
		}

		/// <summary>
		/// Adds a delayed action to the Queue and automatically starts it if this is the first one.
		/// </summary>
		/// <param name="action">Action to be invoked</param>
		/// <param name="afterFrame">Action invoke delay in frame count</param>
		public void AddToQueue(Action action, int afterFrame)
		{
			_AddToQueue(_Run(action, afterFrame));
		}

		/// <summary>
		/// Inserts a delayed action to the Queue in the specific index and automatically starts it if this is the first one.
		/// </summary>
		/// <param name="index">Index for the Action to be inserted in the Queue</param>
		/// <param name="action">Action to be invoked</param>
		/// <param name="afterFrame">Action invoke delay in frame count</param>
		public void AddToQueue(int index, Action action, int afterFrame)
		{
			_AddToQueue(index, _Run(action, afterFrame));
		}

		/// <summary>
		/// Adds a FixedUpdate delayedaction to the Queue and automatically starts it if this is the first one.
		/// </summary>
		/// <param name="action">Action to be invoked</param>
		/// <param name="afterFrame">Action invoke delay in fixed frame count</param>
		public void AddToQueueFixed(Action action, int afterFrame)
		{
			_AddToQueue(_RunFixed(action, afterFrame));
		}

		/// <summary>
		/// Inserts a FixedUpdate action to the Queue in the specific index and automatically starts it if this is the first one.
		/// </summary>
		/// <param name="index">Index for the Action to be inserted in the Queue</param>
		/// <param name="action">Action to be invoked</param>
		/// <param name="afterFrame">Action invoke delay in fixed frame count</param>
		public void AddToQueueFixed(int index, Action action, int afterFrame)
		{
			_AddToQueue(index, _RunFixed(action, afterFrame));
		}

		/// <summary>
		/// Adds a continuous action to the Queue and automatically starts it if this is the first one.
		/// </summary>
		/// <param name="continuousAction">The action which will be invoked each frame</param>
		/// <param name="duration">The duration in seconds</param>
		/// <param name="startAction">The action which will be invoked after delay, before continuousAction</param>
		/// <param name="endAction">The action which will be invoked after continuousAction invoke duration ends</param>
		/// <param name="delay">First continuousAction (or startAction) invoke delay in seconds</param>
		public void AddToQueueContinuous(Action continuousAction, float duration, Action startAction = null, Action endAction = null, float delay = 0f)
		{
			_AddToQueue(_RunContinuous(continuousAction, duration, startAction, endAction, delay));
		}

		/// <summary>
		/// Inserts a continuous action to the Queue in the specific index and automatically starts it if this is the first one.
		/// </summary>
		/// <param name="index">Index for this Action group to be inserted in the Queue</param>
		/// <param name="continuousAction">The action which will be invoked each frame</param>
		/// <param name="duration">The duration in seconds</param>
		/// <param name="startAction">The action which will be invoked after delay, before continuousAction</param>
		/// <param name="endAction">The action which will be invoked after continuousAction invoke duration ends</param>
		/// <param name="delay">First continuousAction (or startAction) invoke delay in seconds</param>
		public void AddToQueueContinuous(int index, Action continuousAction, float duration, Action startAction = null, Action endAction = null, float delay = 0f)
		{
			_AddToQueue(index, _RunContinuous(continuousAction, duration, startAction, endAction, delay));
		}

		/// <summary>
		/// Stops all the Enumerators in the queue
		/// </summary>
		public void StopAllQueue()
		{
			_StopAllQueue();
		}

	#endregion


		/// <summary>
		/// Invokes the action after a delay in seconds
		/// </summary>
		/// <param name="action">The action to be invoked</param>
		/// <param name="delay">Action invoke delay in seconds</param>
		/// <returns>The Coroutine of this function</returns>
		public static Coroutine Run(Action action, float delay)
		{
			return Instance?.StartCoroutine(_Run(action, delay));
		}

		/// <summary>
		/// Invokes the action after a delay in frame count
		/// </summary>
		/// <param name="action">The action to be invoked</param>
		/// <param name="afterFrame">Action invoke delay in frame count</param>
		/// <returns>The Coroutine of this function</returns>
		public static Coroutine Run(Action action, int afterFrame)
		{
			return Instance?.StartCoroutine(_Run(action, afterFrame));
		}

		/// <summary>
		/// Invokes the action after a delay in fixed frame count
		/// </summary>
		/// <param name="action">The action to be invoked</param>
		/// <param name="afterFrame">Action invoke delay in fixed frame count</param>
		/// <returns>The Coroutine of this function</returns>
		public static Coroutine RunFixed(Action action, int afterFrame)
		{
			return Instance?.StartCoroutine(_RunFixed(action, afterFrame));
		}

		/// <summary>
		/// Invokes an action in a duration in each frame.
		/// </summary>
		/// <param name="continuousAction">The action which will be invoked each frame</param>
		/// <param name="duration">The duration in seconds</param>
		/// <param name="startAction">The action which will be invoked after delay, before continuousAction</param>
		/// <param name="endAction">The action which will be invoked after continuousAction invoke duration ends</param>
		/// <param name="delay">First continuousAction (or startAction) invoke delay in seconds</param>
		/// <returns>The Coroutine of this function</returns>
		public static Coroutine RunContinuous(Action continuousAction, float duration, Action startAction = null, Action endAction = null, float delay = 0f)
		{
			return Instance?.StartCoroutine(_RunContinuous(continuousAction, duration, startAction, endAction, delay));
		}

		/// <summary>
		/// Invokes an action in a duration in each frame.
		/// </summary>
		/// <param name="continuousAction">The action which will be invoked each frame and sends elapsed time</param>
		/// <param name="duration">The duration in seconds</param>
		/// <param name="startAction">The action which will be invoked after delay, before continuousAction</param>
		/// <param name="endAction">The action which will be invoked after continuousAction invoke duration ends</param>
		/// <param name="delay">First continuousAction (or startAction) invoke delay in seconds</param>
		/// <returns>The Coroutine of this function</returns>
		public static Coroutine RunContinuous(Action<float> continuousAction, float duration, Action startAction = null, Action endAction = null, float delay = 0f)
		{
			return Instance?.StartCoroutine(_RunContinuous(continuousAction, duration, startAction, endAction, delay));
		}

		/// <summary>
		/// Stops a specific Coroutine
		/// </summary>
		/// <param name="coroutine">The Coroutine that will ve stopped</param>
		public static void Stop(Coroutine coroutine)
		{
			Instance?.StopCoroutine(coroutine);
		}


		/// <summary>
		/// Stops a specific Coroutine after a delay in seconds
		/// </summary>
		/// <param name="coroutine">The Coroutine that will be stopped</param>
		/// <param name="delay">Coroutine stop delay in seconds</param>
		public static void Stop(Coroutine coroutine, float delay)
		{
			Instance?.StartCoroutine(_Stop(coroutine, delay));
		}

		//TODO: Düzgünce çalışır hale getirilecek. Not olsun diye ekledim.
		public static IEnumerator WaitUntil(Action action, WaitUntil waitUntil)
		{
			yield return waitUntil;
			action?.Invoke();
		}

		/// <summary>
		/// Wait until specified task is completed or canceled or faulted
		/// </summary>
		/// <param name="task">Task that is using</param>
		public static IEnumerator WaitForTask(Task task)
		{
			while(!task.IsCompleted)
				yield return null;

			if(task.IsFaulted || task.IsCanceled)
				Debug.LogError($"task.IsFaulted || task.IsCanceled => {task.Exception}");

			yield return null;
		}

		/// <summary>
		/// Garbage-free WaitForSeconds
		/// </summary>
		/// <param name="waitTime"></param>
		/// <returns></returns>
		public static IEnumerator WaitForSeconds(float waitTime)
		{
			var passed = 0f;
			while(passed < waitTime)
			{
				passed += Time.deltaTime;
				yield return null;
			}
		}

		/// <summary>
		/// Waits until specified frame count has passed
		/// </summary>
		/// <param name="frameCount"></param>
		/// <returns></returns>
		public static IEnumerator WaitForFrames(int frameCount)
		{
			var eof = new WaitForEndOfFrame();
			while(frameCount > 0)
			{
				frameCount--;
				yield return eof;
			}
		}

	#endregion
}
