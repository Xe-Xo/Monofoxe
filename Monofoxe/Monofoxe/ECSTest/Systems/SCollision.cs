﻿using System;
using System.Collections.Generic;
using Monofoxe.Engine.ECS;
using Monofoxe.ECSTest.Components;
using Monofoxe.Utils;
using Monofoxe.Engine;
using Microsoft.Xna.Framework;

namespace Monofoxe.ECSTest.Systems
{
	public class SCollision : ISystem, ISystemFixedUpdateEvents
	{
		public string Tag => "collision";

		public void Create(Component component) {}

		public void Destroy(Component component) {}

		public void Update(List<Component> components) {}


		public void FixedUpdate(List<Component> components) 
		{
		
			var movement = ComponentMgr.GetComponentList<CMovement>(components);
			
			var id = 0;
			var otherId = 0;
			foreach(CCollision collider in components)
			{
				otherId = 0;
				foreach(CCollision otherCollider in components)
				{
					if (id != otherId && GameMath.Distance(movement[id].Position, movement[otherId].Position) < collider.MaskR + otherCollider.MaskR - 1)
					{
						var rSum = collider.MaskR + otherCollider.MaskR;
						var dist = GameMath.Distance(movement[id].Position, movement[otherId].Position);

						var v = movement[id].Position - movement[otherId].Position;
						v.Normalize();

						var resVect = v * (rSum - dist) / 2f;

						movement[id].Position += resVect;
						movement[otherId].Position -= resVect;
					}

					otherId += 1;
				}
				id += 1;
			}
			
			id = 0;
			foreach(CCollision collider in components)
			{
				collider.Owner.Depth = -(int)movement[id].Position.Y;
				id += 1;
			}
		}

		
		public void Draw(List<Component> components)
		{
			// Game crashes, if entities are deleted.
			
			var movement = ComponentMgr.GetComponentList<CMovement>(components);
			DrawMgr.CurrentColor = Color.White;
			var id = 0;
			foreach(CCollision collider in components)
			{
				//DrawMgr.DrawCircle(movement[id].Position, collider.MaskR, true);
				DrawMgr.DrawSprite(Resources.Sprites.SpritesDefault.Barrel, movement[id].Position);
				id += 1;
			}
		}
	}
}