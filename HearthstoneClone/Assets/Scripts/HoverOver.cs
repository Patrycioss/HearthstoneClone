using System;
using System.Collections.Generic;
using System.Linq;
using Cards.Interfaces.MouseOver;
using UnityEngine;

namespace DefaultNamespace
{
	public class HoverOver : MonoBehaviour
	{
		private List<IMouseOver> _mouseOvers = new List<IMouseOver>();
		
		public void FixedUpdate()
		{
			if (Camera.main != null)
			{
				Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
				if (Physics.Raycast (ray, out RaycastHit hit, 100))
				{

					var components = hit.transform.GetComponents(typeof(Component));

					var a = components.Select(pComponent =>
					{
						if (pComponent.GetType().GetInterface(typeof(IMouseOver).ToString()) == typeof(IMouseOver))
							return pComponent.GetType();
						return null;
					});

					foreach (var b in a)
					{
						if (b == null) continue;
						
						Component component = hit.transform.GetComponent(b);
						IMouseOver mouseOver = (IMouseOver) component;
						
						if (_mouseOvers.Contains(mouseOver)) continue;
						_mouseOvers.Add(mouseOver);
						mouseOver.OnStartHover();
						mouseOver.IsHovering = true;
					}

					List<IMouseOver> toBeRemoved = new List<IMouseOver>();
					
					foreach (var mouseOver in _mouseOvers)
					{
						if (a == null || !a.Contains(mouseOver.GetType()))
						{
							mouseOver.OnEndHover();
							mouseOver.IsHovering = false;
							
							toBeRemoved.Add(mouseOver);
						}
					}
					
					toBeRemoved.ForEach(pMouseOver => _mouseOvers.Remove(pMouseOver));
				}
			}
		}
	}
}