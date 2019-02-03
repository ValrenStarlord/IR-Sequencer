using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using IRSequencer_v3;
using IRSequencer_v3.API;
using System;


namespace IRSequencer_v3.Gui
{
	public class ServoHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
	{
		public IRWrapper.IServo servo;

		public void OnPointerEnter(PointerEventData eventData)
		{
			if(servo != null)
				servo.Highlight = true;
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			if(servo != null)
				servo.Highlight = false;
		}

		public void OnDestroy()
		{
			if(servo != null)
				servo.Highlight = false;
		}
		
	}
}
