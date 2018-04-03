﻿using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace IRSequencer_v3.Gui
{
	public class PanelResizer : MonoBehaviour, IPointerDownHandler, IDragHandler
	{
		public Vector2 minSize;
		public Vector2 maxSize;

		public RectTransform rectTransform;
		private Vector2 currentPointerPosition;
		private Vector2 previousPointerPosition;
		
		public void OnPointerDown(PointerEventData data)
		{
			rectTransform.SetAsLastSibling();
			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera, out previousPointerPosition);
		}

		public void OnDrag(PointerEventData data)
		{
			if(rectTransform == null)
				return;

			Vector2 sizeDelta = rectTransform.sizeDelta;

			RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, data.position, data.pressEventCamera, out currentPointerPosition);
			Vector2 resizeValue = currentPointerPosition - previousPointerPosition;

			sizeDelta += new Vector2(resizeValue.x, -resizeValue.y);
			sizeDelta = new Vector2(
				Mathf.Clamp(sizeDelta.x, minSize.x, maxSize.x),
				Mathf.Clamp(sizeDelta.y, minSize.y, maxSize.y)
				);

			rectTransform.sizeDelta = sizeDelta;

			previousPointerPosition = currentPointerPosition;
		}
	}
}