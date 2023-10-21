#define _DEUBG_
#define _WARING_

#if UNITY_EDITOR
#undef _DEBUG_
#endif

using FullMotionUIContainer.Runtime.Type;

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.UIElements;

namespace FullMotionUIContainer.Runtime.Container
{
	/// <summary>
	/// UI元件类，用于按钮等互动元素
	/// </summary>
    public abstract class FM_UISelectable : FM_UIBase
	{

		/// <summary>
		/// 此元素的管理容器，所属容器
		/// </summary>
		//protected internal FM_UIContainer ManagingContainer;

		#region 事件委托

		/// <summary>
		/// 当有一个UI项目被选中时
		/// </summary>
		public event Action<FM_UISelectable> WhenHasUISelected;

		/// <summary>
		/// 当有一个UI项目被取消选中时
		/// </summary>
		public event Action<FM_UISelectable> WhenHasUIDiselected;

		#endregion

		#region 字段属性

		/// <summary>
		/// 事件系统
		/// </summary>
		public static EventSystem UIEventSystem = null;

		/// <summary>
		/// 事件系统找不到时，不再查找，避免存在大量UI时反复对一个不存在的物体进行查找
		/// </summary>
		private static bool UIEventSystemNotFind = false;

		/// <summary>
		/// 默认图像
		/// </summary>
		private Sprite normalImage;
		/// <summary>
		/// 普通图像
		/// </summary>
		public Sprite NormalImage => normalImage;
		/// <summary>
		/// 高亮图像
		/// </summary>
		public Sprite HighlightedImage => SelectableTarget.spriteState.highlightedSprite;
		/// <summary>
		/// 按下图像
		/// </summary>
		public Sprite PressedImage => SelectableTarget.spriteState.pressedSprite;
		/// <summary>
		/// 选中图像
		/// </summary>
		public Sprite SelectedSprite => SelectableTarget.spriteState.selectedSprite;
		/// <summary>
		/// 禁用图像
		/// </summary>
		public Sprite DisabledImage => SelectableTarget.spriteState.disabledSprite;

		/// <summary>
		/// 按钮图像
		/// </summary>
		private UnityEngine.UI.Image image;

		/// <summary>
		/// 按钮图像
		/// </summary>
		public UnityEngine.UI.Image Image => image;


		/// <summary>
		/// 可交互的对象
		/// </summary>
		protected internal Selectable SelectableTarget;

		#endregion

		#region 生命周期

		protected override void Awake()
		{
			base.Awake();

			// 初始化可交互对象
			SelectableTarget = transform.GetComponent<Selectable>();
			if(SelectableTarget == null)
			{
				Debug.LogError($"FMUI警告：未能在同层级中找到一个可选择的UI对象，FMUI可能需要依赖此类对象");
			}
			else
			{
				// 按钮背景图像
				image = SelectableTarget.GetComponent<UnityEngine.UI.Image>();
				if(image != null)
					image = SelectableTarget.GetComponentInChildren<UnityEngine.UI.Image>();
				if(image != null)
					normalImage = image.sprite;
			}
			
			// 查找事件系统
			if(!UIEventSystemNotFind && UIEventSystem == null)
			{
				UIEventSystem = FindObjectOfType<EventSystem>();
				if(UIEventSystem == null)
					UIEventSystemNotFind = true;
			}
		}

		#endregion

		#region 方法

		/**
		 * 代理原生按钮事件
		 */
#if false
		public void OnPointerDown()
		{
			PointerEventData eventData = new PointerEventData(EventSystem.current);
			eventData.pointerEnter = SelectableTarget.gameObject;
			SelectableTarget.OnPointerDown(eventData);
		}
		public void OnPointerDown(PointerEventData eventData)
		{
			SelectableTarget.OnPointerDown(eventData);
		}

		public void OnPointerUp()
		{
			PointerEventData eventData = new PointerEventData(EventSystem.current);
			eventData.pointerEnter = SelectableTarget.gameObject;
			SelectableTarget.OnPointerUp(eventData);
		}

		public void OnPointerUp(PointerEventData eventData)
		{
			SelectableTarget.OnPointerUp(eventData);
		}

		public void OnPointerEnter()
		{
			PointerEventData eventData = new PointerEventData(EventSystem.current);
			eventData.pointerEnter = SelectableTarget.gameObject;
			SelectableTarget.OnPointerEnter(eventData);
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
			SelectableTarget.OnPointerEnter(eventData);
		}

		public void OnPointerExit()
		{
			PointerEventData eventData = new PointerEventData(EventSystem.current);
			eventData.pointerEnter = SelectableTarget.gameObject;
			SelectableTarget.OnPointerExit(eventData);
		}

		public void OnPointerExit(PointerEventData eventData)
		{
			SelectableTarget.OnPointerExit(eventData);
		}

		public void OnSelect()
		{
			PointerEventData eventData = new PointerEventData(EventSystem.current);
			eventData.pointerEnter = SelectableTarget.gameObject;
			SelectableTarget.OnSelect(eventData);
		}

		public void OnSelect(PointerEventData eventData)
		{
			SelectableTarget.OnSelect(eventData);
		}

		public void OnDeselect()
		{
			PointerEventData eventData = new PointerEventData(EventSystem.current);
			eventData.pointerEnter = SelectableTarget.gameObject;
			SelectableTarget.OnDeselect(eventData);
		}

		public void OnDeselect(PointerEventData eventData)
		{
			SelectableTarget.OnDeselect(eventData);
		}

		public void Select()
		{
			SelectableTarget.Select();
		}

#endif

		/**
		 * 基本按钮事件
		 */

		/// <summary>
		/// 当项目被选中时，表示此项目可以高亮
		/// </summary>
		/// <param name="container">如果是由容器调用，则必须提供容器引用，且保证此项目与容器均被初始化</param>
		protected internal virtual void OnUISelected(FM_UIBase container = null)
		{
#if _DEBUG_
			Debug.Log($"选中{this.name}");
#endif
			// 如果不是容器发送的选中事件，那么反馈一个选中事件
			if(container != null && ParentContainer != null && container == ParentContainer)
				return;
			WhenHasUISelected?.Invoke(this);
		}

		/// <summary>
		/// 当项目不再被选中时，表示此项目不再高亮
		/// </summary>
		/// <param name="container">如果是由容器调用，则必须提供容器引用，且保证此项目与容器均被初始化</param>
		protected internal virtual void OnUIDeselected(FM_UIBase container = null)
		{
#if _DEBUG_
			Debug.Log($"取消选中{this.name}");
#endif
			// 如果不是容器发送的取消选中事件，那么反馈一个取消选中事件
			if(container != null && ParentContainer != null && container == ParentContainer)
				return;
			WhenHasUIDiselected?.Invoke(this);
		}


		protected internal abstract void OnPressed();

		protected internal abstract void OnReleased();


		#endregion
	}

	public static partial class FM_PhysicInteractionUI_Extent
	{
		/// <summary>
		/// 获取其中可交互的UI
		/// </summary>
		/// <param name="collider"></param>
		public static Selectable GetSelectableUI(this Collider collider)
		{
			return collider.GetComponent<Selectable>();
		}

		/// <summary>
		/// 获取其中可交互的按钮
		/// </summary>
		/// <param name="collider"></param>
		public static UnityEngine.UI.Button GetButtonUI(this Collider collider)
		{
			return collider.GetComponent<UnityEngine.UI.Button>();
		}
	}
}