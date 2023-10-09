using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using Cards.Interfaces.MouseOver;
using UnityEngine;

namespace Engine
{
	public class HoverOver : MonoBehaviour
	{
		private Camera mainCamera;

		private Transform hoveringOver;
		private List<IMouseOver> mouseOvers = new();

		private void Awake()
		{
			mainCamera = Camera.main;
		}

		public void FixedUpdate()
		{
			Ray ray = mainCamera.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out RaycastHit hit, 100))
			{
				if (hit.transform == hoveringOver) return;

				hoveringOver = hit.transform;
				mouseOvers.ForEach(pOver =>
				{
					pOver.OnEndHover();
					pOver.IsHovering = false;
				});
				
				mouseOvers = hit.transform.GetComponents<IMouseOver>().ToList();
				
				mouseOvers.ForEach(pOver =>
				{
					pOver.OnStartHover();
					pOver.IsHovering = true;
				});
			}
		}
	}
}