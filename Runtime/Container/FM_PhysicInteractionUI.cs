using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace FullMotionUIContainer.Runtime.Container
{
	/// <summary>
	/// 物理检测的UI控件，一种取巧的方法，直接允许使用 Physics.Raycast 完成UI检测，适用于较为简单的UI界面
	/// </summary>
	public abstract class FM_PhysicInteractionUI : FM_UISelectable
	{
		#region 字段属性

		/// <summary>
		/// UI碰撞体，一般来说是方形的
		/// </summary>
		private Collider ui_Collider;

		#endregion

		#region 生命周期

		protected override void Awake()
		{
			base.Awake();
			ui_Collider = GetComponent<Collider>();
			if(ui_Collider == null)
			{
				var collider = gameObject.AddComponent<BoxCollider>();
				ui_Collider = collider;
				UpdateColliderSizeToRect();
			}
		}

		#endregion

		#region 方法

		/// <summary>
		/// 更新此处的UI碰撞体尺寸到矩形变换组件
		/// </summary>
		protected virtual void UpdateColliderSizeToRect()
		{
			if (ui_Collider is BoxCollider collider)
				collider.size = new Vector3(this.rectTransform.sizeDelta.x, this.rectTransform.sizeDelta.y, 1);
		}

		#endregion

		#region 物理交互

		/// <summary>
		/// 光标放上
		/// </summary>
		public virtual void CurrentOnPlace()
		{
			OnUISelected();
			// 不建议使用，因为没有成对的取消事件
			//if(SelectableTarget is Button button)
			//	button.OnSelect(new BaseEventData(EventSystem.current));
		}

		/// <summary>
		/// 光标互动/进入按钮
		/// </summary>
		public virtual void CurrentOnClick()
		{
			if(SelectableTarget is Button button)
			{
				button.onClick?.Invoke();
			}
		}

		#endregion

		#region 碰撞检测

		/**
		 * 按钮无需添加刚体等复杂的计算组件，
		 * 进而不会产生碰撞事件，
		 * 所以可以忽略碰撞检测；
		 * 不过不能开启is trigger
		 */

		//private void OnCollisionEnter(Collision collision)
		//{ }

		//private void OnCollisionExit(Collision collision)
		//{ }

		//private void OnTriggerEnter(Collider other)
		//{ }

		//private void OnTriggerExit(Collider other)
		//{ }

		#endregion

	}



	/// <summary>
	/// 扩展物理交互UI
	/// </summary>
	public static partial class FM_PhysicInteractionUI_Extent
	{
		/// <summary>
		/// 获取其中物理交互的UI
		/// </summary>
		/// <param name="collider"></param>
		public static FM_PhysicInteractionUI GetPhysicUI(this Collider collider)
		{
			var target = collider.GetComponent<FM_PhysicInteractionUI>();
			if(target == null)
				Debug.LogError("无法获取其中的物理交互UI组件");
			return target;
		}
	}
}