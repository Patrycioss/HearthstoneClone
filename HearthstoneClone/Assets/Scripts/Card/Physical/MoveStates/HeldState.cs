﻿using System;

namespace Card.Physical.MoveStates
{
	/// <summary>
	/// State that handles what happens when a <see cref="PhysicalCard"/> is in the player's hand.
	/// </summary>
	public class HeldState : CustomStates.MovePhysicalCardState
	{
		public HeldState(PhysicalCard card) : base(card) {}

		public override void Start(){}

		public override void Update(){}

		public override void Stop(Action onCompleteCallback)
		{
			onCompleteCallback();
		}
	}
}